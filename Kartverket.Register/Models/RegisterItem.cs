///////////////////////////////////////////////////////////
//  RegisterItem.cs
//  Implementation of the Class RegisterItem
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:53
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using Kartverket.Register.Helpers;
using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Kartverket.Register.Models
{
    public abstract class RegisterItem
    {
        [Key]
        public Guid systemId { get; set; }
        //public virtual ICollection<Version> replaces { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Registers))]
        public string name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Registers))]
        public string description { get; set; }

        [ForeignKey("submitter")]
        [Display(Name = "Submitter", ResourceType = typeof(Registers))]
        public Guid submitterId { get; set; }
        public virtual Organization submitter { get; set; }

        [Display(Name = "DateSubmitted", ResourceType = typeof(Registers))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateSubmitted { get; set; }

        [Display(Name = "Modified", ResourceType = typeof(Registers))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime modified { get; set; }

        [ForeignKey("status")]
        [Display(Name = "Status:")]
        public string statusId { get; set; }
        public virtual Status status { get; set; }

        [Display(Name = "DateAccepted", ResourceType = typeof(Registers))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateAccepted { get; set; }

        [Display(Name = "DateNotAccepted", ResourceType = typeof(Registers))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateNotAccepted { get; set; }

        [Display(Name = "DateSuperseded", ResourceType = typeof(Registers))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateSuperseded { get; set; }

        [Display(Name = "DateRetired", ResourceType = typeof(Registers))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateRetired { get; set; }

        [Display(Name = "Register:")]
        public virtual Register register { get; set; }
        [ForeignKey("register")]
        public Guid registerId { get; set; }
        public string seoname { get; set; }

        [ForeignKey("versioning")]
        public Guid versioningId { get; set; }
        public virtual Version versioning { get; set; }

        [Display(Name = "VersionNumber", ResourceType = typeof(Registers))]
        public int versionNumber { get; set; }

        [Display(Name = "VersionName", ResourceType = typeof(Registers))]
        public string versionName { get; set; }

        [Display(Name = "DocumentUrl", ResourceType = typeof(Registers))]
        public string documentUrl { get; set; }

        [Display(Name = "ApprovalDocument", ResourceType = typeof(Registers))]
        public string approvalDocument { get; set; }

        [Display(Name = "ApprovalReference", ResourceType = typeof(Registers))]
        public string approvalReference { get; set; }

        [Display(Name = "Accepted", ResourceType = typeof(Registers))]
        public bool? Accepted { get; set; }

        [Display(Name = "UML-modell")]
        public string ApplicationSchema { get; set; }

        [Display(Name = "GML-applikasjonsskjema")]
        public string GMLApplicationSchema { get; set; }

        [Display(Name = "Kartografi-fil")]
        public string CartographyFile { get; set; }

        //end RegisterItem

        public string NameTranslated()
        {
            var cultureName = CultureHelper.GetCurrentCulture();

            var nameTranslated = name;

            if (this is CodelistValue)
            {
                CodelistValue codelistValue = (CodelistValue)this;
                nameTranslated = codelistValue.Translations[cultureName]?.Name;
                if (string.IsNullOrEmpty(nameTranslated))
                    nameTranslated = name;
            }
            else if (this is EPSG)
            {
                EPSG epsg = (EPSG)this;
                nameTranslated = epsg.Translations[cultureName]?.Name;
                if (string.IsNullOrEmpty(nameTranslated))
                    nameTranslated = name;
            }
            else if (this is Organization)
            {
                Organization organization = (Organization)this;
                nameTranslated = organization.Translations[cultureName]?.Name;
                if (string.IsNullOrEmpty(nameTranslated))
                    nameTranslated = name;
            }
            else if (this is Dataset)
            {
                Dataset dataset = (Dataset)this;
                nameTranslated = dataset.Translations[cultureName]?.Name;
                if (string.IsNullOrEmpty(nameTranslated))
                    nameTranslated = name;
            }

            else if (this is Document)
            {
                Document document = (Document)this;
                nameTranslated = document.Translations[cultureName]?.Name;
                if (string.IsNullOrEmpty(nameTranslated))
                    nameTranslated = name;
            }
            else if (this is ServiceAlert)
            {
                ServiceAlert serviceAlert = (ServiceAlert)this;
                nameTranslated = serviceAlert.Translations[cultureName]?.Name;
                if (string.IsNullOrEmpty(nameTranslated))
                    nameTranslated = name;
            }

            return nameTranslated;
        }

        public string DescriptionTranslated()
        {
            var cultureName = CultureHelper.GetCurrentCulture();

            var descriptionTranslated = description;

            if (this is CodelistValue)
            {
                CodelistValue codelistValue = (CodelistValue)this;
                descriptionTranslated = codelistValue.Translations[cultureName]?.Description;
                if (string.IsNullOrEmpty(descriptionTranslated))
                    descriptionTranslated = description;
            }
            else if (this is EPSG)
            {
                EPSG epsg = (EPSG)this;
                descriptionTranslated = epsg.Translations[cultureName]?.Description;
                if (string.IsNullOrEmpty(descriptionTranslated))
                    descriptionTranslated = description;
            }
            else if (this is Organization)
            {
                Organization organization = (Organization)this;
                descriptionTranslated = organization.Translations[cultureName]?.Description;
                if (string.IsNullOrEmpty(descriptionTranslated))
                    descriptionTranslated = description;
            }
            else if (this is Dataset)
            {
                Dataset dataset = (Dataset)this;
                descriptionTranslated = dataset.Translations[cultureName]?.Description;
                if (string.IsNullOrEmpty(descriptionTranslated))
                    descriptionTranslated = description;
            }

            else if (this is Document)
            {
                Document document = (Document)this;
                descriptionTranslated = document.Translations[cultureName]?.Description;
                if (string.IsNullOrEmpty(descriptionTranslated))
                    descriptionTranslated = description;
            }
            else if (this is ServiceAlert)
            {
                ServiceAlert serviceAlert = (ServiceAlert)this;
                descriptionTranslated = serviceAlert.Translations[cultureName]?.Description;
                if (string.IsNullOrEmpty(descriptionTranslated))
                    descriptionTranslated = description;
            }

            return descriptionTranslated;
        }


        public virtual string GetObjectUrl()
        {
            if (this is Dataset)
            {
                Dataset dataset = (Dataset)this;
                return dataset.GetDatasetUrl();
            }
            else if (this is Document)
            {
                Document document = (Document)this;
                return document.GetDocumentUrl();
            }
            else if (this is ServiceAlert)
            {
                ServiceAlert serviceAlert = (ServiceAlert)this;
                return serviceAlert.GetServiceAlertUrl();
            }
            else {
                if(submitter!=null && !string.IsNullOrEmpty(seoname))
                    return register.GetObjectUrl() + "/" + submitter.seoname + "/" + seoname;
                else
                    return register.GetObjectUrl();
            }
        }

        
        public virtual Guid GetSystemId()
        {
            if (systemId == null || systemId == Guid.Empty)
            {
                return Guid.NewGuid();
            }
            else {
                return systemId;
            }
        }

        public DateTime GetDateModified()
        {
            return DateTime.Now;
        }

        public DateTime GetDateSubmbitted()
        {
            return DateTime.Now;
        }

        public string SetStatusId(string status = null)
        {
            if (register != null)
            {
                if (register.IsOfTypeDataset())
                {
                    return "Valid";
                }
            }
            if (status == null || statusId == null)
            {
                return "Submitted";
            }
            else
                return status;
        }

        public int GetVersionNr()
        {
            if (versionNumber == 0)
            {
                return 1;
            }
            else
            {
                return versionNumber++;
            }
        }

        public string GetName()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "ikke angitt";
            }
            else
            {
                return name;
            }
        }

        public string GetDescription()
        {
            return description;
        }

        public Guid GetVersioningId()
        {
            return versioningId;
        }

        public void InitializeNew()
        {
            systemId = Guid.NewGuid();
            modified = DateTime.Now;
            dateSubmitted = DateTime.Now;
            modified = DateTime.Now;
            statusId = "Submitted";
            seoname = RegisterUrls.MakeSeoFriendlyString(name);
            versionNumber = 1;
            if (submitter != null) submitterId = submitter.systemId;
            if (register != null) registerId = register.systemId;
        }

        public Organization GetOwner()
        {
            switch (this)
            {
                case Dataset _:
                    var dataset = (Dataset)this;
                    return dataset.datasetowner;
                case Document _:
                    var document = (Document)this;
                    return document.documentowner;
            }
            return submitter;
        }

        public virtual string GetObjectEditUrl()
        {
            switch (this)
            {
                case Document _:
                    var document = (Document)this;
                    return document.GetDocumentEditUrl();
                case Dataset _:
                    var dataset = (Dataset)this;
                    return dataset.GetDatasetEditUrl();
                case EPSG _:
                    var epsg = (EPSG)this;
                    return epsg.GetEPSGEditUrl();
                case CodelistValue _:
                    var codelistValue = (CodelistValue)this;
                    return codelistValue.GetCodelistValueEditUrl();
                case NameSpace _:
                    var nameSpace = (NameSpace)this;
                    return nameSpace.GetNameSpaceEditUrl();
                case Organization _:
                    var organization = (Organization)this;
                    return organization.GetOrganizationEditUrl();
                case ServiceAlert _:
                    var serviceAlert = (ServiceAlert)this;
                    return serviceAlert.GetServiceAlertEditUrl();
            }
            return "#";
        }

        public string GetObjectDeleteUrl()
        {
            switch (this)
            {
                case Document _:
                    var document = (Document)this;
                    return document.GetDocumentDeleteUrl();
                case Dataset _:
                    var dataset = (Dataset)this;
                    return dataset.GetDatasetDeleteUrl();
                case EPSG _:
                    var epsg = (EPSG)this;
                    return epsg.GetEPSGDeleteUrl();
                case CodelistValue _:
                    var codelistValue = (CodelistValue)this;
                    return codelistValue.GetCodelistValueDeleteUrl();
                case NameSpace _:
                    var nameSpace = (NameSpace)this;
                    return nameSpace.GetNameSpaceDeleteUrl();
                case Organization _:
                    var organization = (Organization)this;
                    return organization.GetOrganizationDeleteUrl();
                case ServiceAlert _:
                    var serviceAlert = (ServiceAlert)this;
                    return serviceAlert.GetServiceAlertDeleteUrl();
            }
            return "#";
        }

        public string CreateNewRegisterItemVersionUrl(/*string parentRegister, string registerOwner, string register, string itemOwner, string item, string itemClass*/)
        {
            if (register.parentRegister == null)
            {
                string url = "versjon/" + register.seoname + "/" + submitter.seoname + "/" + seoname + "/ny";

                if (this is Document) return "/dokument/" + url;
                if (this is CodelistValue) return "/kodeliste/" + url;
                if (this is Organization) return "/organisasjoner/" + url;
                if (this is Dataset) return "/dataset/" + url;
                if (this is EPSG) return "/epsg/" + url;
                if (this is NameSpace) return "/navnerom/" + url;
            }
            else
            {
                string url = "versjon/" + register.parentRegister.seoname + "/" + register.owner.seoname + "/" + register + "/" + submitter.seoname + "/" + seoname + "/ny";

                if (this is Document) return "/dokument/" + url;
                if (this is CodelistValue) return "/kodeliste/" + url;
                if (this is Organization) return "/organisasjoner/" + url;
                if (this is Dataset) return "/dataset/" + url;
                if (this is EPSG) return "/epsg/" + url;
                if (this is NameSpace) return "/navnerom/" + url;
            }
            return "#";
        }

        public string ItemsByOwnerUrl()
        {
            switch (this)
            {
                case Document document:
                    return document.ItemsByDocumentOwnerUrl();
                case Dataset dataset:
                    return dataset.ItemsByDatasetOwnerUrl();
            }
            return register.GetObjectUrl() + "/" + submitter.seoname;
        }
    }
}//end namespace Datamodell