using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace AllInOne.Integration.Tests.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToQueryString(this object obj)
        {
            if (!obj.GetType().IsComplex())
            {
                return obj.ToString();
            }

            var values = obj
                .GetType()
                .GetProperties()
                .Where(o => o.GetValue(obj, null) != null);

            var result = new QueryString();

            foreach (var value in values)
            {
                if (!typeof(string).IsAssignableFrom(value.PropertyType)
                    && typeof(IEnumerable).IsAssignableFrom(value.PropertyType))
                {
                    var items = value.GetValue(obj) as IList;
                    if (items.Count > 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            result = result.Add(value.Name, ToQueryString(items[i]));
                        }
                    }
                }
                else if (value.PropertyType.IsComplex())
                {
                    result = result.Add(value.Name, ToQueryString(value));
                }
                else
                {
                    result = result.Add(value.Name, value.GetValue(obj).ToString());
                }
            }

            return result.Value;
        }

        public static StringContent ToStringContent(this object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }

        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        private static bool IsComplex(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsComplex(typeInfo.GetGenericArguments()[0]);
            }
            return !(typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(Guid))
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal)));
        }
    }
}
