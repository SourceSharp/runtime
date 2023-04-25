using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SourceSharp.Core.Utils;

internal static class Reflection
{
    public static void CheckReturnAndParameters(this MethodInfo method, Type returnType, Type[] @paramsType)
    {
        if (method.ReturnParameter.ParameterType != typeof(void))
        {
            throw new BadImageFormatException("Bad return value: " + returnType.Name);
        }

        var @params = method.GetParameters();
        if (@params.Length != @paramsType.Length)
        {
            throw new BadImageFormatException("Parameters count mismatch");
        }

        for (var i = 0; i < @paramsType.Length; i++)
        {
            var type = @params[i].ParameterType;
            if (type != @paramsType[i])
            {
                throw new BadImageFormatException("Bad parameter type: " + type.Name);
            }
        }
    }

    public static void SetProtectedReadOnlyField(this Type type, string name, object instance, object value)
    {
        var field = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new MissingFieldException(type.FullName, name);

        field.SetValue(instance, value);
    }

    public static IEnumerable<T> GetAllServices<T>(this IServiceProvider provider)
    {
        var site = typeof(ServiceProvider).GetProperty("CallSiteFactory", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(provider)!;
        var desc = site.GetType().GetField("_descriptors", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(site) as ServiceDescriptor[];
        return desc!.Select(s => provider.GetRequiredService(s.ServiceType)).OfType<T>();
    }
}