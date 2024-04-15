namespace LinkShortening.Services.Models
{
    public class LinkDto
    {
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClicksNumber { get; set; }
    }
}
