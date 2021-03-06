﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.Register.Models.ViewModels
{
    public class DokMunicipalEdit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Themegroup { get; set; }
        public string Owner { get; set; }
        public Guid OwnerId { get; set; }
        public string NationalDokStatus { get; set; }
        public string Type { get; set; }
        public bool Confirmed { get; set; }
        public bool Coverage { get; set; }
        public string Note { get; set; }
        public bool Delete { get; set; }
        public Guid MunicipalityId { get; set; }

        public bool RegionalPlan { get; set; }
        public bool MunicipalSocialPlan { get; set; }
        public bool MunicipalLandUseElementPlan { get; set; }
        public bool ZoningPlanArea { get; set; }
        public bool ZoningPlanDetails { get; set; }
        public bool BuildingMatter { get; set; }
        public bool PartitionOff { get; set; }
        public bool EnvironmentalImpactAssessment { get; set; }
        public string SuitabilityAssessmentText { get; set; }

        public int NationalAssessmentRegionalPlan { get; set; }
        public int NationalAssessmentMunicipalSocialPlan { get; set; }
        public int NationalAssessmentMunicipalLandUseElementPlan { get; set; }
        public int NationalAssessmentZoningPlanArea { get; set; }
        public int NationalAssessmentZoningPlanDetails { get; set; }
        public int NationalAssessmentBuildingMatter { get; set; }
        public int NationalAssessmentPartitionOff { get; set; }
        public int NationalAssessmentEnvironmentalImpactAssessment { get; set; }

        public DokMunicipalEdit(Dataset dataset, RegisterItem municipality)
        {
            Id = dataset.systemId;
            Name = dataset.name;
            Themegroup = dataset.ThemeGroupId;
            Owner = dataset.datasetowner.name;
            OwnerId = dataset.datasetownerId;
            MunicipalityId = municipality.systemId;
            if (dataset.IsNationalDataset())
            {
                NationalDokStatus = dataset.dokStatus.description;
            }
            Type = dataset.DatasetType;
            Confirmed = dataset.GetCoverageConfirmedByUser(municipality.systemId);
            Coverage = dataset.GetCoverageByUser(municipality.systemId);
            Note = dataset.GetCoverageNoteByUser(municipality.systemId);
            Delete = false;
            RegionalPlan = dataset.GetCoverageRegionalPlaneByUser(municipality.systemId);
            MunicipalSocialPlan = dataset.GetCoverageMunicipalSocialPlanByUser(municipality.systemId);
            MunicipalLandUseElementPlan = dataset.GetCoverageMunicipalLandUseElementPlanByUser(municipality.systemId);
            ZoningPlanArea = dataset.GetCoverageZoningPlanAreaByUser(municipality.systemId);
            ZoningPlanDetails = dataset.GetCoverageZoningPlanDetailsByUser(municipality.systemId);
            BuildingMatter = dataset.GetCoverageBuildingMatterByUser(municipality.systemId);
            PartitionOff = dataset.GetCoveragePartitionOffByUser(municipality.systemId);
            EnvironmentalImpactAssessment = dataset.GetCoverageEenvironmentalImpactAssessmentByUser(municipality.systemId);
            SuitabilityAssessmentText = dataset.GetCoverageSuitabilityAssessmentTextByUser(municipality.systemId);
            NationalAssessmentRegionalPlan = dataset.RegionalPlan.GetValueOrDefault();
            NationalAssessmentMunicipalSocialPlan = dataset.MunicipalSocialPlan.GetValueOrDefault();
            NationalAssessmentMunicipalLandUseElementPlan = dataset.MunicipalLandUseElementPlan.GetValueOrDefault();
            NationalAssessmentZoningPlanArea = dataset.ZoningPlanArea.GetValueOrDefault();
            NationalAssessmentZoningPlanDetails = dataset.ZoningPlanDetails.GetValueOrDefault();
            NationalAssessmentBuildingMatter = dataset.BuildingMatter.GetValueOrDefault();
            NationalAssessmentPartitionOff = dataset.PartitionOff.GetValueOrDefault();
            NationalAssessmentEnvironmentalImpactAssessment = dataset.EenvironmentalImpactAssessment.GetValueOrDefault();
        }

        public DokMunicipalEdit() {

        }

        public bool IsMunicipalDataset() {
            return Type == "Kommunalt";
        }
    }

}