﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kartverket.Register.Models.Translations;
using Resources;

namespace Kartverket.Register.Models.ViewModels
{
    public class RegisterV2ViewModel
    {
        public Guid SystemId { get; set; }

        [Display(Name = "Owner", ResourceType = typeof(Registers))]
        public Organization Owner { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Registers))]
        public string Name { get; set; }

        public string Seoname { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Registers))]
        public string Description { get; set; }

        public Status Status { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateSubmitted { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Modified { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateAccepted { get; set; }

        [Display(Name = "ContainedItemClass", ResourceType = typeof(Registers))]
        public string ContainedItemClass { get; set; }

        public ICollection<RegisterItem> RegisterItems { get; set; }
        public ICollection<RegisterItemV2ViewModel> RegisterItemsV2 { get; set; }

        public Register ParentRegister { get; set; }

        public ICollection<Register> Subregisters { get; set; }

        public string TargetNamespace { get; set; }

        public Version Versioning { get; set; }

        public int VersionNumber { get; set; }

        public TranslationCollection<RegisterTranslation> Translations { get; set; }

        public string OrderBy { get; set; }
        public int Access { get; set; }


        public RegisterV2ViewModel(Register register)
        {
            if (register != null)
            {
                SystemId = register.systemId;
                Owner = register.owner;
                Name = register.NameTranslated();
                Description = register.DescriptionTranslated();
                Status = register.status;
                DateSubmitted = register.dateSubmitted;
                Modified = register.modified;
                DateAccepted = register.dateAccepted;
                ContainedItemClass = register.containedItemClass;
                RegisterItems = register.items;
                ParentRegister = register.parentRegister;
                Subregisters = register.subregisters;
                TargetNamespace = register.targetNamespace;
                Seoname = register.seoname;
                Versioning = register.versioning;
                VersionNumber = register.versionNumber;
                RegisterItemsV2 = GetRegisterItems(register.containedItemClass, register.RegisterItems);
                if (register.accessId != null) Access = register.accessId.Value;
            }

            if (register.IsServiceAlertRegister())
            {
                if (string.IsNullOrWhiteSpace(OrderBy))
                {
                    OrderBy = "dateSubmitted_desc";
                }
            }
        }

        private ICollection<RegisterItemV2ViewModel> GetRegisterItems(string containedItemClass, ICollection<RegisterItemV2> registerItems)
        {
            var registerItemsViewModel = new Collection<RegisterItemV2ViewModel>();
            switch (containedItemClass)
            {
                case "InspireDataset":
                    foreach (InspireDataset inspireDataset in registerItems)
                    {
                        registerItemsViewModel.Add(new InspireDatasetViewModel(inspireDataset));
                    }
                    break;
                case "GeodatalovDataset":
                    foreach (GeodatalovDataset geodatalovDataset in registerItems)
                    {
                        registerItemsViewModel.Add(new GeodatalovDatasetViewModel(geodatalovDataset));
                    }
                    break;
            }
            return registerItemsViewModel;
        }

        public bool IsDokMunicipal()
        {
            return Name == "Det offentlige kartgrunnlaget - Kommunalt";
        }

        public bool IsServiceAlertRegister()
        {
            return SystemId == Guid.Parse("0f428034-0b2d-4fb7-84ea-c547b872b418");
        }

        public bool ContainedItemClassIsOrganization()
        {
            return ContainedItemClass == "Organization";
        }

        public bool ContainedItemClassIsCodelistValue()
        {
            return ContainedItemClass == "CodelistValue";
        }

        public bool ContainedItemClassIsRegister()
        {
            return ContainedItemClass == "Register";
        }

        public bool ContainedItemClassIsDocument()
        {
            return ContainedItemClass == "Document";
        }

        public bool ContainedItemClassIsDataset()
        {
            return ContainedItemClass == "Dataset";
        }

        public bool ContainedItemClassIsEpsg()
        {
            return ContainedItemClass == "EPSG";
        }

        public bool ContainedItemClassIsNameSpace()
        {
            return ContainedItemClass == "NameSpace";
        }

        public bool ContainedItemClassIsServiceAlert()
        {
            return ContainedItemClass == "ServiceAlert";
        }

        public bool ContainedItemClassIsInspireDataset()
        {
            return ContainedItemClass == "InspireDataset";
        }

        public bool ContainedItemClassIsGeodatalovDataset()
        {
            return ContainedItemClass == "GeodatalovDataset";
        }

        public string GetObjectCreateUrl(string municipalityCode = null)
        {
            var url = ParentRegister == null
                ? Seoname + "/ny"
                : ParentRegister.seoname + "/" + Owner.seoname + "/" + Seoname + "/ny";

            if (ContainedItemClassIsDocument()) return "/dokument/" + url;
            if (ContainedItemClassIsCodelistValue()) return "/kodeliste/" + url;
            if (ContainedItemClassIsRegister()) return "/subregister/" + url;
            if (ContainedItemClassIsOrganization()) return "/organisasjoner/" + url;
            if (ContainedItemClassIsEpsg()) return "/epsg/" + url;
            if (ContainedItemClassIsNameSpace()) return "/navnerom/" + url;
            if (ContainedItemClassIsServiceAlert()) return "/tjenestevarsler/" + url;
            if (ContainedItemClassIsInspireDataset()) return "/inspire/" + url;
            if (ContainedItemClassIsGeodatalovDataset()) return "/geodatalov/" + url;
            if (ContainedItemClassIsDataset())
            {
                if (IsDokMunicipal()) return "/dataset/" + Seoname + "/" + municipalityCode + "/ny";
                return "/dataset/" + url;
            }
            return "#";
        }

        public string GetEditObjectUrl()
        {
            return ParentRegister == null
                ? "/rediger/" + Seoname
                : "/subregister/" + ParentRegister.seoname + "/" + Owner.seoname + "/" + Seoname + "/rediger";
        }

        public virtual string GetObjectUrl()
        {
            return ParentRegister == null
                ? "/register/" + Seoname
                : "/subregister/" + ParentRegister.seoname + "/" + Owner.seoname + "/" + Seoname;
        }
    }
}
