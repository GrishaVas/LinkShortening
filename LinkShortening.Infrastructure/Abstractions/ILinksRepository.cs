using System.Linq.Expressions;
using LinkShortening.Infrastructure.Models;

namespace LinkShortening.Infrastructure.Abstractions
{
    public interface ILinksRepository
    {
        public Task<Link> Create(Link link);
        public Task<Link> GetById(Guid id);
        public Task<Link> GetLink(Expression<Func<Link, bool>> predicate = null);
        public Task<List<Link>> GetLinks(Expression<Func<Link, bool>> predicate);
        public Task<List<Link>> GetLinks();
        public Task Update(Link link);
        public Task DeleteLinks(List<Guid> ids);
        public IQueryable<Link> GetQuery();
    }
}
