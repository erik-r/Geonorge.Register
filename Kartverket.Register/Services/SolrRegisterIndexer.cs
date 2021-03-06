﻿using Kartverket.Register.Models;
using Kartverket.Register.Services.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.Register.Services
{
    public class SolrRegisterIndexer : RegisterIndexer
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Indexer _indexer;
        private readonly IndexDocumentCreator _indexDocumentCreator;
        private readonly SearchService _searchService;
        private RegisterDbContext db = new RegisterDbContext();

        public SolrRegisterIndexer(Indexer indexer, IndexDocumentCreator indexDocumentCreator)
        {
            _indexer = indexer;
            _indexDocumentCreator = indexDocumentCreator;
            _searchService = new SearchService(db);
        }

        public void RunIndexing()
        {
            RunSearch();
        }

        public void RunIndexingOn(string systemID)
        {

             _indexer.RemoveIndexDocument(systemID);

             SearchParameters parameters = new SearchParameters();
             parameters.Text = systemID;
             parameters.Limit = 1;
             parameters.Register = "Alle registre";
             parameters.IncludeObjektkatalog = false;
             SearchResult searchResult = _searchService.Search(parameters);

             if (searchResult != null && searchResult.Items.Count > 0) 
             { 
                 List<RegisterIndexDoc> indexDocs = _indexDocumentCreator.CreateIndexDocs(searchResult.Items);
                 _indexer.Index(indexDocs);
             }

        }

        private void RunSearch()
        {

            SearchResult searchResult = null;
            try
            {
                SearchParameters parameters = new SearchParameters();
                parameters.Text = "";
                parameters.Limit = 1000000;
                parameters.Register = "Alle registre";
                parameters.IncludeObjektkatalog = false; 
                searchResult = _searchService.Search(parameters);
                List<RegisterIndexDoc> indexDocs = _indexDocumentCreator.CreateIndexDocs(searchResult.Items);
                _indexer.Index(indexDocs);

            }
            catch (Exception exception)
            {
                Log.Error(exception);
               
            }
            
        }



        public void RunReIndexing()
        {
            _indexer.DeleteIndex();

            RunSearch();
        }
    }
}