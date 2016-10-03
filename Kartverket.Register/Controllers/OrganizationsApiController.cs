﻿using System.Web.Http;
using System.Web.Http.Cors;
using Kartverket.Register.Models;
using Kartverket.Register.Services;

namespace Kartverket.Register.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods:"*")]
    public class OrganizationsApiController : ApiController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationsApiController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [Route("api/organisasjon/navn/{name}")]
        public IHttpActionResult GetOrganizationByName(string name)
        {
            Organization organization = _organizationService.GetOrganizationByName(name);

            if (organization == null)
                return NotFound();

            var externalModel = new Models.Api.Organization();
            externalModel.Convert(organization);
            return Ok(externalModel);
        }

        [Route("api/organisasjon/orgnr/{number}")]
        public IHttpActionResult GetOrganizationByNumber(string number)
        {
            Organization organization = _organizationService.GetOrganizationByNumber(number);

            if (organization == null)
                return NotFound();

            var externalModel = new Models.Api.Organization();
            externalModel.Convert(organization);
            return Ok(externalModel);
        }

        [Route("api/v2/organisasjon/navn/{name}")]
        public IHttpActionResult GetOrganizationByNameV2(string name)
        {
            Organization organization = _organizationService.GetOrganizationByName(name);

            if (organization == null)
                return NotFound();

            var externalModel = new Models.Api.OrganizationV2();
            externalModel.Convert(organization);
            return Ok(externalModel);
        }

        [Route("api/v2/organisasjon/orgnr/{number}")]
        public IHttpActionResult GetOrganizationByNumberV2(string number)
        {
            Organization organization = _organizationService.GetOrganizationByNumber(number);

            if (organization == null)
                return NotFound();

            var externalModel = new Models.Api.OrganizationV2();
            externalModel.Convert(organization);
            return Ok(externalModel);
        }

    }
}
