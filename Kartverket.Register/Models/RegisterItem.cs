///////////////////////////////////////////////////////////
//  RegisterItem.cs
//  Implementation of the Class RegisterItem
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:53
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using Kartverket.Register.Helpers;
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

        [Display(Name = "Navn:")]
        public string name { get; set; }

        [Display(Name = "Beskrivelse:")]
        public string description { get; set; }

        [ForeignKey("submitter")]
        [Display(Name = "Innsender:")]
        public Guid submitterId { get; set; }
        public virtual Organization submitter { get; set; }

        [Display(Name = "Dato innsendt:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateSubmitted { get; set; }

        [Display(Name = "Dato endret:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime modified { get; set; }

        [ForeignKey("status")]
        [Display(Name = "Status:")]
        public string statusId { get; set; }
        public virtual Status status { get; set; }

        [Display(Name = "Dato godkjent:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateAccepted { get; set; }

        [Display(Name = "Dato Utkast:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateNotAccepted { get; set; }

        [Display(Name = "Dato erstattet:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateSuperseded { get; set; }

        [Display(Name = "Dato utg�tt:")]
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

        [Display(Name = "Versjons ID:")]
        public int versionNumber { get; set; }

        [Display(Name = "Utgave:")]
        public string versionName { get; set; }

        [Display(Name = "Dokument url:")]
        public string documentUrl { get; set; }

        [Display(Name = "Godkjenningstekst:")]
        public string approvalDocument { get; set; }

        [Display(Name = "Godkjenningsreferanse:")]
        public string approvalReference { get; set; }

        [Display(Name = "Godkjent")]
        public bool? Accepted { get; set; }

        [Display(Name = "UML-modell")]
        public string ApplicationSchema { get; set; }

        [Display(Name = "GML-applikasjonsskjema")]
        public string GMLApplicationSchema { get; set; }

        //end RegisterItem


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

        public virtual string GetObjectEditUrl()
        {
            if (this is Document)
            {
                Document document = (Document)this;
                return document.GetDocumentEditUrl();
            }
            else if (this is Dataset)
            {
                Dataset dataset = (Dataset)this;
                return dataset.GetDatasetEditUrl();
            }
            else if (this is EPSG)
            {
                EPSG epsg = (EPSG)this;
                return epsg.GetEPSGEditUrl();
            }
            else if (this is CodelistValue)
            {
                CodelistValue codelistValue = (CodelistValue)this;
                return codelistValue.GetCodelistValueEditUrl();
            }
            else if (this is NameSpace)
            {
                NameSpace nameSpace = (NameSpace)this;
                return nameSpace.GetNameSpaceEditUrl();
            }
            else if (this is Organization)
            {
                Organization organization = (Organization)this;
                return organization.GetOrganizationEditUrl();
            }
            else if (this is ServiceAlert)
            {
                ServiceAlert serviceAlert = (ServiceAlert)this;
                return serviceAlert.GetServiceAlertEditUrl();
            }
            return "#";
        }

        public virtual string GetObjectDeleteUrl()
        {
            if (this is Document)
            {
                Document document = (Document)this;
                return document.GetDocumentDeleteUrl();
            }
            else if (this is Dataset)
            {
                Dataset dataset = (Dataset)this;
                return dataset.GetDatasetDeleteUrl();
            }
            else if (this is EPSG)
            {
                EPSG epsg = (EPSG)this;
                return epsg.GetEPSGDeleteUrl();
            }
            else if (this is CodelistValue)
            {
                CodelistValue codelistValue = (CodelistValue)this;
                return codelistValue.GetCodelistValueDeleteUrl();
            }
            else if (this is NameSpace)
            {
                NameSpace nameSpace = (NameSpace)this;
                return nameSpace.GetNameSpaceDeleteUrl();
            }
            else if (this is Organization)
            {
                Organization organization = (Organization)this;
                return organization.GetOrganizationDeleteUrl();
            }
            else if (this is ServiceAlert)
            {
                ServiceAlert serviceAlert = (ServiceAlert)this;
                return serviceAlert.GetServiceAlertDeleteUrl();
            }
            return "#";
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
    }
}//end namespace Datamodell