using System;
using System.Reflection;
using Elsa.Contracts;

namespace Elsa.Extensions
{
    public static class NodeDriverExtensions
    {
        public static TDelegate GetDelegate<TDelegate>(this IActivityDriver driver, string methodName) where TDelegate : Delegate
        {
            var driverType = driver!.GetType();
            var resumeMethodInfo = driverType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
            return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), driver, resumeMethodInfo);
        }

        public static ExecuteActivityDelegate GetResumeNodeDelegate(this IActivityDriver driver, string resumeMethodName) => driver.GetDelegate<ExecuteActivityDelegate>(resumeMethodName);
        public static ActivityCompletionCallback GetNodeCompletionCallback(this IActivityDriver driver, string completionMethodName) => driver.GetDelegate<ActivityCompletionCallback>(completionMethodName);
    }
}