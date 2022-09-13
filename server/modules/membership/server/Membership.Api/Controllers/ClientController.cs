using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Api.Controllers
{
    public class ClientController : DTApiController<UserBaseModel>
    {
        private readonly IMembership _membership;
        private readonly IUserService _userService;

        public ClientController(IUserService userService, IMembership membership)
        {
            _userService = userService;
            _membership = membership;
        }

        #region Data tables adapter

        protected override IQueryable<UserBaseModel> GetFilteredEntities()
        {
            var bizOwner = _membership.GetCurrentBizOwner();
            if (bizOwner == null)
            {
                //throw new HttpException(403, "Only Biz Owner can access this data");
                return new List<UserBaseModel>().AsQueryable();
            }

            var filteredEntities = _userService.Fetch(x => x.Company != null && x.Company.Id == bizOwner.Id);
            return _userService.ToQueryableDtos(filteredEntities);
        }

        protected override void TableCustomize(DataTablesConfig<UserBaseModel> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Id, x => x.FirstName + x.LastName, x => x.Email);
        }

        #endregion Data tables adapter
    }
}