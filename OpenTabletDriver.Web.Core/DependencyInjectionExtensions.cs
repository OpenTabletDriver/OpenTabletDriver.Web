using System;
using Microsoft.Extensions.DependencyInjection;

namespace OpenTabletDriver.Web.Core
{
    public static class DependencyInjectionExtensions
    {
        public static object CreateInstance(
            this IServiceProvider provider,
            Type instanceType,
            params object[] parameters
        )
        {
            return ActivatorUtilities.CreateInstance(provider, instanceType, parameters);
        }

        public static T CreateInstance<T>(this IServiceProvider provider, params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(provider, parameters);
        }
    }
}