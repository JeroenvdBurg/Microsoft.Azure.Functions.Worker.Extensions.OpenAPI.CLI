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
            var hostJsonPath = Path.Combine(path, "host.json");

            // Ensure host.json exists for the test
            if (!File.Exists(hostJsonPath))
            {
                File.WriteAllText(hostJsonPath, """
                    {
                        "version": "2.0",
                        "extensions": {
                            "http": {
                                "routePrefix": "api"
                            }
                        }
                    }
                    """);
            }

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
            var compiledDllPath = Assembly.GetExecutingAssembly().Location;

            // Act
            var result = compiledDllPath.SetOpenApiInfo();

            // Assert
            result.Should().NotBeNull();
        }
    }
}
