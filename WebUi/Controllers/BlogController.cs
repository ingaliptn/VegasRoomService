using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUi.Lib;

namespace WebUi.Controllers
{
    public class BlogController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("blog.php")]
        public IActionResult Index()
        {
            ViewBag.BackGroundImage = $"{WorkLib.GetRandomNumber(2, 14)}.jpg";
            ViewBag.CanonicalUrl = GetCanonicalUrl();
            ViewBag.SiteTitle = "Escort Blog At VegasRoomService";
            ViewBag.SiteDescription = "See our guides to escort industry on our blog at VegasRoomService";
            
            return View();
        }

#if !DEBUG
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("blog/{name}")]
        public IActionResult Blog(string name)
        {
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            switch (name)
            {
                case "backpage-alternative.php":
                    ViewBag.SiteDescription = "You might not know this, but Backpage got shut down by federals. That is why we are going to provide you the best Backpage alternative - VegasRoomService blog.";
                    ViewBag.SiteTitle = "Alternative To Backpage: Find The Best Website - VegasRoomService Blog";
                    return View("backpage-alternative");

                case "why-tantra-massage-good.php":
                    ViewBag.SiteDescription = "Click and read why is Tantra massage good for your sexual health. We'll give multiple unexpected reasons why you should try it now - VegasRoomService blog.";
                    ViewBag.SiteTitle = "Why Is Tantra Massage Good For You - VegasRoomService Blog";
                    return View("why-tantra-massage-good");

                case "what-is-nuru-massage.php":
                    ViewBag.SiteDescription = "What is sensual NURU massage: find the definition, health benefits and much more interesting facts about this type of massage - VegasRoomService blog.";
                    ViewBag.SiteTitle = "What Is NURU Massage: Definition, History And Benefits - VegasRoomService Blog";
                    return View("what-is-nuru-massage");

                case "what-is-body-rubs.php":
                    ViewBag.SiteDescription = "The definition of term body rubs and why you should try it now if you did not do it before. Click and find more - VegasRoomService blog.";
                    ViewBag.SiteTitle = "What Is A Body Rubs: Definition And Benefits - VegasRoomService Blog";
                    return View("what-is-body-rubs");

                case "is-happy-ending-massage-worth.php":
                    ViewBag.SiteDescription = "Is happy ending massage worth trying? We'll give you multiple reasons on why this massage is good for you - VegasRoomService blog.";
                    ViewBag.SiteTitle = "Why You Should Try Happy Ending Massage - VegasRoomService Blog";
                    return View("is-happy-ending-massage-worth");

                case "gd-or-vrs.php":
                    ViewBag.SiteDescription = "Find out why you should avoid Girl Directory and hire girls on VegasRoomService instead. VegasRoomService blog.";
                    ViewBag.SiteTitle = "Girl Directory Or VegasRoomService: What To Choose? VegasRoomService Blog";
                    return View("gd-or-vrs");

                default: return RedirectToAction("Error");
            }
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
    }
}
