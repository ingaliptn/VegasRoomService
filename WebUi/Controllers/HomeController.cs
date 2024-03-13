using System;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Infrastructure;
using WebUi.Lib;
using WebUi.Models;

namespace WebUi.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public HomeController(IEscortRepository escortRepository,
            ITextRepository textRepository,
            IMenuRepository menuRepository,
            IMapper mapper,
            IWebHostEnvironment env,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger) : base (escortRepository, textRepository, menuRepository, memoryCache)
        {
            _logger = logger;
            _mapper = mapper;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        public IActionResult Index()
        {            
            ViewBag.CanonicalUrl = GetCanonicalUrl();
            ViewBag.SiteTitle = "Las Vegas Erotic Happy Ending Massage At VegasRoomService";
            ViewBag.SiteDescription =
                "Happy ending massage is where you get to enjoy at the end of the massage by getting pleasures you have never felt before - VegasRoomService";

            return View();
        }

        private string GetCanonicalUrl()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var request = _httpContextAccessor.HttpContext.Request;
                return string.Concat(
                    request.Scheme,
                    "://",
                    request.Host.ToUriComponent(),
                    request.PathBase.ToUriComponent(),
                    request.Path.ToUriComponent(),
                    request.QueryString.ToUriComponent());
            }

            return string.Empty;
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("london-escorts.php")]
        public IActionResult LondonEscorts()
        {
            return View();
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("manchester-escorts.php")]
        public IActionResult ManchesterEscorts()
        {
            return View();
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("birmingham-escorts.php")]
        public IActionResult BirminghamEscorts()
        {
            return View();
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("liverpool-escorts.php")]
        public IActionResult LiverpoolEscorts()
        {
            return View();
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("bristol-escorts.php")]
        public IActionResult BristolEscorts()
        {
            return View();
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("about.php")]
        public IActionResult AboutUs()
        {            
            ViewBag.CanonicalUrl = GetCanonicalUrl();
            ViewBag.SiteTitle = "Read about VegasIndependents - the best escorts in Las Vegas";
            ViewBag.SiteDescription = "Welcome to VegasIndependents. We are the most reputable and well known site in Las Vegas. Find out more about us.";

            return View();
        }

        public async Task<IActionResult> Massage(string name)
        {
            var m = new MassageViewModel();
            
            var escorts = await GetAllEscorts();
            var texts = await GetAllTexts();
            var list = new List<Escort>();

            switch (name)
            {
                case "asia-massage":
                    list = escorts.Where(z => z.Nationality == "Asian").ToList();
                    break;
                case "erotic-massage":
                    list = escorts.Where(z => z.Nationality == "Erotic").ToList();
                    break;
                case "couples-massage":
                    list = escorts.Where(z => z.Nationality == "Couples").ToList();
                    break;
                case "nuru-massage":
                    list = escorts.Where(z => z.Nationality == "Nuru").ToList();
                    break;
                case "happy-ending-massage":
                    list = escorts.Where(z => z.Nationality == "Happy Ending").ToList();
                    break;
                case "nude-massage":
                    list = escorts.Where(z => z.Nationality == "Nude").ToList();
                    break;
                case "body-massage":
                    list = escorts.Where(z => z.Nationality == "Body Rubs").ToList();
                    break;
            }

            var s = name.Replace("-", " ");
            m.EscortsNavTitle = Regex.Replace(s, @"(^\w)|(\s\w)", m => m.Value.ToUpper());

            m.PositionMassageTitle = texts.Where(z => z.Position == $"Massage-{name}-Title").Select(z => z.Description)
                .FirstOrDefault();
            m.PositionMassageTop = texts.Where(z => z.Position == $"Massage-{name}-Top").Select(z => z.Description)
                .FirstOrDefault();
            m.PositionMassageBottom = texts.Where(z => z.Position == $"Massage-{name}-Bottom").Select(z => z.Description)
                .FirstOrDefault();
            ViewBag.SiteTitle = texts.Where(z => z.Position == $"Massage-{name}-SiteTitle").Select(z => z.Description)
                .FirstOrDefault();
            ViewBag.SiteDescription = texts.Where(z => z.Position == $"Massage-{name}-SiteDescription").Select(z => z.Description)
                .FirstOrDefault();
            
            foreach (var i in list.Select(p => _mapper.Map<HomeViewItem>(p)))
            {
                m.List.Add(i);
            }

            ViewBag.BackGroundImage = $"{WorkLib.GetRandomNumber(2, 14)}.jpg";
            ViewBag.CanonicalUrl = GetCanonicalUrl();
            ViewBag.MenuEscorts = await GetAllMenu();
            ViewBag.GoogleAnalyticsObject = texts.Where(z => z.Position == "GoogleAnalyticsObject").Select(z => z.Description)
                .FirstOrDefault();

            return View("Massage",m);
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("{name}")]
        public async Task<IActionResult> Escorts(string name)
        {
            var texts = await GetAllTexts();
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            switch (name)
            {
                case "las-vegas-strippers.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvStrippers").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvStrippers").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-strippers");

                case "las-vegas-erotic-massage.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvEroticMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvEroticMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-erotic-massage");

                case "las-vegas-nuru-massage.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvNuruMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvNuruMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-nuru-massage");

                case "las-vegas-outcall-massage.php":
                    ViewBag.SiteTitle = "Las Vegas Outcall Massage At VegasRoomService";
                    ViewBag.SiteDescription = "We have beautiful and elegant therapists who are trained to perform Las Vegas outcall massage with their aim being client satisfaction - VegasRoomService";
                    return View("las-vegas-outcall-massage");

                case "las-vegas-in-room-massage.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvInRoomMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvInRoomMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-in-room-massage");

                case "las-vegas-tantra-massage.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvTantraMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvTantraMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-tantra-massage");

                case "las-vegas-body-rubs.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvBodyRubsMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvBodyRubsMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-body-rubs");

                case "las-vegas-asian-massage.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvAsianMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvAsianMassage").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-asian-massage");

                case "las-vegas-asian-escorts.php":
                    ViewBag.SiteTitle = "Las Vegas Asian Escorts To Your Room At VegasRoomService";
                    ViewBag.SiteDescription = "Book the best Las Vegas Asian escorts which will make your time in Las Vegas a memorable one - VegasRoomService.";
                    return View("las-vegas-asian-escorts");

                case "las-vegas-black-escorts.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvBlackEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvBlackEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-black-escorts");

                case "las-vegas-vip-escorts.php":
                    ViewBag.SiteTitle = "Las Vegas VIP (Upscale) Escorts At VegasRoomService";
                    ViewBag.SiteDescription = "At VegasRoomService, we ensure you get the best when looking for a Las Vegas VIP Escort with a class to treat you to a moment of your life";
                    return View("las-vegas-vip-escorts");

                case "las-vegas-blonde-escorts.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvBlondeEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvBlondeEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-blonde-escorts");

                case "las-vegas-brunette-escorts.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvBrunetteEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvBrunetteEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-brunette-escorts");

                case "las-vegas-gfe.php":
                    ViewBag.SiteTitle = texts.Where(z => z.Position == "SiteTitleLvGfeEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    ViewBag.SiteDescription = texts.Where(z => z.Position == "SiteDescriptionLvGfeEscorts").Select(z => z.Description)
                        .FirstOrDefault();
                    return View("las-vegas-gfe");

                default: return RedirectToAction("Error");
            }
        }

        [Route("robots.txt")]
        public ContentResult RobotsTxt()
        {
            var filePath = Path.Combine(_env.WebRootPath,"robots.txt");
            var s = System.IO.File.ReadAllText(filePath);
            return this.Content(s, "text/plain", Encoding.UTF8);
        }

        [Route("sitemap.xml")]
        public ContentResult SiteMap()
        {
            var filePath = Path.Combine(_env.WebRootPath, "sitemap.xml");
            var s = System.IO.File.ReadAllText(filePath);
            return this.Content(s, "text/plain", Encoding.UTF8);
        }

        private async Task<string> GetEscortsHeading(string position)
        {
            var texts = await GetAllTexts();
            return texts.Where(z => z.Position == position)
                .Select(z => z.Description).FirstOrDefault();
        }

        [Route("{seg1?}/{seg2}")]
        public IActionResult BadUrl()
        {
            return RedirectToAction("Error");
        }

        [Route("404.php")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            ViewBag.BackGroundImage = $"{WorkLib.GetRandomNumber(2, 14)}.jpg";
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "";
            ViewBag.SiteDescription = "";

            Response.StatusCode = 404;

            ViewBag.MenuEscorts = await GetAllMenu();

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class HomeViewModel
    {
        public string PositionHomeTopTitle { get; set; }
        public string PositionHomeTop { get; set; }
        public string PositionHomeBottom { get; set; }
        public List<HomeViewItem> List { get; set; } = new List<HomeViewItem>();
    }

    public class EscortsViewModel
    {
        public string EscortsHeading { get; set; }
        public string EscortsNavTitle { get; set; }
        public string PositionEscortsTop { get; set; }
        public string PositionEscortsBottom { get; set; }
        public List<HomeViewItem> List { get; set; } = new List<HomeViewItem>();
    }

    public class MassageViewModel
    {
        public string EscortsNavTitle { get; set; }
        public string PositionMassageTitle { get; set; }
        public string PositionMassageTop { get; set; }
        public string PositionMassageBottom { get; set; }
        public List<HomeViewItem> List { get; set; } = new List<HomeViewItem>();
    }

    public class BodyRubsViewModel
    {
        public string PositionBodyRubsTitle { get; set; }
        public string PositionBodyRubsTop { get; set; }
        public string PositionBodyRubsBottom { get; set; }
        public List<HomeViewItem> List { get; set; } = new List<HomeViewItem>();
    }

    public class AboutUsViewModel
    {
        public string PositionAbout { get; set; }
        public string SiteDescriptionAbout { get; set; }
        public List<HomeViewItem> List { get; set; } = new List<HomeViewItem>();
    }

    public class HomeViewItem : Escort
    {
        
    }
}
