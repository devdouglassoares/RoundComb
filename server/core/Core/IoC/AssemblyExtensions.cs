using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.IoC
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Assembly> FilterNotGacAssemblies(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.Where(x => !x.FullName.StartsWith("System") &&
                                         !x.FullName.StartsWith("Microsoft") &&
                                         !x.FullName.StartsWith("mscorlib") &&
                                         !x.FullName.StartsWith("Antlr") &&
                                         !x.FullName.StartsWith("WebGrease") &&
                                         !x.FullName.StartsWith("Owin") &&
                                         !x.FullName.StartsWith("EntityFramework") &&
                                         !x.FullName.StartsWith("Newtonsoft") &&
                                         !x.FullName.StartsWith("Autofac") &&
                                         !x.FullName.Contains("Anonymously") &&
                                         !x.FullName.StartsWith("CommonServiceLocator") &&
                                         !x.FullName.StartsWith("AutoMapper") &&
                                         !x.FullName.StartsWith("Castle"));
        }

        public static bool IsNotSystemType(this Type type)
        {
            return type.Namespace == null || (!type.Namespace.StartsWith("System") &&
                                              !type.Namespace.StartsWith("Microsoft") &&
                                              !type.Namespace.StartsWith("mscorlib") &&
                                              !type.Namespace.StartsWith("Antlr") &&
                                              !type.Namespace.StartsWith("WebGrease") &&
                                              !type.Namespace.StartsWith("Owin") &&
                                              !type.Namespace.StartsWith("EntityFramework") &&
                                              !type.Namespace.StartsWith("Newtonsoft") &&
                                              !type.Namespace.StartsWith("Autofac") &&
                                              !type.Namespace.Contains("Anonymously") &&
                                              !type.Namespace.StartsWith("CommonServiceLocator") &&
                                              !type.Namespace.StartsWith("AutoMapper") &&
                                              !type.Namespace.StartsWith("Castle"));
        }
    }
}