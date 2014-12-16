///////////////////////////////////////////////////////////
//  Document.cs
//  Implementation of the Class Document
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:38
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
	public class Document : RegisterItem {
        // logos will be stored in this directory
        public const string DataDirectory = "documents/";

        [Display(Name = "Thumbnail:")]
        public string thumbnail { get; set; }
        [Display(Name = "Dokumenteier:")]
        [ForeignKey("documentowner")]
        public Guid? documentownerId { get; set; }
        public virtual Organization documentowner { get; set; }
        [Display(Name = "Dokument url:")]
        public string documentUrl { get; set; }

		

	}//end Document

}//end namespace Datamodell