﻿using System;
using System.Web.Mvc;
using Kartverket.Register.Models;
using Kartverket.DOK.Service;
using Kartverket.Register.Services.Register;
using Kartverket.Register.Services.RegisterItem;
using Kartverket.Register.Services;
using System.Web;
using Kartverket.Register.Helpers;

namespace Kartverket.Register.Controllers
{
    [HandleError]
    public class DatasetsController : Controller
    {
        private RegisterDbContext db = new RegisterDbContext();

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IRegisterService _registerService;
        private IRegisterItemService _registerItemService;
        private IAccessControlService _accessControlService;

        public DatasetsController(IRegisterItemService registerItemService, IRegisterService registerService, IAccessControlService accessControllService)
        {
            _registerItemService = registerItemService;
            _registerService = registerService;
            _accessControlService = accessControllService;
        }

        public DatasetsController()
        {
            _registerItemService = new RegisterItemService(db);
            _registerService = new RegisterService(db);
            _accessControlService = new AccessControlService();
        }

        // GET: Datasets/Create
        [Authorize]
        [Route("dataset/{parentRegister}/{registerowner}/{registername}/ny")]
        [Route("dataset/{registername}/ny")]
        public ActionResult Create(string registername, string parentRegister)
        {
            Dataset dataset = new Dataset();
            dataset.register = _registerService.GetRegister(parentRegister, registername);
            if (dataset.register != null)
            {
                dataset.DatasetType = GetDatasetType(dataset.register.name);
                Viewbags(dataset);
                if (_accessControlService.Access(dataset.register))
                {
                    return View(dataset);
                }
                else
                {
                    throw new HttpException(401, "Access Denied");
                }
            }
            return HttpNotFound("Finner ikke registeret");
        }


        // POST: Datasets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("dataset/{parentRegister}/{registerowner}/{registername}/ny")]
        [Route("dataset/{registername}/ny")]
        public ActionResult Create(Dataset dataset, string registername, string uuid, string parentRegister, string registerowner)
        {
            dataset.register = _registerService.GetRegister(parentRegister, registername);
            if (dataset.register != null)
            {
                dataset.DatasetType = dataset.GetDatasetType();
                if (uuid != null)
                {
                    Dataset model = GetMetadataFromKartkatalogen(dataset, uuid);
                    Viewbags(dataset);
                    return View(model);
                }
                else if (_accessControlService.Access(dataset.register))
                {
                    dataset.systemId = dataset.GetSystemId();
                    dataset.registerId = dataset.register.GetSystemId();
                    dataset.datasetownerId = GetDatasetOwnerId(dataset.datasetownerId);
                    dataset.datasetowner = (Organization)_registerItemService.GetRegisterItemBySystemId(dataset.datasetownerId);


                    // TODO fikse validering... 
                    if (!NameIsValid(dataset))
                    {
                        ModelState.AddModelError("ErrorMessage", HtmlHelperExtensions.ErrorMessageValidationDataset());
                        Viewbags(dataset);
                        return View(dataset);
                    }
                    if (ModelState.IsValid)
                    {
                        dataset = initialisationDataset(dataset);
                        _registerItemService.SaveNewRegisterItem(dataset);
                        return Redirect(dataset.GetObjectUrl(dataset));
                    }
                }
                else
                {
                    throw new HttpException(401, "Access Denied");
                }
            }
            Viewbags(dataset);
            return View(dataset);
        }

        private Guid GetDatasetOwnerId(Guid datasetownerId)
        {
            if (datasetownerId == null || datasetownerId == Guid.Empty)
            {
                Organization datasetOwner = _registerService.GetOrganizationByUserName();
                return datasetOwner.systemId;
            }
            return datasetownerId;
        }


        // GET: Datasets/Edit/5
        [Authorize]
        [Route("dataset/{parentRegister}/{registerowner}/{registername}/{itemowner}/{itemname}/rediger")]
        [Route("dataset/{registername}/{itemowner}/{itemname}/rediger")]
        public ActionResult Edit(string registername, string itemowner, string itemname, string parentRegister)
        {
            Dataset dataset = (Dataset)_registerItemService.GetRegisterItem(parentRegister, registername, itemname, 1, itemowner);
            if (dataset != null)
            {
                if (_accessControlService.Access(dataset))
                {
                    Viewbags(dataset);
                    return View(dataset);
                }
                else
                {
                    throw new HttpException(401, "Access Denied");
                }
            }
            return HttpNotFound("Finner ikke datasettet");
        }


