﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kartverket.Register.Models;
using System.Text.RegularExpressions;
using PagedList;
using System.Text;
using System.Xml.Linq;
using Kartverket.Register.Services.Versioning;
using Kartverket.Register.Models.ViewModels;
using Kartverket.Register.Services.Search;
using System.Web.Routing;
using Kartverket.Register.Services.Register;
using Kartverket.Register.Services.RegisterItem;

namespace Kartverket.Register.Controllers
{
    [HandleError]
    public class RegistersController : Controller
    {
        private RegisterDbContext db = new RegisterDbContext();

        private IVersioningService _versioningService;
        private IRegisterService _registerService;
        private ISearchService _searchService;
        private IRegisterItemService _registerItemService;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RegistersController()
        {
            _registerItemService = new RegisterItemService(db);
            _searchService = new SearchService(db);
            _versioningService = new VersioningService(db);
            _registerService = new RegisterService(db);
        }

        // GET: Registers
        public ActionResult Index()
        {
            setAccessRole();
            removeSessionSearchParams();

            return View(db.Registers.OrderBy(r => r.name).ToList());
        }

        // GET: Registers/Details/5
        [Route("register/{name}")]
        [Route("register/{name}.{format}")]
        [Route("register/{name}/{filterOrganization}")]
        public ActionResult Details(string name, string sorting, int? page, string format, FilterParameters filter)
        {
            string redirectToApiUrl = RedirectToApiIfFormatIsNotNull(format);
            if (!string.IsNullOrWhiteSpace(redirectToApiUrl)) return Redirect(redirectToApiUrl);                          

            Kartverket.Register.Models.Register register = _registerService.GetRegister(null, name);
            if (register == null) return HttpNotFound();
            
            if (!string.IsNullOrWhiteSpace(filter.text)) register = _searchService.Search(register, filter.text);
            register = _registerService.FilterRegisterItems(register, filter);

            ViewBag.search = filter.text;
            ViewBag.page = page;
            ViewBag.SortOrder = sorting;
            ViewBag.sorting = new SelectList(db.Sorting.ToList(), "value", "description");
            ViewBag.register = register.name;
            ViewBag.registerSEO = register.seoname;
            ViewBag.InspireRequirement = new SelectList(db.requirements, "value", "description", null);
            ViewBag.nationalRequirement = new SelectList(db.requirements, "value", "description", null);
            ViewBag.nationalSeaRequirement = new SelectList(db.requirements, "value", "description", null);

            return View(register);
        }

        [Route("register/versjoner/{registername}/{submitter}/{itemname}/{version}/no.{format}")]
        [Route("register/{registername}/{itemOwner}/{itemname}.{format}")]
        [Route("register/versjoner/{registername}/{submitter}/{itemname}/{version}/no")]
        [Route("register/{registername}/{itemOwner}/{itemname}")]
        public ActionResult DetailsRegisterItem(string registername, string itemowner, string itemname, int? version, string format)
        {
            string redirectToApiUrl = RedirectToApiIfFormatIsNotNull(format);
            if (!string.IsNullOrWhiteSpace(redirectToApiUrl)) return Redirect(redirectToApiUrl);

            Kartverket.Register.Models.RegisterItem registerItem; 
            if (version != null)
            {
                registerItem = _registerItemService.GetRegisterItemByVersionNr(registername, itemname, version);
                // TODO Flytte owner til RegisterItems
                Document document = db.Documents.Find(registerItem.systemId);
                ViewBag.documentOwner = document.documentowner.seoname;
            }
            else
            {
                registerItem = _registerItemService.GetCurrentRegisterItem(registername, itemname);
            }

            if (registerItem.register.containedItemClass == "Document")
            {
                Kartverket.Register.Models.Document document = db.Documents.Find(registerItem.systemId);
                ViewBag.documentOwner = document.documentowner.name;
            }

            if (registerItem.register.containedItemClass == "Dataset")
            {
                Kartverket.Register.Models.Dataset dataset = db.Datasets.Find(registerItem.systemId);
                ViewBag.datasetOwner = dataset.datasetowner.name;
            }
            return View(registerItem);
        }

        //TODO - API
        [Route("subregister/versjoner/{parentRegister}/{owner}/{registername}/{registerItemOwner}/{itemname}.{format}")]        
        [Route("register/versjoner/{registername}/{registerItemOwner}/{itemname}.{format}")]
        [Route("subregister/versjoner/{parentRegister}/{owner}/{registername}/{registerItemOwner}/{itemname}")]
        [Route("register/versjoner/{registername}/{registerItemOwner}/{itemname}")]
        public ActionResult DetailsRegisterItemVersions(string registername, string parentRegister, string itemname, string registerItemOwner, string format)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                return Redirect("/api/" + Request.FilePath);
            }
            else
            {
                format = _registerService.ContentNegotiation(ControllerContext);
                if (!string.IsNullOrWhiteSpace(format))
                {
                    return Redirect("/api/" + Request.FilePath + "." + format);
                }
            }
            _versioningService = new VersioningService(db);
            VersionsItem versionsItem = _versioningService.Versions(registername, parentRegister, itemname);
            RegisterItemVeiwModel model = new RegisterItemVeiwModel(versionsItem);

