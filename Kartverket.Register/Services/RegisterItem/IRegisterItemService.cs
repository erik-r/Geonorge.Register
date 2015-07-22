﻿using Kartverket.Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kartverket.Register.Services.RegisterItem
{
    public interface IRegisterItemService
    {
        void SetNarrowerItems(List<Guid> narrowerItems, CodelistValue codelistValue);
        void SetBroaderItem(Guid broader, CodelistValue codelistValue);
        void SetBroaderItem(CodelistValue codelistValue);
        void RemoveBroaderAndNarrower(CodelistValue codelistValue);
        Kartverket.Register.Models.RegisterItem getCurrentRegisterItem(string parentregister, string register, string name);
        Models.Version GetVersionGroup(Guid? versioningId);
        Models.RegisterItem GetRegisterItemByVersionNr(string parentRegister, string register, string item, int? vnr);
        List<Models.RegisterItem> GetAllVersionsOfDocument(Guid versjonsGruppeId);
    }
}
