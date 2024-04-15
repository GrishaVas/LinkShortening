using LinkShortening.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortening.Infrastructure
{
    public class LinkShorteningDbContext : DbContext
    {
        public LinkShorteningDbContext(DbContextOptions<LinkShorteningDbContext> options) : base(options)
        {
        }

        public DbSet<Link> Links { get; set; }
    }
}
