using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Shouldly;
using Xunit;

namespace BBT.Resource.Resources;

public abstract class ResourceAuthorizeAppServiceTests<TStartupModule> : ResourceApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IResourceAuthorizeAppService _appService;
    private readonly TestData _testData;

    protected ResourceAuthorizeAppServiceTests()
    {
        _appService = GetRequiredService<IResourceAuthorizeAppService>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task CheckAsync_ByRule_ShouldBeSuccess()
    {
        var response = await _appService.CheckAsync("Rule", new CheckAuthorizeInput()
        {
            Url = "/api/users",
            Method = "POST",
            Data = ""
        });
        response.StatusCode.ShouldBe(200);
    }

    [Fact]
    public async Task CheckAsync_ByRule_ShouldBeUnauthorized()
    {
        var response = await _appService.CheckAsync("Rule", new CheckAuthorizeInput()
        {
            Url = "/api/users/25",
            Method = "GET",
            Data = ""
        });
        response.StatusCode.ShouldBe(403);
    }
    
    [Fact]
    public async Task CheckAsync_ByRule_ShouldBeSuccessWhenNotFoundResource()
    {
        var response = await _appService.CheckAsync("Rule", new CheckAuthorizeInput()
        {
            Url = "/api/test/15",
            Method = "GET",
            Data = ""
        });
        response.StatusCode.ShouldBe(200);
        response.Reason.ShouldBe("Resource not found.");
    }
}
