﻿using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Kartverket.Register.Models;
using Kartverket.DOK.Service;
using Kartverket.Register.Services.Register;
using Kartverket.Register.Services.RegisterItem;
using Kartverket.Register.Services;
using System.Web;
using System.Net.Http;
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
                ViewBag.ThemeGroupId = new SelectList(db.DOKThemes, "value", "description");

                if (_accessControlService.Access(dataset))
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
            if (uuid != null)
            {
                Dataset model = GetMetadataFromKartkatalogen(dataset, uuid);
                return View(model);
            }

            dataset.register = _registerService.GetRegister(parentRegister, registername);
            if (_accessControlService.Access(dataset))
            {
                if (!NameIsValid(dataset))
                {
                    ModelState.AddModelError("ErrorMessage", HtmlHelperExtensions.ErrorMessageValidationName());
                }
                else if (ModelState.IsValid)
                {
                    initialisationDataset(dataset);
                    return Redirect(RegisterUrls.DeatilsRegisterItemUrl(parentRegister, registerowner, registername, dataset.datasetowner.seoname, dataset.seoname));
                }
            }
            else
            {
                throw new HttpException(401, "Access Denied");
            }
            ViewBag.ThemeGroupId = new SelectList(db.DOKThemes, "value", "description", dataset.ThemeGroupId);
            ViewBag.statusId = new SelectList(db.Statuses, "value", "description");
            return View(dataset);
        }

        // GET: Datasets/Edit/5
        [Authorize]
        [Route("dataset/{parentRegister}/{registerowner}/{registername}/{itemowner}/{datasetname}/rediger")]
        [Route("dataset/{registername}/{organization}/{datasetname}/rediger")]
        public ActionResult Edit(string registername, string datasetname, string parentRegister)
        {
            Dataset dataset = (Dataset)_registerItemService.GetRegisterItem(parentRegister, registername, datasetname, 1);
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


        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("dataset/{parentRegister}/{registerowner}/{registername}/{itemowner}/{datasetname}/rediger")]
        [Route("dataset/{registername}/{organization}/{datasetname}/rediger")]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Dataset dataset, string registername, string datasetname, string uuid, bool dontUpdateDescription, string parentRegister, string registerowner)
        {
            Dataset originalDataset = (Dataset)_registerItemService.GetRegisterItem(parentRegister, registername, datasetname, 1);
            if (originalDataset != null)
            {
                if (uuid != null)
                {
                    Dataset model = GetMetadataFromKartkatalogen(dataset, uuid);
                    if (dontUpdateDescription) model.description = originalDataset.description;
                    Viewbags(model);
                    return View(model);
                }

                if (_accessControlService.Access(originalDataset))
                {
                    if (!NameIsValid(dataset))
                    {
                        ModelState.AddModelError("ErrorMessage", HtmlHelperExtensions.ErrorMessageValidationName());
                    }
                    else if (ModelState.IsValid)
                    {
                        initialisationDataset(dataset, originalDataset);
                        return Redirect(RegisterUrls.DeatilsRegisterItemUrl(parentRegister, registerowner, registername, originalDataset.datasetowner.seoname, originalDataset.seoname));
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

        // GET: Documents/Delete/5
        [Authorize]
        [Route("dataset/{parentregister}/{parentregisterowner}/{registername}/{itemowner}/{datasetname}/slett")]
        [Route("dataset/{registername}/{organization}/{datasetname}/slett")]
        public ActionResult Delete(string registername, string datasetname, string parentregister, string parentregisterowner)
        {
            string role = GetSecurityClaim("role");
            string user = GetSecurityClaim("organization");


            var queryResults = from o in db.Datasets
                               where o.seoname == datasetname && o.register.seoname == registername
                               && o.register.parentRegister.seoname == parentregister
                               select o.systemId;

            Guid systId = queryResults.FirstOrDefault();

            if (systId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dataset dataset = db.Datasets.Find(systId);
            if (dataset == null)
            {
                return HttpNotFound();
            }
            if (role == "nd.metadata_admin" || ((role == "nd.metadata" || role == "nd.metadata_editor") && dataset.register.accessId == 2 && dataset.datasetowner.name.ToLower() == user.ToLower()))
            {
                Viewbags(dataset);
                return View(dataset);
            }
            return HttpNotFound("Ingen tilgang");
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("dataset/{parentregister}/{parentregisterowner}/{registername}/{itemowner}/{datasetname}/slett")]
        [Route("dataset/{registername}/{organization}/{datasetname}/slett")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string registername, string datasetname, string parentregister, string parentregisterowner)
        {
            var queryResults = from o in db.Datasets
                               where o.seoname == datasetname && o.register.seoname == registername
                               && o.register.parentRegister.seoname == parentregister
                               select o.systemId;

            Guid systId = queryResults.FirstOrDefault();
            Dataset dataset = db.Datasets.Find(systId);

            string parent = null;
            if (dataset.register.parentRegisterId != null)
            {
                parent = dataset.register.parentRegister.seoname;
            }

            db.RegisterItems.Remove(dataset);
            db.SaveChanges();

            if (parent != null)
            {
                return Redirect("/subregister/" + parentregister + "/" + parentregisterowner + "/" + registername);
            }

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

        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error("Error", filterContext.Exception);
        }

        public static implicit operator DatasetsController(HttpRequestMessage v)
        {
            throw new NotImplementedException();
        }

        private void initialisationDataset(Dataset dataset)
        {
            dataset.systemId = Guid.NewGuid();
            dataset.modified = DateTime.Now;
            dataset.dateSubmitted = DateTime.Now;
            dataset.registerId = dataset.register.systemId;
            dataset.statusId = "Valid";
            dataset.dokStatusId = "Proposal";
            dataset.versionNumber = GetVersionNr(dataset.versionNumber);
            dataset.name = DatasetName(dataset.name);
            dataset.seoname = DatasetSeoName(dataset.name);
            dataset.versioningId = GetVersioningId(dataset);

            SetDatasetOwnerAndSubmitter(dataset);

            _registerItemService.SaveNewRegisterItem(dataset);
        }


        private void initialisationDataset(Dataset dataset, Dataset originalDataset)
        {
            originalDataset.name = DatasetName(dataset.name);
            originalDataset.seoname = DatasetSeoName(dataset.name);
            originalDataset.description = dataset.description;
            originalDataset.datasetownerId = DatasetOwnerId(dataset, originalDataset);
            originalDataset.submitterId = DatasetSubmitterId(dataset, originalDataset);

            SetDokStatusId(dataset, originalDataset);

            originalDataset.DistributionUrl = dataset.DistributionUrl;
            originalDataset.MetadataUrl = dataset.MetadataUrl;
            originalDataset.PresentationRulesUrl = dataset.PresentationRulesUrl;
            originalDataset.ProductSheetUrl = dataset.ProductSheetUrl;
            originalDataset.ProductSpecificationUrl = dataset.ProductSpecificationUrl;
            originalDataset.WmsUrl = dataset.WmsUrl;
            originalDataset.DistributionFormat = dataset.DistributionFormat;
            originalDataset.DistributionArea = dataset.DistributionArea;
            originalDataset.Notes = dataset.Notes;
            originalDataset.ThemeGroupId = dataset.ThemeGroupId;
            originalDataset.datasetthumbnail = dataset.datasetthumbnail;

            originalDataset.modified = DateTime.Now;
            _registerItemService.SaveEditedRegisterItem(originalDataset);
        }

        private void SetDokStatusId(Dataset dataset, Dataset originalDataset)
        {
            if (dataset.dokStatusId != null)
            {
                originalDataset.statusId = dataset.statusId;
                if (originalDataset.statusId == "Accepted")
                {
                    if (dataset.dokStatusDateAccepted == null)
                    {
                        originalDataset.dokStatusDateAccepted = DateTime.Now;
                    }
                    else
                    {
                        originalDataset.dokStatusDateAccepted = dataset.dokStatusDateAccepted;
                    }
                }
                else
                {
                    originalDataset.dokStatusDateAccepted = null;
                }
            }
        }

        private Guid DatasetSubmitterId(Dataset dataset, Dataset originalDataset)
        {
            if (dataset.submitterId != null) return dataset.submitterId;
            else return originalDataset.submitterId;
        }

        private Guid DatasetOwnerId(Dataset dataset, Dataset originalDataset)
        {
            if (dataset.datasetownerId != null) return dataset.datasetownerId;
            else return originalDataset.datasetownerId;
        }

        private void SetDatasetOwnerAndSubmitter(Dataset dataset)
        {
            string organizationLogin = _accessControlService.GetSecurityClaim("organization");
            Organization submitterOrganisasjon = _registerService.GetOrganization(organizationLogin);
            dataset.submitterId = submitterOrganisasjon.systemId;
            dataset.submitter = submitterOrganisasjon;
            dataset.datasetowner = submitterOrganisasjon;
            dataset.datasetownerId = submitterOrganisasjon.systemId;
        }

        private Guid? GetVersioningId(Dataset dataset)
        {
            if (dataset.versioningId == null)
            {
                return _registerItemService.NewVersioningGroup(dataset);
            }
            else
            {
                return dataset.versioningId;
            }
        }

        private string DatasetSeoName(string name)
        {
            return Helpers.RegisterUrls.MakeSeoFriendlyString(name);
        }

        private string DatasetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "ikke angitt";
            }
            else
            {
                return name;
            }
        }

        private int GetVersionNr(int versionNumber)
        {
            if (versionNumber == 0)
            {
                versionNumber = 1;
            }
            else
            {
                versionNumber++;
            }
            return versionNumber;
        }


        private bool NameIsValid(Dataset dataset)
        {
            return _registerItemService.validateName(dataset);
        }

        private Dataset GetMetadataFromKartkatalogen(Dataset dataset, string uuid)
        {
            var model = new Dataset();
            try
            {
                new MetadataService().UpdateDatasetWithMetadata(model, uuid);
            }
            catch (Exception e)
            {
                TempData["error"] = "Det oppstod en feil ved henting av metadata: " + e.Message;
            }

            ViewBag.ThemeGroupId = new SelectList(db.DOKThemes, "value", "description", dataset.ThemeGroupId);
            ViewBag.statusId = new SelectList(db.Statuses, "value", "description");
            return model;
        }

        private void ValidationName(Dataset dataset, Kartverket.Register.Models.Register register)
        {
            var queryResultsDataset = from o in db.Datasets
                                      where o.name == dataset.name &&
                                      o.systemId != dataset.systemId &&
                                      o.register.name == register.name &&
                                      o.register.parentRegisterId == register.parentRegisterId
                                      select o.systemId;

            if (queryResultsDataset.Count() > 0)
            {
                ModelState.AddModelError("ErrorMessage", "Navnet finnes fra før!");
            }
        }

        private void Viewbags(Dataset dataset)
        {
            ViewBag.registerId = _registerItemService.GetRegisterSelectList(dataset.registerId);
            ViewBag.dokStatusId = _registerItemService.GetDokStatusSelectList(dataset.dokStatusId);
            ViewBag.submitterId = _registerItemService.GetSubmitterSelectList(dataset.submitterId);
            ViewBag.datasetownerId = _registerItemService.GetOwnerSelectList(dataset.datasetownerId);
            ViewBag.ThemeGroupId = _registerItemService.GetThemeGroupSelectList(dataset.ThemeGroupId);
            //ViewBag.registerId = new SelectList(db.Registers, "systemId", "name", dataset.registerId);
            //ViewBag.dokStatusId = new SelectList(db.DokStatuses.OrderBy(s => s.description), "value", "description", dataset.dokStatusId);
            //ViewBag.submitterId = new SelectList(db.Organizations.OrderBy(s => s.name), "systemId", "name", dataset.submitterId);
            //ViewBag.datasetownerId = new SelectList(db.Organizations.OrderBy(s => s.name), "systemId", "name", dataset.datasetownerId);
            //ViewBag.ThemeGroupId = new SelectList(db.DOKThemes, "value", "description", dataset.ThemeGroupId);
        }
    }
}
