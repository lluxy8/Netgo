using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Netgo.Persistence.Helper
{
    public class ExpressionParameterExtractor : ExpressionVisitor
    {
        private readonly Dictionary<string, object?> _values = new();

        public Dictionary<string, object?> Extract(Expression expression)
        {
            Visit(expression);
            return _values;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ConstantExpression constant)
            {
                var container = constant.Value;
                var value = node.Member switch
                {
                    System.Reflection.FieldInfo f => f.GetValue(container),
                    System.Reflection.PropertyInfo p => p.GetValue(container),
                    _ => null
                };
                _values[node.Member.Name] = value;
            }
            return base.VisitMember(node);
        }

    }
}
