using BBT.Prism.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BBT.Resource.Endpoints.Resources;

public class ResourceEndpoint: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("resources")
            .WithTags("Resources")
            .MapToApiVersion(1);
    }
}
