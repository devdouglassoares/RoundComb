using System.Linq;

namespace DTWrapper.Core.DTConfig.PropertiesHandler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class PropertiesHandler
    {
        public object GetConcatedPropertiesValue(string name, object obj)
        {
            return string.Join<object>(" ", from x in name.Split(new char[] { '+' })
                                            select this.GetNestedPropertyValue(x, obj) into x
                                            where (x != null) && !string.IsNullOrEmpty(x.ToString())
                                            select x);
        }

        public string GetFullPropertyName(Expression expressionBody)
        {
            MemberExpression expression;
            if (!this.tryFindMemberExpression(expressionBody, out expression))
            {
                return string.Empty;
            }
            return this.GetFullPropertyName(expression);
        }

        public string GetFullPropertyName<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            MemberExpression expression;
            if (!this.tryFindMemberExpression(exp.Body, out expression))
            {
                return string.Empty;
            }
            return this.GetFullPropertyName(expression);
        }

        public string GetFullPropertyName(MemberExpression memberExp)
        {
            Stack<string> stack = new Stack<string>();
            do
            {
                stack.Push(memberExp.Member.Name);
            }
            while (this.tryFindMemberExpression(memberExp.Expression, out memberExp));
            return string.Join(".", stack.ToArray());
        }

        public object GetNestedPropertyValue(string name, object obj)
        {
            foreach (string str in name.Split(new char[] { '.' }))
            {
                if (obj == null)
                {
                    return null;
                }
                Type type = obj.GetType();
                if (obj is IEnumerable)
                {
                    MethodInfo method = (obj as IEnumerable).GetType().GetMethod("get_Item");
                    int num = int.Parse(str.Split(new char[] { '(' })[1].Replace(")", string.Empty));
                    try
                    {
                        obj = method.Invoke(obj, new object[] { num });
                    }
                    catch (Exception)
                    {
                        obj = null;
                    }
                }
                else
                {
                    PropertyInfo property = type.GetProperty(str);
                    if (property == null)
                    {
                        return null;
                    }
                    obj = property.GetValue(obj, null);
                }
            }
            return obj;
        }

        private bool isConversion(Expression exp)
        {
            return ((exp.NodeType == ExpressionType.Convert) || (exp.NodeType == ExpressionType.ConvertChecked));
        }

        private bool tryFindMemberExpression(Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                return true;
            }
            if (this.isConversion(exp) && (exp is UnaryExpression))
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

