namespace DTWrapper.Core
{
    using DTWrapper.Core.DTModel;
    using System;
    using System.Linq;
    using System.Linq.Dynamic;

    public class DataTableFilter
    {
        private string BuildConcatedStringCriteria(string columnName, string searchCriteria)
        {
            return Contains(this.Concat(columnName.Replace("+", ",\" \",")), searchCriteria);
        }

        private string BuildDateSelector(string columnName)
        {
            string str = Trim(this.StringConvert(this.DatePart("month", columnName)));
            string str2 = Trim(this.StringConvert(this.DatePart("day", columnName)));
            string str3 = Trim(this.StringConvert(this.DatePart("year", columnName)));
            return this.Concat(this.Concat(str, "\"/\""), this.Concat(str2, "\"/\""), str3);
        }

        private string BuildDatetimeCriteria(string columnName, string searchCriteria, bool isLongDate)
        {
            string str = this.BuildDateSelector(columnName);
            if (isLongDate)
            {
                string str2 = this.BuildTimeSelector(columnName);
                str = this.Concat(str, "\" \"", str2);
            }
            return Contains(str, searchCriteria);
        }

        private string BuildStringCriteria(string columnName, string searchCriteria)
        {
            if (columnName.Contains("+"))
            {
                return this.BuildConcatedStringCriteria(columnName, searchCriteria);
            }
            return Contains(columnName, searchCriteria);
        }

        private string BuildTimeSelector(string columnName)
        {
            string str = this.DatePart("hour", columnName);
            string str2 = Trim(this.StringConvert(string.Format("{0} = 0 ? 12 : {0} > 12 ? {0} - 12 : {0}", str)));
            string str3 = this.DatePart("minute", columnName);
            string str4 = string.Format("{0} > 9 ? {1} : {2}", str3, Trim(this.StringConvert(str3)), this.Concat("\"0\"", Trim(this.StringConvert(str3))));
            string str5 = this.DatePart("second", columnName);
            string str6 = string.Format("{0} > 9 ? {1} : {2}", str5, Trim(this.StringConvert(str5)), this.Concat("\"0\"", Trim(this.StringConvert(str5))));
            string str7 = string.Format("{0} < 12 ? \"AM\" : \"PM\"", str);
            string str8 = this.Concat(this.Concat(str2, "\":\""), this.Concat(str4, "\":\""), str6);
            return this.Concat(str8, "\" \"", str7);
        }

        private string Concat(string param)
        {
            return string.Format("String.Concat({0})", param);
        }

        private string Concat(string param1, string param2)
        {
            return string.Format("String.Concat({0}, {1})", param1, param2);
        }

        private string Concat(string param1, string param2, string param3)
        {
            return string.Format("String.Concat({0}, {1}, {2})", param1, param2, param3);
        }

        private string Concat(string param1, string param2, string param3, string param4)
        {
            return string.Format("String.Concat({0}, {1}, {2}, {3})", new object[] { param1, param2, param3, param4 });
        }

        private static string Contains(string str, string param)
        {
            return string.Format("({0} != null && {0}.ToLower().Contains(\"{1}\"))", str, param.Trim().ToLower());
        }

        private string DatePart(string part, string param)
        {
            return string.Format("SqlFunctions.DatePart(\"{0}\", {1})", part, param);
        }

        public IQueryable FilterPagingSortingSearch(DataTablesModel DTParams, IQueryable data, out int totalRecordsDisplay, string[] columnNames, Type[] types, bool[] flags)
        {
            string str;
            int num;
            string str2;
            if (!string.IsNullOrEmpty(DTParams.search.value))
            {
                str = string.Empty;
                bool flag = true;
                for (num = 0; num < DTParams.columns.Count; num++)
                {
                    if (DTParams.columns[num].searchable)
                    {
                        string str3;
                        str2 = columnNames[num];
                        if (types[num] == typeof(string))
                        {
                            str3 = this.BuildStringCriteria(str2, DTParams.search.value);
                        }
                        else if ((types[num] == typeof(DateTime)) || (types[num] == typeof(DateTime?)))
                        {
                            str3 = this.BuildDatetimeCriteria(str2, DTParams.search.value, flags[num]);
                        }
                        else if ((types[num] == typeof(bool)) || (types[num] == typeof(bool?)))
                        {
                            str3 = string.Empty;
                        }
                        else
                        {
                            str3 = Contains(this.StringConvert(str2), DTParams.search.value);
                        }
                        if (!string.IsNullOrEmpty(str3))
                        {
                            if (!flag)
                            {
                                str = str + " or ";
                            }
                            else
                            {
                                flag = false;
                            }
                            str = str + str3;
                        }
                    }
                }
                data = data.Where(str, new object[0]);
            }
            num = 0;
            while (num < DTParams.columns.Count)
            {
                string str4 = DTParams.columns[num].search.value;
                if (!string.IsNullOrEmpty(str4))
                {
                    str2 = columnNames[num];
                    if (types[num] == typeof(string))
                    {
                        str = this.BuildStringCriteria(str2, str4);
                    }
                    else if ((types[num] == typeof(DateTime)) || (types[num] == typeof(DateTime?)))
                    {
                        str = this.BuildDatetimeCriteria(str2, str4, flags[num]);
                    }
                    else if ((types[num] == typeof(bool)) || (types[num] == typeof(bool?)))
                    {
                        str = string.Empty;
                    }
                    else
                    {
                        str = Contains(this.StringConvert(str2), str4);
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        data = data.Where(str, new object[0]);
                    }
                }
                num++;
            }
            string ordering = "";
            for (num = 0; num < DTParams.order.Count; num++)
            {
                int column = DTParams.order[num].column;
                str2 = columnNames[column];
                if (str2.Contains("+"))
                {
                    str2 = this.Concat(str2.Replace("+", ","));
                }
                string dir = DTParams.order[num].dir;
                if (num != 0)
                {
                    ordering = ordering + ", ";
                }
                ordering = ordering + str2 + " " + dir;
            }
            totalRecordsDisplay = data.Count();
            data = data.OrderBy(ordering, new object[0]);
            if (DTParams.length == -1)
            {
                DTParams.length = totalRecordsDisplay;
            }
            data = data.Skip(DTParams.start).Take(DTParams.length);
            return data;
        }

        private string StringConvert(string param)
        {
            return string.Format("SqlFunctions.StringConvert({0})", param);
        }

        private static string Trim(string str)
        {
            return string.Format("{0}.Trim()", str);
        }
    }
}

