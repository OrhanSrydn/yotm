namespace yotm.Web.Models
{
    public class UpdateApplicationStatusRequest
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
