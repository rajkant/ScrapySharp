using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using ScrapySharp.Extensions;
using System.Net.Http;
using HtmlAgilityPack;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            NetworkCredential proxyCreds = new NetworkCredential("GSQ3HHC", "Feb@2018");
            string url = "http://desilouisville.com/indian-desi-community-classifieds-sales-car-sale-room-mates-babysitting-beauty-parlor.php?cat=6";
            string result = ScrapeSourceAsync(url, proxyCreds).GetAwaiter().GetResult();

            var webGet = new HtmlWeb();
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            string text = string.Empty;
            var list = new List<String>();
            if (doc is HtmlDocument document)
            {
                //var nodes = document.DocumentNode.CssSelect("#item-search-results li").ToList();
                var nodes = document.DocumentNode.CssSelect("div.col-sm-7.hidden-xs.padding_new.fontsizesmall").ToList();
                foreach (var node in nodes)
                {
                    text += node.CssSelect("a").Single().OuterHtml;
                    list.Add(node.CssSelect("a").Single().OuterHtml);
                }
            }
            ViewBag.Data = list;
            return View();
        }

        private async Task<string> ScrapeSourceAsync(string url, NetworkCredential proxyCreds)
        {
            string responseBody = string.Empty;
            WebProxy proxy = new WebProxy("proxy.ups.com:8080", false)
            {
                UseDefaultCredentials = false,
                Credentials = proxyCreds,
            };

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                Credentials = proxyCreds
            };

            var httpClient = new HttpClient(httpClientHandler);
            if (url.StartsWith("//"))
                url = $"http:{url}";

            responseBody = await httpClient.GetStringAsync(url);
            return responseBody;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
