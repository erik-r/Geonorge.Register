﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kartverket.Register.Models;

namespace Kartverket.Register.Controllers
{
    public class EPSGsController : Controller
    {
        private RegisterDbContext db = new RegisterDbContext();

        // GET: EPSGs
        public ActionResult Index()
        {
            var registerItems = db.EPSGs.Include(e => e.register).Include(e => e.status).Include(e => e.submitter).Include(e => e.inspireRequirement).Include(e => e.nationalRequirement).Include(e => e.nationalSeasRequirement);
            return View(registerItems.ToList());
        }

        // GET: EPSGs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EPSG ePSG = db.EPSGs.Find(id);
            if (ePSG == null)
            {
                return HttpNotFound();
            }
            return View(ePSG);
        }

        // GET: EPSGs/Create
        public ActionResult Create()
        {
            ViewBag.registerId = new SelectList(db.Registers, "systemId", "name");
            ViewBag.statusId = new SelectList(db.Statuses, "value", "description");
            ViewBag.submitterId = new SelectList(db.RegisterItems, "systemId", "name");
            ViewBag.inspireRequirementId = new SelectList(db.requirements, "value", "description");
            ViewBag.nationalRequirementId = new SelectList(db.requirements, "value", "description");
            ViewBag.nationalSeasRequirementId = new SelectList(db.requirements, "value", "description");
            return View();
        }

        // POST: EPSGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "systemId,name,description,submitterId,dateSubmitted,modified,statusId,dateAccepted,registerId,epsg,sosiReferencesystem,externalReference,inspireRequirementId,inspireRequirementDescription,nationalRequirementId,nationalRequirementDescription,nationalSeasRequirementId,nationalSeasRequirementDescription")] EPSG ePSG)
        {
            if (ModelState.IsValid)
            {
                ePSG.systemId = Guid.NewGuid();
                db.RegisterItems.Add(ePSG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", ePSG.registerId);
            ViewBag.statusId = new SelectList(db.Statuses, "value", "description", ePSG.statusId);
            ViewBag.submitterId = new SelectList(db.RegisterItems, "systemId", "name", ePSG.submitterId);
            ViewBag.inspireRequirementId = new SelectList(db.requirements, "value", "description", ePSG.inspireRequirementId);
            ViewBag.nationalRequirementId = new SelectList(db.requirements, "value", "description", ePSG.nationalRequirementId);
            ViewBag.nationalSeasRequirementId = new SelectList(db.requirements, "value", "description", ePSG.nationalSeasRequirementId);
            return View(ePSG);
        }

        // GET: EPSGs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EPSG ePSG = db.EPSGs.Find(id);
            if (ePSG == null)
            {
                return HttpNotFound();
            }
            ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", ePSG.registerId);
            ViewBag.statusId = new SelectList(db.Statuses, "value", "description", ePSG.statusId);
            ViewBag.submitterId = new SelectList(db.RegisterItems, "systemId", "name", ePSG.submitterId);
            ViewBag.inspireRequirementId = new SelectList(db.requirements, "value", "description", ePSG.inspireRequirementId);
            ViewBag.nationalRequirementId = new SelectList(db.requirements, "value", "description", ePSG.nationalRequirementId);
            ViewBag.nationalSeasRequirementId = new SelectList(db.requirements, "value", "description", ePSG.nationalSeasRequirementId);
            return View(ePSG);
        }

        // POST: EPSGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "systemId,name,description,submitterId,dateSubmitted,modified,statusId,dateAccepted,registerId,epsg,sosiReferencesystem,externalReference,inspireRequirementId,inspireRequirementDescription,nationalRequirementId,nationalRequirementDescription,nationalSeasRequirementId,nationalSeasRequirementDescription")] EPSG ePSG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ePSG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", ePSG.registerId);
            ViewBag.statusId = new SelectList(db.Statuses, "value", "description", ePSG.statusId);
            ViewBag.submitterId = new SelectList(db.RegisterItems, "systemId", "name", ePSG.submitterId);
            ViewBag.inspireRequirementId = new SelectList(db.requirements, "value", "description", ePSG.inspireRequirementId);
            ViewBag.nationalRequirementId = new SelectList(db.requirements, "value", "description", ePSG.nationalRequirementId);
            ViewBag.nationalSeasRequirementId = new SelectList(db.requirements, "value", "description", ePSG.nationalSeasRequirementId);
            return View(ePSG);
        }

        // GET: EPSGs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EPSG ePSG = db.EPSGs.Find(id);
            if (ePSG == null)
            {
                return HttpNotFound();
            }
            return View(ePSG);
        }

        // POST: EPSGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EPSG ePSG = db.EPSGs.Find(id);
            db.RegisterItems.Remove(ePSG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}