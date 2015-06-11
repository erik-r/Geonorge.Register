﻿using Kartverket.Register.Helpers;
using Kartverket.Register.Models;
using Kartverket.Register.Services.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace Kartverket.Register.Controllers
{
    public class ApiRootController : ApiController
    {
        private RegisterDbContext db = new RegisterDbContext();

        private readonly ISearchService _searchService;

        public ApiRootController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// List top level registers. Use id in response to navigate.
        /// </summary>
        [Route("api/register")]
        [HttpGet]
        public IHttpActionResult GetRegisters()
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext); 
            var list = new List<Models.Api.Register>();
            foreach (var l in db.Registers.Include("owner").Include("manager").Where(w => w.parentRegisterId == null))
            {
                list.Add(ConvertRegister(l,urlHelper));
            }
            
            return Ok(list);
        }

        /// <summary>
        /// Gets register by name
        /// </summary>
        /// <param name="seoname">The search engine optimized name of the register</param>
        [Route("api/register/{seoname}")]
        [HttpGet]
        public IHttpActionResult GetRegisterByName(string seoname)
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
            
            var it = db.Registers.Where(w => w.seoname == seoname).FirstOrDefault();

            return Ok(ConvertRegisterAndNextLevel(it, urlHelper));
        }

        /// <summary>
        /// Gets codelist by systemid
        /// </summary>
        /// <param name="systemid">The uniqueidentifier for the register</param>
        [Route("api/kodelister/{systemid}")]
        [HttpGet]
        public IHttpActionResult GetRegisterBySystemId(string systemid)
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);

            var it = db.Registers.Where(w => w.systemId == new Guid(systemid)).FirstOrDefault();

            return Ok(ConvertRegisterAndNextLevel(it, urlHelper));
        }

        /// <summary>
        /// Gets register item by register- organization- and registeritem-name 
        /// </summary>
        /// <param name="seoname">The search engine optimized name of the register</param>
        /// <param name="orgseoname">The search engine optimized name of the organization</param>
        /// <param name="itemseoname">The search engine optimized name of the register item</param>
        [Route("api/register/{seoname}/{orgseoname}/{itemseoname}")]
        [HttpGet]
        public IHttpActionResult GetRegisterItemByName(string seoname, string orgseoname, string itemseoname)
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
            var it = db.Registers.Where(w => w.seoname == seoname).FirstOrDefault();
            var rit = db.RegisterItems.Where(w => w.seoname == itemseoname).FirstOrDefault();

            return Ok(ConvertRegisterItemDetails(it, rit, urlHelper));
        }

        /// <summary>
        /// List items for specific organization 
        /// </summary>
        /// <param name="name">The name of the organization</param>
        [Route("api/register/search/organisasjon/{name}")]
        [HttpGet]
        public IHttpActionResult SearchByOrganizationName(string name)
        {
            List<Kartverket.Register.Models.Api.Item> resultat = new List<Models.Api.Item>();

            SearchParameters parameters = new SearchParameters();
            parameters.Text = name;

            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext); 

            SearchResult searchResult = _searchService.Search(parameters);

            foreach (var it in searchResult.Items)
            {
                resultat.Add(Convert(it, urlHelper));
            }

            return Ok(resultat);
            
        }

        private Models.Api.Register ConvertRegister(Models.Register item, System.Web.Mvc.UrlHelper urlHelper)
        {
            var tmp = new Models.Api.Register
            {
                label = item.name,
                id = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Authority + "/api/register/" + item.seoname,              
                contentsummary = item.description,      
                //containedSubRegisters = new List<Models.Api.Register>()                            
            };
            if (item.owner != null) tmp.owner = item.owner.name;
            if (item.manager != null) tmp.manager = item.manager.name;
            

            return tmp;
        }

        private Models.Api.Register ConvertRegisterAndNextLevel(Models.Register item, System.Web.Mvc.UrlHelper urlHelper)
        {
            var tmp = ConvertRegister(item,urlHelper);
            if (item.items != null) tmp.containeditems = new List<Models.Api.Registeritem>();
            tmp.containedSubRegisters = new List<Models.Api.Register>();
            var queryResultParentRegister = from r in db.Registers
                                            where r.parentRegisterId == item.systemId
                                            select r;

            foreach (var reg in queryResultParentRegister)
            {
                tmp.containedSubRegisters.Add(ConvertRegister(reg, urlHelper));
            }

            foreach (var d in item.items)
            {
                tmp.containeditems.Add(ConvertRegisterItem(item, d, urlHelper));
            }
            
            return tmp;
        }

        private Models.Api.Registeritem ConvertRegisterItem(Models.Register reg, Models.RegisterItem item, System.Web.Mvc.UrlHelper urlHelper)
        {
            var tmp = new Models.Api.Registeritem();
            tmp.label = item.name;
           
                

            if (item.submitter != null) {
                tmp.id = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Authority + "/api/register/" + reg.seoname + "/" + item.submitter.seoname + "/" + item.seoname;
            }
            else {
                tmp.id = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Authority + "/api/register/" + reg.seoname + "/ikke-angitt/" + item.seoname;
            }
         
            if (item is EPSG)
            {

                var d = (EPSG)item;
                tmp.documentreference = "http://www.opengis.net/def/crs/EPSG/0/" + d.epsgcode;
                tmp.inspireRequirement = d.inspireRequirement.description;
                tmp.nationalRequirement = d.nationalRequirement.description;
                tmp.nationalSeasRequirement = d.nationalSeasRequirement != null ? d.nationalSeasRequirement.description : "";
                tmp.horizontalReferenceSystem = d.horizontalReferenceSystem;
                tmp.verticalReferenceSystem = d.verticalReferenceSystem;
                tmp.dimension = d.dimension != null ? d.dimension.description : "";
            }

            if (item is CodelistValue) 
            {
                var c = (CodelistValue)item;
                tmp.codevalue = c.value;
            }
            


            return tmp;
        }

        private Models.Api.Registeritem ConvertRegisterItemDetails(Models.Register reg, Models.RegisterItem item, System.Web.Mvc.UrlHelper urlHelper)
        {
            var tmp = new Models.Api.Registeritem();
            tmp.label = item.name;

            if (item.submitter != null)
            {
                tmp.id = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Authority + "/api/register/" + reg.seoname + "/" + item.submitter.seoname + "/" + item.seoname;
            }
            else
            {
                tmp.id = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Authority + "/api/register/" + reg.seoname + "/ikke-angitt/" + item.seoname;
            }
            if (item.status != null) tmp.status = item.status.description;
            if (item.description != null) tmp.description = item.description;

            if (item is Document)
            {
                tmp.itemclass = "Document";
                var d = (Document)item;
                tmp.documentreference = d.documentUrl;
            }
            else if (item is Dataset)
            {
                tmp.itemclass = "Dataset";
            }
            else if (item is Organization)
            {
                tmp.itemclass = "Organization";
                var d = (Organization)item;
                tmp.logo = d.logoFilename;
            }
            else if (item is NameSpace)
            {
                tmp.itemclass = "NameSpace";
                var d = (NameSpace)item;
                tmp.serviceUrl = d.serviceUrl;
            }
            else if (item is EPSG)
            {
                tmp.itemclass = "EPSG";
                var d = (EPSG)item;
                tmp.documentreference = "http://www.opengis.net/def/crs/EPSG/0/" + d.epsgcode;
                tmp.inspireRequirement = d.inspireRequirement.description;
                tmp.nationalRequirement = d.nationalRequirement.description;
                tmp.nationalSeasRequirement = d.nationalSeasRequirement !=null ? d.nationalSeasRequirement.description : "";
                tmp.horizontalReferenceSystem = d.horizontalReferenceSystem;
                tmp.verticalReferenceSystem = d.verticalReferenceSystem;
                tmp.dimension = d.dimension != null ? d.dimension.description : "";
            }
            else tmp.itemclass = "RegisterItem";


            return tmp;
        }

        private Models.Api.Item Convert(SearchResultItem searchitem, System.Web.Mvc.UrlHelper urlHelper)
        {
            var tmp= new Models.Api.Item
            {
                name = searchitem.RegisterItemName,
                description = searchitem.RegisterItemDescription,
                status = searchitem.RegisterItemStatus,
                updated = searchitem.RegisterItemUpdated,
                author = searchitem.DocumentOwner,
                showUrl =  searchitem.RegisteItemUrl,
                editUrl = null
                
            };
            
            return tmp;
        }
    }
}
