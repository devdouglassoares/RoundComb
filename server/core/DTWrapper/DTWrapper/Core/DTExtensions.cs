namespace DTWrapper.Core
{
    using DTWrapper.Core.DTModel;
    using System;
    using System.Collections.Specialized;
    using System.Net.Http.Formatting;
    using System.Web.Script.Serialization;

    public static class DTExtensions
    {
        public static DataTablesModel BindModel(NameValueCollection request)
        {
            DataTablesModel model = new DataTablesModel();
            if (request["customFilter"] != null)
            {
                model.customFilter = new JavaScriptSerializer().Deserialize<object>(request["customFilter"]);
            }
            return model;
        }

        public static DataTablesModel GetModelForApi(FormDataCollection formDataCollection)
        {
            return BindModel(formDataCollection.ReadAsNameValueCollection());
        }
    }
}

