﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.Register.Models.Api
{
    public class Registeritem
    {
        
        public string id { get; set; }
        public string label { get; set; }
        public string itemclass { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string seoname { get; set; }
        public string owner { get; set; }
        public string versionName { get; set; }
        public int versionNumber { get; set; }

        public string logo { get; set; }
        public string documentreference { get; set; }

        public string epsgcode { get; set; }
        public string sosiReferencesystem { get; set; }
        public string inspireRequirement { get; set; }
        public string nationalRequirement { get; set; }
        public string nationalSeasRequirement { get; set; }

        public string verticalReferenceSystem { get; set; }
        public string horizontalReferenceSystem { get; set; }
        public string dimension { get; set; }

        public string codevalue { get; set; }

        public string serviceUrl { get; set; }

        public string broader { get; set; }

        public ICollection<string> narrower { get; set; }

        public string theme { get; set; }
        public string dokStatus { get; set; }

        public ICollection<Registeritem> versions { get; set; } 

        public DateTime lastUpdated { get; set; }
        public DateTime dateSubmitted { get; set; }
        public DateTime dateAccepted { get; set; }

        public Registeritem(Models.RegisterItem item, string baseUrl)
        {
            this.versions = new HashSet<Registeritem>();
            this.narrower = new HashSet<string>();

            id = baseUrl + item.GetObjectUrl();
            label = item.name;
            lastUpdated = item.modified;
            itemclass = item.register.containedItemClass;
            if (item.submitter != null)owner = item.submitter.name;
            if (item.status != null) status = item.status.description;
            if (item.description != null) description = item.description;
            if (item.versionName != null) versionName = item.description;
            versionNumber = item.versionNumber;
            versionName = item.versionName;
            dateSubmitted = item.dateSubmitted;
            dateAccepted = item.dateAccepted.GetValueOrDefault();
            itemclass = "RegisterItem";

            if (item is EPSG)
            {
                itemclass = "EPSG";
                var d = (EPSG)item;
                epsgcode = d.epsgcode;
                sosiReferencesystem = d.sosiReferencesystem;
                documentreference = "http://www.opengis.net/def/crs/EPSG/0/" + d.epsgcode;
                inspireRequirement = d.inspireRequirement.description;
                nationalRequirement = d.nationalRequirement.description;
                nationalSeasRequirement = d.nationalSeasRequirement != null ? d.nationalSeasRequirement.description : "";
                horizontalReferenceSystem = d.horizontalReferenceSystem;
                verticalReferenceSystem = d.verticalReferenceSystem;
                dimension = d.dimension != null ? d.dimension.description : "";
            }
            if (item is CodelistValue)
            {
                itemclass = "CodelistValue";
                var c = (CodelistValue)item;
                codevalue = c.value;
                if (c.broaderItemId != null)
                    broader = baseUrl + c.broaderItem.GetObjectUrl(); 
                foreach (var codelistvalue in c.narrowerItems)
                {
                    narrower.Add(baseUrl + codelistvalue.GetObjectUrl());
                }
            }
            if (item is Document)
            {
                itemclass = "Document";
                var d = (Document)item;
                if (d.documentowner != null) owner = d.documentowner.name;
                documentreference = d.documentUrl;
            }

            if (item is Dataset)
            {
                itemclass = "Dataset";
                var d = (Dataset)item;
                if (d.datasetowner != null) owner = d.datasetowner.name;
                if (d.theme != null) theme = d.theme.description;
                if (d.dokStatus != null) dokStatus = d.dokStatus.description;
            }
            if (item is NameSpace)
            {
                itemclass = "NameSpace";
                var n = (NameSpace)item;
                serviceUrl = n.serviceUrl;
            }

        }
    }
}