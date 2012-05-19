// -----------------------------------------------------------------------
// <copyright file="FeedListController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using AzureDashboardService.Models;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using Wp7AzureMgmt.DashboardFeeds.Utilities;

    /// <summary>
    /// Feed List (rss feeds) api controller
    /// </summary>
    public class FeedListController : DashboardBaseController
    {
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
        /// Display list of grouped feeds in grid
        /// using jqGrid.
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult FeedListjqGrid()
        {
            this.GetFeeds();

            if ((this.DashboardModel.Feeds.Feeds != null) && (this.DashboardModel.Feeds.Feeds.Count() > 0))
            {
                return View("FeedListGroupGrid", this.DashboardModel.Feeds);
            }
            else
            {
                return View("NoFeeds");
            }
        }

        /// <summary>
        /// Return OPML file as download. 
        /// </summary>
        /// <returns>opml content as ActionResult</returns>
        public ActionResult OPML()
        {
            this.GetFeeds();

            if ((this.DashboardModel.Feeds.Feeds != null) && (this.DashboardModel.Feeds.Feeds.Count() > 0))
            {
                byte[] opmlFile = StrToByteArray(this.DashboardModel.Feeds.OPML());

                return File(opmlFile, "application/xml", "WazServiceDashboardOpml.xml");
            }
            else
            {
                return View("NoFeeds");
            }
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
        /// <param name="sidx">id of row to begin at</param>
        /// <param name="sord">sort order</param>
        /// <param name="page">page count</param>
        /// <param name="rows">row count for pagesize</param>
        /// <returns>JsonResult specific to jqGrid</returns>
        public JsonResult DynamicGridData(string sidx, string sord, int page, int rows)
        {
            this.GetFeeds();

            if ((this.DashboardModel.Feeds.Feeds == null) || (this.DashboardModel.Feeds.Feeds.Count() == 0))
            {
                return null;
            }
            
            // Order data by service and location
            var feedsSorted = from feed in this.DashboardModel.Feeds.Feeds
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

        /// <summary>
        /// Grab feeds from serialized file
        /// </summary>
        private void GetFeeds()
        {
            try
            {
                this.DashboardModel.Feeds = this.DashboardMgr.GetStoredRssFeeds(this.PathToFiles);
            }
            catch (Exception ex)
            {
                throw new Exception("FeedListController::GetFeeds - couldn't get feeds");
            }
        }
    }
}
