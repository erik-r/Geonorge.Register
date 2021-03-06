﻿using System;
using System.Collections.Generic;
using System.Linq;
using SearchParameters = Kartverket.Register.Models.SearchParameters;
using SearchResult = Kartverket.Register.Models.SearchResult;
using Kartverket.Register.Models;
using System.Web.Configuration;
using Kartverket.Register.Helpers;
using Resources;

namespace Kartverket.Register.Services.Search
{
    public class SearchService : ISearchService
    {
        private readonly RegisterDbContext _dbContext;

        public SearchService(RegisterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Models.Register Search(Models.Register register, string text)
        {
            string role = HtmlHelperExtensions.GetSecurityClaim("role");
            string user = HtmlHelperExtensions.GetSecurityClaim("organization");

            List<Models.RegisterItem> registerItems = new List<Models.RegisterItem>();
            List<Models.Register> subregisters = new List<Models.Register>();

            if (register.containedItemClass == "Organization")
            {
                var queryResults = (from o in _dbContext.Organizations
                                    where o.name.Contains(text)
                                    || o.description.Contains(text)
                                    || o.shortname.Contains(text)
                                    || o.Translations.Any(oo => oo.Name.Contains(text))
                                    || o.Translations.Any(oo => oo.Description.Contains(text))
                                    select o);

                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        Organization organisasjon = item;
                        registerItems.Add(organisasjon);
                    }
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            if (register.containedItemClass == "ServiceAlert")
            {
                var queryResults = (from o in _dbContext.ServiceAlerts
                                    where o.name.Contains(text)
                                    || o.Translations.Any(oo => oo.Name.Contains(text))
                                    select o);

                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        ServiceAlert serviceAlert = item;
                        registerItems.Add(serviceAlert);
                    }
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            else if (register.containedItemClass == "Document")
            {
                var queryResults = (from d in _dbContext.Documents
                                    where (d.register.name.Contains(register.name)
                                    || d.register.seoname.Contains(register.seoname))
                                    && (d.name.Contains(text)
                                    || d.description.Contains(text)
                                    || d.documentowner.name.Contains(text)
                                    || d.Translations.Any(dd => dd.Name.Contains(text))
                                    || d.Translations.Any(dd => dd.Description.Contains(text))
                                    )
                                    select d);

                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        if (item.isCurrentVersion())
                        {
                            if ((item.statusId != "Submitted") || HtmlHelperExtensions.accessRegisterItem(item))
                            {
                                Document document = item;
                                registerItems.Add(document);
                            }
                        }
                    }
                    register.items = registerItems;
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            else if (register.containedItemClass == "NameSpace")
            {
                var queryResults = (from d in _dbContext.NameSpases
                                    where (d.register.name.Contains(register.name) || d.register.seoname.Contains(register.seoname))
                                    && (d.name.Contains(text)
                                    || d.description.Contains(text)
                                    || d.Translations.Any(oo => oo.Name.Contains(text))
                                    || d.Translations.Any(oo => oo.Description.Contains(text))
                                    )
                                    select d);

                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        NameSpace nameSpace = item;
                        registerItems.Add(nameSpace);
                    }
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            else if (register.containedItemClass == "Dataset")
            {
                var queryResults = (from d in _dbContext.Datasets
                                    where (d.register.name.Contains(register.name) || d.register.seoname.Contains(register.seoname))
                                    && (d.name.Contains(text)
                                    || d.description.Contains(text)
                                    || d.datasetowner.name.Contains(text))
                                    || d.Translations.Any(dd => dd.Name.Contains(text))
                                    || d.Translations.Any(dd => dd.Description.Contains(text))
                                    select d);

                if (register.name == "DOK-statusregisteret")
                    queryResults = queryResults.Where(k => k.DatasetType == "Nasjonalt");


                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        Dataset dataset = item;
                        registerItems.Add(dataset);
                    }
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            else if (register.containedItemClass == "EPSG")
            {
                var queryResults = (from e in _dbContext.EPSGs
                                    where e.name.Contains(text)
                                    || e.description.Contains(text)
                                    || e.epsgcode.Contains(text)
                                    || e.Translations.Any(ee => ee.Name.Contains(text))
                                    || e.Translations.Any(ee => ee.Description.Contains(text))

                                    select e);

                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        EPSG epsg = item;
                        registerItems.Add(epsg);
                    }
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            else if (register.containedItemClass == "CodelistValue")
            {
                var queryResults = (from e in _dbContext.CodelistValues
                                    where e.register.systemId == register.systemId && (e.name.Contains(text)
                                    || e.description.Contains(text)
                                    || e.value.Contains(text)
                                    || e.Translations.Any(ee => ee.Name.Contains(text))
                                    || e.Translations.Any(ee => ee.Description.Contains(text))
                                    )
                                    select e);

                if (queryResults.Count() > 0)
                {
                    foreach (var item in queryResults.ToList())
                    {
                        CodelistValue code = item;
                        registerItems.Add(code);
                    }
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = register.subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }

            else if (register.containedItemClass == "Register")
            {
                //Finn alle subregistre
                List<Models.Register> allSubregisters = GetSubregisters(register);
                ICollection<Models.RegisterItem> searchResultItems = new List<Models.RegisterItem>();
                ICollection<Models.Register> searchResultSub = new List<Models.Register>();
                bool itemSearchResult = false;

                //Gå gjennom alle subregistre og finn ut hva de inneholder og om innholdet samsvarer med søkestrengen. 
                foreach (Models.Register sub in allSubregisters)
                {
                    if (sub.items.Count != 0)
                    {
                        foreach (var item in sub.items.ToList())
                        {
                            searchResultItems.Add(item);
                        }
                        sub.items.Clear();
                        foreach (Models.RegisterItem item in searchResultItems)
                        {
                            if (item.name.Contains(text) || (!string.IsNullOrWhiteSpace(item.description) && item.description.Contains(text))    
                                ||
                                item.NameTranslated().Contains(text) || (!string.IsNullOrWhiteSpace(item.DescriptionTranslated()) && item.DescriptionTranslated().Contains(text)))
                            {
                                if (item.register.containedItemClass == "Document" && (item.statusId != "Submitted") || item.submitter.seoname == user || role == "nd.metadata_admin")
                                {
                                    sub.items.Add(item);
                                    itemSearchResult = true;
                                }
                                else if(item.register.containedItemClass != "Document") {
                                    sub.items.Add(item);
                                    itemSearchResult = true;
                                }
                            }
                        }
                        if (itemSearchResult)
                        {
                            subregisters.Add(sub);
                        }
                        itemSearchResult = false;
                        searchResultItems.Clear();
                    }

                    if (sub.subregisters.Count != 0)
                    {
                        foreach (var subReg in sub.subregisters.ToList())
                        {
                            searchResultSub.Add(subReg);
                        }
                        sub.subregisters.Clear();
                        foreach (Models.Register reg in searchResultSub)
                        {
                            if (reg.name.Contains(text) || (!string.IsNullOrWhiteSpace(reg.description) && reg.description.Contains(text))  
                                ||
                                reg.NameTranslated().Contains(text) || (!string.IsNullOrWhiteSpace(reg.DescriptionTranslated()) && reg.DescriptionTranslated().Contains(text))
                            )
                            {
                                sub.subregisters.Add(reg);
                            }
                        }
                        searchResultItems.Clear();
                    }
                }

                //Gå igjennom alle subregistrene og finn de subregistrene som samsvarer med søkestrengen..
                List<Models.Register> moreSubregisters = new List<Models.Register>();
                foreach (var reg in allSubregisters)
                {
                    bool finnesFraFor = false;
                    if (reg.name.Contains(text) || (!string.IsNullOrWhiteSpace(reg.description) && reg.description.Contains(text))
                        ||
                        reg.NameTranslated().Contains(text) || (!string.IsNullOrWhiteSpace(reg.DescriptionTranslated()) && reg.DescriptionTranslated().Contains(text))
                    )
                    {
                        if (subregisters.Count() != 0)
                        {
                            foreach (Models.Register r in subregisters)
                            {
                                if (r == reg)
                                {
                                    finnesFraFor = true;
                                }
                                if (!finnesFraFor)
                                {
                                    moreSubregisters.Add(reg);
                                }
                                finnesFraFor = false;
                            }
                        }
                        else
                        {
                            moreSubregisters.Add(reg);
                        }
                    }
                }
                foreach (Models.Register reg in moreSubregisters)
                {
                    subregisters.Add(reg);
                }

                return new Models.Register
                {
                    systemId = register.systemId,
                    name = register.name,
                    containedItemClass = register.containedItemClass,
                    dateAccepted = register.dateAccepted,
                    dateSubmitted = register.dateSubmitted,
                    description = register.description,
                    items = registerItems,
                    manager = register.manager,
                    managerId = register.managerId,
                    modified = register.modified,
                    owner = register.owner,
                    ownerId = register.ownerId,
                    parentRegister = register.parentRegister,
                    parentRegisterId = register.parentRegisterId,
                    seoname = register.seoname,
                    status = register.status,
                    statusId = register.statusId,
                    subregisters = subregisters,
                    replaces = register.replaces,
                    targetNamespace = register.targetNamespace,
                    versioning = register.versioning,
                    versioningId = register.versioningId,
                    versionNumber = register.versionNumber
                };
            }
            else { return register; }
        }

        private List<Models.Register> GetSubregisters(Models.Register register)
        {
            List<Models.Register> subRegisterList = new List<Models.Register>();
            //Finn alle subregisterne
            var queryResult = from r in _dbContext.Registers
                              where r.parentRegister.name == register.name
                              select r;

            List<Models.Register> resultList = queryResult.ToList();

            foreach (Models.Register sub in resultList)
            {
                subRegisterList.Add(sub);
                if (sub.containedItemClass == "Register")
                {
                    List<Models.Register> nextLevelOfSubregisters = GetSubregisters(sub);
                    foreach (Models.Register nextLevel in nextLevelOfSubregisters)
                    {
                        subRegisterList.Add(nextLevel);
                    }
                }
            }
            return subRegisterList;
        }

        public SearchResult Search(SearchParameters parameters)
        {
            string role = HtmlHelperExtensions.GetSecurityClaim("role");
            string user = HtmlHelperExtensions.GetSecurityClaim("organization");
            string itemClass = "";

            if (parameters.Register != Shared.Search_AllRegisters)
            {
                var queryResultsRegister = from o in _dbContext.Registers
                                           where o.name == parameters.Register || o.seoname == parameters.Register
                                           select o.containedItemClass;

                itemClass = queryResultsRegister.FirstOrDefault();
            }


            if (itemClass == "Organization")
            {
                var queryResults = (from o in _dbContext.Organizations
                                    where o.name.Contains(parameters.Text)
                                    || o.description.Contains(parameters.Text)
                                    || o.shortname.Contains(parameters.Text)
                                    select new SearchResultItem
                                    {
                                        RegisterName = o.register.name,
                                        RegisterDescription = o.register.description,
                                        RegisterItemName = o.name,
                                        RegisterItemDescription = o.description,
                                        RegisterID = o.registerId,
                                        SystemID = o.systemId,
                                        Discriminator = o.register.containedItemClass,
                                        RegisterSeoname = o.register.seoname,
                                        RegisterItemSeoname = o.seoname,
                                        DocumentOwner = null,
                                        RegisterItemUpdated = o.modified,
                                        RegisterItemStatus = o.statusId,
                                        Submitter = o.submitter.seoname,
                                        Shortname = o.shortname,
                                        currentVersion = o.versioning.currentVersion,
                                    });

                int NumFound = queryResults.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in queryResults)
                {
                    var item = new SearchResultItem
                    {
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        RegisteItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.Submitter) + "/" + register.RegisterItemSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        currentVersion = register.currentVersion,
                    };

                    items.Add(item);
                }

                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }

            if (itemClass == "ServiceAlert")
            {
                var queryResults = (from o in _dbContext.ServiceAlerts
                                    where o.name.Contains(parameters.Text)
                                    select new SearchResultItem
                                    {
                                        RegisterName = o.register.name,
                                        RegisterDescription = o.register.description,
                                        RegisterItemName = o.name,
                                        RegisterItemDescription = o.description,
                                        RegisterID = o.registerId,
                                        SystemID = o.systemId,
                                        Discriminator = o.register.containedItemClass,
                                        RegisterSeoname = o.register.seoname,
                                        RegisterItemSeoname = o.seoname,
                                        DocumentOwner = null,
                                        RegisterItemUpdated = o.modified,
                                        RegisterItemStatus = o.statusId,
                                        Submitter = o.submitter.seoname,
                                        currentVersion = o.versioning.currentVersion,
                                    });

                int NumFound = queryResults.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in queryResults)
                {
                    var item = new SearchResultItem
                    {
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        RegisteItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.Submitter) + "/" + register.RegisterItemSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        currentVersion = register.currentVersion,
                    };

                    items.Add(item);
                }

                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }

