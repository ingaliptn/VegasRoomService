﻿using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WebUi.Infrastructure;
using WebUi.lib;
using WebUi.Lib;
using WebUi.Models;

namespace WebUi.Controllers
{
    public class BookingController : BaseController
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailSender _email;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

#if DEBUG
        private readonly List<string> _emailsList = new(new string[] { "pmx@ukr.net", "nc@ukr.net", "ncmail@ukr.net", "maxim.paramonov.ua@gmail.com" });
        //private readonly List<string> _emailsList = new(new string[] { "v.bokhonov@serverpipe.com", "v.bokhonov@gmail.com", "vbokhonov@outlook.com", "stef.haeb34@hotmail.com" });
#else
        private readonly List<string> _emailsList = new(new string[] { "support@usescortagency.com", "manager@usescortagency.com", "girls@usescortagency.com", "supervisor@usescortagency.com" });
        //private readonly List<string> _emailsList = new(new string[] { "v.bokhonov@serverpipe.com", "v.bokhonov@gmail.com", "vbokhonov@outlook.com", "stef.haeb34@hotmail.com" });
#endif

        public BookingController(IEscortRepository escortRepository,
            IMapper mapper,
            IEmailSender email,
            IMenuRepository menuRepository,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache,
            ITextRepository textRepository,
            ILogger<BookingController> logger) : base(escortRepository, textRepository, menuRepository, memoryCache)
        {
            _logger = logger;
            _mapper = mapper;
            _email = email;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

#if !DEBUG
                [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("booking.php")]
        public async Task<IActionResult> Index()
        {
            var m = new BookingViewModel();

            var escorts = await GetAllEscorts();

            foreach (var p in escorts)
            {
                m.List.Add(_mapper.Map<BookingViewItem>(p));
            }

            
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "VegasRoomService Booking Page";
            ViewBag.SiteDescription = "Book affordable girls at VegasRoomService agency";

            return View(m);
        }

#if !DEBUG
                [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("contact.php")]
        public IActionResult Contact()
        {          
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "Contact VegasRoomService Agency";
            ViewBag.SiteDescription = "Get your questions answered by contacting VegasRoomService agency";

            return View();
        }

#if !DEBUG
                [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
#endif
        [Route("employment.php")]
        public IActionResult Employment()
        {
            var m = new EmploymentForm();
            
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "VegasIndependents employment form";
            ViewBag.SiteDescription = "VegasIndependents employment form";

            return View(m);
        }

        [HttpPost]
        [Route("thankyou")]
        public async Task<IActionResult> ThankYou(BookingForm m)
        {
            var escorts = await GetAllEscorts();
            var body = string.Empty;
            if (m.escortname != 0)
            {

                var t = escorts
                    .Where(z => z.Id == m.escortname).Select(z => z.EscortName).First();
                body = $"Preferred Escort: {t}<br/>";
            }
            if (m.escortname1 != 0)
            {
                var t = escorts
                    .Where(z => z.Id == m.escortname1).Select(z => z.EscortName).First();
                body = $"{body}<br/>Alternate Escort: {t}<br/>";
            }

            body += m.ToString();

            foreach (var email in _emailsList)
            {
                await _email.SendEmailAsync(email, Constants.SiteName, body);
            }

            ViewBag.BackGroundImage = $"{WorkLib.GetRandomNumber(2, 14)}.jpg";
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "";
            ViewBag.SiteDescription = "";

            ViewBag.MenuEscorts = await GetAllMenu();

            return View();
        }


        [HttpPost]
        [Route("ThankYouContact")]
        public async Task<IActionResult> ThankYouContact(ContactUsForm m)
        {
            foreach (var email in _emailsList)
            {
                await _email.SendEmailAsync(email, Constants.SiteName, m.ToString());
            }

            ViewBag.BackGroundImage = $"{WorkLib.GetRandomNumber(2, 14)}.jpg";
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "";
            ViewBag.SiteDescription = "";

            ViewBag.MenuEscorts = await GetAllMenu();

            return View("ThankYou");
        }


        [HttpPost]
        [Route("ThankYouEmployment")]
        public async Task<IActionResult> ThankYouEmployment(EmploymentForm m)
        {

            if (m.FileUpload != null && m.FileUpload.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, m.FileUpload.FileName);
                await using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await m.FileUpload.CopyToAsync(fileStream);
                }

                foreach (var email in _emailsList)
                {
                    await _email.SendEmailFileAsync(email, Constants.SiteName, m.ToString(), filePath);
                }
            }
            else
            {
                foreach (var email in _emailsList)
                {
                    await _email.SendEmailAsync(email, Constants.SiteName, m.ToString());
                }
            }

            ViewBag.BackGroundImage = $"{WorkLib.GetRandomNumber(2, 14)}.jpg";
            ViewBag.CanonicalUrl = GetCanonicalUrl();

            ViewBag.SiteTitle = "";
            ViewBag.SiteDescription = "";

            ViewBag.MenuEscorts = await GetAllMenu();

            return View("ThankYou");
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

    public class BookingViewModel
    {
        public List<BookingViewItem> List { get; set; } = new List<BookingViewItem>();
    }

    public class BookingViewItem
    {
        public int Id { get; set; }
        public string EscortName { get; set; }
    }

    public class BookingForm
    {
        public int escortname { get; set; }
        public int escortname1 { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string hotelname { get; set; }
        public string hotelroom { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string request { get; set; }
        public override string ToString()
        {
            return $"First Name: {this.firstname}<br/>Last Name: {this.lastname}<br/>" +
                   $"Hotel Name: {this.hotelname}<br/>Room Number: {this.hotelroom}<br/>" +
                   $"Email: {this.email}<br/>Phone No.: {this.phone}<br/>" +
                   $"Special Request: {this.request}";
        }
    }

    public class ContactUsForm
    {
        public string name { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public override string ToString()
        {
            return $"Name: {this.name}<br/>Email: {this.email}<br/>" +
                   $"Subject: {this.subject}<br/>Your Message: {this.message}";
        }
    }

    public class EmploymentForm
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string sdescription { get; set; }
        //public string file_name { get; set; }
        public IFormFile FileUpload { get; set; }
        public override string ToString()
        {
            return $"First Name: {this.firstname}<br/>Last Name: {this.lastname}<br/>" +
                   $"Age: {this.age}<br/>Email: {this.email}<br/>" +
                   $"Phone No.: {this.phone}<br/>Short Description: {this.sdescription}";
        }
    }
}
