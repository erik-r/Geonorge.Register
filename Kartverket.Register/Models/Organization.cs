///////////////////////////////////////////////////////////
//  Organization1.cs
//  Implementation of the Class Organization
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:49
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Resources;



namespace Kartverket.Register.Models
{
	public class Organization : RegisterItem {
        // logos will be stored in this directory
        public const string DataDirectory = "organizations/";

        [Display(Name = "Organization_Number", ResourceType = typeof(UI))]
        public string number { get; set; }

        [Display(Name = "Organization_Logo", ResourceType = typeof(UI))]
        public string logoFilename { get; set; }

        [Display(Name = "Stor logo")]
        public string largeLogo { get; set; }

        public string contact { get; set; }

	}//end Organization

}//end namespace Datamodell