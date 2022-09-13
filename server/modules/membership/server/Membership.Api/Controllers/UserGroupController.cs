using Core.UI;
using Core.UI.DataTablesExtensions;
using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Membership.Api.Controllers
{
	public class UserGroupController : DTApiController<GroupModel>
	{
		private readonly IDataTableService _datatableService;
		private readonly IUserGroupService _userGroupService;

		public UserGroupController(IUserGroupService userGroupService, IDataTableService datatableService)
		{
			_userGroupService = userGroupService;
			_datatableService = datatableService;
		}

		[HttpGet]
		public GroupModel Get(long id)
		{
			return _userGroupService.QueryGroups().FirstOrDefault(x => x.Id == id);
		}

		[Route("api/userGroup/getAll")]
		[HttpGet]
		[RequireAuthTokenApi]
		public IHttpActionResult GetAll()
		{
			return Ok(_userGroupService.QueryGroups().Select(x => new { x.Name, x.Id }));
		}

		[HttpGet]
		[RequireAuthTokenApi]
		public IEnumerable<dynamic> Autocomplete(int page_limit, int page, string q = null)
		{
			var query = _userGroupService.QueryGroups();

			if (string.IsNullOrEmpty(q))
			{
				query = query.OrderBy(x => x.Name);
			}
			else
			{
				q = q.ToLower().Trim();

				query = query.Where(x => x.Name.ToLower().Contains(q))
							 .OrderBy(x => SqlFunctions.PatIndex(q, x.Name));
			}

			return query
				.Skip((page - 1) * page_limit)
				.Take(page_limit)
				.ToList()
				.Select(x => new { x.Name, x.Id, Users = x.Users.Select(user => user.Email) });
		}

		[Route("api/userGroup/getUsers/{id}")]
		[HttpGet]
		[RequireAuthTokenApi]
		public IHttpActionResult GetUsers(long id)
		{
			var group = _userGroupService.QueryGroups().FirstOrDefault(x => x.Id == id);
			if (group == null)
			{
				return BadRequest();
			}

			return Ok(group.Users);
		}

		[HttpPost]
		public IHttpActionResult Post(GroupModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			model = _userGroupService.Save(model);
			if (model == null)
			{
				return BadRequest();
			}

			return Ok(new { model.Id });
		}

		[HttpPut]
		public IHttpActionResult Put(long id, GroupModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != model.Id)
			{
				return BadRequest();
			}

			if (_userGroupService.Save(model) != null)
			{
				return StatusCode(HttpStatusCode.NoContent);
			}
			return NotFound();
		}

		[HttpDelete]
		public IHttpActionResult Delete(long id)
		{
			if (_userGroupService.Delete(id))
			{
				return StatusCode(HttpStatusCode.NoContent);
			}
			return NotFound();
		}

		[HttpPost]
		public HttpResponseMessage Datatables(
			[ModelBinder(typeof(DataTableModelBinderProvider))] DefaultDataTablesRequest requestModel)
		{
			var query = _userGroupService.QueryGroups();

			var result = _datatableService.GetResponse(query, requestModel);

			return Request.CreateResponse(HttpStatusCode.OK, result);
		}

		#region Data tables adapter

		protected override IQueryable<GroupModel> GetFilteredEntities()
		{
			return _userGroupService.QueryGroups();
		}

		protected override void TableCustomize(DataTablesConfig<GroupModel> dtCfg)
		{
			dtCfg.Set.Properties(x => x.Id, x => x.Name);
		}

		#endregion Data tables adapter
	}
}