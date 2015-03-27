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
using System.Text;
using System.Xml.Linq;

namespace Kartverket.Register.Controllers
{
    public class SubregisterController : Controller
    {
        private RegisterDbContext db = new RegisterDbContext();

        // GET: Subregister
        public ActionResult Index()
        {
            var registers = db.Registers.Include(r => r.manager).Include(r => r.owner).Include(r => r.parentRegister).Include(r => r.status);
            return View(registers.ToList());
        }

        // GET: Registers/Details/5
        [Route("subregister/{registername}/{owner}/{subregister}")]
        public ActionResult Details(string registername, string owner, string subregister, string sorting, int? page, string export)
        {
            var queryResultsSubregister = from r in db.Registers
                                          where r.seoname == subregister && r.parentRegister.seoname == registername
                                          select r.systemId;

            if (queryResultsSubregister.Count() > 0)
            {
                Guid systId = queryResultsSubregister.First();
                Kartverket.Register.Models.Register register = db.Registers.Find(systId);
                ViewBag.page = page;
                ViewBag.SortOrder = sorting;
                ViewBag.sorting = new SelectList(db.Sorting.ToList(), "value", "description");
                ViewBag.register = register.parentRegister.name;
                ViewBag.registerSEO = register.parentRegister.seoname;
                ViewBag.ownerSEO = owner;
                ViewBag.subregister = subregister;

                if (register == null)
                {
                    return HttpNotFound();
                }
                if (!string.IsNullOrEmpty(export))
                {
                    return exportCodelist(register, export);
                }
                return View(register);
            }
            return HttpNotFound();
        }

        private ActionResult exportCodelist(Kartverket.Register.Models.Register register, string export)
        {
            if (export == "csv")
            {

                string text = "Kode; Initialverdi; Beskrivelse\n";

                foreach (CodelistValue item in register.items)
                {
                    string description = item.description;
                    string replaceWith = " ";
                    string removedBreaksDescription = description.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);

                    text += item.name + ";" + item.value + ";" + removedBreaksDescription + "\n";
                }

                byte[] data = Encoding.UTF8.GetBytes(text);

                return File(data, "text/csv", register.name + "_kodeliste.csv");
            }

            else if (export == "gml")
            {
                //var queryResult = from x in db.CodelistValues
                //                  select x.value;

                XNamespace ns = "http://www.opengis.net/gml/3.2";
                XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
                XNamespace gmlNs = "http://www.opengis.net/gml/3.2";

                XDocument xdoc = new XDocument
                    (new XElement(ns + "Dictionary", new XAttribute(XNamespace.Xmlns + "xsi", xsiNs),
                        new XAttribute(XNamespace.Xmlns + "gml", gmlNs),
                        new XAttribute(xsiNs + "schemaLocation", "http://www.opengis.net/gml/3.2 http://schemas.opengis.net/gml/3.2.1/gml.xsd"),
                        new XAttribute(gmlNs + "id", "FaseType"),
                        new XElement("description"),
                        new XElement("identifier",
                            new XAttribute("codeSpace", "http://rep.geointegrasjon.no/Sak/Faser/xml.schema/2012.01.31"), "FaseType"),

                        from k in db.CodelistValues.ToList()
                        where k.register.name == register.name && k.register.parentRegisterId == register.parentRegisterId
                        select new XElement("dictionaryEntry", new XElement("Definition", new XAttribute(gmlNs + "id", "_25" + k.name),
                          new XElement("description", k.description),
                          new XElement("identifier", new XAttribute("codeSpace", "http://rep.geointegrasjon.no/Sak/Faser/xml.schema/2012.01.31/FaseType"), k.value),
                          new XElement("name", k.name)
                    ))));

                byte[] data = Encoding.UTF8.GetBytes(xdoc.ToString());
                return File(data, "text/xml", register.name + "_kodeliste.xml");
            }
            return View(register);

        }

        [Route("subregister/{registername}/{owner}/{subregister}/{submitter}/{itemname}")]
        public ActionResult DetailsSubregisterItem(string registername, string owner, string subregister, string itemname)
        {

            var queryResults = from o in db.RegisterItems
                               where o.seoname == itemname && o.register.seoname == subregister && o.register.parentRegister.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();
            Kartverket.Register.Models.RegisterItem registerItem = db.RegisterItems.Find(systId);

            if (registerItem.register.containedItemClass == "Document")
            {
                Kartverket.Register.Models.Document document = db.Documents.Find(systId);
                ViewBag.documentOwner = document.documentowner.name;
            }
            return View(registerItem);
        }

        // GET: subregister/Create
        [Authorize]
        [Route("subregister/{registername}/ny")]
        public ActionResult Create(string registername)
        {
            var queryResultsRegister = from o in db.Registers
                                       where o.seoname == registername
                                       select o.systemId;
            Guid regId = queryResultsRegister.First();
            Kartverket.Register.Models.Register register = db.Registers.Find(regId);
            
            
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");
            ViewBagSubregister(register);


            if (role == "nd.metadata_admin" || role == "nd.metadata" || role == "nd.metadata_editor")
            {
                return View();
            }
            return HttpNotFound();
        }

        private void ViewBagSubregister(Kartverket.Register.Models.Register register)
        {
            ViewBag.containedItemClassID = new SelectList(db.ContainedItemClass.OrderBy(s => s.description).Where(s => s.value == "CodelistValue"), "value", "description", String.Empty);
            ViewBag.parentRegister = register.name;
            ViewBag.parentRegisterSEO = register.seoname;
        }

