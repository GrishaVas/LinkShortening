using System.Linq.Expressions;
using System.Text.RegularExpressions;
using LinkShortening.Infrastructure.Abstractions;
using LinkShortening.Infrastructure.Models;
using LinkShortening.Services.Abstractions;
using LinkShortening.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortening.Services.Implemintations
{
    public class LinksService : ILinksService
    {
        private readonly ILinksRepository _linksRepository;

        public LinksService(ILinksRepository linksRepository)
        {
            _linksRepository = linksRepository;
        }

        public async Task<LinkDto> CreateLink(string linkUrl)
        {
            if (String.IsNullOrEmpty(linkUrl) || !isValidLinkUrl(linkUrl))
            {
                throw new ArgumentException("Url is not valid ");
            }

            var link = await _linksRepository.GetLink(l => l.Url == linkUrl);

            if (link != null)
            {
                throw new ArgumentException("Url already exists");
            }

            link = new Link
            {
                Url = linkUrl,
                CreatedAt = DateTime.Now,
                ShortUrl = await getShortUrl(linkUrl),
                ClicksNumber = 0
            };

            await _linksRepository.Create(link);

            return mapLink(link);
        }

        public async Task DeleteLinks(List<string> linkUrls)
        {
            var links = await _linksRepository.GetLinks(l => linkUrls.Contains(l.Url));
            var ids = links.Select(l => l.Id).ToList();

            await _linksRepository.DeleteLinks(ids);
        }

        public async Task<List<LinkDto>> GetLinks()
        {
            var links = await _linksRepository.GetLinks();

            return links.Select(l => mapLink(l)).ToList();
        }

        public async Task<LinkDto> UpdateLink(LinkDto link)
        {
            var linkToUpdate = await _linksRepository.GetLink(l => l.Url == link.Url);

            if (linkToUpdate == null)
            {
                throw new ArgumentException($"Link with url: {link.Url} does not exist");
            }

            await checkLinkForChange(link);

            linkToUpdate.ShortUrl = link.ShortUrl;
            linkToUpdate.CreatedAt = link.CreatedAt;
            linkToUpdate.ClicksNumber = link.ClicksNumber;

            await _linksRepository.Update(linkToUpdate);

            return link;
        }

        private LinkDto mapLink(Link link)
        {
            var linkDto = new LinkDto
            {
                Url = link.Url,
                ShortUrl = link.ShortUrl,
                CreatedAt = link.CreatedAt,
                ClicksNumber = link.ClicksNumber
            };

            return linkDto;
        }
        public async Task<LinkDto> GetLink(Expression<Func<Link, bool>> predicate)
        {
            var link = await _linksRepository.GetLink(predicate);

            return mapLink(link);
        }

        private async Task<string> getShortUrl(string url)
        {
            var shortUrl = String.Format("{0:X}", url.GetHashCode());
            var isExeists = await _linksRepository.GetQuery().AnyAsync(l => l.ShortUrl == shortUrl);

            while (isExeists)
            {
                shortUrl = String.Format("{0:X}", url.GetHashCode());
                isExeists = await _linksRepository.GetQuery().AnyAsync(l => l.ShortUrl == shortUrl);
            }

            return shortUrl;
        }

        private bool isValidLinkUrl(string url)
        {
            var regex = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/=]*)");

            return regex.IsMatch(url);
        }

        private async Task checkLinkForChange(LinkDto link)
        {
            var existingLink = await _linksRepository.GetQuery().FirstOrDefaultAsync(l => l.ShortUrl == link.ShortUrl);

            if (existingLink != null && existingLink.Url != link.Url)
            {
                throw new ArgumentException($"Link with shortUrl: {link.ShortUrl} already exists");
            }

            if (String.IsNullOrEmpty(link.ShortUrl))
            {
                throw new ArgumentException($"Incorrect ShortUrl");
            }
        }
    }
}
