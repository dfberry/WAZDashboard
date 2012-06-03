using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureDashboardService.Controllers
{
    public class FeedIssueController : DashboardBaseController
    {
        //
        // GET: /FeedIssue/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display list of grouped issues in grid
        /// using jqGrid.
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult IssueListGrid()
        {
            this.InternalBuild();

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && (this.DashboardIssueModel.RssIssues.Issues != null)
                && (this.DashboardIssueModel.RssIssues.Issues.Count() > 0))
            {
                return View("IssueListGrid", this.DashboardIssueModel.RssIssues);
            }
            else
            {
                return View("NoIssues");
            }
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
            this.InternalBuild();

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && ((this.DashboardIssueModel.RssIssues.Issues == null) || (this.DashboardIssueModel.RssIssues.Issues.Count() == 0)))
            {
                return null;
            }

            // Order data by service and location
            var issuesSorted = from issue in this.DashboardIssueModel.RssIssues.Issues
                              orderby issue.RssFeed.ServiceName, issue.RssFeed.LocationName
                              select issue;

            // Paging 
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = issuesSorted.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            // feeds paged subset
            var issuesPaged = issuesSorted.Skip(pageIndex * pageSize).Take(pageSize);

            int rowId = 0;

            // build response specific to jgGrid
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from issue in issuesSorted
                    select new
                    {
                        i = rowId++,
                        cell = new string[] { rowId.ToString(), issue.RssFeed.ServiceName, issue.RssFeed.LocationName, issue.RssFeed.FeedCode, issue.RssFeed.RSSLink }
                    }).ToArray()
            };

            return Json(jsonData);
        }

        /// <summary>
        /// Build new Rss issue list and then show View
        /// that contains the datetime stamp
        /// verifying the new datetime
        /// </summary>
        /// <returns>ActionResult of IssueListGroupGrid</returns>
        public ActionResult Build()
        {
            this.InternalBuild();

            this.Notify("/FeedIssue/Build called", "test");

            return this.IssueListGrid();
        }

        /// <summary>
        /// Grab issue xml  from Azure, save to serialized file
        /// </summary>
        public void InternalBuild()
        {
            // build new file - should also have a new feeddata as well
            // to verify new file
            this.IssueMgr.SetRssIssuesFromUri(this.PathToFiles);

            this.DashboardIssueModel.RssIssues = this.IssueMgr.GetStoredRssIssues(this.PathToFiles);
        }}
}
