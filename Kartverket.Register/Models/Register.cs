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
        public virtual Version currentVersion { get; set; }
        public virtual ICollection<Version> replaces { get; set; }
        [ForeignKey("owner")]
        public Guid? ownerId { get; set; }
        public virtual Organization owner { get; set; }
        [ForeignKey("manager")]
        public Guid? managerId { get; set; }
        public virtual Organization manager { get; set; }
        [DisplayName("Navn")]
        public string name { get; set; }
        public string description { get; set; }
        [ForeignKey("status")]
        public string statusId { get; set; }
        public virtual Status status { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime dateSubmitted { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime modified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? dateAccepted { get; set; }
        public string containedItemClass { get; set; }
        public virtual ICollection<RegisterItem> items { get; set; }
        [ForeignKey("parentRegister")]
        public Guid? parentRegisterId { get; set; }
        public virtual Register parentRegister { get; set; }
        public virtual ICollection<Register> subregisters { get; set; }
        
        public string seoname { get; set; }



    }//end Register

}//end namespace Datamodell