using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Roles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Roles;

public class RoleEndpoint: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("roles")
            .WithTags("Roles")
            .MapToApiVersion(1);

        group.MapGet("/", async (
                [FromServices] IRoleAppService appService,
                [AsParameters] PagedRoleInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IRoleAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IRoleAppService appService,
                [FromBody] CreateRoleInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IRoleAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdateRoleInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IRoleAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );
    }
}
