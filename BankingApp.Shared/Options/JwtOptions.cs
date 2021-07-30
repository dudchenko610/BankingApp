
namespace BankingApp.Shared.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public int LengthRefreshToken { get; set; }
        public string SecretKey { get; set; }
    }
}
