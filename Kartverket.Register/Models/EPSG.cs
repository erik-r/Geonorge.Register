///////////////////////////////////////////////////////////
//  EPSG.cs
//  Implementation of the Class EPSG
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:42
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace Kartverket.Register.Models
{
	public class EPSG : RegisterItem {

        [Display(Name = "EPSG:")]
        public string epsgcode { get; set; }
        [Display(Name = "SOSI referansesystem:")]
        public string sosiReferencesystem { get; set; }
        [Display(Name = "Ekstern referanser:")]
        public string externalReference { get; set; }
        [ForeignKey("inspireRequirement")]
        [Display(Name = "Inspirekrav:")]
        public string inspireRequirementId { get; set; }
        [Display(Name = "Inspirekrav:")]
        public virtual Requirement inspireRequirement { get; set; }
        [Display(Name = "Beskrivelse av krav:")]
        public string inspireRequirementDescription { get; set; }
        [ForeignKey("nationalRequirement")]
        [Display(Name = "Nasjonalt krav:")]
        public string nationalRequirementId { get; set; }
        [Display(Name = "Nasjonalt krav:")]
        public virtual Requirement nationalRequirement { get; set; }
        [Display(Name = "Beskrivelse av krav:")]
        public string nationalRequirementDescription { get; set; }
        [ForeignKey("nationalSeasRequirement")]
        [Display(Name = "Nasjonalt krav for havområder:")]
        public string nationalSeasRequirementId { get; set; }
        [Display(Name = "Nasjonalt krav for havområder:")]
        public virtual Requirement nationalSeasRequirement { get; set; }
        [Display(Name = "Beskrivelse av krav:")]
        public string nationalSeasRequirementDescription { get; set; }

	}//end EPSG

}//end namespace Datamodell