﻿using System;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Kartverket.DOK.Service;
using Kartverket.Register.Models;
using Kartverket.Register.Models.ViewModels;
using Kartverket.Register.Services;

namespace Kartverket.Register.Controllers
{
    public class InspireDatasetsController : Controller
    {
        private readonly RegisterDbContext _db = new RegisterDbContext();
        private readonly IInspireDatasetService _inspireDatasetService;
        private readonly MetadataService _metadataService;

        public InspireDatasetsController(IInspireDatasetService inspireDatasetService) {
            _inspireDatasetService = inspireDatasetService;
            _metadataService = new MetadataService();
        }
        

        // GET: InspireDatasets/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspireDataset inspireDataset = _db.InspireDatasets.Find(id);
            if (inspireDataset == null)
            {
                return HttpNotFound();
            }
            return View(inspireDataset);
        }

        // GET: InspireDatasets/Create
        //[Authorize]
        [Route("inspire/{registername}/ny")]
        [Route("inspire/{parentregister}/{registerowner}/{registername}/ny")]
        public ActionResult Create(string registername, string parentregister)
        {
            InspireDatasetViewModel model = _inspireDatasetService.NewInspireDataset(parentregister, registername);

            return View(model);
        }

        // POST: InspireDatasets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("inspire/{registername}/ny")]
        [Route("inspire/{parentregister}/{registerowner}/{registername}/ny")]
        public ActionResult Create(InspireDatasetViewModel viewModel, string parentregister, string registername, string metadataUuid)
        {
            if (viewModel.SearchString != null)
            {
                viewModel.SearchResultList = _metadataService.SearchMetadataFromKartkatalogen(viewModel.SearchString);
            }
            else if (metadataUuid != null)
            {
                viewModel.Update(_metadataService.FetchInspireDatasetFromKartkatalogen(metadataUuid));
            }
            else if (ModelState.IsValid)
            {
                _inspireDatasetService.CreateNewInspireDataset(viewModel, parentregister, registername);
                return RedirectToAction("Details");
            }
            return View(viewModel);
        }

        // GET: InspireDatasets/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspireDataset inspireDataset = _db.InspireDatasets.Find(id);
            if (inspireDataset == null)
            {
                return HttpNotFound();
            }
            return View(inspireDataset);
        }

        // POST: InspireDatasets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SystemId,InspireDeliveryMetadataId,InspireDeliveryMetadataServiceId,InspireDeliveryDistributionId,InspireDeliveryWmsId,InspireDeliveryWfsId,InspireDeliveryAtomFeedId,InspireDeliveryWfsOrAtomId,InspireDeliveryHarmonizedDataId,InspireDeliverySpatialDataServiceId,Uuid,Notes,SpecificUsage,ProductSheetUrl,PresentationRulesUrl,ProductSpecificationUrl,MetadataUrl,DistributionFormat,DistributionUrl,DistributionArea,WmsUrl,ThemeGroupId,DatasetThumbnail,DokStatusId,DokStatusDateAccepted,Name,Seoname,Description,SubmitterId,OwnerId,DateSubmitted,Modified,StatusId,RegisterId")] InspireDataset inspireDataset)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(inspireDataset).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(inspireDataset);
        }

        // GET: InspireDatasets/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspireDataset inspireDataset = _db.InspireDatasets.Find(id);
            if (inspireDataset == null)
            {
                return HttpNotFound();
            }
            return View(inspireDataset);
        }

        // POST: InspireDatasets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var inspireDataset = _db.InspireDatasets.Find(id);
            _db.InspireDatasets.Remove(inspireDataset ?? throw new InvalidOperationException());
            _db.SaveChanges();
            return RedirectToAction("Details");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
