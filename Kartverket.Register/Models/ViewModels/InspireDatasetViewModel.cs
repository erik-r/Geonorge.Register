﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kartverket.Register.Models.ViewModels
{
    public class InspireDatasetViewModel : DatasetViewModel
    {
        [Display(Name = "Metadata:")]
        public string InspireDeliveryMetadataStatus { get; set; }
        public string InspireDeliveryMetadataNote { get; set; }
        public SelectList InspireDeliveryMetadataStatusSelectList { get; set; }

        [Display(Name = "Metadatatjeneste:")]
        public string InspireDeliveryMetadataServiceStatus { get; set; }
        public string InspireDeliveryMetadataServiceNote { get; set; }
        public SelectList InspireDeliveryMetadataServiceStatusSelectList { get; set; }

        [Display(Name = "Deling av data:")]
        public string InspireDeliveryDistributionStatus { get; set; }
        public string InspireDeliveryDistributionNote { get; set; }
        public SelectList InspireDeliveryDistributionStatusSelectList { get; set; }

        [Display(Name = "WMS:")]
        public string InspireDeliveryWmsStatus { get; set; }
        public string InspireDeliveryWmsNote { get; set; }
        public SelectList InspireDeliveryWmsStatusSelectList { get; set; }

        [Display(Name = "Nedlastingstjeneste WFS:")]
        public string InspireDeliveryWfsStatus { get; set; }
        public string InspireDeliveryWfsNote { get; set; }
        public SelectList InspireDeliveryWfsStatusSelectList { get; set; }

        [Display(Name = "Nedlastingstjeneste Atom-feed:")]
        public string InspireDeliveryAtomFeedStatus { get; set; }
        public string InspireDeliveryAtomFeedNote { get; set; }
        public SelectList InspireDeliveryAtomFeedStatusSelectList { get; set; }

        [Display(Name = "Nedlastingstjeneste WFS eller Atom-feed:")]
        public string InspireDeliveryWfsOrAtomStatus { get; set; }
        public string InspireDeliveryWfsOrAtomNote { get; set; }
        public SelectList InspireDeliveryWfsOrAtomStatusSelectList { get; set; }

        [Display(Name = "Harmoniserte data:")]
        public string InspireDeliveryHarmonizedDataStatus { get; set; }
        public string InspireDeliveryHarmonizedDataNote { get; set; }
        public SelectList InspireDeliveryHarmonizedDataStatusSelectList { get; set; }

        [Display(Name = "Spatial data service:")]
        public string InspireDeliverySpatialDataServiceStatus { get; set; }
        public string InspireDeliverySpatialDataServiceNote { get; set; }
        public SelectList InspireDeliverySpatialDataServiceStatusSelectList { get; set; }

        public void Update(InspireDataset inspireDataset)
        {
            UpdateDataset(inspireDataset);
        }

    }
}