            ViewBag.registerItemOwner = registerItemOwner;
            return View(model);
        }

        // GET: Register/Create
        [Authorize]
        [Route("ny")]
        public ActionResult Create()
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");
            ViewBag.containedItemClass = new SelectList(db.ContainedItemClass.OrderBy(s => s.description), "value", "description", string.Empty);

            if (role == "nd.metadata_admin")
            {
                return View();
            }
            return HttpNotFound();
        }

        // POST: Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [Route("ny")]
        public ActionResult Create(Kartverket.Register.Models.Register register)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");
            if (role == "nd.metadata_admin")
            {
                ValidationName(register);

                if (ModelState.IsValid)
                {
                    register.systemId = Guid.NewGuid();
                    if (register.name == null)
                    {
                        register.name = "ikke angitt";
                    }

                    register.systemId = Guid.NewGuid();
                    register.modified = DateTime.Now;
                    register.dateSubmitted = DateTime.Now;
                    register.statusId = "Submitted";
                    register.seoname = Helpers.HtmlHelperExtensions.MakeSeoFriendlyString(register.name);
                    register.containedItemClass = register.containedItemClass;

                    db.Registers.Add(register);
                    db.SaveChanges();

                    string organizationLogin = GetSecurityClaim("organization");

                    var queryResults = from o in db.Organizations
                                       where o.name == organizationLogin
                                       select o.systemId;

                    Guid orgId = queryResults.FirstOrDefault();
                    Organization submitterOrganisasjon = db.Organizations.Find(orgId);

                    register.ownerId = submitterOrganisasjon.systemId;
                    register.managerId = submitterOrganisasjon.systemId;

                    db.Entry(register).State = EntityState.Modified;

                    db.SaveChanges();
                    return Redirect("/");
                }
                ViewBag.containedItemClass = new SelectList(db.ContainedItemClass.OrderBy(s => s.description), "value", "description", string.Empty);
                return View();
            }
            return HttpNotFound();
        }


        [Authorize]
        [Route("endre/{registername}")]
        public ActionResult Edit(string registername)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            Kartverket.Register.Models.Register register = _registerService.GetRegisterByName(registername);

            if (register == null)
            {
                return HttpNotFound();
            }

            if (role == "nd.metadata_admin")
            {
                Viewbags(register);
                return View(register);
            }
            return HttpNotFound();

        }


        // POST: Registers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("endre/{registername}")]
        [Authorize]
        public ActionResult Edit(Kartverket.Register.Models.Register register, string registername, string accessRegister)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            if (role == "nd.metadata_admin")
            {                
                ValidationName(register);
                Kartverket.Register.Models.Register originalRegister = new Models.Register();
                originalRegister = _registerService.GetRegister(null, registername);
               
                if (ModelState.IsValid)
                {
                    if (register.name != null) originalRegister.name = register.name; originalRegister.seoname = Helpers.HtmlHelperExtensions.MakeSeoFriendlyString(originalRegister.name);
                    if (register.description != null) originalRegister.description = register.description;
                    if (register.ownerId != null) originalRegister.ownerId = register.ownerId;
                    if (register.managerId != null) originalRegister.managerId = register.managerId;
                    if (register.targetNamespace != null) originalRegister.targetNamespace = register.targetNamespace;
                    originalRegister.accessId = register.accessId;
                    originalRegister.parentRegisterId = register.parentRegisterId;


                    originalRegister.modified = DateTime.Now;
                    if (register.statusId != null)
                    {
                        originalRegister.statusId = register.statusId;
                        if (originalRegister.statusId != "Valid" && register.statusId == "Valid")
                        {
                            originalRegister.dateAccepted = DateTime.Now;
                        }
                        if (originalRegister.statusId == "Valid" && register.statusId != "Valid")
                        {
                            originalRegister.dateAccepted = null;
                        }
                    }

                    db.Entry(originalRegister).State = EntityState.Modified;
                    db.SaveChanges();
                    Viewbags(register);

                    if (register.parentRegisterId != null)
                    {
                        return Redirect("/subregister/" + originalRegister.parentRegister.seoname + "/" + originalRegister.parentRegister.owner.seoname + "/" + originalRegister.seoname);
                    }
                    return Redirect("/register/" + originalRegister.seoname);
                }
                Viewbags(register);
                return View(originalRegister);
            }
            return HttpNotFound();
        }

        // GET: Registers/Delete/5
        [Authorize]
        [Route("slett/{registername}")]
        public ActionResult Delete(string registername)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            if (role == "nd.metadata_admin")
            {
                var queryResults = from o in db.Registers
                                   where o.seoname == registername
                                   select o.systemId;

                Guid systId = queryResults.FirstOrDefault();
                Kartverket.Register.Models.Register register = db.Registers.Find(systId);

                if (register == null)
                {
                    return HttpNotFound();
                }
                return View(register);
            }
            return HttpNotFound();
        }

        // POST: Registers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("slett/{registername}")]
        [Authorize]
        public ActionResult DeleteConfirmed(string registername)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            if (role == "nd.metadata_admin")
            {
                var queryResults = from o in db.Registers
                                   where o.seoname == registername
                                   select o.systemId;

                Guid systId = queryResults.FirstOrDefault();
                Kartverket.Register.Models.Register register = db.Registers.Find(systId);

                var queryResultsRegisterItem = ((from o in db.RegisterItems
                                                 where o.register.seoname == registername
                                                 || o.register.parentRegister.seoname == registername
                                                 select o.systemId).Union(
                                               from r in db.Registers
                                               where r.parentRegister.seoname == registername
                                               select r.systemId));

                if (queryResultsRegisterItem.Count() > 0)
                {
                    ModelState.AddModelError("ErrorMessageDelete", "Registeret kan ikke slettes fordi det inneholder elementer som må slettes først!");
                    return View(register);
                }
                else
                {
                    db.Registers.Remove(register);
                    db.SaveChanges();
                    return Redirect("/");
                }
            }
            return HttpNotFound();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string HasAccessToRegister()
        {
            string role = GetSecurityClaim("role");

            bool isAdmin = !string.IsNullOrWhiteSpace(role) && role.Equals("nd.metadata_admin");
            bool isEditor = !string.IsNullOrWhiteSpace(role) && role.Equals("nd.metadata"); //nd.metadata_editor

            if (isAdmin)
            {
                return "admin";
            }
            else if (isEditor)
            {
                return "editor";
            }
            else
            {
                return "guest";
            }
        }

        private string GetSecurityClaim(string type)
        {
            string result = null;
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {
                if (claim.Type == type && !string.IsNullOrWhiteSpace(claim.Value))
                {
                    result = claim.Value;
                    break;
                }
            }

            // bad hack, must fix BAAT
            if (!string.IsNullOrWhiteSpace(result) && type.Equals("organization") && result.Equals("Statens kartverk"))
            {
                result = "Kartverket";
            }

            return result;
        }

        private void setAccessRole()
        {
            string organization = GetSecurityClaim("organization");

            string role = HasAccessToRegister();
            if (role == "admin")
            {
                Session["role"] = "admin";
                Session["user"] = organization;
            }
            else if (role == "editor")
            {
                Session["role"] = "editor";
                Session["user"] = organization;
            }
            else
            {
                Session["role"] = "guest";
                Session["user"] = "guest";
            }


        }

        private void removeSessionSearchParams()
        {

            Session["sortingType"] = null;
            Session["text"] = null;
            Session["filterVertikalt"] = null;
            Session["filterHorisontalt"] = null;
            Session["InspireRequirement"] = null;
            Session["nationalRequirement"] = null;
            Session["nationalSeaRequirement"] = null;

        }

        private void ValidationName(Kartverket.Register.Models.Register register)
        {
            var queryResultsDataset = from o in db.Registers
                                      where o.name == register.name && o.systemId != register.systemId
                                      select o.systemId;

            if (queryResultsDataset.Count() > 0)
            {
                ModelState.AddModelError("ErrorMessage", "Navnet finnes fra før!");
            }
        }

        private void Viewbags(Kartverket.Register.Models.Register register)
        {
            //ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", document.registerId);
            ViewBag.statusId = new SelectList(db.Statuses.OrderBy(s => s.description), "value", "description", register.statusId);
            ViewBag.ownerId = new SelectList(db.Organizations.OrderBy(s => s.name), "systemId", "name", register.ownerId);
            ViewBag.parentRegisterId = new SelectList(db.Registers.Where(r => r.containedItemClass == "Register" && r.name != register.name).OrderBy(s => s.name), "systemId", "name", register.parentRegisterId);
            ViewBag.containedItemClass = new SelectList(db.ContainedItemClass.OrderBy(s => s.description), "value", "description", string.Empty);
        }

        private string RedirectToApiIfFormatIsNotNull(string format)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                return "/api/" + Request.FilePath;
            }
            else
            {
                format = _registerService.ContentNegotiation(ControllerContext);
                if (!string.IsNullOrWhiteSpace(format))
                {
                    return "/api/" + Request.FilePath + "." + format;
                }
            }
            return null;
        }

    }

}
