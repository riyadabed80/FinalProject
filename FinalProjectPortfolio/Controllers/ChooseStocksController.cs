using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProjectPortfolio.Models;

namespace FinalProjectPortfolio.Controllers
{
    public class ChooseStocksController : Controller
    {
        /* this action take the user input and save it to DB */
        public ActionResult SaveSymbol(Portfolio_Table port)
        { 
            portfolioEntities ORM = new portfolioEntities();

               
                port.stock1_beg_share_price = HistoricalSharePrice(port.stock1_tkr, port.stock1_date);
                port.stock2_beg_share_price = HistoricalSharePrice(port.stock2_tkr, port.stock2_date);
                port.stock3_beg_share_price = HistoricalSharePrice(port.stock3_tkr, port.stock3_date);
                port.stock1_closing_share_price = TodaySharePrice(port.stock1_tkr);
                port.stock2_closing_share_price = TodaySharePrice(port.stock2_tkr);
                port.stock3_closing_share_price = TodaySharePrice(port.stock3_tkr);
                port.stock1_no_shares = NumberOfShares(port.stock1_beg_investment_value, port.stock1_beg_share_price);
                port.stock2_no_shares = NumberOfShares(port.stock1_beg_investment_value, port.stock2_beg_share_price);
                port.stock3_no_shares = NumberOfShares(port.stock1_beg_investment_value, port.stock3_beg_share_price);
                port.stock1_ending_investment_value = port.stock1_no_shares * port.stock1_closing_share_price;
                port.stock2_ending_investment_value = port.stock2_no_shares * port.stock2_closing_share_price;
                port.stock3_ending_investment_value = port.stock3_no_shares * port.stock3_closing_share_price;




            ORM.Portfolio_Table.Add(port);
            ORM.SaveChanges();
            ViewBag.StockTable = ORM.Portfolio_Table.ToList();
            return View();
        }

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


        //public string StockSymbol(string userInput)
        //{
        //     //= "AAPL";
        //    HttpWebRequest request = WebRequest.CreateHttp($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={userInput}&outputsize=full&apikey=apikey");
        //    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
        //    //hiding api key

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    //if (response.StatusCode == HttpStatusCode.OK) //Ok is the 200 status
        //    //{
        //    StreamReader data = new StreamReader(response.GetResponseStream());
        //    string stock = data.ReadToEnd();


        //    ////ViewBag.RawData = data.ReadToEnd(); //read all the response data
        //    JObject JsonData = JObject.Parse(stock);
        //    string symbol = JsonData["Meta Data"]["2. Symbol"].ToString();
        //    //ViewBag.StockData2 = JsonData["Time Series (Daily)"][date]["4. close"];
        //    return symbol;
        //}

        public static decimal HistoricalSharePrice(string symbol, string historicalDate)
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
            decimal begSharePrice = Decimal.Parse(JsonData["Time Series (Daily)"][historicalDate]["4. close"].ToString());
            //double todayPrice = Double.Parse(JsonData["Time Series (Daily)"][now]["4. close"].ToString());

            //ViewBag.UserPrice = userPrice;
            // ViewBag.TodayPrice = todayPrice;

            return begSharePrice;

        }

        public static decimal TodaySharePrice(string symbol)
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

            decimal closingSharePrice = Decimal.Parse(JsonData["Time Series (1min)"]["2018-06-21 11:00:00"]["4. close"].ToString());

            //ViewBag.UserPrice = userPrice;
            //ViewBag.TodayPrice = todayPrice;

            return closingSharePrice;

        }

        public static int NumberOfShares(decimal begInvestmentValue, decimal begSharePrice)
        {
            //gives number of shares
            int  numberOfShares = Convert.ToInt32(begInvestmentValue / begSharePrice);
            //current value
            //double currentValue = todaySharePrice * numberOfShares;

            //double percentage = currentvalue / amountInvested;
            //total return on investment
            //double profit = currentvalue - amountInvested;

            return numberOfShares;
        }

        public static double currentValue(double closingSharePrice, double numberOfShares)
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

        public ActionResult StockSelector()
        {

            return View();
        }

        public ActionResult Portfolio()
        {
            return View();
        }


    }
}