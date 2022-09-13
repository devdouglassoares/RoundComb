namespace DTWrapper.Core.DTConfig
{
    using DTWrapper.Core;
    using DTWrapper.Core.DTConfig.PropertiesHandler;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class DTConfigFluentInterface<T>
    {
        private readonly DataTablesConfig<T> dataTablesConfig;
        private readonly DTWrapper.Core.DTConfig.PropertiesHandler.PropertiesHandler propertiesHandler;

        public DTConfigFluentInterface(DataTablesConfig<T> dataTablesConfig)
        {
            this.dataTablesConfig = dataTablesConfig;
            this.propertiesHandler = new DTWrapper.Core.DTConfig.PropertiesHandler.PropertiesHandler();
        }

        private string BuildConcatedPropertyName(BinaryExpression expression)
        {
            if (expression.Left is BinaryExpression)
            {
                BinaryExpression left = expression.Left as BinaryExpression;
                return string.Format("{0}+{1}", this.BuildConcatedPropertyName(left), this.propertiesHandler.GetFullPropertyName(expression.Right));
            }
            return string.Format("{0}+{1}", this.propertiesHandler.GetFullPropertyName(expression.Left), this.propertiesHandler.GetFullPropertyName(expression.Right));
        }

        public DTcol<T> Column()
        {
            DTcol<T> item = new DTcol<T>();
            this.dataTablesConfig.Columns.Add(item);
            return item;
        }

        public void Properties(params Expression<Func<T, object>>[] properties)
        {
            foreach (Expression<Func<T, object>> expression in properties)
            {
                PropertyInfo member = null;
                if (expression.Body is MemberExpression)
                {
                    member = ((MemberExpression) expression.Body).Member as PropertyInfo;
                }
                else if (expression.Body is UnaryExpression)
                {
                    member = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member as PropertyInfo;
                }
                if (member != null)
                {
                    DTcol<T> item = new DTcol<T> {
                        Property = this.propertiesHandler.GetFullPropertyName<T, object>(expression),
                        Type = member.PropertyType,
                        Name = member.Name
                    };
                    this.dataTablesConfig.Columns.Add(item);
                }
                else if (expression.Body is BinaryExpression)
                {
                    BinaryExpression body = expression.Body as BinaryExpression;
                    if (body == null)
                    {
                        throw new Exception("Invalid binary expression");
                    }
                    string str = this.BuildConcatedPropertyName(body);
                    DTcol<T> tcol2 = new DTcol<T> {
                        Property = str,
                        Type = typeof(string),
                        Name = "ConcatedColumn"
                    };
                    this.dataTablesConfig.Columns.Add(tcol2);
                }
            }
        }
    }
}

