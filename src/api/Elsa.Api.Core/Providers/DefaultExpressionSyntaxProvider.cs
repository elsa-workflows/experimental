using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Models;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;

namespace Elsa.Api.Core.Providers
{
    public class DefaultExpressionSyntaxProvider : IExpressionSyntaxProvider
    {
        public ValueTask<IEnumerable<ExpressionSyntaxDescriptor>> GetDescriptorsAsync(CancellationToken cancellationToken = default)
        {
            var literal = new ExpressionSyntaxDescriptor
            {
                Syntax = "Literal",
                Type = typeof(LiteralExpression),
                CreateExpression = CreateLiteralExpression,
                CreateLocationReference = context => new Literal(context.GetExpression<LiteralExpression>().Value),
                CreateSerializableObject = context => new
                {
                    ExpressionType = "Literal",
                    Value = context.GetExpression<LiteralExpression>().Value
                }
            };

            return ValueTask.FromResult<IEnumerable<ExpressionSyntaxDescriptor>>(new[] { literal });
        }

        private IExpression CreateLiteralExpression(ExpressionConstructorContext context)
        {
            var expressionValue = context.Element.GetProperty("value").GetString();
            var expression = new LiteralExpression(expressionValue);
            return expression;
        }
    }
}