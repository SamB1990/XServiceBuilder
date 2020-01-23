using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XServiceBuilderLibrary.Abstractions;

namespace XServiceBuilderLibrary.Extensions
{
    public static class XServiceCollectionDependencyInjection
    {
        internal static IXServiceBuilder AddServiceBuilder(this IServiceCollection services,
            IConfiguration configuration)
        {
            return new XIxServiceBuilder(services, configuration);
        }

        public static IXServiceBuilder AddXServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var builder = services.AddServiceBuilder(configuration);
                
                services.AddSingleton(configuration);
                
                return builder;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
