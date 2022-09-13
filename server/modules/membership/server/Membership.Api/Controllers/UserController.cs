using Core.Exceptions;
using Core.ObjectMapping;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using DTWrapper.Core;
using DTWrapper.Core.DTModel;
using Membership.Api.Controllers.Base;
using Membership.Api.Extensions;
using Membership.Api.Models;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Exceptions;
using Membership.Core.Models;
using Membership.Core.Permissions;
using Membership.Library.Contracts;
using Membership.Library.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace Membership.Api.Controllers
{
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
    [ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
    [ErrorResponseHandler(typeof(PasswordResetRequestExpiredException), HttpStatusCode.BadRequest)]
    public class UserController : DTApiController<UserBaseModel>
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IMembership _membership;
        private readonly IMappingService _mappingService;
        private readonly IUserProfileService _profileService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IDataTableService _dataTableService;

        public UserController(IUserService userService, IPermissionService permissionService, IRoleService roleService,
                              IMembership membership, IMappingService mappingService, IUserProfileService profileService,
                              IUserRegistrationService userRegistrationService, IDataTableService dataTableService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _roleService = roleService;
            _membership = membership;
            _mappingService = mappingService;
            _profileService = profileService;
            _userRegistrationService = userRegistrationService;
            _dataTableService = dataTableService;
        }

        [HttpGet]
        public IList<UserBaseModel> Get()
        {
            return _userService.GetAllDtos().ToList();
        }

        [RequireAuthTokenApi, HttpGet, Route("~/api/user/mypersonalinfo")]
        public HttpResponseMessage GetMyPersonalInfo()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userService.GetUserPersonalInformation(_membership.UserId));
        }

        [HttpGet, HttpHead, Route("~/api/user/phonenumber")]
        public HttpResponseMessage GetUserByPhoneNumber(string phoneNumber)
        {
            var userByPhoneNumber = _userService.GetUserByPhoneNumber(phoneNumber);

            if (userByPhoneNumber == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, userByPhoneNumber, userByPhoneNumber?.ModifiedDate);
        }

        [RequireAuthTokenApi, HttpGet, Route("~/api/user/{userId}/personalinfo")]
        public HttpResponseMessage GetUserPersonalInfo(long userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userService.GetUserPersonalInformation(userId));
        }

        [RequireAuthTokenApi, HttpPost, Route("~/api/user/mypersonalinfo")]
        public HttpResponseMessage UpdateMyPersonalInfo(UserPersonalInformation model)
        {
            if (_membership.Email != model.Email &&
                !_membership.IsAccessAllowed(PermissionAuthorize.Feature(MembershipPermissions.EditUserEmail)))
            {
                throw new UnauthorizedAccessException("You don't have permission to modify user email.");
            }

            _userService.UpdateUserPersonalInformation(_membership.UserId, model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [RequireAuthTokenApi, HttpPost, Route("~/api/user/{userId}/personalinfo")]
        public HttpResponseMessage UpdateUserPersonalInfo(long userId, UserPersonalInformation model)
        {
            var user = _userService.GetUser(userId);
            if (user.Email != model.Email &&
                !_membership.IsAccessAllowed(PermissionAuthorize.Feature(MembershipPermissions.EditUserEmail)))
            {
                throw new UnauthorizedAccessException("You don't have permission to modify user email.");
            }

            _userService.UpdateUserPersonalInformation(userId, model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }



        [HttpPost, Route("~/api/user/{userId}/changePassword")]
        [RequireAuthTokenApi]
        [PermissionAuthorize(MembershipPermissions.ChangeUserPassword)]
        public HttpResponseMessage ChangePasswordForUser(long userId, ChangePasswordModel model)
        {
            if (model.NewPassword != model.ConfirmNewPassword)
                throw new InvalidOperationException("New password and confirm password must match");

            _membership.ChangePassword(userId, model.NewPassword);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [ResponseType(typeof(UserBaseModel))]
        public IHttpActionResult Get(long id)
        {
            var user = _userService.Get(id);

            return Ok(user);
        }

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

            _userService.Update(userModel);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [RequireAuthTokenApi, HttpPost, Route("user/CreateUser")]
        [ErrorResponseHandler(typeof(UserEmailAlreadyInUsedException), HttpStatusCode.BadRequest)]
        [ErrorResponseHandler(typeof(UserPhoneNumberAlreadyInUsedException), HttpStatusCode.BadRequest)]
        public HttpResponseMessage CreateUser(UserWithProfileModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (userModel.NoEmailNeeded)
            {
                // for client email and password are not required
                // check if email and password fields are empty
                // use default values
                if (string.IsNullOrEmpty(userModel.Password))
                {
                    userModel.Password =
                        WebConfigurationManager.AppSettings[MembershipConstant.C_DEFAULT_TEMP_PASSWORD_KEY];
                }

                if (string.IsNullOrEmpty(userModel.Email))
                {
                    userModel.Email = MembershipConstant.AUTOGENERATED_USER_EMAIL_PREFIX + Guid.NewGuid().ToString("N") + "@roundcomb.com";
                }
            }

            var baseModel = (userModel as UserBaseModel);
            if (baseModel.Roles == null || !baseModel.Roles.Any())
            {
                baseModel.Roles = new List<string>(new[] { MembershipConstant.C_ROLE_CODE_COMPANYCLIENT });
            }
            baseModel = _userService.Register(baseModel);

            if (baseModel == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var profileModel = _mappingService.Map<UserProfileModel>(userModel);
            _profileService.UpdateUserProfileForUser(baseModel.Id, profileModel);

            return Request.CreateResponse(HttpStatusCode.Created, baseModel);
        }

        [HttpPost]
        [ResponseType(typeof(UserBaseModel))]
        public IHttpActionResult Post(UserBaseModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userModel = _userService.Register(userModel);
            if (userModel == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("Api_default", new { id = userModel.Id }, userModel);
        }

        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            IHttpActionResult result = null;
            var errMsg = string.Empty;

            try
            {
                if (_userService.Delete(id))
                {
                    result = StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    result = NotFound();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex + " | Custom Msg : custom messages comes ";
                throw new Exception(errMsg);
            }

            return result;
        }

        [HttpPost, Route("user/requestResetPassword")]
        public HttpResponseMessage RequestResetPassword(ForgotPasswordModel model)
        {
            _userRegistrationService.ForgotPasswordRemind(model.Email, Request.RequestUri);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("user/resetPassword")]
        public HttpResponseMessage ResetPassword(ResetPasswordModel model)
        {
            _userRegistrationService.ResetPassword(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("user/contactus")]
        public HttpResponseMessage contactus(EmailContactUsModel model)
        {
           _userRegistrationService.SendEmailContactUs(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPost, Route("user/applycareer")]
        public HttpResponseMessage applycareer(EmailApplyCareerModel model)
        {
            _userRegistrationService.SendEmailApplyCareer(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("user/validatePasswordToken")]
        [ErrorResponseHandler(typeof(AccessViolationException), HttpStatusCode.BadRequest)]
        public HttpResponseMessage ValidatePasswordToken(ValidateResetPasswordTokenModel model)
        {
            var userEmail = _userRegistrationService.ValidatePasswordToken(model);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Email = userEmail
            });
        }

        [HttpGet]
        [Route("user/{userId}/permission")]
        public IList<PermissionModel> GetUserPermissions(long userId)
        {
            return _permissionService.GetForUser(userId).ToList();
        }

        [HttpPost]
        [Route("user/{userId}/permission/{permissionId}")]
        public IHttpActionResult AssignUserPermission(long userId, long permissionId)
        {
            if (_permissionService.IsAssignedToUser(userId, permissionId))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            if (_permissionService.AssignToUser(userId, permissionId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [HttpDelete]
        [Route("user/{userId}/permission/{permissionId}")]
        public IHttpActionResult UnassignUserPermission(long userId, long permissionId)
        {
            if (!_permissionService.IsAssignedToUser(userId, permissionId))
            {
                return NotFound();
            }

            if (_permissionService.RemoveFromUser(userId, permissionId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("user/{userId}/role")]
        public IList<RoleModel> GetUserRoles(long userId)
        {
            return _roleService.GetForUser(userId);
        }

        [HttpPost]
        [Route("user/{userId}/role/{roleId}")]
        public IHttpActionResult AssignUserRole(long userId, long roleId)
        {
            if (_roleService.IsAssignedTo(userId, roleId))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            if (_roleService.AssignToUser(userId, roleId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [HttpDelete]
        [Route("user/{userId}/role/{roleId}")]
        public IHttpActionResult UnassignUserRole(long userId, long roleId)
        {
            if (!_roleService.IsAssignedTo(userId, roleId))
            {
                return NotFound();
            }

            if (_roleService.RemoveFromUser(userId, roleId))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Route("user/{userId}/permissions/dataSource")]
        public HttpResponseMessage GetUserPermissionsDataSource(long userId, DataTablesModel model)
        {
            Action<DataTablesConfig<PermissionModel>> configure =
                config => config.Set.Properties(x => x.Id, x => x.Name, x => x.Type, x => x.RoleName);

            return this.DataSource(model, _permissionService.GetForUser(userId), configure);
        }

        [Route("api/user/getall")]
        [HttpGet]
        [RequireAuthTokenApi]
        public IHttpActionResult GetAll()
        {
            return
                Ok(
                    _userService.GetUsers()
                                .Select(
                                    x => new { x.FirstName, x.LastName, x.Email, x.Id, PhoneNumber = x.CellPhoneNumber }));
        }

        [Route("api/user/getroles")]
        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            return Ok(_userService.GetRoles().Select(x => new { x.Name, x.Id }));
        }

        [Route("api/user/getbyrole/{id}")]
        [HttpGet]
        public IHttpActionResult GetByRoles(long id)
        {
            return
                Ok(
                    _userService.GetUsersByRole(id)
                                .Select(
                                    x => new { x.FirstName, x.LastName, x.Email, x.Id, PhoneNumber = x.CellPhoneNumber }));
        }

        [Route("api/user/GetGroups")]
        [HttpGet]
        [RequireAuthTokenApi]
        public IHttpActionResult GetGroups()
        {
            return Ok(_userService.GetGroups().Select(x => new { x.Name, x.Id }));
        }

        [Route("api/user/GetByGroup/{id}")]
        [HttpGet]
        public IHttpActionResult GetByGroup(long id)
        {
            return
                Ok(
                    _userService.GetUsersByGroup(id)
                                .Select(
                                    x => new { x.FirstName, x.LastName, x.Email, x.Id, PhoneNumber = x.CellPhoneNumber }));
        }

        [HttpGet]
        [RequireAuthTokenApi]
        public IEnumerable<dynamic> Autocomplete(int page_limit, int page, string q = null, string roleCode = null, bool virtualUser = false)
        {
            //Filling user address information from UserProfile table
            if (_userService.IsEmptyAddresses())
            {
                _profileService.FillUserAddresses();
            }

            // From now we are showing all master company clients + all customers clients and staff
            if (!string.IsNullOrEmpty(q) && q.StartsWith("*"))
            {
                return new[] { new { Id = (long)-1, FirstName = "*", LastName = "ALL USERS" } };
            }
			
            var query = _userService.Fetch(user => !user.IsDeleted);

            if (!virtualUser)
            {
                query = query.Where(user => (string.IsNullOrEmpty(user.Email) ||
                                             !user.Email.Contains(MembershipConstant.AUTOGENERATED_USER_EMAIL_PREFIX)) &&
                                            user.IsVirtual == virtualUser);
            }

            //var query = _userService.GetAllClientsUsers().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(roleCode))
            {
                var roles = roleCode.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x => x.Roles.Any(r => roles.Contains(r.Code) || roles.Contains(r.Name)));
            }

            if (string.IsNullOrEmpty(q))
            {
                query = query.OrderBy(x => x.LastName + " " + x.FirstName + " " + x.Email + " " + x.CellPhoneNumber);
            }
            else
            {
                q = q.ToLower().Trim();
                var queries = q.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var phoneNumberSmartQuery = q.Replace("-", "").Replace("+", "");

                query = queries.Length > 1
                            // multiple query with space 
                            ? query.Where(x => queries.Any(que => x.Email.ToLower().Contains(que))
                                               ||
                                               (queries.Any(que => x.FirstName.ToLower().Contains(que)) &&
                                                queries.Any(que => x.LastName.ToLower().Contains(que)))
                                               ||
                                               x.CellPhoneNumber.ToLower()
                                                .Replace("-", "")
                                                .Replace("+", "")
                                                .Contains(phoneNumberSmartQuery)
                                               ||
                                               queries.Any(que => x.Address.ToLower().Contains(que)))
                                   .OrderBy(
                                       x =>
                                       SqlFunctions.PatIndex(q,
                                                             x.LastName + " " + x.FirstName + " " + x.Email + " " +
                                                             x.CellPhoneNumber))
                            // single query
                            : query.Where(x => x.Email.ToLower().Contains(q)
                                               || x.FirstName.ToLower().Contains(q)
                                               || x.LastName.ToLower().Contains(q)
                                               ||
                                               x.CellPhoneNumber.ToLower()
                                                .Replace("-", "")
                                                .Replace("+", "")
                                                .Contains(phoneNumberSmartQuery)
                                               ||
                                               x.Address.ToLower().Contains(q))
                                   .OrderBy(
                                       x =>
                                       SqlFunctions.PatIndex(q,
                                                             x.LastName + " " + x.FirstName + " " + x.Email + " " +
                                                             x.CellPhoneNumber));
                ;
            }

            return query
                .Skip((page - 1) * page_limit)
                .Take(page_limit)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    CompanyName = x.Company != null ? x.Company.Name : "",
                    Email = !x.IsVirtual ? (x.Email ?? "") : "",
                    FirstName = x.FirstName ?? "",
                    LastName = x.LastName ?? "",
                    CellPhoneNumber = x.CellPhoneNumber ?? ""
                });
        }


        [HttpPost, Route("users/datatables")]
		[RequireAuthTokenApi]
        public HttpResponseMessage GetUsers([ModelBinder(typeof(DataTableModelBinderProvider))]UserDataTableRequest requestModel)
        {
            var result = _userService.GetUsers();

            result = result.Where(x => x.IsDeleted == requestModel.ShowDeletedOnly);

            if (requestModel.ShowApprovalPendingOnly)
            {
                result = result.Where(x => x.IsApproved == false);
            }

            if (!string.IsNullOrEmpty(requestModel.Role))
            {
                result = result.Where(user => user.Roles.Any(role => role.Name == requestModel.Role));
            }

            if (requestModel.ClientCompanyId.HasValue)
            {
                result =
                    result.Where(
                        user =>
                            user.ClientCompany != null && user.ClientCompany.Id == requestModel.ClientCompanyId.Value);
            }

            var dtos = _mappingService.Project<User, UserBaseModel>(result);

            var response = _dataTableService.GetResponse(dtos, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        #region Data tables adapter

        protected override IQueryable<UserBaseModel> GetFilteredEntities()
        {
            return GetFilteredEntities(null);
        }

        protected override IQueryable<UserBaseModel> GetFilteredEntities(dynamic customFilter)
        {
            var customFilterDictionary = customFilter as Dictionary<string, object>;

            var query = _userService.QueryUsers(false);

            if (customFilterDictionary != null)
            {
                if (customFilterDictionary.ContainsKey("company"))
                {
                    var companyId = customFilterDictionary["company"] as int?;
                    if (companyId.HasValue)
                    {
                        query = query.Where(x => x.BizOwnerId == companyId);
                    }
                }

                if (customFilterDictionary.ContainsKey("role"))
                {
                    var selectedRole = customFilterDictionary["role"] as string;
                    if (!string.IsNullOrEmpty(selectedRole))
                    {
                        query = query.Where(x => x.Roles.Any(role => role.Contains(selectedRole)));
                    }
                }
            }

            return query;
        }

        protected override void TableCustomize(DataTablesConfig<UserBaseModel> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Id, x => x.BizCompanyName, x => x.RolesString, x => x.FirstName, x => x.LastName,
                x => x.Email, x => x.IsActive, x => x.FormattedCreatedDate);
        }

        #endregion Data tables adapter
    }
}