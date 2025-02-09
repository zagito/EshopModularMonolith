using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace EshopModularMonolith.AppHost;

internal static class ResouceBuilderExtensions
{
    internal static IResourceBuilder<T> WithSwaggerUi<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints =>
        builder.WithOpenApiDocs("swagger-ui-docs", "Swagger API Documentation", "swagger");

    internal static IResourceBuilder<T> WithScalar<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints =>
        builder.WithOpenApiDocs("scalar-docs", "Scalar UI Documentation", "scalar/v1");


    internal static IResourceBuilder<T> WithReDoc<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints =>
        builder.WithOpenApiDocs("redoc-docs", "ReDoc API Documentation", "api-docs");

    private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder,
        string name,
        string displayName,
        string openApiUiPath)
        where T : IResourceWithEndpoints
    {
        return builder.WithCommand(
            name,
            displayName,
            executeCommand: async _ =>
            {
                try
                {
                    var endpoint = builder.GetEndpoint("https");

                    var url = $"{endpoint.Url}/{openApiUiPath}";

                    await Task.Run(() => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }));

                    return new ExecuteCommandResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ExecuteCommandResult { Success = false, ErrorMessage = ex.Message };
                }
            },
            updateState: context => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ?
                ResourceCommandState.Enabled : ResourceCommandState.Disabled,
            iconName: "Document",
            iconVariant: IconVariant.Filled);

    }
}