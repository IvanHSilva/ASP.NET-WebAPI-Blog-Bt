namespace Blog;

public static class Configuration {

    // Token JWT (Json Web Token)
    public static string JwtKey { get; set; } = "476956c0BF104D248C1aCE6755a2d474";
    public static string ApiKeyName { get; set; } = "api_key";
    public static string ApiKey { get; set; } = "blog-api_GHhddjfdjGDtG2dffsd3";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration {
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
