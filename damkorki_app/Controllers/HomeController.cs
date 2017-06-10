using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.SpaServices.Prerendering;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplicationBasic.Controllers
{
    public class HomeController : Controller
    {
         public async Task<IActionResult> Index()
        {
            
            var nodeServices = Request.HttpContext.RequestServices.GetRequiredService<INodeServices>();
            var hostEnv = Request.HttpContext.RequestServices.GetRequiredService<IHostingEnvironment>();

            var applicationBasePath = hostEnv.ContentRootPath;
            var requestFeature = Request.HttpContext.Features.Get<IHttpRequestFeature>();
            var unencodedPathAndQuery = requestFeature.RawTarget;
            var unencodedAbsoluteUrl = $"{Request.Scheme}://{Request.Host}{unencodedPathAndQuery}";

            // *********************************
            // This parameter is where you'd pass in an Object of data you want passed down to Angular 
            // to be used in the Server-rendering

            // ** TransferData concept **
            // Here we can pass any Custom Data we want !

            // By default we're passing down the REQUEST Object (Cookies, Headers, Host) from the Request object here
            TransferData transferData = new TransferData();
            transferData.request = AbstractHttpContextRequestInfo(Request); // You can automatically grab things from the REQUEST object in Angular because of this
            transferData.thisCameFromDotNET = "Hi Angular it's asp.net :)";
            // Add more customData here, add it to the TransferData class

            // Prerender / Serialize application (with Universal)
            var prerenderResult = await Prerenderer.RenderToString(
                "/", // baseURL
                nodeServices,
                new JavaScriptModuleExport(applicationBasePath + "/Client/dist/main-server"),
                unencodedAbsoluteUrl,
                unencodedPathAndQuery,
                // Our Transfer data here will be passed down to Angular (within the boot-server file)
                // Available there via `params.data.yourData`
                transferData, 
                30000, // timeout duration
                Request.PathBase.ToString()
            );

            // This is where everything is now spliced out, and given to .NET in pieces
            ViewData["SpaHtml"] = prerenderResult.Html;
            ViewData["Title"] = prerenderResult.Globals["title"];
            ViewData["Styles"] = prerenderResult.Globals["styles"];
            ViewData["Meta"] = prerenderResult.Globals["meta"];
            ViewData["Links"] = prerenderResult.Globals["links"];
            ViewData["TransferData"] = prerenderResult.Globals["transferData"]; // our transfer data set to window.TRANSFER_CACHE = {};

            

            // Let's render that Home/Index view
            return View();
        }

        private IRequest AbstractHttpContextRequestInfo(HttpRequest request)
        {

            IRequest requestSimplified = new IRequest();
            requestSimplified.cookies = request.Cookies;
            requestSimplified.headers = request.Headers;
            requestSimplified.host = request.Host;

            return requestSimplified;
        }
        
    }

    public class IRequest
    {
        public object cookies { get; set; }
        public object headers { get; set; }
        public object host { get; set; }
    }

    public class TransferData
    {
        // By default we're expecting the REQUEST Object (in the aspnet engine), so leave this one here
        public dynamic request { get; set; } 

        // Your data here ?
        public object thisCameFromDotNET { get; set; }
    }
    
}
