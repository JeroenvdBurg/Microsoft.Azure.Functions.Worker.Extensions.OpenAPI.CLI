using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI
{
    public static class Setup
    {
        public static void ConfigureInternalServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomApiMockCreator, CustomApiMockCreator>();
            services.AddSingleton<ICustomOpenApiCreator, CustomOpenApiCreator>();
            services.AddSingleton<ICustomOpenApiWriter, CustomOpenApiWriter>();
        }
    }
}
