namespace yotm.Web.Models.ApiResponses
{
    public class ApiMessageResponse
    {
        public string Message { get; set; } = string.Empty;
        public int? ApplicationId { get; set; }
    }

    public class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
