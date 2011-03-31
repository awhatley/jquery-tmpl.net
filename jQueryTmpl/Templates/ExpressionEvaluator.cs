using System;
using System.Linq;
using System.Reflection;

namespace jQueryTmpl.Templates
{
    public class ExpressionEvaluator
    {
        private readonly TemplateItem _item;

        public ExpressionEvaluator(TemplateItem item)
        {
            _item = item;
        }

        public EvaluatedExpression Evaluate(Expression expression)
        {
            var value = EvaluateValue(expression);
            return new EvaluatedExpression(_item, value);
        }

        private object EvaluateValue(Expression expression)
        {
            var value = _item.Data;
            while(expression != null && value != null)
            {
                if(expression.Dollar)
                    value = _item;

                var memberInfo = FindMember(expression.Member, value);

                if(memberInfo.Length == 0 && value is TemplateItem)
                    memberInfo = FindMember(expression.Member,  value = ((TemplateItem)value).Options);

                if(memberInfo.Length == 0)
                    return EvaluateLiteral(expression);

                value = InvokeBestMatch(memberInfo, expression, value);
                expression = expression.Next;
            }

            return value;
        }

        private object EvaluateLiteral(Expression expression)
        {
            if(expression.Arguments.Length == 0 && expression.Indices.Length == 0 && expression.Next == null && expression.Dollar == false)
            {
                int number;
                if(Int32.TryParse(expression.Member, out number))
                    return number;

                if((expression.Member.StartsWith("\"") && expression.Member.EndsWith("\"")) || 
                   (expression.Member.StartsWith("'") && expression.Member.EndsWith("'")))
                    return expression.Member.Trim('"', '\'');
                
                if(_item.ValueParameter != null && _item.ValueParameter.Matches(expression.Member))
                    return _item.Value;

                if(_item.IndexParameter != null && _item.IndexParameter.Matches(expression.Member))
                    return _item.Index;
            }

            return null;
        }

        private object InvokeBestMatch(MemberInfo[] candidates, Expression expression, object target)
        {
            if(expression.Member == null)
                return null;

            foreach(var memberInfo in candidates)
            {
                var propertyInfo = memberInfo as PropertyInfo;
                if(propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(target, null);

                    if(expression.Arguments.Length > 0 && value is Delegate)
                    {
                        var func = (Delegate)value;
                        var paramValues = expression.Arguments.Select(EvaluateValue).ToArray();
                        return func.DynamicInvoke(paramValues);
                    }

                    if(expression.Indices.Length > 0)
                    {
                        if(value is Array)
                        {
                            var subIndices = expression.Indices.Select(EvaluateValue).Cast<int>().ToArray();
                            value = ((Array)value).GetValue(subIndices);
                        }

                        else if(value != null)
                        {
                            var indexer = value.GetType().GetProperty("Item") ?? value.GetType().GetProperty("Chars");
                            if(indexer != null)
                            {
                                var subIndices = expression.Indices.Select(EvaluateValue).ToArray();
                                value = indexer.GetValue(value, subIndices);
                            }
                        }
                    }

                    return value;
                }

                var fieldInfo = memberInfo as FieldInfo;
                if(fieldInfo != null)
                {
                    if(expression.Arguments.Length > 0)
                        continue;

                    if(expression.Indices.Length > 0)
                        continue;

                    return fieldInfo.GetValue(target);
                }

                var methodInfo = memberInfo as MethodInfo;
                if(methodInfo != null)
                {
                    var methodParameters = methodInfo.GetParameters();
                    if(methodParameters.Length == expression.Arguments.Length)
                    {
                        var paramValues = expression.Arguments.Select(EvaluateValue).ToArray();
                        var value = methodInfo.Invoke(target, paramValues);

                        if(expression.Indices.Length > 0)
                        {
                            if(value is Array)
                            {
                                var indices = expression.Indices.Select(EvaluateValue).Cast<int>().ToArray();
                                value = ((Array)value).GetValue(indices);
                            }

                            else if(value != null)
                            {
                                var indexer = value.GetType().GetProperty("Item");
                                if(indexer != null)
                                {
                                    var indices = expression.Indices.Select(EvaluateValue).ToArray();
                                    value = indexer.GetValue(value, indices);
                                }
                            }
                        }

                        return value;
                    }
                }
            }

            return null;
        }

        private MemberInfo[] FindMember(string name, object target)
        {
            return target.GetType().GetMember(name,
                MemberTypes.Property | MemberTypes.Field | MemberTypes.Method,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        }
    }
}