﻿// -----------------------------------------------------------------------
// <copyright file="FeedsController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AzureDashboardService.Factories;

    /// <summary>
    /// Feed List (rss feeds) api controller
    /// </summary>
    public class FeedsController : DashboardBaseController
    {
        /// <summary>
        /// Display list of grouped feeds in grid
        /// using jqGrid.
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult Show()
        {
            this.InternalBuild();

            if ((this.DashboardModel != null) 
                && (this.DashboardModel.Feeds != null)
                && (this.DashboardModel.Feeds.Feeds != null) 
                && (this.DashboardModel.Feeds.Feeds.Count() > 0))
            {
                return View("FeedListGroupGrid", this.DashboardModel.Feeds);
            }
            else
            {
                return View("NoFeeds");
            }
        }

        /// <summary>
        /// Return instructions for OPML download
        /// </summary>
        /// <returns>opml content as ActionResult</returns>
        public ActionResult Download()
        {
            return View();
        }

        /// <summary>
        /// Return OPML file as download. 
        /// </summary>
        /// <returns>opml content as ActionResult</returns>
        public ActionResult OPML()
        {
            this.InternalBuild();

            if ((this.DashboardModel != null)
                && (this.DashboardModel.Feeds != null)
                && (this.DashboardModel.Feeds.Feeds != null) 
                && (this.DashboardModel.Feeds.Feeds.Count() > 0))
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
        public JsonResult List(string sidx, string sord, int page, int rows)
        {
            this.InternalBuild();

            if ((this.DashboardModel != null)
                && (this.DashboardModel.Feeds != null)
                && ((this.DashboardModel.Feeds.Feeds == null) || (this.DashboardModel.Feeds.Feeds.Count() == 0)))
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

            HttpContext.Response.AddHeader("Cache-Control", "no-cache"); 

            return Json(jsonData);
        }

        /// <summary>
        /// Build new Rss feed list and then show View
        /// that contains the datetime stamp
        /// verifying the new datetime
        /// </summary>
        /// <returns>ActionResult of FeedListGroupGrid</returns>
        public ActionResult Build()
        {
            this.InternalBuild();
 
            this.Notify("/FeedList/Build called", "test");

            return this.Show();
        }

        /// <summary>
        /// Converts string to byte array. Used to return
        /// opml file as download from website.
        /// </summary>
        /// <param name="opmlfilecontents">opml file string</param>
        /// <returns>opml file contents as byte[]</returns>
        private static byte[] StrToByteArray(string opmlfilecontents)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(opmlfilecontents);
        }

        /// <summary>
        /// Grab html from Azure, parse, save to serialized file
        /// </summary>
        private void InternalBuild()
        {
            // build new file - should also have a new feeddata as well
            // to verify new file
            this.DashboardMgr.SetRssFeedsFromUri(this.PathToFiles);

            this.DashboardModel.Feeds = this.DashboardMgr.GetStoredRssFeeds(this.PathToFiles);
        }
    }
}
