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
        public virtual Version currentVersion { get; set; }
        //public virtual ICollection<Version> replaces { get; set; }
        [Display(Name = "Navn:")]
        public string name { get; set; }
        [Display(Name = "Beskrivelse:")]
        public string description { get; set; }
        [ForeignKey("submitter")]
        [Display(Name = "Innsender:")]
        public Guid? submitterId { get; set; }
        public virtual Organization submitter { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime dateSubmitted { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime modified { get; set; }
        [ForeignKey("status")]
        [Display(Name = "Status:")]
        public string statusId { get; set; }
        public virtual Status status { get; set; }
        public DateTime? dateAccepted { get; set; }
        public virtual Register register { get; set; }
        [ForeignKey("register")]
        public Guid registerId { get; set; }
        public string seoname { get; set; }

    }
    //end RegisterItem

}//end namespace Datamodell