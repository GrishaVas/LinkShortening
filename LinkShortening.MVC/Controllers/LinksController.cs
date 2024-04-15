using System.Diagnostics;
using LinkShortening.MVC.Models;
using LinkShortening.Services.Abstractions;
using LinkShortening.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortening.MVC.Controllers
{
    public class LinksController : Controller
    {
        private readonly ILogger<LinksController> _logger;
        private readonly ILinksService _linksService;

        public LinksController(ILogger<LinksController> logger, ILinksService linksService)
        {
            _logger = logger;
            _linksService = linksService;
        }

        public async Task<IActionResult> Index()
        {
            var links = await _linksService.GetLinks();
            ViewData["goToLinkUrl"] = $"{this.Request.Scheme}://{this.Request.Host}/Links/GoToLink/";

            return View(links);
        }

        public async Task<IActionResult> Edit()
        {
            var links = await _linksService.GetLinks();

            return View(links);
        }
        [HttpGet]
        public async Task<IActionResult> GoTo([FromRoute] string id)
        {
            var link = await _linksService.GetLink(l => l.ShortUrl == id);

            if (link == null)
            {
                return Redirect("Edit");
            }

            link.ClicksNumber++;
            await _linksService.UpdateLink(link);

            return RedirectPermanent(link.Url);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string Url)
        {
            try
            {
                await _linksService.CreateLink(Url);
            }
            catch (ArgumentException ex)
            {

                TempData["exception"] = ex.Message;
            }


            return Redirect("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(List<string> LinkUrls)
        {
            await _linksService.DeleteLinks(LinkUrls);

            return Redirect("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Change(LinkDto link)
        {
            try
            {
                await _linksService.UpdateLink(link);
            }
            catch (ArgumentException ex)
            {

                TempData["exception"] = ex.Message;
            }

            return Redirect("Edit");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
