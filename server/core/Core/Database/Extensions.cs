using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace Core.Database
{
    /// <summary>
    ///     The data extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Registers all entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="baseTypes">The base types.</param>
        /// <param name="namespaces">The namespaces.</param>
        public static void RegisterAllEntities(this DbModelBuilder modelBuilder, Assembly assembly,
                                               IEnumerable<Type> baseTypes,
                                               params string[] namespaces)
        {
            modelBuilder.Configurations.AddFromAssembly(assembly);

            var entityTypeConfiguredTypes = assembly.GetTypes()
                                                    .Where(
                                                        x =>
                                                            x.BaseType != null &&
                                                            x.BaseType.IsGenericType &&
                                                            x.BaseType.GetGenericTypeDefinition() ==
                                                            typeof(EntityTypeConfiguration<>));

            MethodInfo method = typeof(DbModelBuilder).GetMethod("Entity");

            var configuredTypes = entityTypeConfiguredTypes.SelectMany(x => x.BaseType.GenericTypeArguments).ToList();

            var typeToRegister = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !configuredTypes.Contains(t) && namespaces.Contains(t.Namespace) && baseTypes.Any(ty => ty.IsAssignableFrom(t)))
                .ToList();

            typeToRegister.ForEach(t => method.MakeGenericMethod(t).Invoke(modelBuilder, new object[0]));
        }

        /// <summary>
        ///     Registers all entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="assemblies"></param>
        /// <param name="baseTypes">The base types.</param>
        /// <param name="namespaces">The namespaces.</param>
        public static void RegisterAllEntities(this DbModelBuilder modelBuilder, Assembly[] assemblies,
                                               IEnumerable<Type> baseTypes,
                                               params string[] namespaces)
        {

            assemblies.ToList().ForEach(x => modelBuilder.Configurations.AddFromAssembly(x));

            var entityTypeConfiguredTypes = assemblies.SelectMany(a => a.GetTypes())
                                                      .Where(
                                                          x =>
                                                              x.BaseType != null &&
                                                              x.BaseType.IsGenericType &&
                                                              x.BaseType.GetGenericTypeDefinition() ==
                                                              typeof(EntityTypeConfiguration<>));

            MethodInfo method = typeof(DbModelBuilder).GetMethod("Entity");

            var configuredTypes = entityTypeConfiguredTypes.SelectMany(x => x.BaseType.GenericTypeArguments).ToList();

            var typeToRegister = assemblies.SelectMany(a => a.GetTypes())
                                           .Where(
                                               t =>
                                                   !configuredTypes.Contains(t) && namespaces.Contains(t.Namespace) &&
                                                   baseTypes.Any(ty => ty.IsAssignableFrom(t)))
                                           .ToList();

            typeToRegister.ForEach(t => method.MakeGenericMethod(t).Invoke(modelBuilder, new object[0]));
        }
    }
}
