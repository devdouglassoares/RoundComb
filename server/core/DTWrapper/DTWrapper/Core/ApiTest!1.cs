namespace DTWrapper.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ApiTest<T>
    {
        public void Test<U>(Expression<Func<T, U>> expression)
        {
        }

        public void Test2(Expression<Func<T, object>> expression)
        {
            LambdaExpression expression2 = expression;
            NewArrayExpression body = expression.Body as NewArrayExpression;
            foreach (Expression expression4 in body.Expressions)
            {
                Expression expression5 = expression4;
            }
            MemberExpression expression6 = (MemberExpression) expression2.Body;
            PropertyInfo member = expression6.Member as PropertyInfo;
            if (member == null)
            {
                throw new InvalidOperationException("Expression must be a property reference.");
            }
        }

        public void Test3(params Expression<Func<T, object>>[] properties)
        {
            foreach (Expression<Func<T, object>> expression in properties)
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
            }
        }
    }
}

