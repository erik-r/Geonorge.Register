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


namespace Kartverket.Register.Models
{
	public class Document : RegisterItem {

        public string thumbnail { get; set; }
        [ForeignKey("documentowner")]
        public Guid documentownerId { get; set; }
        public virtual Organization documentowner { get; set; }
        public string document { get; set; }

		

	}//end Document

}//end namespace Datamodell