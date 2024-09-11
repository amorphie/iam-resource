using System;
using System.Threading;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Resource.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Resources;

public class ResourceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("resources")
            .WithTags("Resources")
            .MapToApiVersion(1);

        #region Resources

        group.MapGet("/", async (
                [FromServices] IResourceAppService appService,
                [AsParameters] PagedResourceInput input,
                CancellationToken cancellationToken
            ) => await appService.GetAllAsync(input, cancellationToken)
        );

        group.MapGet("/{id}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/", async (
                [FromServices] IResourceAppService appService,
                [FromBody] CreateResourceInput input,
                CancellationToken cancellationToken
            ) => await appService.CreateAsync(input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid id,
                [FromBody] UpdateResourceInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await appService.DeleteAsync(id, cancellationToken)
        );

        #endregion

        #region Rules

        group.MapGet("/{resourceId}/rules", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                CancellationToken cancellationToken
            ) => await appService.GetRulesAsync(resourceId, cancellationToken)
        );

        group.MapGet("/{resourceId}/rules/{ruleId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid ruleId,
                CancellationToken cancellationToken
            ) => await appService.GetRuleAsync(resourceId, ruleId, cancellationToken)
        );

        group.MapPost("/{resourceId}/rules", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromBody] AddRuleToResourceInput input,
                CancellationToken cancellationToken
            ) => await appService.AddRuleAsync(resourceId, input, cancellationToken)
        );

        group.MapPut("/{resourceId}/rules/{ruleId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid ruleId,
                [FromBody] UpdateResourceRuleInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdateRuleAsync(resourceId, ruleId, input, cancellationToken)
        );

        group.MapDelete("/{resourceId}/rules/{ruleId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid ruleId,
                CancellationToken cancellationToken
            ) => await appService.RemoveRuleAsync(resourceId, ruleId, cancellationToken)
        );

        #endregion

        #region Privileges

        group.MapGet("/{resourceId}/privileges", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                CancellationToken cancellationToken
            ) => await appService.GetPrivilegesAsync(resourceId, cancellationToken)
        );

        group.MapGet("/{resourceId}/privileges/{privilegeId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid privilegeId,
                CancellationToken cancellationToken
            ) => await appService.GetPrivilegeAsync(resourceId, privilegeId, cancellationToken)
        );

        group.MapPost("/{resourceId}/privileges", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromBody] AddPrivilegeToResourceInput input,
                CancellationToken cancellationToken
            ) => await appService.AddPrivilegeAsync(resourceId, input, cancellationToken)
        );

        group.MapPut("/{resourceId}/privileges/{privilegeId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid privilegeId,
                [FromBody] UpdateResourcePrivilegeInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdatePrivilegeAsync(resourceId, privilegeId, input, cancellationToken)
        );

        group.MapDelete("/{resourceId}/privileges/{privilegeId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid privilegeId,
                CancellationToken cancellationToken
            ) => await appService.RemovePrivilegeAsync(resourceId, privilegeId, cancellationToken)
        );

        #endregion

        #region Policies
        group.MapGet("/{resourceId}/policies", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                CancellationToken cancellationToken
            ) => await appService.GetPoliciesAsync(resourceId, cancellationToken)
        );

        group.MapGet("/{resourceId}/policies/{policyId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid policyId,
                CancellationToken cancellationToken
            ) => await appService.GetPolicyAsync(resourceId, policyId, cancellationToken)
        );

        group.MapPost("/{resourceId}/policies", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromBody] AddPolicyToResourceInput input,
                CancellationToken cancellationToken
            ) => await appService.AddPolicyAsync(resourceId, input, cancellationToken)
        );

        group.MapPut("/{resourceId}/policies/{policyId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid policyId,
                [FromBody] UpdateResourcePolicyInput input,
                CancellationToken cancellationToken
            ) => await appService.UpdatePolicyAsync(resourceId, policyId, input, cancellationToken)
        );

        group.MapDelete("/{resourceId}/policies/{policyId}", async (
                [FromServices] IResourceAppService appService,
                [FromRoute] Guid resourceId,
                [FromRoute] Guid policyId,
                CancellationToken cancellationToken
            ) => await appService.RemovePolicyAsync(resourceId, policyId, cancellationToken)
        );
        

        #endregion

        group.MapPost("/map", async (
                [FromServices] IResourceAppService appService,
                [FromBody] ResourceRuleMapInput input,
                CancellationToken cancellationToken
            ) => await appService.MapAsync(input, cancellationToken)
        );
    }
}
