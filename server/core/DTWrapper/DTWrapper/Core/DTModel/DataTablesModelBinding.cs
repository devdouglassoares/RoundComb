namespace DTWrapper.Core.DTModel
{
    using DTWrapper.Core;
    using System;
    using System.Web.Mvc;

    public class DataTablesModelBinding : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return DTExtensions.BindModel(controllerContext.HttpContext.Request.Params);
        }
    }
}

