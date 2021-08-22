using Elsa.Contracts;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Nodes.Primitives;

namespace Elsa.Samples.Console1.Workflows
{
    public static class BlockingWorkflow
    {
        public static INode Create()
        {
            return new Sequence
            {
                Nodes = new INode[]
                {
                    new WriteLine("Waiting for event..."),
                    new Sequence
                    {
                        Nodes = new INode[]
                        {
                            new Event("SomeEvent"), // Block here.
                            new Sequence
                            {
                                Nodes = new INode[]
                                {
                                    new WriteLine("Resumed!"),
                                    new WriteLine("!!!")
                                } 
                            }
                            
                        }
                    },
                    new WriteLine("Done")
                }
            };
        }
    }
}