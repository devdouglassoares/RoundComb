using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Core.UI.DataTablesExtensions
{
    public class DataTableModelBinderProvider : ModelBinderProvider
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return new DataTablesBinder();
        }
    }
}