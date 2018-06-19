using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalProjectPortfolio.Controllers
{
    public class ChooseStocksController : Controller
    {
        // GET: ChooseStocks
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewsOfTheDay()
        {

            HttpWebRequest request =
          WebRequest.CreateHttp($"https://api.nytimes.com/svc/search/v2/articlesearch.json?q=business&begin_date=20000202&end_date=20000202");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";


            request.Headers.Add("api-key", ConfigurationManager.AppSettings["api-key"]);


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader data = new StreamReader(response.GetResponseStream());
            string article = data.ReadToEnd();
            JObject articleData = JObject.Parse(article);

            ViewBag.RawData = articleData["response"]["docs"][4]["abstract"];
            ViewBag.URL = articleData["response"]["docs"][4]["web_url"];
            return View();

        }

        public ActionResult StockSelector()
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=AAPL&outputsize=full&apikey=apikey");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
            //hiding api key

               HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //if (response.StatusCode == HttpStatusCode.OK) //Ok is the 200 status
            //{
            StreamReader data = new StreamReader(response.GetResponseStream());
            string stock = data.ReadToEnd();
            //string date = "2018-06-12";

            ////ViewBag.RawData = data.ReadToEnd(); //read all the response data
            JObject JsonData = JObject.Parse(stock);
            ViewBag.StockData = JsonData["Meta Data"]["2. Symbol"];
            //ViewBag.StockData2 = JsonData["Time Series (Daily)"][date]["4. close"];
            return View();

        }
    }
}