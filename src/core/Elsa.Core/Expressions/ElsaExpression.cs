using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Expressions
{
    public class ElsaExpression : IExpression
    {
        public ElsaExpression(string expression)
        {
            Expression = expression;
        }
        
        public string Expression { get; }
    }
    
    public class ElsaExpressionHandler : IExpressionHandler
    {
        public ValueTask<object?> EvaluateAsync(IExpression expression, ExpressionExecutionContext context)
        {
            var elsaExpression = (ElsaExpression)expression;
            
            // TODO: evaluate expression.
            return new ValueTask<object?>(elsaExpression.Expression);
        }
    }
}