using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T? ToNullable<T>(this object obj) where T : struct
        {
            if (obj == null)
            {
                return default(T);
            }
            var objString = obj as string;
            if (objString != null)
            {
                return objString.ToNullable<T>();
            }
            var objType = typeof(T);
            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                objType = Nullable.GetUnderlyingType(objType);
            }
            return (T)Convert.ChangeType(obj, objType);
        }

        public static T Pop<T>(this List<T> list) where T : class
        {
            if (!list.Any())
                return null;

            var element = list.Last();
            list.RemoveAt(list.Count - 1);
            return element;
        }
    }

    public static class ExpressionExtensions
    {
        public static IEnumerable<PropertyInfo> GetSimplePropertyAccessList(this LambdaExpression propertyAccessExpression)
        {
            var propertyPaths
                = MatchPropertyAccessList(propertyAccessExpression, (p, e) => e.MatchSimplePropertyAccess(p));

            if (propertyPaths == null)
            {
                throw new InvalidOperationException();
            }

            return propertyPaths;
        }


        private static PropertyInfo MatchSimplePropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            var propertyPath = MatchPropertyAccess(parameterExpression, propertyAccessExpression).ToArray();

            return propertyPath != null && propertyPath.Count() == 1 ? propertyPath.FirstOrDefault() : null;
        }

        private static IEnumerable<PropertyInfo> MatchPropertyAccessList(
            this LambdaExpression lambdaExpression, Func<Expression, Expression, PropertyInfo> propertyMatcher)
        {

            var newExpression
                = RemoveConvert(lambdaExpression.Body) as NewExpression;

            if (newExpression != null)
            {
                var parameterExpression
                    = lambdaExpression.Parameters.Single();

                var propertyPaths
                    = newExpression.Arguments
                                   .Select(a => propertyMatcher(a, parameterExpression))
                                   .Where(p => p != null);

                if (propertyPaths.Count()
                    == newExpression.Arguments.Count())
                {
                    return newExpression.HasDefaultMembersOnly(propertyPaths) ? propertyPaths : null;
                }
            }

            var propertyPath = propertyMatcher(lambdaExpression.Body, lambdaExpression.Parameters.Single());

            return (propertyPath != null) ? new[] { propertyPath } : null;
        }



        private static bool HasDefaultMembersOnly(
            this NewExpression newExpression, IEnumerable<PropertyInfo> propertyPaths)
        {
            return !newExpression.Members
                                 .Where(
                                     (t, i) =>
                                         !string.Equals(t.Name, propertyPaths.ElementAt(i).Name, StringComparison.Ordinal))
                                 .Any();
        }

        public static IEnumerable<PropertyInfo> MatchPropertyAccess(
            this Expression parameterExpression,
            Expression propertyAccessExpression)
        {
            var propertyInfos = new List<PropertyInfo>();

            MemberExpression memberExpression;

            do
            {
                memberExpression = RemoveConvert(propertyAccessExpression) as MemberExpression;

                if (memberExpression == null)
                {
                    return null;
                }

                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo == null)
                {
                    return null;
                }

                propertyInfos.Insert(0, propertyInfo);

                propertyAccessExpression = memberExpression.Expression;
            } while (memberExpression.Expression != parameterExpression);

            return propertyInfos;
        }


        public static Expression RemoveConvert(this Expression expression)
        {
            while ((expression != null)
                   && (expression.NodeType == ExpressionType.Convert
                       || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }
    }
}