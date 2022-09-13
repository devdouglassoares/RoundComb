using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Core.Services
{
	public class PermissionRegistration : IPermissionRegistration
    {
        private readonly ICoreRepository _repository;
        private readonly IEnumerable<IPermissionProvider> _permissionProviders;

        public PermissionRegistration(ICoreRepository repository, IEnumerable<IPermissionProvider> permissionProviders)
        {
            _repository = repository;
            _permissionProviders = permissionProviders;
        }

        public void Execute()
        {
            var permissions = _permissionProviders.SelectMany(x => x.Register());

            foreach (var permission in permissions)
            {
                RegisterPermission(permission);
            }
        }

        private void RegisterPermission(PermissionRegistrationModel model)
        {
            AccessModule accessModule = null;
            if (!string.IsNullOrEmpty(model.ModuleName))
            {
                accessModule = _repository.Get<AccessModule>(x => x.Name == model.ModuleName);
                if (accessModule == null)
                {
                    accessModule = new AccessModule
                    {
                        Name = model.ModuleName
                    };
                    _repository.Insert(accessModule);
                    _repository.SaveChanges();
                }
            }

            if (model.PermisionType == PermisionType.Feature)
            {
                RegisterFeature(model, accessModule);
            }
            else
            {
                RegisterPage(model, accessModule);
            }
        }

        private void RegisterPage(PermissionRegistrationModel model, AccessModule accessModule)
        {
            var siteUrl = _repository.Get<SiteUrl>(x => x.Url == model.Value);
            if (siteUrl != null)
            {
                siteUrl.Name = model.Name;
                _repository.Update(siteUrl);
                _repository.SaveChanges();
            }
            else
            {
                siteUrl = new SiteUrl
                {
                    Type = AccessEntityType.Page,
                    Url = model.Value,
                    Name = model.Name
                };
                if (accessModule != null)
                {
                    siteUrl.AccessModules = new List<AccessModule>
                    {
                        accessModule
                    };
                }
                _repository.Insert(siteUrl);
                _repository.SaveChanges();
            }
        }

        private void RegisterFeature(PermissionRegistrationModel model, AccessModule accessModule)
        {
            var feature = _repository.Get<Feature>(f => f.Code == model.Value);

            if (feature != null)
            {
                feature.Name = model.Name;
                _repository.Update(feature);
                _repository.SaveChanges();
            }

            else
            {
                feature = new Feature
                {
                    Type = AccessEntityType.Feature,
                    Code = model.Value,
                    Name = model.Name
                };

                if (accessModule != null)
                {
                    feature.AccessModules = new List<AccessModule>
                    {
                        accessModule
                    };
                }
                _repository.Insert(feature);
                _repository.SaveChanges();
            }
        }

        public bool ShouldRun()
        {
            return true;
        }
    }
}