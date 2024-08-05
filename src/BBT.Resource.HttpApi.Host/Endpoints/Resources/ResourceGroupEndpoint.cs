using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Resources;

public class ResourceGroupEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("resourceGroups")
            .WithTags("ResourceGroups")
            .MapToApiVersion(1);

        group.MapGet("/", async (
                [FromServices] IResourceGroupAppService appService,
                [FromBody] PagedResourceGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IResourceGroupAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IResourceGroupAppService appService,
                [FromBody] CreateResourceGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IResourceGroupAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdateResourceGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IResourceGroupAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );
    }
}
