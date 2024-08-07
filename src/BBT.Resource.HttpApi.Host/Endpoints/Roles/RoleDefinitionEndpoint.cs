using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Roles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Roles;

public class RoleDefinitionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("role-definitions")
            .WithTags("RoleDefinitions")
            .MapToApiVersion(1);

        group.MapGet("/", async (
                [FromServices] IRoleDefinitionAppService appService,
                [AsParameters] PagedRoleDefinitionInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IRoleDefinitionAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IRoleDefinitionAppService appService,
                [FromBody] CreateRoleDefinitionInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IRoleDefinitionAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdateRoleDefinitionInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IRoleDefinitionAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );
    }
}
