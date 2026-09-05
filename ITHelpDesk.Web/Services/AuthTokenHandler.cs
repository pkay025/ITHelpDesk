using System.Net.Http.Headers;

namespace ITHelpDesk.Web.Services;

public class AuthTokenHandler(AuthSession session) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(session.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