        // POST: Dataset/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("dataset/{parentRegister}/{registerowner}/{registername}/{itemowner}/{itemname}/rediger")]
        [Route("dataset/{registername}/{itemowner}/{itemname}/rediger")]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Dataset dataset, CoverageDataset coverage, string registername, string itemname, string uuid, string parentRegister, string registerowner, string itemowner, bool dontUpdateDescription = false)
        {
            Dataset originalDataset = (Dataset)_registerItemService.GetRegisterItem(parentRegister, registername, itemname, 1, itemowner);
            if (originalDataset != null)
            {
                if (uuid != null)
                {
                    Dataset model = GetMetadataFromKartkatalogen(originalDataset, uuid, dontUpdateDescription);
                    Viewbags(model);
                    return View(model);
                }
                if (originalDataset.IsNationalDataset())
                {
                    if (_accessControlService.IsAdmin())
                    {
                        return EditDataset(dataset, registername, parentRegister, registerowner, originalDataset);
                    }
                    if (_accessControlService.IsMunicipalUser())
                    {
                        return EditCoverageDataset(coverage, registername, parentRegister, registerowner, originalDataset);
                    }
                }
                else if (_accessControlService.Access(originalDataset))
                {
                    if (ModelState.IsValid)
                    {
                        return EditDataset(dataset, registername, parentRegister, registerowner, originalDataset, coverage);
                    }
                }
                else
                {
                    throw new HttpException(401, "Access Denied");
                }
            }
            Viewbags(originalDataset);
            return View(originalDataset);
        }


        // GET: Dataset/Delete/5
        [Authorize]
        [Route("dataset/{parentregister}/{parentregisterowner}/{registername}/{itemowner}/{itemname}/slett")]
        [Route("dataset/{registername}/{itemowner}/{itemname}/slett")]
        public ActionResult Delete(string registername, string itemname, string parentregister, string parentregisterowner, string itemowner)
        {
            Dataset dataset = (Dataset)_registerItemService.GetRegisterItem(parentregister, registername, itemname, 1, itemowner);
            if (dataset != null)
            {
                if (_accessControlService.Access(dataset))
                {
                    Viewbags(dataset);
                    return View(dataset);
                }
                else
                {
                    throw new HttpException(401, "Access Denied");
                }
            }
            return HttpNotFound("Finner ikke datasettet");
        }


        // POST: Dataset/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("dataset/{parentregister}/{registerowner}/{registername}/{itemowner}/{itemname}/slett")]
        [Route("dataset/{registername}/{itemowner}/{itemname}/slett")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string registername, string itemname, string parentregister, string registerowner, string itemowner)
        {
            Dataset dataset = (Dataset)_registerItemService.GetCurrentRegisterItem(parentregister, registername, itemname);
            DeleteCoverageDataset(dataset);
            _registerItemService.SaveDeleteRegisterItem(dataset);
            return Redirect(RegisterUrls.registerUrl(parentregister, registerowner, registername));
        }


