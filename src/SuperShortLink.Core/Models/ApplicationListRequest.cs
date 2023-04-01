namespace SuperShortLink.Models
{
    public class ApplicationListRequest : PageRequestDto
    {
        public string app_code { get; set; }

        public string app_name { get; set; }
    }
}
