using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Policies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Policies;

public class PolicyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("policies")
            .WithTags("Policies")
            .MapToApiVersion(1);

        group.MapGet("/", async (
                [FromServices] IPolicyAppService appService,
                [AsParameters] PagedPolicyInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IPolicyAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IPolicyAppService appService,
                [FromBody] CreatePolicyInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IPolicyAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdatePolicyInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IPolicyAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );

        group.MapGet("/evaluation-steps", async (
                [FromServices] IPolicyAppService appService,
                [AsParameters] PagedPolicyInput input,
                CancellationToken cancellationToken
            ) => await appService.GetEvaluationStepsAsync(cancellationToken)
        );

        group.MapGet("/conflict-resolutions", async (
                [FromServices] IPolicyAppService appService,
                [AsParameters] PagedPolicyInput input,
                CancellationToken cancellationToken
            ) => await appService.GetConflictResolutionsAsync(cancellationToken)
        );
    }
}
