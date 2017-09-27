///////////////////////////////////////////////////////////
//  Organization1.cs
//  Implementation of the Class Organization
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:49
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Resources;
using ExpressiveAnnotations.Attributes;
using System;
using Kartverket.Register.Models.Translations;

namespace Kartverket.Register.Models
{
	public class Organization : RegisterItem {

        public Organization()
        {
            this.Translations = new TranslationCollection<OrganizationTranslation>();
        }

        // logos will be stored in this directory
        public const string DataDirectory = "organizations/";

        [Display(Name = "Organization_Number", ResourceType = typeof(Organizations))]
        public string number { get; set; }

        [Display(Name = "Organization_Logo", ResourceType = typeof(Organizations))]
        public string logoFilename { get; set; }

        [Display(Name = "LargeLogo", ResourceType = typeof(Organizations))]
        public string largeLogo { get; set; }

        [Display(Name = "ContactName", ResourceType = typeof(Organizations))]
        public string contact { get; set; }

        [Display(Name = "EmailAddress", ResourceType = typeof(Organizations))]
        [DataType(DataType.EmailAddress)] 
        public string epost { get; set; }

        [Display(Name = "Member", ResourceType = typeof(Organizations))]
        public bool? member { get; set; }

        [Display(Name = "AgreementYear", ResourceType = typeof(Organizations))]
        [Range(1000,9999, ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "AgreementYearErrorMessage")]
        public int? agreementYear { get; set; }

        [Display(Name = "AgreementDocumentUrl", ResourceType = typeof(Organizations))]
        public string agreementDocumentUrl { get; set; }

        [Display(Name = "PriceFormDocument", ResourceType = typeof(Organizations))]
        public string priceFormDocument { get; set; }

        [Display(Name = "Shortname", ResourceType = typeof(Organizations))]
        public string shortname { get; set; }

        /// <summary>
        /// Determines what kind of organization: regular or municipality.
        /// If empty, then regular organization. 
        /// If set to 'municipality' then the municipality specific attributes below should be populated
        /// </summary>
        [Display(Name = "OrganizationType", ResourceType = typeof(Organizations))]
        public string OrganizationType { get; set; }

        // municipality specific attributes
        [Display(Name = "MunicipalityCode", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "MunicipalityCodeRequired")]
        public string MunicipalityCode { get; set; }
        [Display(Name = "GeographicCenterX", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "GeographicCenterXErrorMessage")]
        public string GeographicCenterX { get; set; }
        [Display(Name = "GeographicCenterY", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "GeographicCenterYErrorMessage")]
        public string GeographicCenterY { get; set; }
        [Display(Name = "BoundingBoxNorth", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "BoundingBoxNorthErrorMessage")]
        public string BoundingBoxNorth { get; set; }
        [Display(Name = "BoundingBoxSouth", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "BoundingBoxSouthErrorMessage")]
        public string BoundingBoxSouth { get; set; }
        [Display(Name = "BoundingBoxEast", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "BoundingBoxEastErrorMessage")]
        public string BoundingBoxEast { get; set; }
        [Display(Name = "BoundingBoxWest", ResourceType = typeof(Organizations))]
        [RequiredIf("IsMunicipality()", ErrorMessageResourceType = typeof(Organizations), ErrorMessageResourceName = "BoundingBoxWestErrorMessage")]
        public string BoundingBoxWest { get; set; }
        [Display(Name = "DateConfirmedMunicipalDOK", ResourceType = typeof(Organizations))]
        public DateTime? DateConfirmedMunicipalDOK { get; set; }
        public string StatusConfirmationMunicipalDOK { get; set; }
        public virtual TranslationCollection<OrganizationTranslation> Translations { get; set; }

        public void AddMissingTranslations()
        {
            Translations.AddMissingTranslations();
        }

        public new string NameTranslated()
        {
            return base.NameTranslated();
        }

        public new string DescriptionTranslated()
        {
            return base.DescriptionTranslated();
        }

        public virtual string GetOrganizationEditUrl()
        {
            if (register.parentRegister == null)
            {
                return "/organisasjoner/" + register.seoname + "/" + submitter.seoname + "/" + seoname + "/rediger";
            }
            else {
                return "/organisasjoner/" + register.parentRegister.seoname + "/" + register.owner.seoname + "/" + register.seoname + "/" + submitter.seoname + "/" + seoname + "/rediger";
            }
        }

        public virtual string GetOrganizationDeleteUrl()
        {
            if (register.parentRegister == null)
            {
                return "/organisasjoner/" + register.seoname + "/" + submitter.seoname + "/" + seoname + "/slett";
            }
            else {
                return "/organisasjoner/" + register.parentRegister.seoname + "/" + register.owner.seoname + "/" + register.seoname + "/" + submitter.seoname + "/" + seoname + "/slett";
            }
        }

	    public bool IsMunicipality()
	    {
	        return OrganizationType != null && OrganizationType.Equals(Models.OrganizationType.Municipality);
	    }

	    public bool HasGeographicCenter()
	    {
	        return !string.IsNullOrEmpty(GeographicCenterX) && !string.IsNullOrEmpty(GeographicCenterY);
	    }

	    public bool HasBoundingBox()
	    {
	        return !string.IsNullOrEmpty(BoundingBoxNorth)
	               && !string.IsNullOrEmpty(BoundingBoxEast)
	               && !string.IsNullOrEmpty(BoundingBoxSouth)
	               && !string.IsNullOrEmpty(BoundingBoxWest);
	    }

        internal void UpdateDOKMunicipalStatus(string statusDOKMunicipal)
        {
            if (StatusConfirmationMunicipalDOK != statusDOKMunicipal)
            {
                SetStatus(statusDOKMunicipal);
            }
            else if (DateConfirmedMunicipalDOK != null)
            {
                if (DateConfirmedMunicipalDOK.Value.Year != DateTime.Now.Year)
                {
                    SetStatus(statusDOKMunicipal);
                }
            };

        }

        private void SetStatus(string statusDOKMunicipal)
        {
            StatusConfirmationMunicipalDOK = statusDOKMunicipal;
            DateConfirmedMunicipalDOK = DateTime.Now;
        }
    }

    public static class OrganizationType
    {
        public static string Regular = "regular";
        public static string Municipality = "municipality";
    }

}