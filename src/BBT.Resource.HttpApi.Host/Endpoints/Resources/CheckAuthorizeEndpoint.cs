using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Resources;

public class CheckAuthorizeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("authorize")
            .WithTags("Authorize")
            .MapToApiVersion(1);

        group.Map("/check", async (
                [FromServices] IResourceAuthorizeAppService appService,
                [FromQuery] string type,
                [AsParameters] CheckAuthorizeInput input,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await appService.CheckAsync(type, input, cancellationToken);
                if (result.StatusCode == 200)
                {
                    return Results.Ok(result);
                }

                return Results.Json(result, statusCode: result.StatusCode);
            }
        );
    }
}
