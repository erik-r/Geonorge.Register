///////////////////////////////////////////////////////////
//  Status.cs
//  Implementation of the Class Status
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:55
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;



namespace Kartverket.Register.Models {
	public class DokStatus {
        [Key]
        public string value { get; set; }
        public string description { get; set; }

	}//end Status

}//end namespace Datamodell