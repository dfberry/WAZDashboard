// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AzureDashboardService.Models;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using System.Net.Mail;

    /// <summary>
    /// Home controller page of the web site
    /// </summary>
    public class HomeController : DashboardBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        public HomeController()
            : base()
        {
        }

        /// <summary>
        /// Converts string to byte array. Used to return
        /// opml file as download from website.
        /// </summary>
        /// <param name="opmlfilecontents">opml file string</param>
        /// <returns>opml file contents as byte[]</returns>
        public static byte[] StrToByteArray(string opmlfilecontents)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(opmlfilecontents);
        }

        /// <summary>
        /// Display list of feeds
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult FeedList()
        {
            return View(this.DashboardModel.FeedList);
        }

        /// <summary>
        /// Display list of grouped feeds in grid
        /// using jqGrid.
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult FeedListGroupGrid()
        {
            return View(this.DashboardModel.FeedList);
        }

        /// <summary>
        /// Display list of grouped feeds in grid
        /// using jqGrid.
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult FeedListjqGrid()
        {
            return View("FeedListGroupGrid", this.DashboardModel.FeedList);
        }

        /// <summary>
        /// Default Home page
        /// </summary>
        /// <returns>Home page content as ActionResult</returns>
        public ActionResult Index()
        {
            return View(this.DashboardModel);
        }

        /// <summary>
        /// Default About page
        /// </summary>
        /// <returns>about page as ActionResult</returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Contact page
        /// </summary>
        /// <returns>contact page as ActionResult</returns>
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Default Download page
        /// </summary>
        /// <returns>download page as ViewResult</returns>
        public ViewResult Download()
        {
            return View();
        }

        /// <summary>
        /// Default Download page
        /// </summary>
        /// <returns>download page as ViewResult</returns>
        public ViewResult Tasks()
        {
            MailAddress from = new MailAddress("wazup.berryintl@gmail.com", "AppHarbor Tasks");
            MailAddress to = new MailAddress("dina@berryintl.com", "AppHarbor Tasks");

            MailMessage message = new MailMessage(from, to);

            message.Subject = "Tasks: AppHarbor Tasks";

            //message.Body = DateTime.Now.ToString() + "\n" + text + "\n";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";   // We use gmail as our smtp client
            client.Port = 587;
            client.UseDefaultCredentials = true;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("wazup.berryintl@gmail.com", "redrum88");


            client.Send(message);
            return View();
        }

        
        /// <summary>
        /// Return OPML file as download. 
        /// </summary>
        /// <returns>opml content as ActionResult</returns>
        public ActionResult OPML()
        {
            byte[] opmlFile = StrToByteArray(this.DashboardMgr.OPML());

            return File(opmlFile, "application/xml", "WazServiceDashboardOpml.xml");
        }

        /// <summary>
        /// Return OPML file as download. 
        /// Output cache set to 1 day = 24 hours = 86400 seconds
        /// </summary>
        /// <returns>cached opml as ActionResult</returns>
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult OPMLCached()
        {
            return this.OPML();
        }

        /// <summary>
        /// Stolen from http://www.timdavis.com.au/code/jquery-grid-with-asp-net-mvc/
        /// </summary>
        /// <param name="sidx">id of row</param>
        /// <param name="sord">sort order</param>
        /// <param name="page">page count</param>
        /// <param name="rows">row count</param>
        /// <returns>JsonResult specific to jqGrid</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DynamicGridData(string sidx, string sord, int page, int rows)
        {
            // Get data
            var feeds = this.DashboardMgr.Feeds();

            // Order data by service and location
            var feedsSorted = from feed in feeds
                              orderby feed.ServiceName, feed.LocationName
                              select feed;

            // Paging 
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = feedsSorted.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            // feeds paged subset
            var feedsPaged = feedsSorted.Skip(pageIndex * pageSize).Take(pageSize);

            int rowId = 0;

            // build response specific to jgGrid
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from feed in feedsSorted
                    select new
                    {
                        i = rowId++,
                        cell = new string[] { rowId.ToString(), feed.ServiceName, feed.LocationName, feed.FeedCode, feed.RSSLink }
                    }).ToArray()
            };

            return Json(jsonData);
        }


    }
}
