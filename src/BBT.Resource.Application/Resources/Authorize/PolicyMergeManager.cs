using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Mapper;
using BBT.Resource.Policies;
using BBT.Resource.PolicyEngine;

namespace BBT.Resource.Resources.Authorize;

public class PolicyMergeManager(
    IPolicyRepository policyRepository,
    IObjectMapper mapper)
{
    public async Task<PolicyDefinition> GetMergedPolicyAsync(PolicyDefinition policy, CancellationToken cancellationToken = default)
    {
        if (!policy.ParentId.IsNullOrEmpty())
        {
            var parentPolicy = await policyRepository.FindAsync(Guid.Parse(policy.ParentId), true, cancellationToken);
            if (parentPolicy == null)
            {
                return policy;
            }

            var parentPolicyDefinition = mapper.Map<Policy, PolicyDefinition>(parentPolicy);
            policy.MergeWithParent(parentPolicyDefinition);
        }

        return policy;
    }
}
