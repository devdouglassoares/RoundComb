using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Api.Models;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using Membership.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Http;

namespace Membership.Api.Controllers.Sync
{
    [RequireAuthTokenApi]
    public class SyncBizUsersController : DTApiController<UserBaseModel>
    {
        private readonly IUserService userService;
        private readonly IPermissionService permissionService;
        private readonly IRoleService roleService;
        private readonly IMembership membership;

        public SyncBizUsersController(IUserService userService, IPermissionService permissionService, IRoleService roleService, IMembership membership)
        {
            this.userService = userService;
            this.permissionService = permissionService;
            this.roleService = roleService;
            this.membership = membership;
        }

        // GET api/bizUsers
        //[Route("bizUsers")]
        [HttpGet]
        public IList<UserBaseModel> Get()
        {
            var bizOwner = membership.GetCurrentBizOwner();
            if (bizOwner == null)
            {
                //throw new HttpException(403, "Only Biz Owner can access this data");
                return new List<UserBaseModel>();
            }

            return this.userService.GetAll().Where(u => u.BizOwnerId == bizOwner.Id).Take(100).ToList();
        }

        // GET api/bizUsers/5
        //[Route("bizUsers/{id}")]
        [HttpGet]
        public UserBaseModel Get(long id)
        {
            return this.userService.Get(id);
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

            var user = this.userService.Get(id);
            if (userModel.Email != user.Email && this.userService.IsEmailExists(userModel.Email))
            {
                return BadRequest();
            }

            if (this.userService.Update(userModel))
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

            if (!string.IsNullOrEmpty(userModel.Email) && this.userService.IsEmailExists(userModel.Email))
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
            var newUser = this.userService.Register(userModel);

            Role role;
            if (newUser == null)
            {
                return BadRequest();
            }

            if (isClient)
            {
                role = this.roleService.AllQueriable().FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYCLIENT);

                if (userModel.IsNeedInvite)
                {
                    // send email to client
                    string error;
                    if (!membership.TryInviteUser(newUser.Id, out error))
                    {
                        return BadRequest(error);
                    }
                }
            }
            else
            {
                if (userModel.Roles.Any(r => r.Contains(MembershipConstant.C_ROLE_CODE_COMPANYSTAFF)))
                {
                    role =
                        this.roleService.AllQueriable()
                            .FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYSTAFF);
                }
                else //if (userModel.Roles.Any(r => r.Contains(MembershipConstant.C_ROLE_CODE_PRACTICEADMIN)))
                {
                    role =
                        this.roleService.AllQueriable()
                            .FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_PRACTICEADMIN);
                }
            }

            this.roleService.AssignToUser(newUser.Id, role.Id);
            return Ok(new { id = newUser.Id });
        }

        //[Route("bizUsers/{userId}")]
        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            if (this.userService.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
                                    
        #region Data tables adapter

        protected override IQueryable<UserBaseModel> GetFilteredEntities()
        {
            return this.GetFilteredEntities(null);
        }

        protected override IQueryable<UserBaseModel> GetFilteredEntities(dynamic customFilter)
        {
            var company = this.membership.GetCurrentBizOwner();

            var query = this.userService.QueryUsers(getDeletedOnly:false);

            if (company != null)
            {
                query = query.Where(u => u.BizOwnerId == company.Id /* && u.Roles.Any(r => r == "Biz Client")*/);
            }

            var customFilterDictionary = customFilter as Dictionary<string, object>;

            if (customFilterDictionary != null)
            {
                if (customFilterDictionary.ContainsKey("role"))
                {
                    if (customFilterDictionary["role"].ToString() == "clients")
                    {
                        query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYCLIENT)
                                              //   && !string.IsNullOrEmpty(u.ExternalKey)
                                              //   && !u.ExternalKey.Contains("MERGED#")
                                              //   && !u.ExternalKey.Contains("TBD_BY_MBAC")
                                              );
                    }
                    else if (customFilterDictionary["role"].ToString() == "newPatients")
                    {
                        query = query.Where(u => u.Roles.Any(r => r == MembershipConstant.C_ROLE_NAME_COMPANYCLIENT)
                                                 && (string.IsNullOrEmpty(u.ExternalKey)
                                                 || u.ExternalKey.Contains("TBD_BY_MBAC")));
                    }
                    /*if (customFilterDictionary["role"].ToString() == "clients")
                {
                    query = query.Where(u => u.Roles.Any(r => r == "Biz Client"));

                    if (customFilterDictionary.ContainsKey("isNewPatients"))
                    {
                        query = query.Where(x => string.IsNullOrEmpty(x.ExternalKey));
                    }
                    else
                    {
                        query = query.Where(x => !string.IsNullOrEmpty(x.ExternalKey) && !x.ExternalKey.Contains("MERGED#"));
                    }
                }*/
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
            }
            else
            {
                return new List<UserBaseModel>().AsQueryable();
            }

            return query;
        }

        protected override void TableCustomize(DataTablesConfig<UserBaseModel> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Id, x => x.ExternalKey, x => x.FirstName, x => x.LastName, x => x.Email, x => x.Address, x => x.CellPhoneNumber);
            //dtCfg.Set.Properties(x => x.Id, x => x.CompanyName, x => x.FirstName, x => x.LastName, x => x.Email, x => x.PhoneNumber, x => x.RolesString);
        }

        #endregion Data tables adapter

        [Route("bizUsers/PostPatient")]
        [HttpPost]
        public IHttpActionResult PostPatient(UserBaseModel userModel)
        {
            userModel = this.userService.Register(userModel);
            Role role = this.roleService.AllQueriable().FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYCLIENT);
            this.roleService.AssignToUser(userModel.Id, role.Id);
            return Ok(new { id = userModel.Id });
        }

        [Route("bizUsers/MergePatient")]
        [HttpPost]
        public IHttpActionResult MergePatient(MergePatientModel model)
        {
            this.userService.MergeUsers(model.Id, model.MergeWithId);
            return Ok(new { });
        }
    }
}
