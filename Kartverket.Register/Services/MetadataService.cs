﻿using System.Linq;
using GeoNorgeAPI;
using Kartverket.Register.Models;
using www.opengis.net;
using System.Web.Configuration;
using Kartverket.Register.Helpers;
using System;
using System.Collections.Generic;
using Kartverket.Register.Models.ViewModels;
using Kartverket.Register.Models.Translations;

namespace Kartverket.DOK.Service
{
    public class MetadataService
    {
        public Dataset UpdateDatasetWithMetadata(Dataset dataset, string uuid, Dataset originalDataset, bool dontUpdateDescription)
        {
            Dataset metadata = FetchDatasetFromKartkatalogen(uuid);
            if (metadata != null)
            {
                dataset.Uuid = uuid;
                dataset.name = metadata.name;
                dataset.seoname = RegisterUrls.MakeSeoFriendlyString(dataset.name);
                dataset.description = metadata.description;
                dataset.MetadataUrl = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "metadata/uuid/" + uuid;
                dataset.PresentationRulesUrl = metadata.PresentationRulesUrl;
                dataset.ProductSheetUrl = metadata.ProductSheetUrl;
                dataset.ProductSpecificationUrl = metadata.ProductSpecificationUrl;
                dataset.datasetthumbnail = metadata.datasetthumbnail;
                dataset.register = originalDataset.register;

                dataset.dokStatusId = originalDataset.dokStatusId;
                dataset.datasetownerId = metadata.datasetownerId;
                dataset.datasetowner = originalDataset.datasetowner;
                dataset.ThemeGroupId = metadata.ThemeGroupId;
                dataset.UuidService = metadata.UuidService;
                dataset.WmsUrl = metadata.WmsUrl;
                dataset.registerId = originalDataset.registerId;
                dataset.dokStatusDateAccepted = originalDataset.dokStatusDateAccepted;
                dataset.Notes = originalDataset.Notes;
                dataset.SpecificUsage = metadata.SpecificUsage;
                dataset.submitterId = originalDataset.submitterId;
                dataset.submitter = originalDataset.submitter;
                dataset.DatasetType = originalDataset.DatasetType;

                dataset.RegionalPlan = originalDataset.RegionalPlan;
                dataset.RegionalPlanNote = originalDataset.RegionalPlanNote;
                dataset.MunicipalSocialPlan = originalDataset.MunicipalSocialPlan;
                dataset.MunicipalSocialPlanNote = originalDataset.MunicipalSocialPlanNote;
                dataset.MunicipalLandUseElementPlan = originalDataset.MunicipalLandUseElementPlan;
                dataset.MunicipalLandUseElementPlanNote = originalDataset.MunicipalLandUseElementPlanNote;
                dataset.ZoningPlanArea = originalDataset.ZoningPlanArea;
                dataset.ZoningPlanAreaNote = originalDataset.ZoningPlanAreaNote;
                dataset.ZoningPlanDetails = originalDataset.ZoningPlanDetails;
                dataset.ZoningPlanDetailsNote = originalDataset.ZoningPlanDetailsNote;
                dataset.BuildingMatter = originalDataset.BuildingMatter;
                dataset.BuildingMatterNote = originalDataset.BuildingMatterNote;
                dataset.PartitionOff = originalDataset.PartitionOff;
                dataset.PartitionOffNote = originalDataset.PartitionOffNote;
                dataset.EenvironmentalImpactAssessment = originalDataset.EenvironmentalImpactAssessment;
                dataset.EenvironmentalImpactAssessmentNote = originalDataset.EenvironmentalImpactAssessmentNote;


                if (dontUpdateDescription) dataset.description = originalDataset.description;

                dataset.restricted = metadata.restricted;

                dataset.DistributionUrl = metadata.DistributionUrl;
                dataset.DistributionArea = metadata.DistributionArea;

                if (metadata.DistributionFormat != null)
                {
                    dataset.DistributionFormat = metadata.DistributionFormat;
                }
                dataset.Translations.Clear();
                dataset.Translations = metadata.Translations;
            }

            return dataset;
        }

        private string AddTheme(string theme)
        {
            RegisterDbContext db = new RegisterDbContext();

            var queryResultsRegisterItem = from o in db.DOKThemes
                                           where o.value == theme
                                           select o.value;

            if (queryResultsRegisterItem.ToList().Count == 0) { 
                db.DOKThemes.Add(new DOKTheme { description = theme, value = theme });
                db.SaveChanges();
            }

            db.Dispose();

            return theme;
        }

