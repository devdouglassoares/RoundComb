using DTWrapper.Core;
using DTWrapper.Core.DTModel;
using Membership.Api.Controllers.Base;
using Membership.Api.Extensions;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Membership.Api.Controllers
{
	public class RoleController : DTApiController<RoleModel>
	{
		private readonly IRoleService roleService;
		private readonly IPermissionService permissionService;

		public RoleController(IRoleService roleService, IPermissionService permissionService)
		{
			this.roleService = roleService;
			this.permissionService = permissionService;
		}

		[HttpGet]
		public IList<RoleModel> Get()
		{
			return this.roleService.GetAll().ToList();
		}

		[HttpGet]
		[Route("role/secureGetRoles")]
		public List<RoleModel> SecureGetRoles()
		{
			var systemCodeRoles = this.roleService.GetSystemRolesCodes();
			systemCodeRoles.Add(MembershipConstant.C_ROLE_CODE_SYSADMIN);

			return this.roleService.GetAll()
					   .Where(_ => !systemCodeRoles.Contains(_.Code))
					   .ToList();
		}

		[ResponseType(typeof(RoleModel))]
		public IHttpActionResult Get(long id)
		{
			var role = this.roleService.Get(id);
			if (role == null)
			{
				return NotFound();
			}

			return Ok(role);
		}

		[HttpPut]
		public IHttpActionResult Put(long id, RoleModel roleModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != roleModel.Id)
			{
				return BadRequest();
			}

			if (this.roleService.Update(roleModel))
			{
				return StatusCode(HttpStatusCode.NoContent);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public IHttpActionResult Post(RoleModel roleModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			roleModel = this.roleService.Add(roleModel);
			if (roleModel == null)
			{
				return BadRequest();
			}

			return Ok(new { Id = roleModel.Id });
		}

		[HttpDelete]
		public IHttpActionResult Delete(long id)
		{
			if (this.roleService.Delete(id))
			{
				return StatusCode(HttpStatusCode.NoContent);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpGet]
		[Route("role/{roleId}/permission")]
		public IList<PermissionModel> GetRolePermissions(long roleId)
		{
			return this.permissionService.GetForRole(roleId).ToList();
		}

		[HttpPost]
		[Route("role/{roleId}/permission/{permissionId}")]
		public IHttpActionResult AssignRolePermission(long roleId, long permissionId)
		{
			if (this.permissionService.IsAssignedToRole(roleId, permissionId))
			{
				return StatusCode(HttpStatusCode.BadRequest);
			}

			if (this.permissionService.AssignToRole(roleId, permissionId))
			{
				return StatusCode(HttpStatusCode.NoContent);
			}
			else
			{
				return StatusCode(HttpStatusCode.BadRequest);
			}
		}

		[HttpDelete]
		[Route("role/{roleId}/permission/{permissionId}")]
		public IHttpActionResult UnassignRolePermission(long roleId, long permissionId)
		{
			if (!this.permissionService.IsAssignedToRole(roleId, permissionId))
			{
				return NotFound();
			}

			if (this.permissionService.RemoveFromRole(roleId, permissionId))
			{
				return StatusCode(HttpStatusCode.NoContent);
			}
			else
			{
				return StatusCode(HttpStatusCode.BadRequest);
			}
		}

		[HttpGet]
		[Route("role/systemRoles")]
		public IHttpActionResult GetSystemRoles()
		{
			return Ok(roleService.GetSystemRolesCodes());
		}

		[HttpPost]
		[Route("role/{roleId}/permissions/dataSource")]
		public HttpResponseMessage GetRolePermissionsDataSource(long roleId, DataTablesModel model)
		{
			Action<DataTablesConfig<PermissionModel>> configure =
				config => config.Set.Properties(x => x.Id, x => x.Name, x => x.Type);

			return this.DataSource(model, this.permissionService.GetForRole(roleId), configure);
		}

		#region Data tables adapter

		protected override IQueryable<RoleModel> GetFilteredEntities()
		{
			return this.roleService.GetAll();
		}

		protected override void TableCustomize(DataTablesConfig<RoleModel> dtCfg)
		{
			dtCfg.Set.Properties(x => x.Id, x => x.Name, x => x.Description);
		}

		#endregion Data tables adapter
	}
}