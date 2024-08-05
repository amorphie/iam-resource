using Xunit;

namespace BBT.Resource.EntityFrameworkCore;

[CollectionDefinition(ResourceTestConsts.CollectionDefinitionName)]
public class ResourceEntityFrameworkCoreCollection: ICollectionFixture<ResourceEntityFrameworkCoreFixture>
{
    
}