        private static string FetchThumbnailUrl(SimpleMetadata metadata)
        {
            string thumbnailUrl = null;
            if (metadata.Thumbnails != null)
            {
                foreach (var thumbnail in metadata.Thumbnails)
                {
                    if (thumbnail.Type == "large_thumbnail")
                    {
                        thumbnailUrl = thumbnail.URL;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(thumbnailUrl))
                {
                    var thumbnail = metadata.Thumbnails.FirstOrDefault();
                    if (thumbnail != null)
                    {
                        thumbnailUrl = thumbnail.URL;
                    }
                }

                if (thumbnailUrl != null && !thumbnailUrl.StartsWith("http"))
                {
                    thumbnailUrl = "https://www.geonorge.no/geonetwork/srv/nor/resources.get?uuid=" + metadata.Uuid + "&access=public&fname=" + thumbnailUrl;
                }
            }
            return thumbnailUrl;
        }

        public static SimpleMetadata FetchMetadata(string uuid)
        {
            GeoNorge g = new GeoNorge("", "", WebConfigurationManager.AppSettings["GeoNetworkUrl"]);
            MD_Metadata_Type metadata = g.GetRecordByUuid(uuid);
            return metadata != null ? new SimpleMetadata(metadata) : null;
        }

        public Dataset FetchDatasetFromKartkatalogen(string uuid)
        {
            Dataset metadata = new Dataset();

            string url = System.Web.Configuration.WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "api/getdata/"+uuid;
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            try
            {
                var json = c.DownloadString(url);

                dynamic data = Newtonsoft.Json.Linq.JObject.Parse(json);
                if (data != null)
                {
                    metadata.name = data.Title;
                    metadata.description = data.Abstract;
                    metadata.PresentationRulesUrl = data.LegendDescriptionUrl;
                    metadata.ProductSheetUrl = data.ProductSheetUrl;
                    metadata.ProductSpecificationUrl = data.ProductSpecificationUrl;
                    metadata.SpecificUsage = data.SpecificUsage;
                    var thumbnails = data.Thumbnails;
                    if (thumbnails != null && thumbnails.Count > 0)
                    {
                        metadata.datasetthumbnail = thumbnails[0].URL.Value;
                    }

                    metadata.datasetownerId = mapOrganizationNameToId(data.ContactOwner != null && data.ContactOwner.Organization != null ? data.ContactOwner.Organization.Value : "");
                    metadata.ThemeGroupId = AddTheme(data.KeywordsNationalTheme != null && data.KeywordsNationalTheme.Count > 0 ? data.KeywordsNationalTheme[0].KeywordValue.Value : "Annen");

                    if (data.ServiceUuid != null)
                        metadata.UuidService = data.ServiceUuid;

                    if (data.ServiceDistributionUrlForDataset != null)
                        metadata.WmsUrl = data.ServiceDistributionUrlForDataset;

                }

                if (data.DistributionDetails != null)
                    metadata.DistributionUrl = data.DistributionDetails.URL;

                if (data.UnitsOfDistribution != null)
                    metadata.DistributionArea = data.UnitsOfDistribution.Value;

                var distributionFormat = data.DistributionFormat;
                if (distributionFormat != null)
                {
                    if (distributionFormat.Name != null)
                        metadata.DistributionFormat = distributionFormat.Name.Value;
                }

                metadata.restricted = false;

                var constraints = data.Constraints;

                if (constraints != null)
                {
                    string accessConstraint = constraints.AccessConstraints.Value;
                    if (!string.IsNullOrEmpty(accessConstraint) && accessConstraint == "Beskyttet")
                    {
                        metadata.restricted = true;
                    }
                }

                var englishTitle = data.EnglishTitle;
                var englishAbstract = data.EnglishAbstract;
                var keywordsNationalThemeEnglish = data.KeywordsNationalTheme[0].EnglishKeyword;
                metadata.Translations.Add(new DatasetTranslation
                {
                    CultureName = Culture.EnglishCode,
                    Name = englishTitle,
                    Description = englishAbstract,
                    ThemeGroupId = keywordsNationalThemeEnglish
                });

                metadata.AddMissingTranslations();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine(url);
                return null;
            }

            return metadata;
        }

        public InspireDataset FetchInspireDatasetFromKartkatalogen(string uuid)
        {
            var inspireDataset = new InspireDataset();
            var url = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "api/getdata/" + uuid;
            var c = new System.Net.WebClient {Encoding = System.Text.Encoding.UTF8};
            try
            {
                var json = c.DownloadString(url);

                dynamic data = Newtonsoft.Json.Linq.JObject.Parse(json);
                if (data != null)
                {
                    inspireDataset.Name = data.Title;
                    inspireDataset.Description = data.Abstract;
                    inspireDataset.PresentationRulesUrl = data.LegendDescriptionUrl;
                    inspireDataset.ProductSheetUrl = data.ProductSheetUrl;
                    inspireDataset.ProductSpecificationUrl = data.ProductSpecificationUrl;
                    inspireDataset.SpecificUsage = data.SpecificUsage;
                    inspireDataset.Uuid = data.Uuid;
                    inspireDataset.MetadataUrl = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "metadata/uuid/" + inspireDataset.Uuid;
                    var thumbnails = data.Thumbnails;
                    if (thumbnails != null && thumbnails.Count > 0)
                    {
                        inspireDataset.DatasetThumbnail = thumbnails[0].URL.Value;
                    }

                    inspireDataset.OwnerId = mapOrganizationNameToId(
                        data.ContactOwner != null && data.ContactOwner.Organization != null
                            ? data.ContactOwner.Organization.Value
                            : "");
                    inspireDataset.ThemeGroupId = AddTheme(data.KeywordsNationalTheme != null && data.KeywordsNationalTheme.Count > 0 ? data.KeywordsNationalTheme[0].KeywordValue.Value : "Annen");

                    inspireDataset.InspireTheme = GetInspireThemeName(
                        data.KeywordsInspire != null && data.KeywordsInspire.Count > 0
                            ? data.KeywordsInspire[0].KeywordValue.Value
                            : "Annen");



                    if (data.ServiceUuid != null)
                        inspireDataset.UuidService = data.ServiceUuid;
                    if (data.ServiceDistributionUrlForDataset != null)
                        inspireDataset.WmsUrl = data.ServiceDistributionUrlForDataset;

                    if (data.DistributionDetails != null)
                        inspireDataset.DistributionUrl = data.DistributionDetails.URL;

                    if (data.UnitsOfDistribution != null)
                        inspireDataset.DistributionArea = data.UnitsOfDistribution.Value;

                    var distributionFormat = data.DistributionFormat;
                    if (distributionFormat != null)
                    {
                        if (distributionFormat.Name != null)
                            inspireDataset.DistributionFormat = distributionFormat.Name.Value;
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine(url);
                return null;
            }

            return inspireDataset;
        }

        private string GetInspireThemeName(string code)
        {
            if (!String.IsNullOrWhiteSpace(code))
            {
                RegisterDbContext db = new RegisterDbContext();
                var queryResultsRegisterItem = from o in db.CodelistValues
                    where o.value == code
                    select o.name;

                return queryResultsRegisterItem.FirstOrDefault();
            }
            return code;
        }

        public GeodatalovDataset FetchGeodatalovDatasetFromKartkatalogen(string uuid)
        {
            var geodatalovDataset = new GeodatalovDataset();
            var url = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "api/getdata/" + uuid;
            var c = new System.Net.WebClient { Encoding = System.Text.Encoding.UTF8 };
            try
            {
                var json = c.DownloadString(url);

                dynamic data = Newtonsoft.Json.Linq.JObject.Parse(json);
                if (data != null)
                {
                    geodatalovDataset.Name = data.Title;
                    geodatalovDataset.Description = data.Abstract;
                    geodatalovDataset.PresentationRulesUrl = data.LegendDescriptionUrl;
                    geodatalovDataset.ProductSheetUrl = data.ProductSheetUrl;
                    geodatalovDataset.ProductSpecificationUrl = data.ProductSpecificationUrl;
                    geodatalovDataset.SpecificUsage = data.SpecificUsage;
                    geodatalovDataset.Uuid = data.Uuid;
                    geodatalovDataset.MetadataUrl = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "metadata/uuid/" + geodatalovDataset.Uuid;
                    var thumbnails = data.Thumbnails;
                    if (thumbnails != null && thumbnails.Count > 0)
                    {
                        geodatalovDataset.DatasetThumbnail = thumbnails[0].URL.Value;
                    }

                    geodatalovDataset.OwnerId = mapOrganizationNameToId(
                        data.ContactOwner != null && data.ContactOwner.Organization != null
                            ? data.ContactOwner.Organization.Value
                            : "");
                    geodatalovDataset.ThemeGroupId =
                        AddTheme(data.KeywordsNationalTheme != null && data.KeywordsNationalTheme.Count > 0
                            ? data.KeywordsNationalTheme[0].KeywordValue.Value
                            : "Annen");

                    if (data.ServiceUuid != null) geodatalovDataset.UuidService = data.ServiceUuid;
                    if (data.ServiceDistributionUrlForDataset != null)
                        geodatalovDataset.WmsUrl = data.ServiceDistributionUrlForDataset;

                    if (data.DistributionDetails != null)
                        geodatalovDataset.DistributionUrl = data.DistributionDetails.URL;

                    if (data.UnitsOfDistribution != null)
                        geodatalovDataset.DistributionArea = data.UnitsOfDistribution.Value;

                    var distributionFormat = data.DistributionFormat;
                    if (distributionFormat != null)
                    {
                        if (distributionFormat.Name != null)
                            geodatalovDataset.DistributionFormat = distributionFormat.Name.Value;
                    }

                    foreach (var keyword in data.KeywordsNationalInitiative)
                    {
                        if (keyword.KeywordValue == "Det offentlige kartgrunnlaget")
                        {
                            geodatalovDataset.Dok = true;
                        }
                        else if (keyword.KeywordValue == "geodataloven")
                        {
                            geodatalovDataset.Geodatalov = true;
                        }
                        else if (keyword.KeywordValue == "Norge digitalt")
                        {
                            geodatalovDataset.NationalDataset = true;
                        }
                        else if (keyword.KeywordValue == "Inspire")
                        {
                            geodatalovDataset.InspireTheme = true;
                        }
                        else if (keyword.KeywordValue == "arealplanerPBL")
                        {
                            geodatalovDataset.Plan = true;
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine(url);
                return null;
            }

            return geodatalovDataset;
        }

        public SearchResultsType SearchMetadata(string searchString)
        {
            GeoNorge g = new GeoNorge("", "", WebConfigurationManager.AppSettings["GeoNetworkUrl"]);
            var filters = new object[]
                   {
                    new BinaryLogicOpType()
                    {
                       Items = new object[]
                        {
                        new PropertyIsLikeType
                        {
                            escapeChar = "\\",
                            singleChar = "_",
                            wildCard = "%",
                            PropertyName = new PropertyNameType {Text = new[] {"srv:title"}},
                            Literal = new LiteralType {Text = new[] {searchString}}
                        },
                        new PropertyIsLikeType
                            {
                                PropertyName = new PropertyNameType {Text = new[] {"srv:type"}},
                                Literal = new LiteralType {Text = new[] {"dataset"}}
                            }
                       },
                       ItemsElementName = new ItemsChoiceType22[]
                        {
                            ItemsChoiceType22.PropertyIsLike, ItemsChoiceType22.PropertyIsLike,
                        }
                    }
                   };


            var filterNames = new ItemsChoiceType23[]
            {
                        ItemsChoiceType23.And
            };

            var result = g.SearchWithFilters(filters, filterNames, 1, 200, true);
            return result;
        }

        private Guid mapOrganizationNameToId(string orgname)
        {
            RegisterDbContext db = new RegisterDbContext();

            var queryResultsRegisterItem = from o in db.Organizations
                                           where o.name == orgname 
                                           select o.systemId;

            Guid ID = queryResultsRegisterItem.FirstOrDefault();

            db.Dispose();

            return ID;
        }

        public string UpdateDatasetsWithMetadata()
        {
            RegisterDbContext db = new RegisterDbContext();

            var queryResultsRegisterItem = from d in db.Datasets
                                           where !string.IsNullOrEmpty(d.Uuid)
                                           select d;

            var datasets = queryResultsRegisterItem.ToList();

            foreach (var dataset in datasets)
            {
                UpdateDatasetWithMetadata(dataset, dataset.Uuid, dataset, false);
                db.Entry(dataset).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            db.Dispose();

            return "updated";
        }

        public List<MetadataItemViewModel> SearchMetadataFromKartkatalogen(string searchString)
        {
            var searchResult = SearchMetadata(searchString);
            var result = new List<MetadataItemViewModel>();

            if (searchResult != null && searchResult.numberOfRecordsMatched != "0")
            {
                result.AddRange(searchResult.Items.Select(t => new MetadataItemViewModel
                {
                    Uuid = ((www.opengis.net.DCMIRecordType) (t)).Items[0].Text[0],
                    Title = ((www.opengis.net.DCMIRecordType) (t)).Items[2].Text[0]
                }));
            }
            return result;
        }
    }
}