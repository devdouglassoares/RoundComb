namespace DTWrapper.Core
{
    using DTWrapper.Core.DTConfig;
    using DTWrapper.Core.DTConfig.PropertiesHandler;
    using DTWrapper.Core.DTModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class DataTablesConfig<T>
    {
        private readonly DTWrapper.Core.DTConfig.PropertiesHandler.PropertiesHandler propertiesHandler;
        private readonly DTConfigFluentInterface<T> set;

        public DataTablesConfig(IQueryable<T> dataSource)
        {
            this.propertiesHandler = new DTWrapper.Core.DTConfig.PropertiesHandler.PropertiesHandler();
            this.set = new DTConfigFluentInterface<T>((DataTablesConfig<T>) this);
            this.dataSource = dataSource;
            this.Columns = new List<DTcol<T>>();
        }

        public string BuildResponse(IList<T> listData, DataTablesModel param)
        {
            string str = string.Concat(new object[] { "{\"recordsTotal\":", this.TotalRecords, ",\"recordsFiltered\":", this.TotalRecordsDisplay, ",\"draw\":", param.draw, ",\"data\":[" });
            for (int i = 0; i < listData.Count; i++)
            {
                T item = listData[i];
                if (i > 0)
                {
                    str = str + ",";
                }
                str = str + this.getJsonResponseForItem(item);
            }
            return (str + "]}");
        }

        public IQueryable<T> GetData(DataTablesModel param)
        {
            int num;
            if (!this.Columns.Any<DTcol<T>>())
            {
                this.setDefaultCols();
            }
            IQueryable<T> dataSource = this.dataSource;
            this.TotalRecords = dataSource.Count();
            DataTableFilter filter = new DataTableFilter();
            IQueryable<T> queryable2 = filter.FilterPagingSortingSearch(param, dataSource, out num, (from x in this.Columns select x.Property).ToArray<string>(), (from x in this.Columns select x.Type).ToArray<Type>(), (from x in this.Columns select x.Flag).ToArray<bool>()) as IQueryable<T>;
            this.TotalRecordsDisplay = num;
            return queryable2;
        }

        private string getJsonResponseForItem(T item)
        {
            List<string> values = new List<string>();
            foreach (DTcol<T> tcol in this.Columns)
            {
                object propertyValue = this.GetPropertyValue(tcol, item);
                values.Add("\"" + ((propertyValue == null) ? string.Empty : HttpUtility.JavaScriptStringEncode(propertyValue.ToString())) + "\"");
            }
            return ("[" + string.Join(",", values) + "]");
        }

        private object GetPropertyValue(DTcol<T> columnInfo, T item)
        {
            if (columnInfo.Name == "ConcatedColumn")
            {
                return this.propertiesHandler.GetConcatedPropertiesValue(columnInfo.Property, item);
            }
            object nestedPropertyValue = this.propertiesHandler.GetNestedPropertyValue(columnInfo.Property, item);
            if ((columnInfo.Type == typeof(DateTime)) || (columnInfo.Type == typeof(DateTime?)))
            {
                DateTime? nullable = nestedPropertyValue as DateTime?;
                if (nullable.HasValue)
                {
                    string format = columnInfo.Flag ? "G" : "d";
                    return nullable.Value.ToString(format);
                }
            }
            return nestedPropertyValue;
        }

        public string ListAjax(DataTablesModel param)
        {
            IQueryable<T> data = this.GetData(param);
            return this.BuildResponse(data.ToList<T>(), param);
        }

        private void setDefaultCols()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo info in properties)
            {
                DTcol<T> item = new DTcol<T> {
                    Name = info.Name,
                    Type = info.PropertyType
                };
                this.Columns.Add(item);
            }
        }

        public IList<DTcol<T>> Columns { get; set; }

        private IQueryable<T> dataSource { get; set; }

        public DTConfigFluentInterface<T> Set
        {
            get
            {
                return this.set;
            }
        }

        public int TotalRecords { get; set; }

        public int TotalRecordsDisplay { get; set; }
    }
}

