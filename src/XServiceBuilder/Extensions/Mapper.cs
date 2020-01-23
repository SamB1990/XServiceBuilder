using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace XServiceBuilderLibrary.Extensions
{
    /// <summary>
    /// Converts a ExpandoObject to a type of T 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Mapper<T> where T : class
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Dictionary<string, PropertyInfo> _propertyMap;

        static Mapper()
        {
            _propertyMap =
                typeof(T)
                .GetProperties()
                .ToDictionary(
                    p => p.Name.ToLower(),
                    p => p
                );
        }

        public static void Map(ExpandoObject source, T destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");

            foreach (var kv in source)
            {
                PropertyInfo p;
                if (_propertyMap.TryGetValue(kv.Key.ToLower(), out p))
                {
                    var val = kv.Value;
                    var propType = p.PropertyType;
                    if (kv.Value == null)
                    {
                        if (!propType.IsByRef && propType.Name != "Nullable`1")
                        {
                            throw new ArgumentException($"type mismatch: Type'{ destination.GetType().FullName }' could not map { kv.Key } as it is not nullable");
                        }
                    }
                    else if (kv.Value.GetType() != propType)
                    {
                        try
                        {
                            val = Convert.ChangeType(kv.Value, propType);
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException($"type mismatch: Type'{ destination.GetType().FullName }' could not map { kv.Key } as it could not convert { kv.Value.GetType().FullName } to { propType.FullName }", e);
                        }
                    }
                    p.SetValue(destination, val, null);
                }
            }
        }
    }
}

