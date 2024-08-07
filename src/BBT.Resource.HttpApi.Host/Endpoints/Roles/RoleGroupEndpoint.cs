using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Roles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Roles;

public class RoleGroupEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("role-groups")
            .WithTags("RoleGroups")
            .MapToApiVersion(1);

        group.MapGet("/", async (
                [FromServices] IRoleGroupAppService appService,
                [AsParameters] PagedRoleGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IRoleGroupAppService appService,
                [FromBody] CreateRoleGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdateRoleGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );

        group.MapGet("/{groupId}/roles", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid groupId,
                CancellationToken cancellationToken
            ) => await appService.GetRolesAsync(groupId, cancellationToken)
        );

        group.MapGet("/{groupId}/roles/{roleId}", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid groupId,
                [FromRoute] Guid roleId,
                CancellationToken cancellationToken
            ) => await appService.GetRolesAsync(groupId, cancellationToken)
        );

        group.MapPost("/{groupId}/roles", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid groupId,
                [FromBody] AddRoleToRoleGroupInput input,
                CancellationToken cancellationToken
            ) => await appService.AddRoleAsync(groupId, input, cancellationToken)
        );

        group.MapPut("/{groupId}/roles/{roleId}/status", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid groupId,
                [FromRoute] Guid roleId,
                [FromBody] ChangeStatusOfGroupRoleInput input,
                CancellationToken cancellationToken
            ) => await appService.ChangeRoleStatusAsync(groupId, roleId, input, cancellationToken)
        );

        group.MapDelete("/{groupId}/roles/{roleId}", async (
                [FromServices] IRoleGroupAppService appService,
                [FromRoute] Guid groupId,
                [FromRoute] Guid roleId,
                CancellationToken cancellationToken
            ) => await appService.RemoveRoleAsync(groupId, roleId, cancellationToken)
        );
    }
}
