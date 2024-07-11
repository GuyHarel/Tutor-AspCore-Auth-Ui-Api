namespace AspNetCoreApi.Controllers
{
    public class OpenIdConfiguration
    {
        public string? issuer { get; set; }
        public string? authorization_endpoint { get; set; }
        
        public string? token_endpoint { get; set; }
        public string? userinfo_endpoint { get; set; }
        public string? jwks_uri { get; set; }
        public IEnumerable<string>? response_types_supported { get; set; }
        public IEnumerable<string>? subject_types_supported { get; set; }
        public IEnumerable<string>? id_token_signing_alg_values_supported { get; set; }
        public IEnumerable<string>? scopes_supported { get; set; }
        public IEnumerable<string>? token_endpoint_auth_methods_supported { get; set; }
    }

    public class JwksKey
    {
        public string? Kty { get; set; }
        public string? Use { get; set; }
        public string? Kid { get; set; }
        public string? Alg { get; set; }
        public string? N { get; set; }
        public string? E { get; set; }
    }

    public class Jwks
    {
        public List<JwksKey>? Keys { get; set; }
    }
}
