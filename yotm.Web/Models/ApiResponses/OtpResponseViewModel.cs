namespace yotm.Web.Models.ApiResponses
{
    public class OtpResponseViewModel
    {
        public string? Message { get; set; }
        public string? Code { get; set; }
        public int ExpiresIn { get; set; }
    }
}
