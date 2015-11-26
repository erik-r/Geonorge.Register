///////////////////////////////////////////////////////////
//  Dataset.cs
//  Implementation of the Class Dataset
//  Generated by Enterprise Architect
//  Created on:      24-nov-2014 22:43:36
//  Original author: Tor Kjetil
///////////////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Kartverket.Register.Models
{
	public class Dataset : RegisterItem {

        //public int Id { get; set; }

        [Display(Name = "Uuid")]
        public string Uuid { get; set; }

        [ForeignKey("datasetowner")]
        public Guid datasetownerId { get; set; }
        public virtual Organization datasetowner { get; set; }

        [Display(Name = "Merknad")]
        public string Notes { get; set; }

        [Display(Name = "Produktark url")]
        public string ProductSheetUrl { get; set; }

        [Display(Name = "Presentasjonsregler url")]
        public string PresentationRulesUrl { get; set; }

        [Display(Name = "Produktspesifikasjon url")]
        public string ProductSpecificationUrl { get; set; }

        [Display(Name = "Metadata url")]
        public string MetadataUrl { get; set; }

        [Display(Name = "Distribusjonsformat")]
        public string DistributionFormat { get; set; }

        [Display(Name = "Distribusjon url")]
        public string DistributionUrl { get; set; }

        [Display(Name = "Distribusjonsområde")]
        public string DistributionArea { get; set; }

        [Display(Name = "WMS url")]
        public string WmsUrl { get; set; }

        [Display(Name = "Tema:")]
        [ForeignKey("theme")]
        public string ThemeGroupId { get; set; }        
        public virtual DOKTheme theme { get; set; }

        [Display(Name = "Miniatyrbilde")]
        public string datasetthumbnail { get; set; }

        [ForeignKey("dokStatus")]
        [Display(Name = "DOK-status:")]
        public string dokStatusId { get; set; }
        public virtual DokStatus dokStatus { get; set; }

        [Display(Name = "DOK-status godkjent:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dokStatusDateAccepted { get; set; }

        public string DatasetType { get; set; }

        public virtual List<CoverageDataset> Coverage{get; set;}
        
       
                   
	}//end Dataset

}//end namespace Datamodell