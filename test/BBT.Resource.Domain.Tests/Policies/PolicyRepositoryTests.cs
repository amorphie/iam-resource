using BBT.Prism.Modularity;

namespace BBT.Resource.Policies;

public abstract class PolicyRepositoryTests<TStartupModule> : ResourceDomainTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IPolicyRepository _repository;
    private readonly TestData _testData;

    protected PolicyRepositoryTests()
    {
        _repository = GetRequiredService<IPolicyRepository>();
        _testData = GetRequiredService<TestData>();
    }
}
