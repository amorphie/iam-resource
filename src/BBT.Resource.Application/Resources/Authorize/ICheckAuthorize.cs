using System.Threading;
using System.Threading.Tasks;

namespace BBT.Resource.Resources.Authorize;

public interface ICheckAuthorize
{
    Task<AuthorizeOutput> CheckAsync(
        string url,
        string method,
        string? data,
        CancellationToken cancellationToken = default
    );
}
