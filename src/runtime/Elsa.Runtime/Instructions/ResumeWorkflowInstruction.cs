using Elsa.Persistence.Abstractions.Entities;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Instructions;

public record ResumeWorkflowInstruction(WorkflowBookmark WorkflowBookmark) : IWorkflowInstruction;