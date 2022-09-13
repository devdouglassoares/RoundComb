using Core.Database;
using Core.Exceptions;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Core.WebApi.Controllers
{
    public abstract class BaseCrudController<TEntity, TDto, TService> : BaseCrudController<TEntity, TDto, TService, DefaultDataTablesRequest>
        where TService : IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected BaseCrudController(TService crudService) : base(crudService)
        {
        }
    }


    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
    [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
    public abstract class BaseCrudController<TEntity, TDto, TService, TDatatableRequest> : ApiController
        where TService : IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
        where TDatatableRequest : DefaultDataTablesRequest
    {
        protected readonly TService CrudService;

        protected BaseCrudController(TService crudService)
        {
            CrudService = crudService;
        }

        [HttpGet, Route("")]
        public virtual HttpResponseMessage GetAll()
        {
            EntityPermissionCheck(null);
            var result = CrudService.GetAll();

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToQueryableDtos(result));
        }

        [HttpGet, Route("{id:long}")]
        [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
        public virtual HttpResponseMessage GetById(long id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, CrudService.GetDto(id));
        }

        [HttpPost, Route("")]
        public virtual HttpResponseMessage Create(TDto model)
        {
            var entity = CrudService.Create(model);

            var dto = CrudService.ToDto(entity);

            return Request.CreateResponse(HttpStatusCode.Created, dto);
        }

        [HttpPut, Route("{id:long}")]
        public virtual HttpResponseMessage Update(long id, TDto model)
        {
            EntityPermissionCheck(id);
            var entity = CrudService.Update(model, id);

            var dto = CrudService.ToDto(entity);
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        [HttpDelete, Route("{id:long}")]
        public virtual HttpResponseMessage Delete(long id)
        {
            EntityPermissionCheck(id);
            CrudService.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("datatables")]
        public virtual HttpResponseMessage GetAllForDatatable([ModelBinder(typeof(DataTableModelBinderProvider))] TDatatableRequest requestModel)
        {
            var result = CrudService.GetDataTableResponse(requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Check for user authorization of managing specific entity
        /// </summary>
        /// <param name="entityId">[Optional]Identifier of the entity</param>
        /// <exception cref="UnauthorizedAccessException">Throw exception if user is not authorized to perform some actiton</exception>
        protected virtual void EntityPermissionCheck(long? entityId)
        {
            
        }
    }
}
