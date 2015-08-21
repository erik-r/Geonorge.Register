///////////////////////////////////////////////////////////
//  RegisterItem.cs
//  Implementation of the Class RegisterItem
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:53
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Routing;



namespace Kartverket.Register.Models
{
    public abstract class RegisterItem
    {
        public RegisterItem()
        {
            //this.replaces = new HashSet<Version>();
        }
        [Key]
        public Guid systemId { get; set; }
        //public virtual ICollection<Version> replaces { get; set; }
        
        [Display(Name = "Navn:")]
        public string name { get; set; }
        
        [Display(Name = "Beskrivelse:")]
        public string description { get; set; }
        
        [ForeignKey("submitter")]
        [Display(Name = "Innsender:")]
        public Guid? submitterId { get; set; }
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

        [Display(Name = "Dato ikke godkjent:")]
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
        public Guid? versioningId { get; set; }
        public virtual Version versioning { get; set; }

        [Display(Name = "Versjonsnummer:")]
        public int versionNumber { get; set; }

        [Display(Name = "Dokument url:")]
        public string documentUrl { get; set; }

        [Display(Name = "Godkjennongsdokument:")]
        public string approvalDocument { get; set; }

        [Display(Name = "Godkjenningsreferanse:")]
        public string approvalReference { get; set; }
        
        [Display(Name = "Godkjent")]
        public bool? Accepted { get; set; }



    //end RegisterItem
    }
}//end namespace Datamodell