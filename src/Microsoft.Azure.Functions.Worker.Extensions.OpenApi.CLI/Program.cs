using Cocona;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.ConsoleApp;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI
{
    /// <summary>
    ///     This represents the console app entity.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            CoconaApp
                .CreateHostBuilder()
                .ConfigureServices(services =>
                {
                    services.ConfigureInternalServices();
                })
                .Run<GenerateSwaggerConsoleApp>(args);
        }
    }
}
