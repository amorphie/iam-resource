namespace BBT.Resource.Resources.Authorize;

public class AuthorizeOutput()
{
    public string Reason { get; private set; }
    public int StatusCode { get; private set; }

    public AuthorizeOutput SetResult(int statusCode, string reason)
    {
        StatusCode = statusCode;
        Reason = reason;
        return this;
    }
}
