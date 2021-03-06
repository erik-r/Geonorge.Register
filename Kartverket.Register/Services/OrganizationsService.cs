﻿using System.Linq;
using Kartverket.Register.Models;
using System.Collections.Generic;

namespace Kartverket.Register.Services
{
    public class OrganizationsService : IOrganizationService
    {
        private readonly RegisterDbContext _dbContext;

        public OrganizationsService(RegisterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Organization GetOrganizationByName(string name)
        {
            return _dbContext.Organizations.SingleOrDefault(o => o.name == name);
        }

        public Organization GetOrganizationByNumber(string number)
        {
            return _dbContext.Organizations.SingleOrDefault(o => o.number == number);
        }

        public Organization GetOrganization(string organizationName)
        {
            var queryResults = from o in _dbContext.Organizations
                               where o.name == organizationName
                               select o;

            Organization organization = queryResults.FirstOrDefault();
            return organization;
        }

        public List<Organization> GetMunicipalityOrganizations()
        {
            var queryResults = from o in _dbContext.Organizations
                               where o.OrganizationType == Models.OrganizationType.Municipality
                               select o;

            List<Organization> organizations = queryResults.ToList();
            return organizations;
        }
    }
}