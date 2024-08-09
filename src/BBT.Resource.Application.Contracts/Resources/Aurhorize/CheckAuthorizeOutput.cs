namespace BBT.Resource.Resources;

public class CheckAuthorizeOutput(int statusCode, string reason)
{
    public string Reason { get; } = reason;
    public int StatusCode { get; } = statusCode;
}