            else if (itemClass == "Document")
            {
                var queryResults = (from d in _dbContext.Documents
                                    where (d.register.name.Contains(parameters.Register)
                                    || d.register.seoname.Contains(parameters.Register))
                                    && (d.name.Contains(parameters.Text)
                                    || d.description.Contains(parameters.Text)
                                    || d.documentowner.name.Contains(parameters.Text))
                                    select new SearchResultItem
                                    {
                                        RegisterName = d.register.name,
                                        RegisterDescription = d.register.description,
                                        RegisterItemName = d.name,
                                        RegisterItemDescription = d.description,
                                        RegisterID = d.registerId,
                                        SystemID = d.systemId,
                                        Discriminator = d.register.containedItemClass,
                                        RegisterSeoname = d.register.seoname,
                                        RegisterItemSeoname = d.seoname,
                                        DocumentOwner = d.documentowner.seoname,
                                        RegisterItemUpdated = d.modified,
                                        RegisterItemStatus = d.statusId,
                                        Submitter = d.submitter.seoname,
                                        currentVersion = d.versioning.currentVersion,
                                    });

                int NumFound = queryResults.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in queryResults)
                {
                    if ((register.RegisterItemStatus != "Submitted") || register.DocumentOwner == user || role == "nd.metadata_admin")
                    {

                        var item = new SearchResultItem
                        {
                            RegisterName = register.RegisterName,
                            RegisterDescription = register.RegisterDescription,
                            RegisterItemName = register.RegisterItemName,
                            RegisterItemDescription = register.RegisterItemDescription,
                            RegisterID = register.RegisterID,
                            SystemID = register.SystemID,
                            Discriminator = register.Discriminator,
                            RegisterSeoname = register.RegisterSeoname,
                            RegisterItemSeoname = register.RegisterItemSeoname,
                            DocumentOwner = register.DocumentOwner,
                            RegisterItemUpdated = register.RegisterItemUpdated,
                            RegisterItemStatus = register.RegisterItemStatus,
                            RegisteItemUrlDocument = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/versjoner/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.DocumentOwner) + "/" + register.RegisterItemSeoname,
                            Submitter = register.Submitter,
                            Shortname = register.Shortname,
                            currentVersion = register.currentVersion,
                        };

                        items.Add(item);
                    }
                }

                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }

            else if (itemClass == "Dataset")
            {
                var queryResults = (from d in _dbContext.Datasets
                                    where (d.register.name.Contains(parameters.Register) || d.register.seoname.Contains(parameters.Register))
                                    && (d.name.Contains(parameters.Text)
                                    || d.description.Contains(parameters.Text)
                                    || d.datasetowner.name.Contains(parameters.Text))
                                    select new SearchResultItem
                                    {
                                        RegisterName = d.register.name,
                                        RegisterDescription = d.register.description,
                                        RegisterItemName = d.name,
                                        RegisterItemDescription = d.description,
                                        RegisterID = d.registerId,
                                        SystemID = d.systemId,
                                        Discriminator = d.register.containedItemClass,
                                        RegisterSeoname = d.register.seoname,
                                        RegisterItemSeoname = d.seoname,
                                        DocumentOwner = null,
                                        RegisterItemUpdated = d.modified,
                                        RegisterItemStatus = d.statusId,
                                        Submitter = d.submitter.seoname,
                                        DatasetOwner = d.datasetowner.seoname,
                                        currentVersion = d.versioning.currentVersion
                                    });

                int NumFound = queryResults.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in queryResults)
                {
                    var item = new SearchResultItem
                    {
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        RegisteItemUrlDataset = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.DatasetOwner) + "/" + register.RegisterItemSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        DatasetOwner = register.DatasetOwner,
                        currentVersion = register.currentVersion
                    };

                    items.Add(item);
                }

                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };

            }


            else if (itemClass == "EPSG")
            {
                var queryResults = (from e in _dbContext.EPSGs
                                    where e.name.Contains(parameters.Text)
                                    || e.description.Contains(parameters.Text)
                                    || e.epsgcode.Contains(parameters.Text)
                                    select new SearchResultItem
                                    {
                                        RegisterName = e.register.name,
                                        RegisterDescription = e.register.description,
                                        RegisterItemName = e.name,
                                        RegisterItemDescription = e.description,
                                        RegisterID = e.registerId,
                                        SystemID = e.systemId,
                                        Discriminator = e.register.containedItemClass,
                                        RegisterSeoname = e.register.seoname,
                                        RegisterItemSeoname = e.seoname,
                                        DocumentOwner = null,
                                        RegisterItemUpdated = e.modified,
                                        RegisterItemStatus = e.statusId,
                                        Submitter = e.submitter.seoname,
                                        currentVersion = e.versioning.currentVersion,
                                    });

                int NumFound = queryResults.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in queryResults)
                {
                    var item = new SearchResultItem
                    {
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        RegisteItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.Submitter) + "/" + register.RegisterItemSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        currentVersion = register.currentVersion,
                    };

                    items.Add(item);
                }

                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }

            else if (itemClass == "NameSpace")
            {
                var queryResults = (from e in _dbContext.NameSpases
                                    where e.name.Contains(parameters.Text)
                                    || e.description.Contains(parameters.Text)
                                    select new SearchResultItem
                                    {
                                        RegisterName = e.register.name,
                                        RegisterDescription = e.register.description,
                                        RegisterItemName = e.name,
                                        RegisterItemDescription = e.description,
                                        RegisterID = e.registerId,
                                        SystemID = e.systemId,
                                        Discriminator = e.register.containedItemClass,
                                        RegisterSeoname = e.register.seoname,
                                        RegisterItemSeoname = e.seoname,
                                        DocumentOwner = null,
                                        RegisterItemUpdated = e.modified,
                                        RegisterItemStatus = e.statusId,
                                        Submitter = e.submitter.seoname,
                                        currentVersion = e.versioning.currentVersion,
                                    });

                int NumFound = queryResults.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in queryResults)
                {
                    var item = new SearchResultItem
                    {
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        RegisteItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.Submitter) + "/" + register.RegisterItemSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        currentVersion = register.currentVersion,
                    };

                    items.Add(item);
                }
                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }

            else if (itemClass == "Register")
            {
                List<SearchResultItem> searchResultItem = new List<SearchResultItem>();
                var queryResults =
                                    (from d in _dbContext.Registers
                                     where d.parentRegister.name == parameters.Register
                                     && d.parentRegister.containedItemClass == itemClass
                                     && (d.name.Contains(parameters.Text)
                                     || d.description.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = d.parentRegisterId,
                                         ParentRegisterName = d.parentRegister.name,
                                         ParentRegisterDescription = d.parentRegister.description,
                                         ParentRegisterSeoname = d.parentRegister.seoname,
                                         ParentregisterOwner = d.parentRegister.owner.seoname,
                                         RegisterName = d.name,
                                         RegisterDescription = d.description,
                                         RegisterItemName = null,
                                         RegisterItemDescription = null,
                                         RegisterID = d.systemId,
                                         SystemID = d.systemId,
                                         Discriminator = d.containedItemClass,
                                         RegisterSeoname = d.seoname,
                                         RegisterItemSeoname = null,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = d.modified,
                                         RegisterItemStatus = d.statusId,
                                         Submitter = null,
                                         Shortname = null,
                                         CodelistValue = null,
                                         currentVersion = d.versioning.currentVersion,

                                     }).Union(
                                    (from o in _dbContext.Organizations
                                     where o.register.parentRegister.name.Contains(parameters.Register) && (
                                        o.register.name.Contains(parameters.Text)
                                     || o.register.description.Contains(parameters.Text)
                                     || o.register.name.Contains(parameters.Text)
                                     || o.name.Contains(parameters.Text)
                                     || o.description.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = o.register.parentRegisterId,
                                         ParentRegisterName = o.register.parentRegister.name,
                                         ParentRegisterDescription = o.register.parentRegister.description,
                                         ParentRegisterSeoname = o.register.parentRegister.seoname,
                                         ParentregisterOwner = o.register.parentRegister.owner.seoname,
                                         RegisterName = o.register.name,
                                         RegisterDescription = o.register.description,
                                         RegisterItemName = o.name,
                                         RegisterItemDescription = o.description,
                                         RegisterID = o.registerId,
                                         SystemID = o.systemId,
                                         Discriminator = o.register.containedItemClass,
                                         RegisterSeoname = o.register.seoname,
                                         RegisterItemSeoname = o.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = o.modified,
                                         RegisterItemStatus = o.statusId,
                                         Submitter = o.submitter.seoname,
                                         Shortname = o.shortname,
                                         CodelistValue = null,
                                         currentVersion = o.versioning.currentVersion
                                     }).Union(
                                     (from o in _dbContext.NameSpases
                                      where o.register.parentRegister.name.Contains(parameters.Register) && (
                                         o.register.name.Contains(parameters.Text)
                                      || o.register.description.Contains(parameters.Text)
                                      || o.register.name.Contains(parameters.Text)
                                      || o.name.Contains(parameters.Text)
                                      || o.description.Contains(parameters.Text))
                                      select new SearchResultItem
                                      {
                                          ParentRegisterId = o.register.parentRegisterId,
                                          ParentRegisterName = o.register.parentRegister.name,
                                          ParentRegisterDescription = o.register.parentRegister.description,
                                          ParentRegisterSeoname = o.register.parentRegister.seoname,
                                          ParentregisterOwner = o.register.parentRegister.owner.seoname,
                                          RegisterName = o.register.name,
                                          RegisterDescription = o.register.description,
                                          RegisterItemName = o.name,
                                          RegisterItemDescription = o.description,
                                          RegisterID = o.registerId,
                                          SystemID = o.systemId,
                                          Discriminator = o.register.containedItemClass,
                                          RegisterSeoname = o.register.seoname,
                                          RegisterItemSeoname = o.seoname,
                                          DocumentOwner = null,
                                          DatasetOwner = null,
                                          RegisterItemUpdated = o.modified,
                                          RegisterItemStatus = o.statusId,
                                          Submitter = o.submitter.seoname,
                                          Shortname = null,
                                          CodelistValue = null,
                                          currentVersion = o.versioning.currentVersion
                                      }).Union(
                                    (from d in _dbContext.Datasets
                                     where d.register.parentRegister.name.Contains(parameters.Register) && (
                                        d.register.name.Contains(parameters.Text)
                                     || d.name.Contains(parameters.Text)
                                     || d.description.Contains(parameters.Text)
                                     || d.datasetowner.name.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = d.register.parentRegisterId,
                                         ParentRegisterName = d.register.parentRegister.name,
                                         ParentRegisterDescription = d.register.parentRegister.description,
                                         ParentRegisterSeoname = d.register.parentRegister.seoname,
                                         ParentregisterOwner = d.register.parentRegister.owner.seoname,
                                         RegisterName = d.register.name,
                                         RegisterDescription = d.register.description,
                                         RegisterItemName = d.name,
                                         RegisterItemDescription = d.description,
                                         RegisterID = d.registerId,
                                         SystemID = d.systemId,
                                         Discriminator = d.register.containedItemClass,
                                         RegisterSeoname = d.register.seoname,
                                         RegisterItemSeoname = d.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = d.datasetowner.seoname,
                                         RegisterItemUpdated = d.modified,
                                         RegisterItemStatus = d.statusId,
                                         Submitter = d.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = null,
                                         currentVersion = d.versioning.currentVersion,
                                     }).Union(
                                    (from d in _dbContext.CodelistValues
                                     where d.register.parentRegister.name.Contains(parameters.Register) && (
                                        d.register.name.Contains(parameters.Text)
                                     || d.name.Contains(parameters.Text)
                                     || d.description.Contains(parameters.Text)
                                     || d.value.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = d.register.parentRegisterId,
                                         ParentRegisterName = d.register.parentRegister.name,
                                         ParentRegisterDescription = d.register.parentRegister.description,
                                         ParentRegisterSeoname = d.register.parentRegister.seoname,
                                         ParentregisterOwner = d.register.parentRegister.owner.seoname,
                                         RegisterName = d.register.name,
                                         RegisterDescription = d.register.description,
                                         RegisterItemName = d.name,
                                         RegisterItemDescription = d.description,
                                         RegisterID = d.registerId,
                                         SystemID = d.systemId,
                                         Discriminator = d.register.containedItemClass,
                                         RegisterSeoname = d.register.seoname,
                                         RegisterItemSeoname = d.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = d.modified,
                                         RegisterItemStatus = d.statusId,
                                         Submitter = d.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = d.value,
                                         currentVersion = d.versioning.currentVersion
                                     }).Union(
                                    (from e in _dbContext.EPSGs
                                     where e.register.parentRegister.name.Contains(parameters.Register) && (
                                        e.register.name.Contains(parameters.Text)
                                    || e.name.Contains(parameters.Text)
                                    || e.description.Contains(parameters.Text)
                                    || e.epsgcode.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = e.register.parentRegisterId,
                                         ParentRegisterName = e.register.parentRegister.name,
                                         ParentRegisterDescription = e.register.parentRegister.description,
                                         ParentRegisterSeoname = e.register.parentRegister.seoname,
                                         ParentregisterOwner = e.register.parentRegister.owner.seoname,
                                         RegisterName = e.register.name,
                                         RegisterDescription = e.register.description,
                                         RegisterItemName = e.name,
                                         RegisterItemDescription = e.description,
                                         RegisterID = e.registerId,
                                         SystemID = e.systemId,
                                         Discriminator = e.register.containedItemClass,
                                         RegisterSeoname = e.register.seoname,
                                         RegisterItemSeoname = e.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = e.modified,
                                         RegisterItemStatus = e.statusId,
                                         Submitter = e.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = null,
                                         currentVersion = e.versioning.currentVersion,
                                     }).Union(
                                    (from o in _dbContext.ServiceAlerts
                                     where o.register.parentRegister.name.Contains(parameters.Register) && (
                                        o.register.name.Contains(parameters.Text)
                                     || o.register.description.Contains(parameters.Text)
                                     || o.register.name.Contains(parameters.Text)
                                     || o.name.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = o.register.parentRegisterId,
                                         ParentRegisterName = o.register.parentRegister.name,
                                         ParentRegisterDescription = o.register.parentRegister.description,
                                         ParentRegisterSeoname = o.register.parentRegister.seoname,
                                         ParentregisterOwner = o.register.parentRegister.owner.seoname,
                                         RegisterName = o.register.name,
                                         RegisterDescription = o.register.description,
                                         RegisterItemName = o.name,
                                         RegisterItemDescription = o.description,
                                         RegisterID = o.registerId,
                                         SystemID = o.systemId,
                                         Discriminator = o.register.containedItemClass,
                                         RegisterSeoname = o.register.seoname,
                                         RegisterItemSeoname = o.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = o.modified,
                                         RegisterItemStatus = o.statusId,
                                         Submitter = o.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = null,
                                         currentVersion = o.versioning.currentVersion
                                     })
                                  ))))));

                searchResultItem = queryResults.ToList();

                var documents = from d in _dbContext.Documents
                                where d.register.parentRegister.name.Contains(parameters.Register) && (
                                    d.register.name.Contains(parameters.Text)
                                || d.name.Contains(parameters.Text)
                                || d.description.Contains(parameters.Text)
                                || d.documentowner.name.Contains(parameters.Text))
                                select new SearchResultItem
                                {
                                    ParentRegisterId = d.register.parentRegisterId,
                                    ParentRegisterName = d.register.parentRegister.name,
                                    ParentRegisterDescription = d.register.parentRegister.description,
                                    ParentRegisterSeoname = d.register.parentRegister.seoname,
                                    ParentregisterOwner = d.register.parentRegister.owner.seoname,
                                    RegisterName = d.register.name,
                                    RegisterDescription = d.register.description,
                                    RegisterItemName = d.name,
                                    RegisterItemDescription = d.description,
                                    RegisterID = d.registerId,
                                    SystemID = d.systemId,
                                    Discriminator = d.register.containedItemClass,
                                    RegisterSeoname = d.register.seoname,
                                    RegisterItemSeoname = d.seoname,
                                    DocumentOwner = d.documentowner.name,
                                    DatasetOwner = null,
                                    RegisterItemUpdated = d.modified,
                                    RegisterItemStatus = d.statusId,
                                    Submitter = d.submitter.seoname,
                                    Shortname = null,
                                    CodelistValue = null,
                                    currentVersion = d.versioning.currentVersion
                                };

                foreach (var doc in documents)
                {
                    if ((doc.RegisterItemStatus != "Submitted") || doc.DocumentOwner == user || role == "nd.metadata_admin")
                    {
                        searchResultItem.Add(doc);
                    }
                }

                int NumFound = searchResultItem.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                //queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit);

                foreach (SearchResultItem register in searchResultItem)
                {
                    var item = new SearchResultItem
                    {
                        ParentRegisterName = register.ParentRegisterName,
                        ParentRegisterId = register.ParentRegisterId,
                        ParentRegisterDescription = register.ParentRegisterDescription,
                        ParentRegisterSeoname = register.ParentRegisterSeoname,
                        ParentregisterOwner = register.ParentregisterOwner,
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        DatasetOwner = register.DatasetOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        SubregisterUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "subregister/" + register.ParentRegisterSeoname + "/" + register.ParentregisterOwner + "/" + register.RegisterSeoname,
                        RegisteItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.Submitter) + "/" + register.RegisterItemSeoname,
                        RegisteItemUrlDocument = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/versjoner/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.DocumentOwner) + "/" + register.RegisterItemSeoname,
                        RegisteItemUrlDataset = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.DatasetOwner) + "/" + register.RegisterItemSeoname,
                        subregisterItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "subregister/" + register.ParentRegisterSeoname + "/" + register.ParentregisterOwner + "/" + register.RegisterSeoname + "/" + register.Submitter + "/" + register.RegisterItemSeoname,
                        ParentRegisterUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.ParentRegisterSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        CodelistValue = register.CodelistValue,
                        currentVersion = register.currentVersion,

                    };

                    items.Add(item);
                }

                return new SearchResult
                {
                    Items = searchResultItem,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }

            else
            {

                var culture = CultureHelper.GetCurrentCulture();
                List<SearchResultItem> searchResultItem = new List<SearchResultItem>();
                Guid systemIDSearch = new Guid();
                Guid.TryParse(parameters.Text, out systemIDSearch);

                var queryResult = (from d in _dbContext.Registers
                                   where d.parentRegister.containedItemClass == "Register"
                                   && (d.name.Contains(parameters.Text)
                                   || d.description.Contains(parameters.Text)
                                   || d.systemId.Equals(systemIDSearch)
                                   || d.Translations.Any(dd => dd.Name.Contains(parameters.Text))
                                   || d.Translations.Any(dd => dd.Description.Contains(parameters.Text))
                                   )
                                   select new SearchResultItem
                                   {
                                       ParentRegisterId = d.parentRegisterId,
                                       ParentRegisterName = d.parentRegister.name,
                                       ParentRegisterDescription = d.parentRegister.description,
                                       ParentRegisterSeoname = d.parentRegister.seoname,
                                       ParentregisterOwner = d.parentRegister.owner.seoname,
                                       RegisterName = d.name,
                                       RegisterDescription = d.description,
                                       RegisterItemName = null,
                                       RegisterItemNameEnglish = null,
                                       RegisterItemDescription = null,
                                       RegisterID = d.systemId,
                                       SystemID = d.systemId,
                                       Discriminator = d.containedItemClass,
                                       RegisterSeoname = d.seoname,
                                       RegisterItemSeoname = null,
                                       DocumentOwner = null,
                                       DatasetOwner = null,
                                       RegisterItemUpdated = d.modified,
                                       RegisterItemStatus = d.statusId,
                                       Submitter = null,
                                       Shortname = null,
                                       CodelistValue = null,
                                       ObjektkatalogUrl = null,
                                       Type = null,
                                       currentVersion = null

                                   }).Union(
                                    (from d in _dbContext.CodelistValues
                                     where d.register.name.Contains(parameters.Text)
                                     || d.name.Contains(parameters.Text)
                                     || d.description.Contains(parameters.Text)
                                     || d.value.Contains(parameters.Text)
                                     || d.systemId.Equals(systemIDSearch)
                                     || d.Translations.Any(dd => dd.Name.Contains(parameters.Text))
                                     || d.Translations.Any(dd => dd.Description.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = d.register.parentRegisterId,
                                         ParentRegisterName = d.register.parentRegister.name,
                                         ParentRegisterDescription = d.register.parentRegister.description,
                                         ParentRegisterSeoname = d.register.parentRegister.seoname,
                                         ParentregisterOwner = d.register.parentRegister.owner.seoname,
                                         RegisterName = d.register.name,
                                         RegisterDescription = d.register.description,
                                         RegisterItemName = d.Translations.Where(t => t.CultureName == culture).Count() > 0 ? d.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Name : d.name,
                                         RegisterItemNameEnglish = d.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).Count() > 0 ? d.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).FirstOrDefault().Name : d.name,
                                         RegisterItemDescription = d.Translations.Where(t => t.CultureName == culture).Count() > 0 ? d.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Description : d.description,
                                         RegisterID = d.registerId,
                                         SystemID = d.systemId,
                                         Discriminator = d.register.containedItemClass,
                                         RegisterSeoname = d.register.seoname,
                                         RegisterItemSeoname = d.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = d.modified,
                                         RegisterItemStatus = d.statusId,
                                         Submitter = d.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = d.value,
                                         ObjektkatalogUrl = null,
                                         Type = null,
                                         currentVersion = d.versioning.currentVersion
                                     }).Union(
                                    (from o in _dbContext.Organizations
                                     where o.register.name.Contains(parameters.Text)
                                     || o.register.description.Contains(parameters.Text)
                                     || o.register.name.Contains(parameters.Text)
                                     || o.name.Contains(parameters.Text)
                                     || o.description.Contains(parameters.Text)
                                     || o.systemId.Equals(systemIDSearch)
                                     || o.Translations.Any(oo => oo.Name.Contains(parameters.Text))
                                     || o.Translations.Any(oo => oo.Description.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = o.register.parentRegisterId,
                                         ParentRegisterName = o.register.parentRegister.name,
                                         ParentRegisterDescription = o.register.parentRegister.description,
                                         ParentRegisterSeoname = o.register.parentRegister.seoname,
                                         ParentregisterOwner = o.register.parentRegister.owner.seoname,
                                         RegisterName =  o.register.name,
                                         RegisterDescription = o.register.description,
                                         RegisterItemName = o.Translations.Where(t => t.CultureName == culture).Count() > 0 ? o.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Name : o.name,
                                         RegisterItemNameEnglish = o.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).Count() > 0 ? o.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).FirstOrDefault().Name : o.name,
                                         RegisterItemDescription = o.Translations.Where(t => t.CultureName == culture).Count() > 0 ? o.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Description : o.description,
                                         RegisterID = o.registerId,
                                         SystemID = o.systemId,
                                         Discriminator = o.register.containedItemClass,
                                         RegisterSeoname = o.register.seoname,
                                         RegisterItemSeoname = o.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = o.modified,
                                         RegisterItemStatus = o.statusId,
                                         Submitter = o.submitter.seoname,
                                         Shortname = o.shortname,
                                         CodelistValue = null,
                                         ObjektkatalogUrl = null,
                                         Type = null,
                                         currentVersion = o.versioning.currentVersion
                                     }).Union(
                                     (from o in _dbContext.NameSpases
                                      where o.register.name.Contains(parameters.Text)
                                      || o.register.description.Contains(parameters.Text)
                                      || o.register.name.Contains(parameters.Text)
                                      || o.name.Contains(parameters.Text)
                                      || o.description.Contains(parameters.Text)
                                      || o.systemId.Equals(systemIDSearch)
                                      select new SearchResultItem
                                      {
                                          ParentRegisterId = o.register.parentRegisterId,
                                          ParentRegisterName = o.register.parentRegister.name,
                                          ParentRegisterDescription = o.register.parentRegister.description,
                                          ParentRegisterSeoname = o.register.parentRegister.seoname,
                                          ParentregisterOwner = o.register.parentRegister.owner.seoname,
                                          RegisterName = o.register.name,
                                          RegisterDescription = o.register.description,
                                          RegisterItemName = o.name,
                                          RegisterItemNameEnglish = null,
                                          RegisterItemDescription = o.description,
                                          RegisterID = o.registerId,
                                          SystemID = o.systemId,
                                          Discriminator = o.register.containedItemClass,
                                          RegisterSeoname = o.register.seoname,
                                          RegisterItemSeoname = o.seoname,
                                          DocumentOwner = null,
                                          DatasetOwner = null,
                                          RegisterItemUpdated = o.modified,
                                          RegisterItemStatus = o.statusId,
                                          Submitter = o.submitter.seoname,
                                          Shortname = null,
                                          CodelistValue = null,
                                          ObjektkatalogUrl = null,
                                          Type = null,
                                          currentVersion = o.versioning.currentVersion
                                      }).Union(
                                    (from d in _dbContext.Datasets
                                     where d.register.name.Contains(parameters.Text)
                                     || d.name.Contains(parameters.Text)
                                     || d.description.Contains(parameters.Text)
                                     || d.datasetowner.name.Contains(parameters.Text)
                                     || d.systemId.Equals(systemIDSearch)
                                     || d.Translations.Any(dd => dd.Name.Contains(parameters.Text))
                                     || d.Translations.Any(dd => dd.Description.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = d.register.parentRegisterId,
                                         ParentRegisterName = d.register.parentRegister.name,
                                         ParentRegisterDescription = d.register.parentRegister.description,
                                         ParentRegisterSeoname = d.register.parentRegister.seoname,
                                         ParentregisterOwner = d.register.parentRegister.owner.seoname,
                                         RegisterName = d.register.name,
                                         RegisterDescription = d.register.description,
                                         RegisterItemName = d.Translations.Where(t => t.CultureName == culture).Count() > 0 ? d.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Name : d.name,
                                         RegisterItemNameEnglish = d.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).Count() > 0 ? d.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).FirstOrDefault().Name : d.name,
                                         RegisterItemDescription = d.Translations.Where(t => t.CultureName == culture).Count() > 0 ? d.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Description : d.description,
                                         RegisterID = d.registerId,
                                         SystemID = d.systemId,
                                         Discriminator = d.register.containedItemClass,
                                         RegisterSeoname = d.register.seoname,
                                         RegisterItemSeoname = d.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = d.datasetowner.seoname,
                                         RegisterItemUpdated = d.modified,
                                         RegisterItemStatus = d.statusId,
                                         Submitter = d.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = null,
                                         ObjektkatalogUrl = null,
                                         Type = null,
                                         currentVersion = d.versioning.currentVersion

                                     }).Union(
                                    (from e in _dbContext.EPSGs
                                     where e.register.name.Contains(parameters.Text)
                                    || e.name.Contains(parameters.Text)
                                    || e.description.Contains(parameters.Text)
                                    || e.epsgcode.Contains(parameters.Text)
                                    || e.systemId.Equals(systemIDSearch)
                                    || e.Translations.Any(ee => ee.Name.Contains(parameters.Text))
                                    || e.Translations.Any(ee => ee.Description.Contains(parameters.Text))
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = e.register.parentRegisterId,
                                         ParentRegisterName = e.register.parentRegister.name,
                                         ParentRegisterDescription = e.register.parentRegister.description,
                                         ParentRegisterSeoname = e.register.parentRegister.seoname,
                                         ParentregisterOwner = e.register.parentRegister.owner.seoname,
                                         RegisterName = e.register.name,
                                         RegisterDescription = e.register.description,
                                         RegisterItemName = e.Translations.Where(t => t.CultureName == culture).Count() > 0 ? e.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Name : e.name,
                                         RegisterItemNameEnglish = e.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).Count() > 0 ? e.Translations.Where(t => t.CultureName == Kartverket.Register.Models.Translations.Culture.EnglishCode).FirstOrDefault().Name : e.name,
                                         RegisterItemDescription = e.Translations.Where(t => t.CultureName == culture).Count() > 0 ? e.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Description : e.description,
                                         RegisterID = e.registerId,
                                         SystemID = e.systemId,
                                         Discriminator = e.register.containedItemClass,
                                         RegisterSeoname = e.register.seoname,
                                         RegisterItemSeoname = e.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = e.modified,
                                         RegisterItemStatus = e.statusId,
                                         Submitter = e.submitter.name,
                                         Shortname = null,
                                         CodelistValue = null,
                                         ObjektkatalogUrl = null,
                                         Type = null,
                                         currentVersion = e.versioning.currentVersion
                                     }).Union(
                                    (from o in _dbContext.ServiceAlerts
                                     where o.register.name.Contains(parameters.Text)
                                     || o.register.description.Contains(parameters.Text)
                                     || o.register.name.Contains(parameters.Text)
                                     || o.name.Contains(parameters.Text)
                                     select new SearchResultItem
                                     {
                                         ParentRegisterId = o.register.parentRegisterId,
                                         ParentRegisterName = o.register.parentRegister.name,
                                         ParentRegisterDescription = o.register.parentRegister.description,
                                         ParentRegisterSeoname = o.register.parentRegister.seoname,
                                         ParentregisterOwner = o.register.parentRegister.owner.seoname,
                                         RegisterName = o.register.name,
                                         RegisterDescription = o.register.description,
                                         RegisterItemName = o.name,
                                         RegisterItemNameEnglish = null,
                                         RegisterItemDescription = o.description,
                                         RegisterID = o.registerId,
                                         SystemID = o.systemId,
                                         Discriminator = o.register.containedItemClass,
                                         RegisterSeoname = o.register.seoname,
                                         RegisterItemSeoname = o.seoname,
                                         DocumentOwner = null,
                                         DatasetOwner = null,
                                         RegisterItemUpdated = o.modified,
                                         RegisterItemStatus = o.statusId,
                                         Submitter = o.submitter.seoname,
                                         Shortname = null,
                                         CodelistValue = null,
                                         ObjektkatalogUrl = null,
                                         Type = null,
                                         currentVersion = o.versioning.currentVersion
                                     })
                                  ))))));

                searchResultItem = queryResult.ToList();

                var documents = from d in _dbContext.Documents
                                where d.register.name.Contains(parameters.Text)
                                || d.name.Contains(parameters.Text)
                                || d.description.Contains(parameters.Text)
                                || d.documentowner.name.Contains(parameters.Text)
                                || d.systemId.Equals(systemIDSearch)
                                select new SearchResultItem
                                {
                                    ParentRegisterId = d.register.parentRegisterId,
                                    ParentRegisterName = d.register.parentRegister.name,
                                    ParentRegisterDescription = d.register.parentRegister.description,
                                    ParentRegisterSeoname = d.register.parentRegister.seoname,
                                    ParentregisterOwner = d.register.parentRegister.owner.seoname,
                                    RegisterName = d.register.name,
                                    RegisterDescription = d.register.description,
                                    RegisterItemName = d.name,
                                    RegisterItemNameEnglish = null,
                                    RegisterItemDescription = d.description,
                                    RegisterID = d.registerId,
                                    SystemID = d.systemId,
                                    Discriminator = d.register.containedItemClass,
                                    RegisterSeoname = d.register.seoname,
                                    RegisterItemSeoname = d.seoname,
                                    DocumentOwner = d.documentowner.seoname,
                                    DatasetOwner = null,
                                    RegisterItemUpdated = d.modified,
                                    RegisterItemStatus = d.statusId,
                                    Submitter = d.submitter.seoname,
                                    Shortname = null,
                                    CodelistValue = null,
                                    ObjektkatalogUrl = null,
                                    Type = null,
                                    currentVersion = d.versioning.currentVersion
                                };
                foreach (var doc in documents)
                {
                    if ((doc.RegisterItemStatus != "Submitted") || doc.DocumentOwner == user || role == "nd.metadata_admin")
                    {
                        searchResultItem.Add(doc);
                    }
                }

                IQueryable<SearchResultItem> queryResultsListObjektKat = null;

                if (parameters.IncludeObjektkatalog)
                {
                    System.Net.WebClient c = new System.Net.WebClient();
                    c.Encoding = System.Text.Encoding.UTF8;
                    var data = c.DownloadString(System.Web.Configuration.WebConfigurationManager.AppSettings["ObjektkatalogUrl"] + "api/search/?text=" + parameters.Text + "&limit=1000");
                    var response = Newtonsoft.Json.Linq.JObject.Parse(data);

                    var objectKats = response["Results"];

                    List<SearchResultItem> objList = new List<SearchResultItem>();

                    foreach (var obj in objectKats)
                    {

                        objList.Add(new SearchResultItem
                        {
                            RegisterName = "Objektregister",
                            RegisterItemName = obj["name"] != null ? obj["name"].ToString() : null,
                            RegisterItemNameEnglish = null,
                            RegisterItemDescription = obj["description"] != null ? obj["description"].ToString() : null,
                            Discriminator = "Objektregister",
                            ObjektkatalogUrl = obj["url"] != null ? obj["url"].ToString() : null,
                            Type = obj["type"] != null ? obj["type"].ToString() : null
                        });

                    }

                    var qObjList = objList.AsQueryable();

                    queryResultsListObjektKat =
                    (from o in qObjList
                     select new SearchResultItem
                     {
                         ParentRegisterId = null,
                         ParentRegisterName = null,
                         ParentRegisterDescription = null,
                         ParentRegisterSeoname = null,
                         ParentregisterOwner = null,
                         RegisterName = o.RegisterName,
                         RegisterDescription = null,
                         RegisterItemName = o.RegisterItemName,
                         RegisterItemNameEnglish = null,
                         RegisterItemDescription = o.RegisterItemDescription,
                         RegisterID = new Guid(),
                         SystemID = new Guid(),
                         Discriminator = o.Discriminator,
                         RegisterSeoname = null,
                         RegisterItemSeoname = null,
                         DocumentOwner = null,
                         DatasetOwner = null,
                         RegisterItemUpdated = new System.DateTime(),
                         RegisterItemStatus = null,
                         Submitter = null,
                         Shortname = null,
                         CodelistValue = null,
                         ObjektkatalogUrl = o.ObjektkatalogUrl,
                         Type = o.Type,
                         currentVersion = o.currentVersion
                     }
                    );

                }

                var queryResultsListAll = queryResultsListObjektKat != null ? searchResultItem.Concat(queryResultsListObjektKat.ToList()).ToList() : searchResultItem.ToList();
                var queryResults = queryResultsListAll;

                int NumFound = queryResultsListAll.Count();
                List<SearchResultItem> items = new List<SearchResultItem>();
                int skip = parameters.Offset;
                skip = skip - 1;
                queryResults = queryResults.OrderBy(ri => ri.RegisterItemName).Skip(skip).Take(parameters.Limit).ToList();

                foreach (SearchResultItem register in queryResults)
                {
                    var item = new SearchResultItem
                    {
                        ParentRegisterName = register.ParentRegisterName,
                        ParentRegisterId = register.ParentRegisterId,
                        ParentRegisterDescription = register.ParentRegisterDescription,
                        ParentRegisterSeoname = register.ParentRegisterSeoname,
                        ParentregisterOwner = register.ParentregisterOwner,
                        RegisterName = register.RegisterName,
                        RegisterDescription = register.RegisterDescription,
                        RegisterItemName = register.RegisterItemName,
                        RegisterItemNameEnglish = register.RegisterItemNameEnglish,
                        RegisterItemDescription = register.RegisterItemDescription,
                        RegisterID = register.RegisterID,
                        SystemID = register.SystemID,
                        Discriminator = register.Discriminator,
                        RegisterSeoname = register.RegisterSeoname,
                        RegisterItemSeoname = register.RegisterItemSeoname,
                        DocumentOwner = register.DocumentOwner,
                        RegisterItemUpdated = register.RegisterItemUpdated,
                        RegisterItemStatus = register.RegisterItemStatus,
                        SubregisterUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "subregister/" + register.ParentRegisterSeoname + "/" + register.ParentregisterOwner + "/" + register.RegisterSeoname,
                        RegisteItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.Submitter) + "/" + register.RegisterItemSeoname,
                        RegisteItemUrlDocument = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/versjoner/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.DocumentOwner) + "/" + register.RegisterItemSeoname,
                        RegisteItemUrlDataset = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.RegisterSeoname + "/" + RegisterUrls.MakeSeoFriendlyString(register.DatasetOwner) + "/" + register.RegisterItemSeoname,
                        subregisterItemUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "subregister/" + register.ParentRegisterSeoname + "/" + register.ParentregisterOwner + "/" + register.RegisterSeoname + "/" + register.Submitter + "/" + register.RegisterItemSeoname,
                        ParentRegisterUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "register/" + register.ParentRegisterSeoname,
                        Submitter = register.Submitter,
                        Shortname = register.Shortname,
                        CodelistValue = register.CodelistValue,
                        ObjektkatalogUrl = register.ObjektkatalogUrl,
                        Type = register.Type,
                        currentVersion = register.currentVersion
                    };

                    items.Add(item);
                }

                return new SearchResult
                {
                    Items = items,
                    Limit = parameters.Limit,
                    Offset = parameters.Offset,
                    NumFound = NumFound
                };
            }
            //return new SearchResult();
        }
    }
}