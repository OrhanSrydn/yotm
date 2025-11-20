namespace yotm.Web.Models.ApiResponses
{
    public class AuthResponseViewModel
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
