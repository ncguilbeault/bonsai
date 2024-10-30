using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bonsai
{
    static class ReflectionHelper
    {
static object l = new object();
static HashSet<Type> scanned = new();

        public static CustomAttributeData[] GetCustomAttributesData(this Type type, bool inherit)
        {
            lock (l)
            {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var attributeLists = new List<IList<CustomAttributeData>>();
            while (type != null)
            {
                //if (scanned.Add(type))
                //    Console.WriteLine($"Checking {type.FullName}");
                attributeLists.Add(type.GetCustomAttributesData());
                try
                {
                type = inherit ? type.BaseType : null;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Ex while getting base of {type.FullName}: {ex}");
                    throw;
                }
            }

            var offset = 0;
            var count = attributeLists.Sum(attributes => attributes.Count);
            var result = new CustomAttributeData[count];
            for (int i = 0; i < attributeLists.Count; i++)
            {
                attributeLists[i].CopyTo(result, offset);
                offset += attributeLists[i].Count;
            }

            return result;
            }
        }

        public static IEnumerable<CustomAttributeData> OfType<TAttribute>(this IEnumerable<CustomAttributeData> customAttributes)
        {
            var attributeTypeName = typeof(TAttribute).FullName;
            return customAttributes.Where(
                attribute => attribute.Constructor.DeclaringType.FullName == attributeTypeName);
        }

        public static bool IsDefined(this CustomAttributeData[] customAttributes, Type attributeType)
        {
            return GetCustomAttributeData(customAttributes, attributeType) != null;
        }

        public static CustomAttributeData GetCustomAttributeData(
            this CustomAttributeData[] customAttributes,
            Type attributeType)
        {
            if (customAttributes == null)
            {
                throw new ArgumentNullException(nameof(customAttributes));
            }

            return Array.Find(
                customAttributes,
                attribute => attribute.Constructor.DeclaringType.FullName == attributeType.FullName);
        }

        public static object GetConstructorArgument(this CustomAttributeData attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.ConstructorArguments.Count > 0 ? attribute.ConstructorArguments[0].Value : null;
        }

        public static bool IsMatchSubclassOf(this Type type, Type baseType)
        {
            var typeName = baseType.AssemblyQualifiedName;
            if (type.AssemblyQualifiedName == typeName)
            {
                return false;
            }

            while (type != null)
            {
                if (type.AssemblyQualifiedName == typeName)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }
}
