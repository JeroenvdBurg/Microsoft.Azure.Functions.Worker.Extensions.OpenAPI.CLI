using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Model;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Abstractions
{
    public interface ICustomApiMockCreator
    {
        ApiMock SetupApi(string projectPath, string configuration, string targetFramework);
    }
}
