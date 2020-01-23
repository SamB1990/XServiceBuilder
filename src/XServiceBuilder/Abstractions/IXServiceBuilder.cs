using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XServiceBuilderLibrary.Abstractions
{
    public interface IXServiceBuilder
    {
        /// <summary>
        /// Site configuration object
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        IServiceCollection Services { get; }

        /// <summary>
        /// Dependency inject a library into the services (will use the calling assembly to do so).
        /// </summary>
        /* Uses reflection to loop the calling assemblies services that implement IXService then adds them into the services */
        void RegisterLibrary();


        IMvcBuilder MvcBuilder { get; }

        // ReSharper disable once InconsistentNaming
        T RegisterOptions<T, I>()
            where T : class
            where I : class;

        IMvcBuilder AddRazorPagesOptions(Action<RazorPagesOptions> setupAction);

        IMvcBuilder AddMvc();
    }
}
