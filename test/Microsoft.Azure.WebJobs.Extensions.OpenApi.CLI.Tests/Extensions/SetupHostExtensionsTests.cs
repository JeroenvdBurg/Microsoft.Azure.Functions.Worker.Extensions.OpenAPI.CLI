using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Tests.Extensions
{
    [TestClass]
    public class SetupHostExtensionsTests
    {
        [TestMethod]
        public void HttpSettings()
        {
            // Arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var hostJsonPath = $"{path}/host.json";

            // Act
            var result = hostJsonPath.SetHostSettings();

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void SetOpenApiInfo()
        {
            // Arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var compiledDllPath = $"{path}/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.dll";

            // Act
            var result = compiledDllPath.SetOpenApiInfo();

            // Assert
            result.Should().NotBeNull();
        }
    }
}
