using Core.ObjectMapping;
using Core.UI;
using Core.UI.DataTablesExtensions;
using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Api.Models;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Membership.Api.Controllers
{
    [RequireAuthTokenApi]
    public class BizUsersController : DTApiController<UserBaseModel>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMembership _membership;
        private readonly IDataTableService _dataTableService;
        private readonly IMappingService _mappingService;

        public BizUsersController(IUserService userService, IRoleService roleService, IMembership membership, IDataTableService dataTableService, IMappingService mappingService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._membership = membership;
            _dataTableService = dataTableService;
            _mappingService = mappingService;
        }

        // GET api/bizUsers
        //[Route("bizUsers")]
        [HttpGet]
        public IList<UserBaseModel> Get()
        {
            var bizOwner = _membership.GetCurrentBizOwner();
            if (bizOwner == null)
            {
                //throw new HttpException(403, "Only Biz Owner can access this data");
                return new List<UserBaseModel>();
            }

            //return this.userService.GetAll().Where(u => u.BizOwnerId == bizOwner.Id).ToList();
            return this._userService.QueryUsers(false).Where(u => u.BizOwnerId == bizOwner.Id).ToList();
        }

        // GET api/bizUsers/5
        //[Route("bizUsers/{id}")]
        [HttpGet]
        public UserBaseModel Get(long id)
        {
            return this._userService.Get(id);
        }

        [HttpPost, Route("bizusers/datatables")]
        public HttpResponseMessage GetUsers([ModelBinder(typeof(DataTableModelBinderProvider))]UserDataTableRequest requestModel)
        {
            var result = _userService.GetUsers();

            result = result.Where(x => x.IsDeleted == requestModel.ShowDeletedOnly)
                           .Where(x => x.IsVirtual == requestModel.ShowVirtualUsers);

            if (requestModel.ShowApprovalPendingOnly)
            {
                result = result.Where(x => x.IsApproved == false);
            }

            if (!string.IsNullOrEmpty(requestModel.Role))
            {
                result = result.Where(user => user.Roles.Any(role => role.Name == requestModel.Role));
            }

            if (requestModel.CompanyId.HasValue)
            {
                result = result.Where(user =>
                                      (user.Company != null && user.CompanyId == requestModel.CompanyId) ||
                                      (user.ClientCompany != null && user.ClientCompanyId == requestModel.CompanyId) // TODO: (Justin) Obsoleted ?
                    );
            }

            var dtos = _mappingService.Project<User, UserBaseModel>(result);

            var response = _dataTableService.GetResponse(dtos, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // PUT api/User/5
        //[Route("bizUsers/{id}")]
        [HttpPut]
        public IHttpActionResult Put(long id, UserBaseModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userModel.Id)
            {
                return BadRequest();
            }

            var user = this._userService.Get(id);
            if (userModel.Email != user.Email && this._userService.IsEmailExists(userModel.Email))
            {
                return BadRequest();
            }

            if (this._userService.Update(userModel))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return NotFound();
            }
        }

        //[Route("bizUsers")]
        [HttpPost]
        public IHttpActionResult Post(UserBaseModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(userModel.Email) && this._userService.IsEmailExists(userModel.Email))
            {
                return BadRequest();
            }

            var isClient = userModel.Roles != null && userModel.Roles.Any(r => r.Contains(MembershipConstant.C_ROLE_CODE_COMPANYCLIENT));
            if (isClient)
            {
                // for client email and password are not required
                // check if email and password fields are empty
                // use default values
                if (string.IsNullOrEmpty(userModel.Password))
                {
                    userModel.Password = WebConfigurationManager.AppSettings[MembershipConstant.C_DEFAULT_TEMP_PASSWORD_KEY];
                }

                if (string.IsNullOrEmpty(userModel.Email))
                {
                    userModel.Email = Guid.NewGuid().ToString("N").Substring(0, 5) + "@mail.com";
                }
            }
            var newUser = this._userService.Register(userModel);
            Role role;
            if (newUser == null)
            {
                return BadRequest();
            }

            if (isClient)
            {
                role = this._roleService.AllQueriable().FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYCLIENT);

                if (userModel.IsNeedInvite)
                {
                    // send email to client
                    string error;
                    if (!_membership.TryInviteUser(newUser.Id, out error))
                    {
                        return BadRequest(error);
                    }
                }
            }
            else
            {
                role = this._roleService.AllQueriable().FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYSTAFF);
            }

            this._roleService.AssignToUser(newUser.Id, role.Id);
            return Ok(new { id = newUser.Id });
        }

        //[Route("bizUsers/{userId}")]
        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            if (this._userService.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost, Route("bizUsers/{userId}/deactivate")]
        [PermissionAuthorize(MembershipPermissions.ActivateDeactivateUsers)]
        public HttpResponseMessage DeactivateUser(long userId)
        {
            _userService.DeactivateUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("bizUsers/{userId}/reactivate")]
        [PermissionAuthorize(MembershipPermissions.ActivateDeactivateUsers)]
        public HttpResponseMessage ReactivateUser(long userId)
        {
            _userService.ActivateUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("bizUsers/{userId}/undoDeletion")]
        [PermissionAuthorize(MembershipPermissions.UndoDeletionOfUser)]
        public HttpResponseMessage UndoDeletion(long userId)
        {
            _userService.UndoDeletion(userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, Route("bizUsers/checkEmail/{email}")]
        public IHttpActionResult CheckEmail(string email)
        {
            return Ok(new
            {
                exists = this._userService.IsEmailExists(email)
            });
        }

        #region Data tables adapter

        protected override IQueryable<UserBaseModel> GetFilteredEntities()
        {
            return this.GetFilteredEntities(null);
        }

        /*[HttpPost]
        public HttpResponseMessage Datatables([ModelBinder(typeof (DataTableModelBinderProvider))] UsersDataTableRequest requestModel)
        {
            var query = this.userService.QueryUsers();

            long companyId = 0;

            if (requestModel.CompanyId.HasValue)
            {
                companyId = requestModel.CompanyId.Value;
            }

            if (companyId == 0)
            {
                var company = this.membership.GetCurrentBizOwner();

                if (company != null)
                {
                    companyId = company.Id;
                }
            }

            if (companyId != 0)
            {
                query = query.Where(u => u.BizOwnerId == companyId);
            }

            if (requestModel.Role == MembershipConstant.C_ROLE_NAME_COMPANYCLIENT)
            {
                query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYCLIENT));
            }
            else if (requestModel.Role == MembershipConstant.C_ROLE_NAME_COMPANYSTAFF)
            {
                query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYSTAFF));
            }

            if (requestModel.ClientCompanyId.HasValue)
            {
                query = query.Where(x => x.ClientCompanyId == requestModel.ClientCompanyId.Value);
            }

            var result = _dataTableService.GetResponse(query.ProjectTo<CompanyShortDto>(), requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }*/

        protected override IQueryable<UserBaseModel> GetFilteredEntities(dynamic customFilter)
        {
            var query = this._userService.QueryUsers(false);

            var customFilterDictionary = customFilter as Dictionary<string, object>;

            long companyId = 0;

            if (customFilterDictionary != null)
            {
                if (customFilterDictionary.ContainsKey("companyId") && customFilterDictionary["companyId"] != null)
                {
                    long value;
                    long.TryParse(customFilterDictionary["companyId"].ToString(), out value);

                    if (value > 0)
                    {
                        //query = query.Where(x => x.ClientCompanyId == value);
                        companyId = value;
                    }
                }

                if (customFilterDictionary.ContainsKey("q") && customFilterDictionary["q"] != null)
                {
                    var q = customFilterDictionary["q"].ToString();
                    if (!string.IsNullOrEmpty(q))
                    {
                        query = query.Where(x => x.FirstName.Contains(q) || x.LastName.Contains(q) || x.Email.Contains(q) || x.CellPhoneNumber.Contains(q));
                    }
                }
            }

            if (companyId == 0)
            {
                var company = this._membership.GetCurrentBizOwner();

                if (company != null)
                {
                    companyId = company.Id;
                    //query = query.Where(u => u.BizOwnerId == company.Id /* && u.Roles.Any(r => r == "Biz Client")*/);
                }
            }

            if (companyId != 0)
            {
                query = query.Where(u => u.BizOwnerId == companyId /* && u.Roles.Any(r => r == "Biz Client")*/);
            }

            /*if (customFilterDictionary != null && customFilterDictionary.ContainsKey("isNewPatients"))
            {
                query = query.Where(x => string.IsNullOrEmpty(x.ExternalKey));
            }
            else
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.ExternalKey) && !x.ExternalKey.Contains("MERGED#"));
            }*/

            if (customFilterDictionary != null)
            {
                if (customFilterDictionary.ContainsKey("role"))
                {
                    if (customFilterDictionary["role"].ToString() == "clients")
                    {
                        query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYCLIENT)
                                                 && !string.IsNullOrEmpty(u.ExternalKey)
                                                 && !u.ExternalKey.Contains("MERGED#")
                                                 && !u.ExternalKey.Contains("TBD_BY_MBAC"));
                    }
                    else if (customFilterDictionary["role"].ToString() == "newPatients")
                    {
                        query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYCLIENT)
                                                 && (string.IsNullOrEmpty(u.ExternalKey)
                                                 || u.ExternalKey.Contains("TBD_BY_MBAC")));
                    }
                    else if (customFilterDictionary["role"].ToString() == "employees")
                    {
                        query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYSTAFF));
                    }
                    else if (customFilterDictionary["role"].ToString() == "users")
                    {
                        query =
                            query.Where(
                                u => u.Roles.Any(r => r == "Biz Owner") || u.Roles.Any(r => r == "Practice Admin"));
                    }
                }

                if (customFilterDictionary.ContainsKey("clientCompanyId"))
                {
                    long value;
                    long.TryParse(customFilterDictionary["clientCompanyId"].ToString(), out value);

                    if (value > 0)
                    {
                        query = query.Where(x => x.ClientCompanyId == value);
                    }
                }
            }

            return query;
        }

        protected override void TableCustomize(DataTablesConfig<UserBaseModel> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Id,
                x => x.CompanyName,
                x => x.FirstName,
                x => x.LastName,
                x => x.Email,
                x => x.CellPhoneNumber,
                x => x.RolesString,
                x => x.FormattedCreatedDate,
                x => x.IsApproved,
                x => x.IsActive);
        }

        #endregion Data tables adapter
    }
}
