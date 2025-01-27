using System.Text;

namespace VisualSoft.WebApi.Authorization;

public class AuthorizationService
{
    private const string _username = "vs";
    private const string _password = "rekrutacja";

    public bool Authorize(HttpContext httpContext)
    {
        try
        {
            var authHeader = httpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return false;
            }

            var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));
            var authSplit = authBase64Decoded.Split(':');

            if (authSplit.Length != 2)
            {
                return false;
            }

            var clientId = authSplit[0];
            var clientSecret = authSplit[1];

            return clientId == _username && clientSecret == _password;
        }
        catch
        {
            return false;
        }
    }
}
