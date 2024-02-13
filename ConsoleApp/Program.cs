using System;
using System.Linq;
using ScrapySharp.Extensions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            NetworkCredential proxyCreds = new NetworkCredential("GSQ3HHC", "");
            string url = "http://tipidpc.com/catalog.php?cat=0&sec=s";            
            //string url = "https://blog.scrapinghub.com";
            
            string result = ScrapeSourceAsync(url, proxyCreds).GetAwaiter().GetResult();
                        
            var webGet = new HtmlWeb();
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            if (doc is HtmlDocument document)
            {
                //var nodes = document.DocumentNode.CssSelect("#item-search-results li").ToList();
                var nodes = document.DocumentNode.CssSelect("div.col-sm-7.hidden-xs.padding_new.fontsizesmall").ToList();
                foreach (var node in nodes)
                {
                    Console.WriteLine("Text: " + node.CssSelect("a").Single().InnerText);                  
                }
            }

            Console.ReadLine();
        }

        private static async Task<string> ScrapeSourceAsync(string url, NetworkCredential proxyCreds)
        {
            string responseBody = string.Empty;
            //WebProxy proxy = new WebProxy("proxy.ups.com:8080", false)
            //{
            //    UseDefaultCredentials = false,
            //    Credentials = proxyCreds,
            //};

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                //Proxy = proxy,
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
    }
}
