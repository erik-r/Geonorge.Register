﻿using Kartverket.Register.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kartverket.Register.Services.RegisterItem
{
    public class RegisterItemService : IRegisterItemService
    {
        private readonly RegisterDbContext _dbContext;

        public RegisterItemService(RegisterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SetNarrowerItems(List<Guid> narrowerList, CodelistValue codelistValue)
        {
            if (codelistValue.narrowerItems != null)
            {
                bool skalSlette = true;
                CodelistValue kodeSomSkalSlettes = null;
                List<CodelistValue> koderSomSkalSlettes = new List<CodelistValue>();

                foreach (CodelistValue narrower in codelistValue.narrowerItems)
                {
                    if (narrowerList != null)
                    {
                        foreach (Guid idNewNarrower in narrowerList)
                        {
                            if (idNewNarrower == narrower.systemId)
                            {
                                skalSlette = false;
                            }
                        }
                    }
                    kodeSomSkalSlettes = narrower;

                    if (skalSlette == true)
                    {
                        CodelistValue NarrowerItemsBroader = (CodelistValue)getItemById(kodeSomSkalSlettes.systemId);
                        if (NarrowerItemsBroader == kodeSomSkalSlettes)
                        {
                            NarrowerItemsBroader.broaderItemId = null;
                        }
                        koderSomSkalSlettes.Add(kodeSomSkalSlettes);
                    }
                    skalSlette = true;
                }
                foreach (CodelistValue item in koderSomSkalSlettes)
                {
                    codelistValue.narrowerItems.Remove(item);
                }
            }

            if (narrowerList != null)
            {
                foreach (Guid narrowerId in narrowerList)
                {
                    CodelistValue narrowerItem = _dbContext.CodelistValues.Find(narrowerId);
                    codelistValue.narrowerItems.Add(narrowerItem);
                    narrowerItem.broaderItemId = codelistValue.systemId;
                    narrowerItem.modified = DateTime.Now;
                }
            }
        }

        public void SetBroaderItem(Guid broader, CodelistValue codelistValue)
        {
            if (codelistValue.broaderItemId != null)
            {
                CodelistValue originalBroaderItem = (CodelistValue)codelistValue.broaderItem;
                originalBroaderItem.narrowerItems.Remove(codelistValue);
            }
            codelistValue.broaderItemId = broader;
            CodelistValue broaderItem = (CodelistValue)getItemById(broader);
            broaderItem.narrowerItems.Add(codelistValue);
        }

        public void SetBroaderItem(CodelistValue originalCodelistValue)
        {
            if (originalCodelistValue.broaderItemId != null)
            {
                CodelistValue originalBroaderItem = (CodelistValue)originalCodelistValue.broaderItem;
                originalBroaderItem.narrowerItems.Remove(originalCodelistValue);
            }
            originalCodelistValue.broaderItemId = null;
        }

        public void RemoveBroaderAndNarrower(CodelistValue codelistValue)
        {
            foreach (CodelistValue narrower in codelistValue.narrowerItems)
            {
                narrower.broaderItemId = null;
            }
            codelistValue.narrowerItems.Clear();

            if (codelistValue.broaderItemId != null)
            {
                CodelistValue broaderItem = (CodelistValue)codelistValue.broaderItem;   
                broaderItem.narrowerItems.Remove(codelistValue);
                codelistValue.broaderItemId = null;
            }
        }

        private Kartverket.Register.Models.RegisterItem getItemById(Guid id)
        {
            var queryresult = from ri in _dbContext.RegisterItems
                              where ri.systemId == id
                              select ri;

            Kartverket.Register.Models.RegisterItem item = queryresult.FirstOrDefault();
            return item;
        }

        //public void SetNarrowerItems(List<Guid> narrowerItems, CodelistValue codelistValue)
        //{
        //    foreach (Guid narrowerId in narrowerItems)
        //    {
        //        codelistValue
        //    }
        //}

        //public void SetBroaderItem(Guid broader, CodelistValue codelistValue)
        //{

        //}
    }
}