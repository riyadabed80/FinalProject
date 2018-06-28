using FinalProjectPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProjectPortfolio.Controllers
{
    public class Leaders
    {
        public string UserName { get; set; }
        public Portfolio UserPortfolio { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Leaderboard()
        {
            portfolioEntities ORM = new portfolioEntities();
            List<Portfolio> results = ORM.Portfolios.ToList();
            List<Leaders> leaders = new List<Leaders>();

            foreach (Portfolio portfolio in results)
            {
                string userName = ORM.AspNetUsers.Find(portfolio.userID).UserName;
                leaders.Add(new Leaders() { UserPortfolio = portfolio, UserName = userName });
            }

            ViewBag.StockTable = leaders.OrderByDescending(x => x.UserPortfolio.totalprofiit).ToList();
            return View();
        }

        public ActionResult StockSelector()
        { return View(); }



        public JsonResult SearchStockName(string name)
        {
            // ORM

            portfolioEntities ORM = new portfolioEntities();

            // search by name

            List<StockTkr> Result = ORM.StockTkrs.Where(c => c.Name.Contains(name)).ToList();

            // Return data as Jason!

            return Json(Result);
        }
    }
}