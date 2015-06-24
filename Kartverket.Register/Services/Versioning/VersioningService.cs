﻿using Kartverket.Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.Register.Services.Versioning
{
    public class VersioningService : IVersioningService
    {
        private readonly RegisterDbContext _dbContext;

        public VersioningService(RegisterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public VersionsItem Versions(string registername, string parantRegister, string itemname)
        {
            // Finn versjonsgruppen
            var queryResultsRegisteritem = from ri in _dbContext.RegisterItems
                                           where ri.register.seoname == registername && ri.register.parentRegister.seoname == parantRegister
                                           && ri.seoname == itemname
                                           select ri.versioning;

            Kartverket.Register.Models.Version versjonsGruppe = queryResultsRegisteritem.FirstOrDefault();
            Guid? versjonsGruppeId = versjonsGruppe.systemId;
            Guid currentVersionId = versjonsGruppe.currentVersion;

            List<RegisterItem> suggestionsItems = new List<RegisterItem>();
            List<RegisterItem> historicalItems = new List<RegisterItem>();

            // finne Gjeldende versjon i versjonsgruppen
            var queryResults = from ri in _dbContext.RegisterItems
                               where ri.systemId == currentVersionId
                               select ri;

            RegisterItem currentVersion = queryResults.FirstOrDefault();

            // Finne alle versjoner som står som forslag
            queryResults = from ri in _dbContext.RegisterItems
                           where ri.register.seoname == registername
                           && ri.versioningId == versjonsGruppeId
                           && (ri.status.value == "Submitted"
                           || ri.status.value == "Proposal"
                           || ri.status.value == "InProgress"
                           || ri.status.value == "NotAccepted"
                           || ri.status.value == "Accepted"
                           || ri.status.value == "Experimental"
                           || ri.status.value == "Candidate")
                           select ri;

            foreach (RegisterItem item in queryResults)
            {
                suggestionsItems.Add(item);
            }


            //finne alle historiske versjoner
            var queryResultsHistorical = from ri in _dbContext.RegisterItems
                                         where ri.register.seoname == registername
                                          && ri.versioningId == currentVersion.versioningId
                                          && (ri.status.value == "Deprecated"
                                          || ri.status.value == "Superseded"
                                          || ri.status.value == "Retired")
                                         select ri;

            foreach (RegisterItem item in queryResultsHistorical)
            {
                historicalItems.Add(item);
            }


            return new VersionsItem
            {
                currentVersion = currentVersion,
                historical = historicalItems,
                suggestions = suggestionsItems
            };
        }
    }
}