        // POST: subregister/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [Route("subregister/{registername}/ny")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Kartverket.Register.Models.Register subregister, string registername)
        {

            ValidationName(subregister, registername);

            var queryResultsRegister = from o in db.Registers
                                       where o.seoname == registername
                                       select o.systemId;
            Guid regId = queryResultsRegister.First();
            Kartverket.Register.Models.Register register = db.Registers.Find(regId);

            if (ModelState.IsValid)
            {
                subregister.systemId = Guid.NewGuid();
                if (subregister.name == null)
                {
                    subregister.name = "ikke angitt";
                }

                subregister.systemId = Guid.NewGuid();
                subregister.modified = DateTime.Now;
                subregister.dateSubmitted = DateTime.Now;
                subregister.statusId = "Submitted";
                subregister.seoname = MakeSeoFriendlyString(subregister.name);
                subregister.parentRegisterId = regId;
                subregister.containedItemClass = "CodelistValue";

                db.Registers.Add(subregister);
                db.SaveChanges();

                string organizationLogin = GetSecurityClaim("organization");

                var queryResults = from o in db.Organizations
                                   where o.name == organizationLogin
                                   select o.systemId;

                Guid orgId = queryResults.First();
                Organization submitterOrganisasjon = db.Organizations.Find(orgId);

                subregister.ownerId = submitterOrganisasjon.systemId;
                subregister.managerId = submitterOrganisasjon.systemId;

                db.Entry(subregister).State = EntityState.Modified;

                db.SaveChanges();
                ViewBagSubregister(register);
                return Redirect("/register/" + registername);
            }
            ViewBagSubregister(register);
            return View(subregister);
        }

        // GET: Subregister/Edit/5
        [Authorize]
        [Route("subregister/{registername}/{owner}/{subregister}/rediger")]
        public ActionResult Edit(string registername, string subregister)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");

            var queryResults = from o in db.Registers
                               where o.seoname == subregister && o.parentRegister.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();
            Kartverket.Register.Models.Register register = db.Registers.Find(systId);

            if (register == null)
            {
                return HttpNotFound();
            }
            if (role == "nd.metadata_admin" || user.ToLower() == register.owner.name.ToLower() || user.ToLower() == register.owner.name.ToLower())
            {
                Viewbags(register);
                return View(register);
            }
            return HttpNotFound();
        }

        // POST: Subregister/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("subregister/{registername}/{ownerSubregister}/{subregister}/rediger")]
        public ActionResult Edit(Kartverket.Register.Models.Register register, string registername, string subregister)
        {
            var queryResults = from o in db.Registers
                               where o.seoname == subregister && o.parentRegister.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();
            Kartverket.Register.Models.Register originalRegister = db.Registers.Find(systId);

            ValidationName(register, registername);


            if (ModelState.IsValid)
            {
                if (register.name != null) originalRegister.name = register.name; originalRegister.seoname = MakeSeoFriendlyString(originalRegister.name);
                if (register.description != null) originalRegister.description = register.description;
                if (register.ownerId != null) originalRegister.ownerId = register.ownerId;
                if (register.managerId != null) originalRegister.managerId = register.managerId;
                
                originalRegister.modified = DateTime.Now;
                if (register.statusId != null)
                { 
                    originalRegister.statusId = register.statusId;
                    if (originalRegister.statusId != "Accepted" && register.statusId == "Accepted")
                    {
                        originalRegister.dateAccepted = DateTime.Now;
                    }
                    if(originalRegister.statusId == "Accepted" && register.statusId != "Accepted")
                    {
                        originalRegister.dateAccepted = null;
                    }
                }

                db.Entry(originalRegister).State = EntityState.Modified;
                db.SaveChanges();
                Viewbags(register);

                return Redirect("/subregister/" + registername + "/" + originalRegister.owner.seoname + "/" + originalRegister.seoname);
            }
            Viewbags(register);
            return View(originalRegister);
        }

        // GET: Subregister/Delete/5
        [Authorize]
        [Route("subregister/{registername}/{owner}/{subregister}/slett")]
        public ActionResult Delete(string registername, string owner, string subregister)
        {
            var queryResults = from o in db.Registers
                               where o.seoname == subregister && o.parentRegister.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();
            Kartverket.Register.Models.Register register = db.Registers.Find(systId);

            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // POST: Subregister/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("subregister/{registername}/{owner}/{subregister}/slett")]
        public ActionResult DeleteConfirmed(string registername, string owner, string subregister)
        {
            var queryResults = from o in db.Registers
                               where o.seoname == subregister && o.parentRegister.seoname == registername
                               select o.systemId;

            Guid systId = queryResults.First();        
            Kartverket.Register.Models.Register register = db.Registers.Find(systId);

            var queryResultsRegisterItem = from o in db.RegisterItems
                               where o.register.seoname == subregister && o.register.parentRegister.seoname == registername
                               select o.systemId;

            if (queryResultsRegisterItem.Count() > 0)
            {
                return View(register);
                //skriv ut feilmelding på at registeret inneholder registeritems.. kan ikke slettes...
            }
            else { 
                db.Registers.Remove(register);
                db.SaveChanges();
                return Redirect("/register/" + registername);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        // *********************** Hjelpemetoder

        private void ValidationName(Kartverket.Register.Models.Register subregister, string register)
        {
            var queryResultsDataset = from o in db.Registers
                                      where o.name == subregister.name && o.systemId != subregister.systemId && o.parentRegister.seoname == register
                                      select o.systemId;

            if (queryResultsDataset.Count() > 0)
            {
                ModelState.AddModelError("ErrorMessage", "Navnet finnes fra før!");
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

        private void Viewbags(Kartverket.Register.Models.Register register)
        {
            //ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", document.registerId);
            ViewBag.statusId = new SelectList(db.Statuses.OrderBy(s => s.description), "value", "description", register.statusId);
            ViewBag.ownerId = new SelectList(db.Organizations.OrderBy(s => s.name), "systemId", "name", register.ownerId);
        }
    }
}
