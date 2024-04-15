using System.Linq.Expressions;
using LinkShortening.Infrastructure.Models;
using LinkShortening.Services.Models;

namespace LinkShortening.Services.Abstractions
{
    public interface ILinksService
    {
        public Task<LinkDto> CreateLink(string linkUrl);
        public Task<LinkDto> UpdateLink(LinkDto link);
        public Task DeleteLinks(List<string> linkUrls);
        public Task<List<LinkDto>> GetLinks();
        public Task<LinkDto> GetLink(Expression<Func<Link, bool>> predicate);
    }
}
