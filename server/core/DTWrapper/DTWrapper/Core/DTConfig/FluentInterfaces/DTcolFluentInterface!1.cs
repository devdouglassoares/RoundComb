namespace DTWrapper.Core.DTConfig.FluentInterfaces
{
    using DTWrapper.Core.DTConfig;
    using DTWrapper.Core.DTConfig.PropertiesHandler;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class DTcolFluentInterface<T>
    {
        private readonly DTcol<T> dTcol;
        private DTWrapper.Core.DTConfig.PropertiesHandler.PropertiesHandler propertiesHandler;

        public DTcolFluentInterface(DTcol<T> dTcol)
        {
            this.dTcol = dTcol;
            this.propertiesHandler = new DTWrapper.Core.DTConfig.PropertiesHandler.PropertiesHandler();
        }

        public DTcolFluentInterface<T> Name(string name)
        {
            this.dTcol.Name = name;
            return (DTcolFluentInterface<T>) this;
        }

        public DTcolFluentInterface<T> Property(Expression<Func<T, object>> expression)
        {
            PropertyInfo member = null;
            if (expression.Body is MemberExpression)
            {
                member = ((MemberExpression) expression.Body).Member as PropertyInfo;
            }
            else
            {
                member = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member as PropertyInfo;
            }
            this.dTcol.Property = this.propertiesHandler.GetFullPropertyName<T, object>(expression);
            this.dTcol.Type = member.PropertyType;
            return (DTcolFluentInterface<T>) this;
        }
    }
}

