using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.InstructionResults
{
    public class NullInstructionResult : IWorkflowInstructionResult
    {
        public static NullInstructionResult Instance { get; } = new();

        private NullInstructionResult()
        {
        }
    }
}