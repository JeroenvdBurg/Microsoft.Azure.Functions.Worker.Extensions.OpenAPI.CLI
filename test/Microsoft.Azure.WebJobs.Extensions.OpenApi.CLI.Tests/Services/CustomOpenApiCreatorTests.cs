using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Tests.Services
{
    [TestClass]
    public class CustomOpenApiCreatorTests
    {
        [TestMethod]
        public async Task CreateOpenApiDocument()
        {
            // Arrange
            var apiBaseUrl = "test.function.com";

            // Solution root from test assembly path
            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var solutionDirectory = Directory.GetParent(testAssemblyPath)
                                             .Parent!.Parent!.Parent!.Parent!.Parent!
                                             .FullName;

            // Sample function app project
            var sampleProjectPath = Path.Combine(solutionDirectory, "samples", "Azure.Functions.Sample");
            var configuration = "Debug";      // or infer from build if needed
            var targetFramework = "net10.0";

            var compiledPath = Path.Combine(sampleProjectPath, "bin", configuration, targetFramework);
            var compiledDllPath = Path.Combine(compiledPath, "Azure.Functions.Sample.dll");
            var hostJsonPath = Path.Combine(compiledPath, "host.json");

            // If the sample isn't built, mark the test inconclusive instead of failing
            if (!File.Exists(compiledDllPath) || !File.Exists(hostJsonPath))
            {
                Assert.Inconclusive(
                    $"Sample output not found. Expected '{compiledDllPath}' and '{hostJsonPath}'. " +
                    "Build the Azure.Functions.Sample project before running this test.");
            }

            var httpSettings = hostJsonPath.SetHostSettings();
            var openApiInfo = compiledDllPath.SetOpenApiInfo();
            var openApiVersionType = OpenApiVersionType.V2;
            var openApiFormatType = OpenApiFormatType.Json;

            var service = this.SetupSut();

            // Act
            var result = await service.CreateOpenApiDocument(
                apiBaseUrl,
                compiledDllPath,
                httpSettings.RoutePrefix,
                openApiInfo,
                openApiVersionType,
                openApiFormatType);

            // Assert
            result.Should().NotBeNull();
        }

        private CustomOpenApiCreator SetupSut()
        {
            var service = new CustomOpenApiCreator();
            return service;
        }
    }
}
