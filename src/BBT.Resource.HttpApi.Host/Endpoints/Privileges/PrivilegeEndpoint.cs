using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Privileges;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Privileges;

public class PrivilegeEndpoint: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("privileges")
            .WithTags("Privileges")
            .MapToApiVersion(1);
        
        group.MapGet("/", async (
                [FromServices] IPrivilegeAppService appService,
                [FromBody] PagedPrivilegeInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IPrivilegeAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IPrivilegeAppService appService,
                [FromBody] CreatePrivilegeInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IPrivilegeAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdatePrivilegeInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IPrivilegeAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );
    }
}
