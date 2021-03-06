﻿
using System;
using System.Security.Claims;
using Kartverket.Register.Models;
using Kartverket.Register.Services.RegisterItem;
using System.Collections.Generic;
using Kartverket.Register.Models.ViewModels;
using Kartverket.Register.Services.Register;

namespace Kartverket.Register.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly RegisterDbContext db = new RegisterDbContext();
        private readonly IRegisterItemService _registerItemService;
        private readonly IRegisterService _registerService;
        private readonly IOrganizationService _organizationService;

        public AccessControlService()
        {
            _registerItemService = new RegisterItemService(db);
            _registerService = new RegisterService(db);
            _organizationService = new OrganizationsService(db);
        }

        public bool Access(object model)
        {
            if (IsAdmin())
            {
                return true;
            }
            if (model is Models.Register)
            {
                return AccessRegister(model);
            }
            if (model is RegisterV2ViewModel)
            {
                return AccessRegister((RegisterV2ViewModel)model);
            }
            if (model is Models.RegisterItem)
            {
                return accessRegisterItem((Models.RegisterItem)model);
            }
            if (model is RegisterItemV2ViewModel)
            {
                return accessRegisterItem((RegisterItemV2ViewModel)model);
            }
            return false;
        }

        private bool accessRegisterItem(Models.RegisterItem registerItem)
        {
            Organization user = _registerService.GetOrganizationByUserName();

            if (AccessRegister(registerItem.register))
            {
                if (registerItem is Document)
                {
                    Document document = (Document)registerItem;
                    return IsOwner(document.documentowner.name, user.name);
                }
                if (registerItem is Dataset)
                {
                    Dataset dataset = (Dataset)registerItem;
                    if (dataset.IsMunicipalDataset())
                    {
                        return IsOwner(dataset.datasetowner.name, user.name) || IsDokAdmin();
                    }
                }
                else {
                    return IsOwner(registerItem.submitter.name, user.name) || IsRegisterOwner(registerItem.register.owner.name, user.name) ;
                }
            }
            return false;
        }

        private bool IsRegisterOwner(string registerOwner, string userName)
        {
            return registerOwner == userName || registerOwner == userName;
        }

        private bool accessRegisterItem(RegisterItemV2ViewModel registerItemViewModel)
        {
            var user = _registerService.GetOrganizationByUserName();
            return AccessRegister(registerItemViewModel.Register) && IsOwner(registerItemViewModel.Owner.name, user.name);
        }

        private bool IsDokEditor()
        {
            List<string> roles = GetSecurityClaim("role");
            foreach (string role in roles)
            {
                if (role == "nd.dok_editor")
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAdmin()
        {
            List<string> roles = GetSecurityClaim("role");
            foreach (string role in roles)
            {
                if (role == "nd.metadata_admin")
                {
                    return true;
                }
            }
            return false;
        }

        private bool AccessRegister(object model)
        {
            Models.Register register = (Models.Register)model;
            Organization user = _registerService.GetOrganizationByUserName();
            if (register.accessId == 2)
            {
                if (IsEditor())
                {
                    if (register.ContainedItemClassIsCodelistValue())
                    {
                        return IsRegisterOwner(register.owner.name, user.name);
                    }
                    return true;
                }
            }
            else if (register.accessId == 4)
            {
                return IsMunicipalUser() || IsDokEditor() || IsDokAdmin();
            }
            return false;
        }

        private bool AccessRegister(RegisterV2ViewModel register)
        {
            Organization user = _registerService.GetOrganizationByUserName();
            if (register.Access == 2)
            {
                if (IsEditor())
                {
                    if (register.ContainedItemClassIsCodelistValue())
                    {
                        return IsRegisterOwner(register.Owner.name, user.name);
                    }
                    return true;
                }
            }
            else if (register.Access == 4)
            {
                return IsMunicipalUser() || IsDokEditor() || IsDokAdmin();
            }
            return false;
        }

        private bool IsEditor()
        {
            List<string> roles = GetSecurityClaim("role");
            foreach (string role in roles)
            {
                if (role == "nd.metadata" || role == "nd.metadata_editor")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsDokAdmin()
        {
            List<string> roles = GetSecurityClaim("role");
            foreach (string role in roles)
            {
                if (role == "nd.dok_admin")
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOwner(string owner, string user)
        {
            return (!string.IsNullOrEmpty(owner) && !string.IsNullOrEmpty(user)) && (owner.ToLower() == user.ToLower());
        }

        public bool IsMunicipalUser()
        {
            return GetMunicipalityCode() != null;
        }

        public Organization MunicipalUserOrganization()
        {
            return _registerService.GetOrganizationByUserName();
        }

        public string GetOrganizationNumber()
        {
            Claim orgnrClaim = ClaimsPrincipal.Current.FindFirst("orgnr");
            return orgnrClaim?.Value;
        }

        public CodelistValue GetMunicipality()
        {
            string municipalityCode = GetMunicipalityCode();
            if (municipalityCode == null)
            {
                return null;
            }

            List<CodelistValue> municipalities = _registerItemService.GetMunicipalityList();
            foreach (CodelistValue item in municipalities)
            {
                if (municipalityCode.Equals(item.value))
                {
                    return item;
                }
            }
            return null;
        }

        private string GetMunicipalityCode()
        {
            string organizationNumber = GetOrganizationNumber();
            if (organizationNumber == null)
            {
                return null;
            }

            var org = _organizationService.GetOrganizationByNumber(organizationNumber);
            return org != null ? org.MunicipalityCode : null;
        }

        public List<string> GetSecurityClaim(string type)
        {
            List<string> result = new List<string>();
            foreach (var claim in ClaimsPrincipal.Current.Claims)
            {
                if (claim.Type == type && !string.IsNullOrWhiteSpace(claim.Value))
                {
                    result.Add(claim.Value);
                }
            }

            // bad hack, must fix BAAT
            if (result.Count == 0 && type.Equals("organization") && result.Equals("Statens kartverk"))
            {
                result.Add("Kartverket");
            }

            return result;
        }

        public bool EditDOK(Dataset dataset)
        {
            if (dataset.IsNationalDataset())
            {
                if (IsAdmin())
                {
                    return true;
                }
                return false;
            }
            else
            {
                return Access(dataset);
            }
        }

        public bool AccessEditOrCreateDOKMunicipalBySelectedMunicipality(string municipalityCode)
        {
            return IsAdmin() || UserIsSelectedMunicipality(municipalityCode) || IsDokAdmin();
        }

        private bool UserIsSelectedMunicipality(string municipalityCode)
        {
            string currentUserMunicipalityCode = GetMunicipalityCode();
            if (municipalityCode == null || currentUserMunicipalityCode == null)
            {
                return false;
            }

            return currentUserMunicipalityCode == municipalityCode;
        }
    }
}