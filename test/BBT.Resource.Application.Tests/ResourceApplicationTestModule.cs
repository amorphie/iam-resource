using BBT.Prism.Modularity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BBT.Resource;

[Modules(
    typeof(ResourceApplicationModule),
    typeof(ResourceDomainTestModule)
)]
public class ResourceApplicationTestModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        RegisterHttpContextAccessor(context.Services);
    }

    private void RegisterHttpContextAccessor(IServiceCollection services)
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContextMock = new Mock<HttpContext>();
        var requestMock = new Mock<HttpRequest>();
        var headers = new HeaderDictionary
        {
            { "client_id", "test_1" },
            { "user_reference", "11111111111" },
            { "card_number", "1122334455667788" },
        };

        requestMock.Setup(r => r.Headers).Returns(headers);
        httpContextMock.Setup(h => h.Request).Returns(requestMock.Object);
        httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);
        services.AddSingleton(httpContextAccessorMock.Object);
    }
}
