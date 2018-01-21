using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiApp.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.SpaServices.Prerendering;
using Microsoft.Extensions.DependencyInjection;

namespace DamkorkiApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var prerenderResult = await Request.BuildPrerender();

            // This is where everything is now spliced out, and given to .NET in pieces
            ViewData["SpaHtml"] = prerenderResult.Html; // out <app> from Angular 
            ViewData["Title"] = prerenderResult.Globals["title"]; // set our <title> from Angular 
            ViewData["Styles"] = prerenderResult.Globals["styles"]; // put styles in the correct place 
            ViewData["Scripts"] = prerenderResult.Globals["scripts"]; // scripts (that were in our header)
            ViewData["Meta"] = prerenderResult.Globals["meta"]; // set our <meta> SEO tags
            ViewData["Links"] = prerenderResult.Globals["links"]; // set our <link rel="canonical"> etc SEO tags
            ViewData["TransferData"] = prerenderResult.Globals["transferData"]; // our transfer data set to window.TRANSFER_CACHE = {};

            // Let's render that Home/Index view
            return View();
        }

        [HttpGet]
        [Route("sitemap.xml")]
        public async Task<IActionResult> SitemapXml()
        {
            String xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"; 

            xml += "<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
            xml += "<sitemap";
            xml += "<loc>http:localhost:4251/home</loc>";
            xml += "<lastmod>" + DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod>";
            xml += "</sitemap>";
            xml += "<sitemap"; 
            xml += "<loc>http://localhost:4251/counter</loc>";
            xml += "<lastmod>" + DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod>";
            xml += "</sitemap>"; 
            xml += "</sitemapindex>"; 

            return Content(xml, "text/xml"); 
        }

        public IActionResult Error() 
        {
            return View(); 
        }
    }
}
