///////////////////////////////////////////////////////////
//  Register.cs
//  Implementation of the Class Register
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:51
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kartverket.Register.Models.Translations;
using Resources;
using Kartverket.Register.Helpers;

namespace Kartverket.Register.Models
{
    public class Register
    {
        public Register()
        {
            this.RegisterItems = new HashSet<RegisterItemV2>();
            this.items = new HashSet<RegisterItem>();
            this.subregisters = new HashSet<Register>();
            this.replaces = new HashSet<Version>();
            this.Translations = new TranslationCollection<RegisterTranslation>();
        }
        [Key]
        public Guid systemId { get; set; }
        public virtual ICollection<Version> replaces { get; set; }
        [ForeignKey("owner")]
        public Guid? ownerId { get; set; }
        [Display(Name = "Owner", ResourceType = typeof(Registers))]
        public virtual Organization owner { get; set; }
        [ForeignKey("manager")]
        public Guid? managerId { get; set; }
        public virtual Organization manager { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Registers))]
        public string name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Registers))]
        public string description { get; set; }
        [ForeignKey("status")]
        public string statusId { get; set; }
        public virtual Status status { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateSubmitted { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime modified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateAccepted { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registers), ErrorMessageResourceName = "ContainedItemClassErrorMessage")]
        [Display(Name = "ContainedItemClass", ResourceType = typeof(Registers))]
        public string containedItemClass { get; set; }

        public virtual ICollection<RegisterItem> items { get; set; }
        public virtual ICollection<RegisterItemV2> RegisterItems { get; set; }
        [ForeignKey("parentRegister")]
        public Guid? parentRegisterId { get; set; }
        public virtual Register parentRegister { get; set; }
        public virtual ICollection<Register> subregisters { get; set; }
        [Url]
        public string targetNamespace { get; set; }
        public string seoname { get; set; }
        [ForeignKey("versioning")]
        public Guid? versioningId { get; set; }
        public virtual Version versioning { get; set; }
        public int versionNumber { get; set; }

        [ForeignKey("access")]
        public int? accessId { get; set; }
        public virtual accessType access { get; set; }

        public virtual TranslationCollection<RegisterTranslation> Translations { get; set; }

        public void AddMissingTranslations()
        {
            Translations.AddMissingTranslations();
        }

        public string NameTranslated()
        {
            var cultureName = CultureHelper.GetCurrentCulture();
            var nameTranslated = Translations[cultureName]?.Name;
            if (string.IsNullOrEmpty(nameTranslated))
                nameTranslated = name;
            return nameTranslated;
        }

        public string DescriptionTranslated()
        {
            var cultureName = CultureHelper.GetCurrentCulture();
            var descriptionTranslated = Translations[cultureName]?.Description;
            if (string.IsNullOrEmpty(descriptionTranslated))
                descriptionTranslated = description;
            return descriptionTranslated;
        }

        /// <summary>
        /// Gets Url to current object
        /// </summary>
        /// <returns>Url</returns>
        public virtual string GetObjectUrl()
        {
            return parentRegisterId == null
                ? "/register/" + seoname
                : "/subregister/" + parentRegister.seoname + "/" + owner.seoname + "/" + seoname;
        }

        public bool IsServiceAlertRegister()
        {
            return systemId == Guid.Parse("0f428034-0b2d-4fb7-84ea-c547b872b418");
        }

        public bool IsOfTypeDataset()
        {
            return containedItemClass == "Dataset";
        }

        public bool IsDokMunicipal()
        {
            return name == "Det offentlige kartgrunnlaget - Kommunalt";
        }

        public Guid GetSystemId()
        {
            return systemId == Guid.Empty ? Guid.NewGuid() : systemId;
        }

        public string GetDokMunicipalityUrl()
        {
            return "/register/det-offentlige-kartgrunnlaget-kommunalt";
        }

        public bool IsInspireStatusRegister()
        {
            return name == "Inspire statusregister" && parentRegister == null; // TODO, flere registre kan potensielt hete "Inspire statusregister"
        }

        public bool IsGeodatalovStatusRegister()
        {
            return name == "Geodatalov statusregister" && parentRegister == null;
        }

        public bool ContainedItemClassIsOrganization()
        {
            return containedItemClass == "Organization";
        }

        public bool ContainedItemClassIsCodelistValue()
        {
            return containedItemClass == "CodelistValue";
        }

        public bool ContainedItemClassIsRegister()
        {
            return containedItemClass == "Register";
        }

        public bool ContainedItemClassIsDocument()
        {
            return containedItemClass == "Document";
        }

        public bool ContainedItemClassIsDataset()
        {
            return containedItemClass == "Dataset";
        }

        public bool ContainedItemClassIsEpsg()
        {
            return containedItemClass == "EPSG";
        }

        public bool ContainedItemClassIsNameSpace()
        {
            return containedItemClass == "NameSpace";
        }

        public bool ContainedItemClassIsServiceAlert()
        {
            return containedItemClass == "ServiceAlert";
        }

        public bool ContainedItemClassIsInspireDataset()
        {
            return containedItemClass == "InspireDataset";
        }

        public bool ContainedItemClassIsGeodatalovDataset()
        {
            return containedItemClass == "GeodatalovDataset";
        }

        public string GetObjectCreateUrl(string municipalityCode = null)
        {
            var url = parentRegister == null
                ? seoname + "/ny"
                : parentRegister.seoname + "/" + owner.seoname + "/" + seoname + "/ny";

            if (ContainedItemClassIsDocument()) return "/dokument/" + url;
            if (ContainedItemClassIsCodelistValue()) return "/kodeliste/" + url;
            if (ContainedItemClassIsRegister()) return "/subregister/" + url;
            if (ContainedItemClassIsOrganization()) return "/organisasjoner/" + url;
            if (ContainedItemClassIsEpsg()) return "/epsg/" + url;
            if (ContainedItemClassIsNameSpace()) return "/navnerom/" + url;
            if (ContainedItemClassIsServiceAlert()) return "/tjenestevarsler/" + url;
            if (ContainedItemClassIsInspireDataset()) return "/inspire/" + url;
            if (ContainedItemClassIsGeodatalovDataset()) return "/geodatalov/" + url;
            if (ContainedItemClassIsDataset())
            {
                if (IsDokMunicipal()) return "/dataset/" + seoname + "/" + municipalityCode + "/ny";
                return "/dataset/" + url;
            }
            return "#";
        }

        public string GetEditObjectUrl()
        {
            return parentRegister == null
                ? "/rediger/" + seoname
                : "/subregister/" + parentRegister.seoname + "/" + owner.seoname + "/" + seoname + "/rediger";
        }
    }
}