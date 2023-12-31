using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Abstractions
{
    public interface ICustomOpenApiWriter
    {
        Task WriteOpenApiToFile(string openApiDocument, string outputPath, OpenApiFormatType format);
    }
}