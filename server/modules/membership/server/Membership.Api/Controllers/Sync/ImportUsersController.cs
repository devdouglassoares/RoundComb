using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Http;

namespace Membership.Api.Controllers.Sync
{
    [RequireAuthTokenApi]
    public class ImportUsersController : ApiController
    {
        private readonly IUserService userService;
        private readonly IMembership membership;

        public ImportUsersController(IUserService userService, IMembership membership)
        {
            this.userService = userService;
            this.membership = membership;
        }

        [HttpGet]
        public IEnumerable<dynamic> MergeAutocomplete(int page_limit, int page, string q = null)
        {
            var company = this.membership.GetCurrentBizOwner();

            var query = this.userService.QueryUsers(false).Where(u => u.BizOwnerId == company.Id && !string.IsNullOrEmpty(u.ExternalKey));

            if (string.IsNullOrEmpty(q))
            {
                query = query.OrderBy(x => "(" + x.ExternalKey + ") " + x.FirstName + " " + x.LastName);
            }
            else
            {
                q = q.ToLower().Trim();

                query = query.Where(x => ("(" + x.ExternalKey + ") " + x.FirstName + " " + x.LastName).ToLower().Contains(q))
                    .OrderBy(x => SqlFunctions.PatIndex(q, "(" + x.ExternalKey + ") " + x.FirstName + " " + x.LastName));
            }

            return query
                .Skip((page - 1) * page_limit)
                .Take(page_limit)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.ExternalKey,
                    x.FirstName,
                    x.LastName
                });
        }
    }
}