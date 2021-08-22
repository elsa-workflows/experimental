using System;
using System.Reflection;
using Elsa.Contracts;

namespace Elsa.Extensions
{
    public static class NodeDriverExtensions
    {
        public static TDelegate GetDelegate<TDelegate>(this INodeDriver driver, string methodName) where TDelegate : Delegate
        {
            var driverType = driver!.GetType();
            var resumeMethodInfo = driverType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
            return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), driver, resumeMethodInfo);
        }

        public static ExecuteNodeDelegate GetResumeNodeDelegate(this INodeDriver driver, string resumeMethodName) => driver.GetDelegate<ExecuteNodeDelegate>(resumeMethodName);
        public static NodeCompletionCallback GetNodeCompletionCallback(this INodeDriver driver, string completionMethodName) => driver.GetDelegate<NodeCompletionCallback>(completionMethodName);
    }
}