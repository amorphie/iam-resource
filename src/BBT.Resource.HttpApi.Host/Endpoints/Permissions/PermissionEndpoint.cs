using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Permissions;

public class PermissionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("permissions")
            .WithTags("Permissions")
            .MapToApiVersion(1);

        group.MapGet("/{applicationId}/{clientId}/{providerName}/{providerKey}", async (
                [FromServices] IPermissionAppService appService,
                [FromRoute] string applicationId,
                [FromRoute] string clientId,
                [FromRoute] string providerName,
                [FromRoute] string providerKey,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(
                applicationId,
                clientId,
                providerName,
                providerKey,
                cancellationToken)
        );

        group.MapPut("/{applicationId}/{clientId}/{providerName}/{providerKey}", async (
                [FromServices] IPermissionAppService appService,
                [FromRoute] string applicationId,
                [FromRoute] string clientId,
                [FromRoute] string providerName,
                [FromRoute] string providerKey,
                [FromBody] UpdatePermissionInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(
                applicationId,
                clientId,
                providerName,
                providerKey,
                input,
                cancellationToken)
        );

        group.MapGet("/check/{applicationId}/{clientId}/{providerName}/{providerKey}/{name}", async (
                [FromServices] IPermissionAppService appService,
                [FromRoute] string applicationId,
                [FromRoute] string clientId,
                [FromRoute] string providerName,
                [FromRoute] string providerKey,
                [FromRoute] string name,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await appService.CheckAsync(
                    applicationId,
                    clientId,
                    providerName,
                    providerKey,
                    name,
                    cancellationToken);
                return result.IsGranted ? Results.Ok() : Results.Unauthorized();
            }
        );
    }
}
