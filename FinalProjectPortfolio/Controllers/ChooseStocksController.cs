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

        public ActionResult NewsOfTheDay(string historicalDate)
        {

            HttpWebRequest request =
          WebRequest.CreateHttp($"https://api.nytimes.com/svc/search/v2/articlesearch.json?q=business&begin_date={historicalDate}&end_date={historicalDate}");
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

        public string StockSelector(string userInput)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={userInput}&outputsize=full&apikey=apikey");
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
           string symbol = JsonData["Meta Data"]["2. Symbol"].ToString();
            //ViewBag.StockData2 = JsonData["Time Series (Daily)"][date]["4. close"];
            return symbol;
        }



    public static double HistoricalSharePrice(string symbol, string historicalDate)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey=apikey");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
            //hiding api key

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //if (response.StatusCode == HttpStatusCode.OK) //Ok is the 200 status
            //{
            StreamReader data = new StreamReader(response.GetResponseStream());
            string stockprice = data.ReadToEnd();
            //string date = "2018-06-12";
            //string now = 
       

            ////ViewBag.RawData = data.ReadToEnd(); //read all the response data
            JObject JsonData = JObject.Parse(stockprice);
            //ViewBag.StockData = JsonData["Meta Data"]["2. Symbol"];
            double begSharePrice = Double.Parse(JsonData["Time Series (Daily)"][historicalDate]["4. close"].ToString());
            //double todayPrice = Double.Parse(JsonData["Time Series (Daily)"][now]["4. close"].ToString());

            //ViewBag.UserPrice = userPrice;
            // ViewBag.TodayPrice = todayPrice;

            return begSharePrice;

        }

        public static double TodaySharePrice(string symbol)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=1min&apikey=apikey");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
            //hiding api key

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //if (response.StatusCode == HttpStatusCode.OK) //Ok is the 200 status
            //{
            StreamReader data = new StreamReader(response.GetResponseStream());
            string stockprice = data.ReadToEnd();
            //string date = "2018-06-12";
            //string now = DateTime.Today.ToString();
            //string time = "11:00:00";

            //ViewBag.RawData = data.ReadToEnd(); //read all the response data
            JObject JsonData = JObject.Parse(stockprice);
            //ViewBag.StockData = JsonData["Meta Data"]["2. Symbol"];
            //double userPrice = Double.Parse(JsonData["Time Series (Daily)"][date]["4. close"].ToString());
            
            double closingSharePrice = Double.Parse(JsonData["Time Series (1min)"]["2018-06-19 11:00:00"]["4. close"].ToString());

            //ViewBag.UserPrice = userPrice;
            //ViewBag.TodayPrice = todayPrice;

            return closingSharePrice;

        }
        public static double NumberOfShares(double begInvestmentValue, double begSharePrice)
        {
            //gives number of shares
            double numberOfShares = begInvestmentValue / begSharePrice;
            //current value
            //double currentValue = todaySharePrice * numberOfShares;

            //double percentage = currentvalue / amountInvested;
            //total return on investment
            //double profit = currentvalue - amountInvested;

            return numberOfShares;
        }

        public static double currentValue (double closingSharePrice, double numberOfShares)
        {
            //gives the new current value
            double newValue = closingSharePrice * numberOfShares;
            return newValue;
        }

        public static double Percentage(double endingInvestmentValue, double begInvestmentValue)
        {
            //gives the percentage increase
            double percentIncrease = endingInvestmentValue / begInvestmentValue;
            return percentIncrease;
        }

        public static double TotalAmountGained(double endingInvestmentValue, double begInvestmentValue)
        {
            //gives the total amount of gain or loss
            double profit = endingInvestmentValue - begInvestmentValue;
            return profit;
        }

        
    }
}