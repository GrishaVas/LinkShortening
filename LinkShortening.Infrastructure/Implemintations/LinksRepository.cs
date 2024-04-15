using System.Linq.Expressions;
using LinkShortening.Infrastructure.Abstractions;
using LinkShortening.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortening.Infrastructure.Implemintations
{
    public class LinksRepository : ILinksRepository
    {
        private readonly LinkShorteningDbContext _dbContext;

        public LinksRepository(LinkShorteningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Link> Create(Link link)
        {
            _dbContext.Links.Add(link);
            await _dbContext.SaveChangesAsync();

            return link;
        }

        public async Task DeleteLinks(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                var link = await _dbContext.Links.FindAsync(id);

                if (link != null)
                {
                    _dbContext.Links.Remove(link);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public Task<Link> GetById(Guid id)
        {
            return _dbContext.Links.FindAsync(id).AsTask();
        }

        public Task<Link> GetLink(Expression<Func<Link, bool>> predicate)
        {
            return _dbContext.Links.FirstOrDefaultAsync(predicate);
        }

        public Task<List<Link>> GetLinks(Expression<Func<Link, bool>> predicate)
        {
            return _dbContext.Links.Where(predicate).ToListAsync();
        }

        public Task<List<Link>> GetLinks()
        {
            return _dbContext.Links.ToListAsync();
        }

        public IQueryable<Link> GetQuery()
        {
            return _dbContext.Links.AsQueryable();
        }

        public async Task Update(Link link)
        {
            var linkToUpdate = await _dbContext.Links.FindAsync(link.Id);

            if (linkToUpdate != null)
            {
                _dbContext.Links.Update(link);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
