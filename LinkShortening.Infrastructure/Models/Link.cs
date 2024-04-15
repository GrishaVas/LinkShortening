namespace LinkShortening.Infrastructure.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClicksNumber { get; set; }
    }
}
