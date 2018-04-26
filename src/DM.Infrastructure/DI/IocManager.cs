using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DM.Infrastructure.DI
{
    /// <summary>
    /// Container manager
    /// </summary>
    public class IocManager
    {
        private static IContainer container;
        private static List<Assembly> assemblies = new List<Assembly>();

        private static void AddAmb(Assembly assembly)
        {
            Assembly amb = assemblies.FirstOrDefault(a => a == assembly);
            if (amb == null)
            {
                assemblies.Add(assembly);
                var ambNames = assembly.GetReferencedAssemblies().Where(a => a.FullName.StartsWith("PKC"));
                foreach (var ambName in ambNames)
                {
                    AddAmb(Assembly.Load(ambName));
                }
            }
        }
        //Ioc容器初始化
        public static IServiceProvider Initialize(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);

            //所有程序集 和程序集下类型
            Assembly assembly = Assembly.GetEntryAssembly();
            AddAmb(assembly);

            List<Type> types = new List<Type>();
            assemblies.ForEach(item =>
                    types.AddRange(item.GetTypes())
            );

            //找到所有外部IDependencyRegistrar实现，调用注册
            var registrarType = typeof(IDependencyRegistrar);
            var arrRegistrarType = types.Where(t => registrarType.IsAssignableFrom(t) && t != registrarType).ToArray();
            var listRegistrarInstances = new List<IDependencyRegistrar>();
            foreach (var drType in arrRegistrarType)
            {
                listRegistrarInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }
            //排序
            listRegistrarInstances = listRegistrarInstances.OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in listRegistrarInstances)
            {
                dependencyRegistrar.Register(builder, types);
            }

            //注册TransientDependencyAttribute
            var transientTypes = types.Where(t => t.GetCustomAttribute<TransientDependencyAttribute>() != null);
            foreach (Type type in transientTypes)
            {
                if (type.IsGenericType)//注册泛型
                {
                    var registrationBuilder = builder.RegisterGeneric(type).AsSelf().AsImplementedInterfaces().PropertiesAutowired();
                    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                    {
                        registrationBuilder.As(type.BaseType);
                    }
                }
                else
                {
                    var registrationBuilder = builder.RegisterType(type).AsSelf().AsImplementedInterfaces().PropertiesAutowired();
                    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                    {
                        registrationBuilder.As(type.BaseType);
                    }
                }
            }

            //注册ScopedDependencyAtrribute
            var scopedTypes = types.Where(t => t.GetCustomAttribute<ScopedDependencyAttribute>() != null);
            foreach (Type type in scopedTypes)
            {
                if (type.IsGenericType)//注册泛型
                {
                    var registrationBuilder = builder.RegisterGeneric(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
                    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                    {
                        registrationBuilder.As(type.BaseType);
                    }
                }
                else
                {
                    var registrationBuilder = builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
                    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                    {
                        registrationBuilder.As(type.BaseType);
                    }
                }
            }

            //注册SingletonDependencyAttribute
            var singletonTypes = types.Where(t => t.GetCustomAttribute<SingletonDependencyAttribute>() != null);
            foreach (Type type in singletonTypes)
            {
                if (type.IsGenericType)//注册泛型
                {
                    var registrationBuilder = builder.RegisterGeneric(type).AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired();
                    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                    {
                        registrationBuilder.As(type.BaseType);
                    }
                }
                else
                {
                    var registrationBuilder = builder.RegisterType(type).AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired();
                    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                    {
                        registrationBuilder.As(type.BaseType);
                    }
                }
            }

            container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        public static T Resolve<T>() where T : class
        {
            if (container == null)
            {
                throw new Exception("IocManager no Initialize");
            }
            return container.Resolve<T>();
        }
    }
}
