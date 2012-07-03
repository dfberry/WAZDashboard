// -----------------------------------------------------------------------
// <copyright file="FeedIssueController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AzureDashboardService.Factories;
    using AzureDashboardService.Models;

    /// <summary>
    /// MVC controller for FeedIssues
    /// </summary>
    public class FeedIssueController : DashboardBaseController
    {

        public JsonResult IssueSummary()
        {
            string majorFilter = Request.QueryString["majorfilter"];

            switch (majorFilter)
            {
                case "Location":
                    return this.LocationIssueSummary();
                case "Status":
                    return this.StatusIssueSummary();
                case "Service":
                default:
                    return this.ServiceIssueSummary();
            }
            return null;

        }

        public IOrderedEnumerable<Issue> IssueQuery()
        {
            string majorFilter = Request.QueryString["majorfilter"];
            string dateRange = Request.QueryString["minorfilter"];

            Trace.TraceInformation("FeedIssueController::IssueQuery: Request.Url=" + Request.Url.ToString());
            Trace.TraceInformation("FeedIssueController::IssueQuery: major=" + majorFilter + ", minor=" + dateRange);

            int daysToFetch;

            if (!Int32.TryParse(dateRange, out daysToFetch))
            {
                daysToFetch = 30;
            }


            this.InternalRead();

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && (this.DashboardIssueModel.RssIssues.Issues != null)
                && (this.DashboardIssueModel.RssIssues.Issues.Count() > 0))
            {


                switch (majorFilter)
                {
                    case "Location":
                        var locationIssues = (from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                              where issue.IssueDate > DateTime.Today.AddDays(-daysToFetch)
                                              orderby issue.LocationName
                                              select issue);
                        return locationIssues;
                    case "Status":
                        var statusIssues = (from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                            where issue.IssueDate > DateTime.Today.AddDays(-daysToFetch)
                                            orderby issue.IssueStatus
                                            select issue);
                        return statusIssues;
                    case "Service":
                    default:
                        var serviceIssues = (from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                             where issue.IssueDate > DateTime.Today.AddDays(-daysToFetch)
                                             orderby issue.ServiceName
                                             select issue);
                        return serviceIssues;

                }
            }
            else
            {
                throw new Exception("FeedIssuesController::IssueQuery - no rss issues found");
            }

        }

        /// <summary>
        /// Issues graph
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult IssueGraph()
        {
            this.InternalRead();

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && (this.DashboardIssueModel.RssIssues.Issues != null)
                && (this.DashboardIssueModel.RssIssues.Issues.Count() > 0))
            {
                return View("IssueGraph", this.DashboardIssueModel.RssIssues);
            }
            else
            {
                return View("NoIssues");
            }
        }

        /// <summary>
        /// Issues graph
        /// </summary>
        /// <returns>feedlist as table in ViewResult</returns>
        public ViewResult IssueGraph2()
        {
            this.InternalRead();

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && (this.DashboardIssueModel.RssIssues.Issues != null)
                && (this.DashboardIssueModel.RssIssues.Issues.Count() > 0))
            {
                return View("HighChart", this.DashboardIssueModel.RssIssues);
            }
            else
            {
                return View("NoIssues");
            }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/6920346/how-to-return-a-multidimensional-array-as-json-for-jqplot-chart-in-asp-net-mvc-c
        /// </summary>
        /// <returns></returns>
        public JsonResult ServiceIssueSummary()
        {
            var issuesQuery = IssueQuery();

            var feedServiceNames = (from issue in issuesQuery
                                    select issue.ServiceName).Distinct();

            int countServiceNames = feedServiceNames.Count();

            var json = new object[countServiceNames];

            for (int i = 0; i < countServiceNames; i++)
            {
                var feedServiceName = feedServiceNames.ToArray()[i];

                var feedServiceIssuesCount = (from issue in issuesQuery
                                        where issue.ServiceName == feedServiceName
                                        select issue).Count();

                json[i] = new object[] { HttpUtility.HtmlEncode(feedServiceName), feedServiceIssuesCount };
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult LocationIssueSummary()
        {
            var issuesQuery = IssueQuery();

            var names = (from issue in IssueQuery()
                                    select issue.LocationName).Distinct();

            int count = names.Count();

            var json = new object[count];

            for (int i = 0; i < count; i++)
            {
                var name = names.ToArray()[i];

                var feedLocationIssuesCount = (from issue in IssueQuery()
                                                where issue.LocationName == name
                                                select issue).Count();

                json[i] = new object[] { HttpUtility.HtmlEncode(name), feedLocationIssuesCount };
            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult StatusIssueSummary()
        {
            var issuesQuery = IssueQuery();

            var names = (from issue in issuesQuery
                            select issue.IssueStatus).Distinct();

            int count = names.Count();

            var json = new object[count];

            for (int i = 0; i < count; i++)
            {
                var name = names.ToArray()[i];

                var feedStatusIssuesCount = (from issue in issuesQuery
                                                where issue.IssueStatus == name 
                                                select issue).Count();

                json[i] = new object[] { HttpUtility.HtmlEncode(name), feedStatusIssuesCount };
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        
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
            try
            {
                /*var issuesSorted = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                   orderby issue.ServiceName, issue.LocationName
                                   select issue;*/
                var issues = IssueQuery();


                // Paging 
                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;
                int totalRecords = issues.Count();
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                // feeds paged subset
                var issuesPaged = issues.Skip(pageIndex * pageSize).Take(pageSize);

                int rowId = 0;

                // build response specific to jgGrid
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (
                        from issue in issues
                        select new
                        {
                            i = rowId++,
                            cell = new string[] { rowId.ToString(), issue.ServiceName, issue.LocationName, issue.IssueStatus, issue.IssueDate.ToShortDateString(), issue.IssueTitle, issue.IssueDescription }
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
