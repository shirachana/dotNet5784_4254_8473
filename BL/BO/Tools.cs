using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BO;

public static class Tools
{
    //// Static extension method for ToStringProperty
    //public static string ToStringProperty<T>(this T obj)
    //{
    //    if (obj == null)
    //        return "null";

    //    Type type = obj.GetType();
    //    PropertyInfo[] properties = type.GetProperties();

    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendLine($"Type: {type.Name}");

    //    foreach (PropertyInfo property in properties)
    //    {
    //        sb.AppendLine($"{property.Name}: {GetValueString(property.GetValue(obj)!)}");
    //    }

    //    return sb.ToString();
    //}

    //// Helper method to get string representation of property value
    //private static string GetValueString(object value)
    //{
    //    if (value == null)
    //        return "null";

    //    if (value is IEnumerable enumerable)
    //    {
    //        // Handle collections
    //        StringBuilder sb = new StringBuilder();
    //        foreach (var item in enumerable)
    //        {
    //            sb.Append($"{GetValueString(item)}, ");
    //        }
    //        // Remove the trailing comma and space
    //        return $"[{sb.ToString().TrimEnd(',', ' ')}]";
    //    }

    //    // For other types, use ToString
    //    return value.ToString()!;
    //}
}
