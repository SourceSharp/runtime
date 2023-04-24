using System;
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
            var type = @params[i].GetType();
            if (type != @paramsType[i])
            {
                throw new BadImageFormatException("Bad parameter type: " + type.Name);
            }
        }
    }

    public static void SetStaticProtectedPropertyNoSetter(this Type type, string name, object instance, object value)
    {
        var field = type.GetField($"<{name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new MissingMemberException("Property not found.", name);

        field.SetValue(instance, value);
    }

    public static void SetProtectedReadOnlyField(this Type type, string name, object instance, object value)
    {
        var field = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new MissingFieldException("Field not found.", name);

        field.SetValue(instance, value);
    }
}
