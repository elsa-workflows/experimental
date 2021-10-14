using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Scripting.JavaScript
{
    public class JavaScriptExpression : IExpression
    {
        public JavaScriptExpression(string expression)
        {
            Expression = expression;
        }
        
        public string Expression { get; }
    }
    
    public class JavaScriptExpressionHandler : IExpressionHandler
    {
        public ValueTask<object?> EvaluateAsync(IExpression expression, ExpressionExecutionContext context)
        {
            var javaScriptExpression = (JavaScriptExpression)expression;
            
            // TODO: evaluate expression.
            return new ValueTask<object?>(javaScriptExpression.Expression);
        }
    }
}