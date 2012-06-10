// -----------------------------------------------------------------------
// <copyright file="FeedIssueController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AzureDashboardService.Factories;

    /// <summary>
    /// MVC controller for FeedIssues
    /// </summary>
    public class FeedIssueController : DashboardBaseController
    {
        /// <summary>
        /// Display list of grouped issues in grid
        /// using jqGrid.
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult IssueListGrid()
        {
            this.InternalRead();

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
            this.InternalRead();

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && ((this.DashboardIssueModel.RssIssues.Issues == null) || (this.DashboardIssueModel.RssIssues.Issues.Count() == 0)))
            {
                return null;
            }

            try
            {
                var issuesSorted = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                   orderby issue.ServiceName, issue.LocationName
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
                            cell = new string[] { rowId.ToString(), issue.ServiceName, issue.LocationName, issue.IssueStatus, issue.IssueDate, issue.IssueTitle, issue.IssueDescription }
                        }).ToArray()
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("ex = " + ex.InnerException);
            }

            return null;
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
            if (System.IO.File.Exists(this.PathToFiles + this.DashboardMgr.DatasourceFilename))
            {
                this.IssueMgr.SetRssIssuesFromUri(this.PathToFiles);
            }

            this.InternalRead();
        }

        /// <summary>
        /// Serialized file exists, grab model from serialized file.
        /// </summary>
        public void InternalRead()
        {
            if (System.IO.File.Exists(this.PathToFiles + this.IssueMgr.DatasourceFilename))
            {
                if (this.DashboardIssueModel.RssIssues == null)
                {
                    this.DashboardIssueModel.RssIssues = this.IssueMgr.GetStoredRssIssues(this.PathToFiles);
                }
            }
            else
            {
                throw new NullReferenceException("Issue Datasource file not found");
            }
        }
    }
}
