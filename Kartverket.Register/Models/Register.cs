///////////////////////////////////////////////////////////
//  Register.cs
//  Implementation of the Class Register
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:51
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;



namespace Kartverket.Register.Models
{
    public class Register
    {
        public Register()
        {
            this.items = new HashSet<RegisterItem>();
            this.subregisters = new HashSet<Register>();
            this.replaces = new HashSet<Version>();
        }
        [Key]
        public Guid systemId { get; set; }
        public virtual ICollection<Version> replaces { get; set; }
        [ForeignKey("owner")]
        public Guid? ownerId { get; set; }
        public virtual Organization owner { get; set; }
        [ForeignKey("manager")]
        public Guid? managerId { get; set; }
        public virtual Organization manager { get; set; }
        [DisplayName("Navn")]
        public string name { get; set; }
        [Display(Name = "Beskrivelse")]
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

        [Required(ErrorMessage = "Det m� settes lovlig innhold for registeret")]
        [Display(Name = "Lovlig innhold")]
        public string containedItemClass { get; set; }

        public virtual ICollection<RegisterItem> items { get; set; }
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

        /// <summary>
        /// Gets Url to current object
        /// </summary>
        /// <returns>Url</returns>
        public virtual string GetObjectUrl()
        {
            if (parentRegisterId == null)
            {
                return "/register/" + seoname;
            }
            else
            {
                return "/subregister/" + parentRegister.seoname + "/" + owner.seoname + "/" + seoname;
            }
        }

        public bool IsServiceAlertRegister()
        {
            return systemId == Guid.Parse("0f428034-0b2d-4fb7-84ea-c547b872b418");
        }

        public bool IsOfTypeDataset()
        {
            if (containedItemClass == "Dataset")
            {
                return true;
            }
            return false;
        }

        public bool IsDokMunicipal()
        {
            return name == "Det offentlige kartgrunnlaget - Kommunalt";
        }

        public Guid GetSystemId()
        {
            if (systemId == null || systemId == Guid.Empty)
            {
                return Guid.NewGuid();
            }
            else
                return systemId;
        }

        public string GetDokMunicipalityUrl()
        {
            return "/register/det-offentlige-kartgrunnlaget-kommunalt";
        }

        public string GetObjectCreateUrl(string municipalityCode = null)
        {
            string url;
            if (parentRegister == null)
            {
                url = seoname + "/ny";
            }
            else {
                url = parentRegister.seoname + "/" + owner.seoname + "/" + name + "/ny";
            }

            if (containedItemClass == "Document") return "/dokument/" + url;
            else if (containedItemClass == "CodelistValue") return "/kodeliste/" + url;
            else if (containedItemClass == "Register") return "/subregister/" + url;
            else if (containedItemClass == "Organization") return "/organisasjoner/" + url;
            else if (containedItemClass == "EPSG") return "/epsg/" + url;
            else if (containedItemClass == "NameSpace") return "/navnerom/" + url;
            else if (containedItemClass == "ServiceAlert") return "/tjenestevarsler/" + url;
            else if (containedItemClass == "Dataset")
            {
                if (IsDokMunicipal())
                {
                    return "/dataset/" + seoname + "/" + municipalityCode + "/ny";
                }
                else {
                    return "/dataset/" + url;
                }
            }
            else {
                return "#";
            }
        }

        public string GetEditObjectUrl() {
            if (parentRegister == null)
            {
                return "/rediger/" + name;
            }
            else
            {
                return "/subregister/" + parentRegister.seoname + "/" + owner.seoname + "/" + name + "/rediger";
            }
        }

        public string GetDeleteObjectUrl()
        {
            if (parentRegister == null)
            {
                return "/slett/" + name;
            }
            else
            {
                return "/subregister/" + parentRegister.seoname + "/" + owner.seoname + "/" + name + "/slett";
            }
        }

        //end Register

    }//end namespace Datamodell
}