        // *** HJELPEMETODER

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error("Error", filterContext.Exception);
        }

        private string GetDatasetType(string registerName)
        {
            if (DokMunicipalDataset(registerName))
            {
                return "Kommunalt";
            }
            else
            {
                return "Nasjonalt";
            }
        }

        private static bool DokMunicipalDataset(string registerName)
        {
            return registerName == "Det offentlige kartgrunnlaget - Kommunalt";
        }

        private bool GetConfirmedDok(CoverageDataset inputCoverage)
        {
            if (inputCoverage != null)
            {
                return inputCoverage.ConfirmedDok;
            }
            else {
                return false;
            }
        }

        private Guid? GetVersioningId(Dataset dataset)
        {
            if (dataset.versioningId == null)
            {
                return _registerItemService.NewVersioningGroup(dataset);
            }
            else
            {
                return dataset.GetVersioningId();
            }
        }


        private bool NameIsValid(Dataset dataset)
        {
            return _registerItemService.validateName(dataset);
        }

        private Dataset GetMetadataFromKartkatalogen(Dataset dataset, string uuid, bool dontUpdateDescription = false)
        {
            var model = new Dataset();
            try
            {
                new MetadataService().UpdateDatasetWithMetadata(model, uuid, dataset, dontUpdateDescription);
            }
            catch (Exception e)
            {
                TempData["error"] = "Det oppstod en feil ved henting av metadata: " + e.Message;
            }
            return model;
        }

        private void Viewbags(Dataset dataset)
        {
            ViewBag.registerId = _registerItemService.GetRegisterSelectList(dataset.registerId);
            ViewBag.dokStatusId = _registerItemService.GetDokStatusSelectList(dataset.dokStatusId);
            ViewBag.CoverageDOKStatusId = _registerItemService.GetDokStatusSelectList(null);
            ViewBag.submitterId = _registerItemService.GetSubmitterSelectList(dataset.submitterId);
            ViewBag.datasetownerId = _registerItemService.GetOwnerSelectList(dataset.datasetownerId);
            ViewBag.ThemeGroupId = _registerItemService.GetThemeGroupSelectList(dataset.ThemeGroupId);
        }

        private ActionResult EditDataset(Dataset dataset, string registername, string parentRegister, string registerowner, Dataset originalDataset, CoverageDataset coverage = null)
        {
            dataset.register = originalDataset.register;
            if (!NameIsValid(dataset))
            {
                ModelState.AddModelError("ErrorMessage", HtmlHelperExtensions.ErrorMessageValidationName());
                Viewbags(originalDataset);
                return View(originalDataset);
            }
            initialisationDataset(dataset, originalDataset, coverage);
            _registerItemService.SaveEditedRegisterItem(originalDataset);
            return Redirect(originalDataset.GetObjectUrl(originalDataset));
        }

        private ActionResult EditCoverageDataset(CoverageDataset coverage, string registername, string parentRegister, string registerowner, Dataset originalDataset)
        {
            initialisationCoverageDataset(coverage, originalDataset);
            _registerItemService.SaveEditedRegisterItem(originalDataset);
            return Redirect(originalDataset.GetObjectUrl(originalDataset));
        }

        private Guid SetMunicipality()
        {
            Organization municipality = _registerService.GetOrganizationByUserName();
            return municipality.systemId;
        }

        private void DeleteCoverageDataset(Dataset dataset)
        {
            if (dataset.Coverage != null)
            {
                for (int i = 0; i < dataset.Coverage.Count; i++)
                {
                    dataset.Coverage[i].DatasetId = Guid.Empty;
                    dataset.Coverage[i].dataset = null;
                    _registerItemService.DeleteCoverage(dataset.Coverage[i]);
                }
                dataset.Coverage.Clear();
            }
        }

        private Dataset initialisationDataset(Dataset inputDataset, Dataset originalDataset = null, CoverageDataset inputCoverage = null)
        {
            Dataset dataset = GetDataset(originalDataset);
            //dataset.systemId = dataset.GetSystemId();
            dataset.systemId = inputDataset.GetSystemId();
            dataset.modified = dataset.GetDateModified();
            dataset.dateSubmitted = dataset.GetDateSubmbitted();
            dataset.registerId = inputDataset.register.GetSystemId();
            dataset.register = GetRegister(inputDataset.register, dataset.register);
            dataset.DatasetType = dataset.GetDatasetType();
            dataset.statusId = dataset.GetStatusId();
            dataset.dokStatusId = inputDataset.GetDokStatus();
            dataset.dokStatusDateAccepted = inputDataset.GetDokStatusDateAccepted();
            dataset.versionNumber = dataset.GetVersionNr();
            dataset.name = inputDataset.GetName();
            dataset.seoname = RegisterUrls.MakeSeoFriendlyString(dataset.name);
            dataset.description = inputDataset.GetDescription();
            dataset.versioningId = GetVersioningId(dataset);
            //dataset.datasetowner = GetDatasetOwner(inputDataset.datasetowner);
            //dataset.datasetownerId = dataset.datasetowner.systemId;
            Guid originalDatasetownerId = GetDatasetOriginalOwnerId(originalDataset);
            dataset.datasetownerId = GetDatasetOwnerId(inputDataset.datasetownerId);
            dataset.datasetowner = (Organization)_registerItemService.GetRegisterItemBySystemId(dataset.datasetownerId);
            dataset.submitter = GetSubmitter(inputDataset.submitter);
            dataset.submitterId = dataset.submitter.systemId;
            dataset.DistributionUrl = inputDataset.GetDistributionUrl();
            dataset.MetadataUrl = inputDataset.GetMetadataUrl();
            dataset.PresentationRulesUrl = inputDataset.GetPresentationRulesUrl();
            dataset.ProductSheetUrl = inputDataset.GetProductSheetUrl();
            dataset.ProductSpecificationUrl = inputDataset.GetProductSpecificationUrl();
            dataset.WmsUrl = inputDataset.GetWmsUrl();
            dataset.DistributionFormat = inputDataset.GetDistributionFormat();
            dataset.DistributionArea = inputDataset.GetDistributionArea();
            dataset.Notes = inputDataset.GetNotes();
            dataset.ThemeGroupId = inputDataset.GetThemeGroupId();
            dataset.datasetthumbnail = inputDataset.Getdatasetthumbnail();

            initialisationCoverageDataset(inputCoverage, dataset, originalDatasetownerId);
            return dataset;
        }

        private Guid GetDatasetOriginalOwnerId(Dataset originalDataset)
        {
            if (originalDataset != null)
            {
                return originalDataset.datasetownerId;
            }
            else {
                return Guid.Empty;
            }
        }

        private Models.Register GetRegister(Models.Register inputRegister, Models.Register register)
        {
            if (register == null)
            {
                return inputRegister;
            }
            else {
                return register;
            }
        }

        private void initialisationCoverageDataset(CoverageDataset coverage, Dataset dataset, Guid? originalDatasetOwnerId = null)
        {
            if (coverage != null)
            {
                CoverageDataset originalCoverage = _registerItemService.GetMunicipalityCoverage(dataset, originalDatasetOwnerId);

                if (dataset.IsMunicipalDataset())
                {
                    if (originalCoverage != null)
                    {
                        originalCoverage.MunicipalityId = dataset.datasetownerId;
                        originalCoverage.CoverageDOKStatusId = dataset.dokStatusId;
                        originalCoverage.ConfirmedDok = GetConfirmedDok(coverage);
                        _registerItemService.Save();
                    }
                }
                else
                {
                    if (originalCoverage == null)
                    {
                        CoverageDataset newCoverage = new CoverageDataset()
                        {
                            CoverageId = Guid.NewGuid(),
                            CoverageDOKStatus = coverage.CoverageDOKStatus,
                            CoverageDOKStatusId = coverage.CoverageDOKStatusId,
                            ConfirmedDok = coverage.ConfirmedDok,
                            DatasetId = dataset.systemId,
                            MunicipalityId = SetMunicipality(),
                            Note = coverage.Note,
                        };
                        _registerItemService.SaveNewCoverage(newCoverage);
                        dataset.Coverage.Add(newCoverage);
                    }
                    else {
                        originalCoverage.ConfirmedDok = coverage.ConfirmedDok;
                        originalCoverage.CoverageDOKStatusId = coverage.CoverageDOKStatusId;
                        originalCoverage.Note = coverage.Note;
                    }
                }
            }
            else if (coverage == null && dataset.IsMunicipalDataset())
            {
                dataset.Coverage.Add(_registerItemService.NewCoverage(dataset));
            }
        }


        private Organization GetSubmitter(Organization submitter)
        {
            if (submitter == null)
            {
                return _registerService.GetOrganizationByUserName();
            }
            else {
                return submitter;
            }
        }

        private Dataset GetDataset(Dataset originalDataset)
        {
            if (originalDataset != null)
            {
                return originalDataset;
            }
            else {
                return new Dataset();
            }
        }
    }
}
