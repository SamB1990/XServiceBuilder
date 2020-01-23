using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XServiceBuilderLibrary.Abstractions;
using XServiceBuilderLibrary.Abstractions.Options;
using XServiceBuilderLibrary.Extensions;
using XServiceBuilderLibrary.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XServiceBuilderLibrary
{
    public class XIxServiceBuilder : IXServiceBuilder
    {
        public XIxServiceBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration;
            if (_options == null)
            {
                _options = new XServiceOptionsCollection();
                var section = Configuration.GetSection("XServiceOptionsCollection");
                section.Bind(_options);
            }
        }

        private IXServiceOptionsCollection _options;

        public IConfiguration Configuration { get; }

        public IServiceCollection Services { get; }


        public void RegisterLibrary()
        {
            var iService = typeof(IXService);

            var assembly = Assembly.GetCallingAssembly();

            var classes = assembly.GetTypes().Where(t => iService.IsAssignableFrom(t) && !t.IsInterface).ToArray();

            var matches = classes.SelectMany(t =>
                t.GetInterfaces().Where(i => iService.IsAssignableFrom(i) && i != iService)
                    .Select(i => new KeyValuePair<Type, Type>(t, i)));
            foreach (var match in matches)
            {
                Services.TryAddScoped(match.Value, match.Key);
                
            }

            /*Old way of doing*/
            //classes.ToList().ForEach(
            //    t => t.GetInterfaces().Where
            //        (
            //            i => iService.IsAssignableFrom(i) && i != iService )
            //        .ToList()
            //        .ForEach
            //        (
            //            i =>  Services.TryAddScoped(i, t)
            //        )
            //);
        }

        public IMvcBuilder MvcBuilder { get; private set; }

        public T RegisterOptions<T, I>() where T : class where I : class
        {
            if (!typeof(T).IsClass)
                throw new Exception("LoadOptions<T, I> requires T to be a class");

            if (!typeof(I).IsInterface)
                throw new Exception("LoadOptions<T, I> requires I to be a interface");

            if (!typeof(I).IsAssignableFrom(typeof(T)))
                throw new Exception("LoadOptions<T, I> requires I to be assignable from T");

            var obj = Activator.CreateInstance(typeof(T)) as T;

            var optionType = _options.Options.FirstOrDefault(o => o.Type.Split(',').Any(t => t.Trim() == typeof(T).FullName));

            try
            {
                Mapper<T>.Map(optionType?.Data, obj);
            }
            catch (Exception e)
            {
                throw new Exception($"Error whilst mapping the { typeof(T) } options class (check that the configuration contains details for these options)", e);
            }

            // ReSharper disable once RedundantTypeArgumentsOfMethod
            Services.AddSingleton<I>(obj as I);

            return obj;
        }

        public IMvcBuilder AddRazorPagesOptions(Action<RazorPagesOptions> setupAction)
        {
            return MvcBuilder.AddRazorPagesOptions(setupAction);
        }

        public IMvcBuilder AddMvc()
        {
            MvcBuilder = Services.AddMvc();
            return MvcBuilder;
        }
    }
}
