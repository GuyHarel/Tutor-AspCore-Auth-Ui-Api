namespace AspNetCoreRazor.Security.Authentication
{
    public class JwtTokenTest
    {
        public JwtToken JsonToken { get; set; } = new JwtToken();

        public class JwtToken
        {
            public string token { get; set; }
        }

    }
}
