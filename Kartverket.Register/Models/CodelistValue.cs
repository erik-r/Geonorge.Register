///////////////////////////////////////////////////////////
//  CodelistValue.cs
//  Implementation of the Class CodelistValue
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:34
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;



namespace Kartverket.Register.Models
{
	public class CodelistValue : RegisterItem {
        public CodelistValue() 
        {
            //this.subvalues = new HashSet<CodelistValue>();
            
        }
        public const string DataDirectory = "codelistValue/";
        [Display(Name = "Kodeverdi")]
        public string value { get; set; }
        //public virtual CodelistValue parent { get; set; }
        //public virtual ICollection<CodelistValue> subvalues { get; set; }

		

	}//end CodelistValue

}//end namespace Datamodell