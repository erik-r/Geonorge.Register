﻿using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kartverket.DOK.Service;
using Kartverket.Register.Helpers;
using Kartverket.Register.Models;
using Kartverket.Register.Models.ViewModels;
using Kartverket.Register.Services;
using Kartverket.Register.Services.Register;
using Kartverket.Register.Services.RegisterItem;

namespace Kartverket.Register.Controllers
{
    public class GeodatalovDatasetsController : Controller
    {
        private readonly RegisterDbContext _db = new RegisterDbContext();
        private readonly MetadataService _metadataService;
        private readonly IAccessControlService _accessControlService;
        private readonly IRegisterService _registerService;
        private readonly IRegisterItemService _registerItemService;
        private readonly IGeodatalovDatasetService _geodatalovDatasetService;
        private readonly IDatasetDeliveryService _datasetDeliveryService;

        public GeodatalovDatasetsController(IGeodatalovDatasetService geodatalovDatasetService, IAccessControlService accessControllService, IRegisterService registerService, IRegisterItemService registerItemService, IDatasetDeliveryService datasetDeliveryService)
        {
            _geodatalovDatasetService = geodatalovDatasetService;
            _accessControlService = accessControllService;
            _metadataService = new MetadataService();
            _registerService = registerService;
            _registerItemService = registerItemService;
            _datasetDeliveryService = datasetDeliveryService;
        }

        // GET: GeodatalovDatasets/Create
        [Authorize]
        [Route("geodatalov/{registername}/ny")]
        [Route("geodatalov/{parentregister}/{registerowner}/{registername}/ny")]
        public ActionResult Create(string registername, string parentregister)
        {
            var viewModel = _geodatalovDatasetService.NewGeodatalovDatasetViewModel(parentregister, registername);
            if (_accessControlService.Access(viewModel.Register))
                return View(viewModel);
            throw new HttpException(401, "Access Denied");
        }

        // POST: GeodatalovDatasets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("geodatalov/{registername}/ny")]
        [Route("geodatalov/{parentregister}/{registerowner}/{registername}/ny")]
        public ActionResult Create(GeodatalovDatasetViewModel viewModel, string parentregister, string registername, string metadataUuid)
        {
            viewModel.Register = _registerService.GetRegisterBySystemId(viewModel.RegisterId);
            if (!_accessControlService.Access(viewModel.Register)) throw new HttpException(401, "Access Denied");

            if (viewModel.SearchString != null)
            {
                viewModel.SearchResultList = _metadataService.SearchMetadataFromKartkatalogen(viewModel.SearchString);
                return View(viewModel);
            }

            if (metadataUuid != null)
            {
                viewModel.Update(_metadataService.FetchGeodatalovDatasetFromKartkatalogen(metadataUuid));
                if (viewModel.Name == null)
                {
                    ModelState.AddModelError("ErrorMessage", "Det har oppstått en feil ved henting av metadata...");
                }
            }
            if (_registerItemService.ItemNameAlredyExist(viewModel))
            {
                ModelState.AddModelError("ErrorMessage", HtmlHelperExtensions.ErrorMessageValidationDataset());
                return View(viewModel);
            }
            if (!ModelState.IsValid) return View(viewModel);
            var inspireDataset = _geodatalovDatasetService.NewGeodatalovDataset(viewModel, parentregister, registername);
            return Redirect(inspireDataset.Register.GetObjectUrl());
        }

        // GET: GeodatalovDatasets/Edit/5
        [Authorize]
        [Route("geodatalov/{parentRegister}/{registerowner}/{registername}/{itemowner}/{itemname}/rediger")]
        [Route("geodatalov/{registername}/{itemowner}/{itemname}/rediger")]
        public ActionResult Edit(string registername, string itemname)
        {
            var geodatalovDataset = _geodatalovDatasetService.GetGeodatalovDatasetByName(registername, itemname);
            if (geodatalovDataset == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_accessControlService.Access(geodatalovDataset))
            {
                geodatalovDataset = _geodatalovDatasetService.UpdateGeodatalovDatasetFromKartkatalogen(geodatalovDataset);
                var viewModel = new GeodatalovDatasetViewModel(geodatalovDataset);
                ViewBags(viewModel);
                return View(viewModel);
            }
            throw new HttpException(401, "Access Denied");
        }

        // POST: GeodatalovDatasets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("geodatalov/{parentRegister}/{registerowner}/{registername}/{itemowner}/{itemname}/rediger")]
        [Route("geodatalov/{registername}/{itemowner}/{itemname}/rediger")]
        public ActionResult Edit(GeodatalovDatasetViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var geodatalovDataset = _geodatalovDatasetService.UpdateGeodatalovDataset(viewModel);
                return Redirect(geodatalovDataset.DetailPageUrl());
            }
            ViewBags(viewModel);
            return View(viewModel);
        }

        // GET: GeodatalovDatasets/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeodatalovDataset geodatalovDataset = _db.GeodatalovDatasets.Find(id);
            if (geodatalovDataset == null)
            {
                return HttpNotFound();
            }
            return View(geodatalovDataset);
        }

        // POST: GeodatalovDatasets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            GeodatalovDataset geodatalovDataset = _db.GeodatalovDatasets.Find(id);
            _db.GeodatalovDatasets.Remove(geodatalovDataset);
            _db.SaveChanges();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ViewBags(GeodatalovDatasetViewModel viewModel)
        {
            ViewBag.DokStatusId = _registerItemService.GetDokStatusSelectList(viewModel.DokStatusId);
            ViewBag.SubmitterId = _registerItemService.GetSubmitterSelectList(viewModel.SubmitterId);
            ViewBag.OwnerId = _registerItemService.GetOwnerSelectList(viewModel.OwnerId);
            ViewBag.MetadataStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.MetadataStatusId);
            ViewBag.ProductSpesificationStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.ProductSpesificationStatusId);
            ViewBag.SosiDataStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.SosiDataStatusId);
            ViewBag.GmlDataStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.GmlDataStatusId);
            ViewBag.WmsStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.WmsStatusId);
            ViewBag.WfsStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.WfsStatusId);
            ViewBag.AtomFeedStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.AtomFeedStatusId);
            ViewBag.CommonStatus = _datasetDeliveryService.GetDokDeliveryStatusesAsSelectlist(viewModel.CommonStatusId);
        }
    }
}
