using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using DTWrapper.Core;
using DTWrapper.Core.DTModel;
using Membership.Api.Extensions;

namespace Membership.Api.Controllers.Base
{
    public abstract class DTApiController<T> : ApiController where T : class
    {
        /// <summary>
        /// The data source.
        /// </summary>
        /// <param name="formData">
        /// The data table parameters.
        /// </param>
        /// <returns>
        /// The <see cref="System.Web.Mvc.ContentResult"/>.
        /// </returns>
        //[System.Web.Http.ActionName("datasource")]
        /*public virtual HttpResponseMessage DataSource(FormDataCollection formData)
        {
            return this.DataSource(formData, this.GetFilteredEntities(), this.TableCustomize);
        }*/

        public virtual HttpResponseMessage DataSource(DataTablesModel model)
        {
            IQueryable<T> query;
            if (model.customFilter != null && !string.IsNullOrEmpty(model.customFilter.ToString()))
            {
                var jss = new JavaScriptSerializer();
                dynamic customFilter = jss.Deserialize<dynamic>(model.customFilter.ToString());
                query = this.GetFilteredEntities(customFilter);
            }
            else
            {
                query = this.GetFilteredEntities();
            }

            return this.DataSource(model, query, this.TableCustomize);
        }

        /// <summary>
        /// The table customize. Should be overrided in controller to set columns
        /// </summary>
        /// <param name="dtCfg">
        /// The dt cfg.
        /// </param>
        protected virtual void TableCustomize(DataTablesConfig<T> dtCfg)
        {
        }

        /// <summary>
        /// get filtered entities as queryable object. Should be overrided in controller to set the db queryable object. Optional params for custom filters
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        protected virtual IQueryable<T> GetFilteredEntities(dynamic customFilter)
        {
            return customFilter == null ? this.GetFilteredEntities() : null;
        }

        /// <summary>
        /// get filtered entities as queryable object. Should be overrided in controller to set the db queryable object
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        protected virtual IQueryable<T> GetFilteredEntities()
        {
            return null;
        }
    }

    /*public class DataTablesModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<DataTablesOrderModel> order { get; set; }
        public DataTablesSearchModel search { get; set; }
        public List<DataTablesColumnModel> columns { get; set; }
    }

    public class DataTablesColumnModel
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool orderable { get; set; }
        public bool searchable { get; set; }
        public DataTablesSearchModel search { get; set; }
    }
    
    public class DataTablesOrderModel
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
    
    public class DataTablesSearchModel
    {
        public bool regex { get; set; }
        public string value { get; set; }
    }*/
}