namespace Flight.Management.System.API.Configuration
{
    public class UserTokens
    {
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public TimeSpan Validaty { get; set; }
        public string? RefreshToken { get; set; }
        public bool isAdmin { get; set; }
        public long Id { get; set; }
        public string? EmailId { get; set; }
        public string GuidId { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
