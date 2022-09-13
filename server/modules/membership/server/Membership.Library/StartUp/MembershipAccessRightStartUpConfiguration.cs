using Core.StartUp;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Library.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Membership.Library.StartUp
{
    public class MembershipAccessRightStartUpConfiguration : IApplicationStartUpExecution
    {
        private readonly IRepository _repository;

        private readonly List<AccessRight> _accessRights = new List<AccessRight>
            {
                new AccessRight
                {
                    AccessKind = AccessKind.Read,
                    AccessRightName = "Read",
                    Priority = 1
                },
                new AccessRight
                {
                    AccessKind = AccessKind.Write,
                    AccessRightName = "Write",
                    Priority = 1
                },
                new AccessRight
                {
                    AccessKind = AccessKind.Deny,
                    AccessRightName = "Deny",
                    Priority = 1
                },
                new AccessRight
                {
                    AccessKind = AccessKind.Other,
                    AccessRightName = "Other",
                    Priority = 1
                }
            };

        public MembershipAccessRightStartUpConfiguration(IRepository repository)
        {
            _repository = repository;
        }

        public bool ShouldRun()
        {
            var accessKinds = _accessRights.Select(access => access.AccessKind);

            var availableAccessKinds = _repository.GetAll<AccessRight>().Select(x => x.AccessKind);

            // should run if any access kind is not available;
            var shouldRun = accessKinds.Any(accessKind => !availableAccessKinds.Contains(accessKind));

            return shouldRun;
        }

        public void Execute()
        {
            foreach (var item in _accessRights)
            {
                if (!_repository.Any<AccessRight>(x => x.AccessKind == item.AccessKind))
                {
                    _repository.Insert(item);
                }
            }
            _repository.SaveChanges();
        }
    }
}