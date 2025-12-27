using System.Diagnostics;
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
        private bool _isDebug;

        [TestInitialize]
        public void Init()
        {
            this.IsDebugCheck(ref this._isDebug);
        }

        [Conditional("DEBUG")]
        private void IsDebugCheck(ref bool isDebug)
        {
            isDebug = true;
        }

        [TestMethod]
        public async Task CreateOpenApiDocument()
        {
            // Arrange
            var apiBaseUrl = "test.function.com";

            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var solutionDirectory = Directory.GetParent(testAssemblyPath)
                                             .Parent!.Parent!.Parent!.Parent!.Parent!
                                             .FullName;

            var sampleProjectPath = Path.Combine(solutionDirectory, "samples", "Azure.Functions.Sample");
            var configuration = this._isDebug ? "Debug" : "Release";
            var targetFramework = "net10.0";

            var compiledPath = Path.Combine(sampleProjectPath, "bin", configuration, targetFramework);
            var compiledDllPath = Path.Combine(compiledPath, "Azure.Functions.Sample.dll");
            var hostJsonPath = Path.Combine(compiledPath, "host.json");

            if (!File.Exists(compiledDllPath) || !File.Exists(hostJsonPath))
            {
                Assert.Inconclusive(
                    $"Sample output not found. Expected '{compiledDllPath}' and '{hostJsonPath}'. " +
                    $"Build the Azure.Functions.Sample project in '{configuration}' before running this test.");
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
