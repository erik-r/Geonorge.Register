﻿using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using GeoNorgeAPI;
using Kartverket.Register.Models;
using www.opengis.net;
using System.Web.Configuration;

namespace Kartverket.DOK.Service
{
    public class MetadataService
    {
        public void UpdateDatasetWithMetadata(Dataset dataset, string uuid)
        {
            SimpleMetadata metadata = FetchMetadata(uuid);
            if (metadata != null)
            {
                dataset.Uuid = uuid;
                dataset.name = metadata.Title;
                dataset.description = metadata.Abstract;
                dataset.MetadataUrl = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "metadata/uuid/" + uuid;
                dataset.PresentationRulesUrl = metadata.LegendDescriptionUrl;
                dataset.ProductSheetUrl = metadata.ProductSheetUrl;
                dataset.ProductSpecificationUrl = metadata.ProductSpecificationUrl;

                dataset.datasetthumbnail = FetchThumbnailUrl(metadata);

                if (metadata.ContactPublisher != null)
                {
                    dataset.datasetowner.name = metadata.ContactPublisher.Organization;
                }

                SimpleDistributionDetails distributionDetails = metadata.DistributionDetails;
                if (distributionDetails != null)
                {
                    if (distributionDetails.Protocol != null
                        && distributionDetails.Protocol.ToLower().Contains("wms"))
                    {
                        dataset.WmsUrl = distributionDetails.URL;
                    }
                    else
                    {
                        dataset.DistributionUrl = distributionDetails.URL;
                    }
                }

                if (metadata.DistributionFormat != null)
                {
                    dataset.DistributionFormat = metadata.DistributionFormat.Name;
                }
            }
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

        private SimpleMetadata FetchMetadata(string uuid)
        {
            MD_Metadata_Type metadata = new GeoNorge().GetRecordByUuid(uuid);
            return metadata != null ? new SimpleMetadata(metadata) : null;
        }
    }
}