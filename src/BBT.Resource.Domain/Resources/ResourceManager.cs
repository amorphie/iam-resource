using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Services;
using BBT.Resource.Rules;
using Microsoft.EntityFrameworkCore;

namespace BBT.Resource.Resources;

public class ResourceManager(
    IServiceProvider serviceProvider,
    IResourceRepository resourceRepository,
    IRuleRepository ruleRepository) : DomainService(serviceProvider)
{
    public async Task<List<ResourceMapResultModel>> MapAsync(string url, string[] ruleNames, ResourceType[] methods,
        CancellationToken cancellationToken = default)
    {
        var existingResources = await GetResourcesByUrlAsync(url, cancellationToken);
        var ruleIds = await (await ruleRepository.GetQueryableAsync())
            .Where(p => ruleNames.Contains(p.Name))
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);
        
        var mapResults = new List<ResourceMapResultModel>();
        foreach (var method in methods)
        {
            var existingResource = existingResources
                .FirstOrDefault(r => r.Type == method);

            if (existingResource != null)
            {
                //UPDATE
                if (existingResource.Status.Equals(Status.Passive))
                {
                    existingResource.ChangeStatus(Status.Active.Code);
                }
                UpsertRuleToResource(existingResource, ruleIds);
                mapResults.Add(new ResourceMapResultModel(existingResource, true));
            }
            else
            {
                //INSERT
                var newResource = new Resource(
                    GuidGenerator.Create(),
                    null,
                    method,
                    url);
                UpsertRuleToResource(newResource, ruleIds);
                mapResults.Add(new ResourceMapResultModel(newResource, false));
            }
        }

        return mapResults;
    }

    private void UpsertRuleToResource(Resource resource, List<Guid> ruleIds)
    {
        foreach (var rule in resource.Rules)
        {
            if (!ruleIds.Contains(rule.RuleId))
            {
                //TODO: It can be removed instead of making it passive.
                resource.UpdateRule(rule.RuleId, Status.Passive.Code, rule.Priority);
            }
        }
        
        foreach (var ruleId in ruleIds)
        {
            resource.AddRule(ruleId, null, 1);
        }
    }

    private async Task<List<Resource>> GetResourcesByUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        return await (await resourceRepository.GetQueryableAsync())
            .Include(i => i.Rules)
            .Where(p => p.Url == url)
            .ToListAsync(cancellationToken);
    }
}
