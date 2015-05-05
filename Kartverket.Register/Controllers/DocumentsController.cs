﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kartverket.Register.Models;
using System.IO;
using Kartverket.Register.Helpers;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Kartverket.Register.Controllers
{
    [HandleError]
    public class DocumentsController : Controller
    {
        private RegisterDbContext db = new RegisterDbContext();

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Documents
        public ActionResult Index()
        {
            var registerItems = db.Documents.Include(d => d.register).Include(d => d.status).Include(d => d.submitter).Include(d => d.documentowner);
            return View(registerItems.ToList());
        }

        // GET: Documents/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        [Authorize]
        [Route("dokument/{registername}/ny")]
        public ActionResult Create(string registername)
        {
            
            ViewBag.registerSEO = registername;
            var queryResultsRegister = from o in db.Registers
                                       where o.seoname == registername
                                       select o.systemId;

            Guid regId = queryResultsRegister.First();
            Kartverket.Register.Models.Register register = db.Registers.Find(regId);
            ViewBag.registername = register.name;

            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            if (role == "nd.metadata_admin" || role == "nd.metadata" || role == "nd.metadata_editor")
            {
                return View();
            }
            return HttpNotFound();
            
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("dokument/{registername}/ny")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Document document, HttpPostedFileBase documentfile, HttpPostedFileBase thumbnail, string registername)
        {            
            ValidationName(document, registername);
       
            if (ModelState.IsValid)
            {
                var queryResultsRegister = from o in db.Registers
                                           where o.seoname == registername
                                           select o.systemId;

                Guid regId = queryResultsRegister.First();
                Kartverket.Register.Models.Register register = db.Registers.Find(regId);

                document.systemId = Guid.NewGuid();
                document.modified = DateTime.Now;
                document.dateSubmitted = DateTime.Now;
                document.registerId = regId;
                document.statusId = "Submitted";     

                if (document.name == null || document.name.Length == 0)
                {
                    document.name = "ikke angitt";
                    document.seoname = document.systemId.ToString();
                }

                if (document.description == null || document.description.Length == 0)
                {
                    document.description = "ikke angitt";
                }

                document.seoname = MakeSeoFriendlyString(document.name);               

                string url = System.Web.Configuration.WebConfigurationManager.AppSettings["RegistryUrl"] + "data/" + Document.DataDirectory;
           
                if (documentfile != null)
                {
                    document.documentUrl = url + SaveFileToDisk(documentfile, document.name, register.seoname);
                    if (document.documentUrl.Contains(".pdf"))
                    {
                        GenerateThumbnail(document, documentfile, url, registername);
                    }               
                }
                if (thumbnail != null)
                {
                    document.thumbnail = url + SaveFileToDisk(thumbnail, document.name, register.seoname);
                } 

                if (document.documentUrl == null) {
                    document.documentUrl = "ikke angitt";
                }

                string organizationLogin = GetSecurityClaim("organization");

                var queryResults = from o in db.Organizations
                                   where o.name == organizationLogin
                                   select o.systemId;

                Guid orgId = queryResults.First();
                Organization submitterOrganisasjon = db.Organizations.Find(orgId);

                document.submitterId = orgId;
                document.submitter = submitterOrganisasjon;
                document.documentowner = submitterOrganisasjon;
                document.documentownerId = orgId;

                db.Entry(document).State = EntityState.Modified;
                //db.SaveChanges();

                db.RegisterItems.Add(document);
                db.SaveChanges();

                return Redirect("/register/" + registername);
            }

            return View(document);
        }


        // GET: Documents/Edit/5
        [Authorize]
        [Route("dokument/{registername}/{organization}/{documentname}/rediger")]
        public ActionResult Edit(string registername, string documentname)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            var queryResults = from o in db.Documents
                                where o.seoname == documentname && o.register.seoname == registername
                                select o.systemId;

            Guid systId = queryResults.First();
            

            if (systId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(systId);
                
            if (document == null)
            {
                return HttpNotFound();
            }
                
            if (role == "nd.metadata_admin" || user.ToLower() == document.submitter.name.ToLower() || user.ToLower() == document.documentowner.name.ToLower())
            {
                Viewbags(document);
		        return View(document);
            }
            return HttpNotFound();
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("dokument/{registername}/{organization}/{documentname}/rediger")]
        public ActionResult Edit(Document document, string registername, string documentname, HttpPostedFileBase documentfile, HttpPostedFileBase thumbnail)
        {
            var queryResults = from o in db.Documents
                               where o.seoname == documentname && o.register.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();
            Document originalDocument = db.Documents.Find(systId);

            ValidationName(document, registername);

            if (ModelState.IsValid)
            {
                if (document.name != null) originalDocument.name = document.name; originalDocument.seoname = MakeSeoFriendlyString(originalDocument.name);
                if (document.description != null) originalDocument.description = document.description;
                if (document.documentownerId != null) originalDocument.documentownerId = document.documentownerId;
                if (document.documentUrl != null && document.documentUrl != originalDocument.documentUrl)
                {
                    originalDocument.documentUrl = document.documentUrl; 
                }
                if (document.statusId != null)
                {
                    originalDocument.statusId = document.statusId;
                    if (originalDocument.statusId != "Accepted" && document.statusId == "Accepted")
                    {
                        originalDocument.dateAccepted = DateTime.Now;
                    }
                    if (originalDocument.statusId == "Accepted" && document.statusId != "Accepted")
                    {
                        originalDocument.dateAccepted = null;
                    }
                }
                if (document.submitterId != null) originalDocument.submitterId = document.submitterId;

                string url = System.Web.Configuration.WebConfigurationManager.AppSettings["RegistryUrl"] + "data/" + Document.DataDirectory;
                
                if (documentfile != null)
                {
                    originalDocument.documentUrl = url + SaveFileToDisk(documentfile, originalDocument.name, originalDocument.register.seoname);
                    if (originalDocument.documentUrl.Contains(".pdf"))
                    {
                        GenerateThumbnail(document, documentfile, url, registername);
                        originalDocument.thumbnail = document.thumbnail;
                    }
                }

                if (thumbnail != null && document.thumbnail != originalDocument.thumbnail)
                {
                    originalDocument.thumbnail = url + SaveFileToDisk(thumbnail, originalDocument.name, originalDocument.register.seoname);
                }

                originalDocument.modified = DateTime.Now;
                db.Entry(originalDocument).State = EntityState.Modified;
                db.SaveChanges();
                Viewbags(document);

                return Redirect("/register/" + registername + "/" + originalDocument.documentowner.seoname + "/" + originalDocument.seoname);
            }
            Viewbags(document);
            return View(originalDocument);
        }

        // GET: Documents/Delete/5
        [Authorize]
        [Route("dokument/{registername}/{organization}/{documentname}/slett")]
        public ActionResult Delete(string registername, string documentname)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            var queryResults = from o in db.Documents
                                where o.seoname == documentname && o.register.seoname == registername 
                                select o.systemId;

            Guid systId = queryResults.First();

            if (systId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(systId);
            if (document == null)
            {
                return HttpNotFound();
            }
            if (role == "nd.metadata_admin" || user.ToLower() == document.submitter.name.ToLower() || user.ToLower() == document.documentowner.name.ToLower())
            {
                return View(document);
            }
                return HttpNotFound();
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("dokument/{registername}/{organization}/{documentname}/slett")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string registername, string documentname)
        {
            var queryResults = from o in db.Documents
                               where o.seoname == documentname && o.register.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();

            Document document = db.Documents.Find(systId);
            db.RegisterItems.Remove(document);
            db.SaveChanges();
            return Redirect("/register/" + registername);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private string FindRegisterOwner(string registername)
        {
            var queryResults = from o in db.Registers
                               where o.seoname == registername
                               select o.systemId;

            Guid regId = queryResults.First();
            Kartverket.Register.Models.Register register = db.Registers.Find(regId);
            string registerOwner = register.owner.name;
            return registerOwner;
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

        private static string MakeSeoFriendlyString(string input)
        {
            string encodedUrl = (input ?? "").ToLower();

            // replace & with and
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // remove characters
            encodedUrl = encodedUrl.Replace("'", "");

            // replace norwegian characters
            encodedUrl = encodedUrl.Replace("å", "a").Replace("æ", "ae").Replace("ø", "o");

            // remove invalid characters
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

            // remove duplicates
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // trim leading & trailing characters
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }

        private void GenerateThumbnail(Document document, HttpPostedFileBase documentfile, string url, string register)
        {
            string filtype;
            string seofilename;
            MakeSeoFriendlyDocumentName(documentfile, out filtype, out seofilename);

            string input = Path.Combine(Server.MapPath(Constants.DataDirectory + Document.DataDirectory), register + "_" + document.name + "_" + seofilename + "." + filtype); 
            string output = Path.Combine(Server.MapPath(Constants.DataDirectory + Document.DataDirectory), register + "_thumbnail_" + document.name + ".jpg");
            GhostscriptSharp.GhostscriptWrapper.GeneratePageThumb(input, output, 1, 150, 197);
            document.thumbnail = url + register + "_thumbnail_" + document.name + ".jpg";
        }


        private string SaveFileToDisk(HttpPostedFileBase file, string name, string register)
        {
            string filtype;
            string seofilename;
            MakeSeoFriendlyDocumentName(file, out filtype, out seofilename);

            string filename = register + "_" + name + "_" + seofilename + "." + filtype;                       
            var path = Path.Combine(Server.MapPath(Constants.DataDirectory + Document.DataDirectory), filename);
            file.SaveAs(path);
            return filename;
        }

        private static void MakeSeoFriendlyDocumentName(HttpPostedFileBase file, out string filtype, out string seofilename)
        {
            string[] documentfilename = file.FileName.Split('.');
            filtype = documentfilename.Last();
            seofilename = null;
            foreach (string item in documentfilename)
            {
                if (item == filtype)
                {
                    break;
                }
                seofilename += MakeSeoFriendlyString(item) + "_";
            }
        }

        private void ValidationName(Document document, string registername)
        {
            var queryResultsDataset = from o in db.Documents
                                      where o.name == document.name && o.systemId != document.systemId && o.register.seoname == registername
                                      select o.systemId;

            if (queryResultsDataset.Count() > 0)
            {
                ModelState.AddModelError("ErrorMessage", "Navnet finnes fra før!");
            }
        }

        private void Viewbags(Document document)
        {
            //ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", document.registerId);
            ViewBag.statusId = new SelectList(db.Statuses.OrderBy(s => s.description), "value", "description", document.statusId);
            ViewBag.submitterId = new SelectList(db.Organizations.OrderBy(s => s.name), "systemId", "name", document.submitterId);
            ViewBag.documentownerId = new SelectList(db.Organizations.OrderBy(s => s.name), "systemId", "name", document.documentownerId);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error("Error", filterContext.Exception);
        }

    }
}
