// -----------------------------------------------------------------------
// <copyright file="IssuesController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Web;
    using System.Web.Mvc;
    using AzureDashboardService.Factories;
    using AzureDashboardService.Models;
    using Wp7AzureMgmt.Core;
    using Wp7AzureMgmt.DashboardIssues;

    /// <summary>
    /// MVC controller for FeedIssues
    /// </summary>
    public class IssuesController : DashboardBaseController
    {
        /// <summary>
        /// Show the Issues
        /// </summary>
        /// <returns>ViewResult of Graph and Table</returns>
        public ViewResult Show()
        {
            return View("Combined");
        }

        /// <summary>
        /// Request to/from the phone
        /// </summary>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public JsonResult Get(string sidx, string sord, int page, int rows, int issueage)
        {
            this.InternalRead();
            DashboardResponse jsonResponse = IssuesFactory.ToPhoneModel(this.DashboardIssueModel.RssIssues.Issues, issueage);

            //jsonResponse.AppVersion = "1.0.0.0";
            //jsonResponse.FetchAllIncludingEmpties = 1;
            //jsonResponse.IssueAge = 30;
            //jsonResponse.PhoneId = "emulator";
            //jsonResponse.PhoneMaker = "emulator";
            //jsonResponse.Summary = false;
            //jsonResponse.TrialRemaining = 30;
            //jsonResponse.UserId = "emulatoruser";

            HttpContext.Response.AddHeader("Cache-Control", "no-cache");

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);       
        
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

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && (this.DashboardIssueModel.RssIssues.Issues != null)
                && (this.DashboardIssueModel.RssIssues.Issues.Count() > 0))
            {
                return this.Show();
            }
            else
            {
                return View("NoIssues");
            }
        }

        /// <summary>
        /// Grab issue build date - date I fetched data from Azure
        /// </summary>
        /// <returns>JsonResult of retrieval date</returns>
        public JsonResult RetrievalDate()
        {
            this.InternalRead();
            string retrievalDate = "unknown";

            if ((this.DashboardIssueModel != null)
                && (this.DashboardIssueModel.RssIssues != null)
                && (this.DashboardIssueModel.RssIssues.Issues != null)
                && (this.DashboardIssueModel.RssIssues.Issues.Count() > 0))
            {
                retrievalDate = this.DashboardIssueModel.RssIssues.RetrievalDate.ToShortDateString();
            }
            
            HttpContext.Response.AddHeader("Cache-Control", "no-cache"); 

            return Json(retrievalDate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Output cache of 60 minutes
        /// </summary>
        /// <returns>JsonResult of summary data as dictated by jqGrid</returns>
        public JsonResult IssueSummary()
        {
            string majorFilter = Request.QueryString["majorfilter"];

            HttpContext.Response.AddHeader("Cache-Control", "no-cache"); 

            switch (majorFilter)
            {
                case "LocationName":
                    return this.LocationIssueSummary2();
                case "IssueStatus":
                    return this.StatusIssueSummary2();
                case "Service":
                default:
                    return this.ServiceIssueSummary2();
            }
        }

        /// <summary>
        /// Summary of issues by service name
        /// http://stackoverflow.com/questions/6920346/how-to-return-a-multidimensional-array-as-json-for-jqplot-chart-in-asp-net-mvc-c
        /// </summary>
        /// <returns>JsonResult of summary data as dictated by jqGrid</returns>
        public JsonResult ServiceIssueSummary2()
        {
            var results = from issue in this.IssueQuery()
                          group issue by issue.ServiceName into g
                          select new { Name = g.Key, Count = g.Count() };

            IEnumerable<object> json = results.Select(result =>
                new object[] 
                { 
                    HttpUtility.HtmlEncode(result.Name), result.Count 
                });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Summary of issues by location name
        /// http://stackoverflow.com/questions/6920346/how-to-return-a-multidimensional-array-as-json-for-jqplot-chart-in-asp-net-mvc-c
        /// </summary>
        /// <returns>JsonResult of summary data as dictated by jqGrid</returns>
        public JsonResult LocationIssueSummary2()
        {
            var results = from issue in this.IssueQuery()
                          group issue by issue.LocationName into g
                          select new 
                          { 
                              Name = g.Key, 
                              Count = g.Count() 
                          };

            IEnumerable<object> json = results.Select(result =>
                new object[] 
                { 
                    HttpUtility.HtmlEncode(result.Name), 
                    result.Count 
                });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Summary of issues by issue status
        /// http://stackoverflow.com/questions/6920346/how-to-return-a-multidimensional-array-as-json-for-jqplot-chart-in-asp-net-mvc-c
        /// </summary>
        /// <returns>JsonResult of summary data as dictated by jqGrid</returns>
        public JsonResult StatusIssueSummary2()
        {
            var results = from issue in this.IssueQuery()
                          group issue by issue.IssueStatus into g
                          select new { Name = g.Key, Count = g.Count() };

            IEnumerable<object> json = results.Select(result =>
                new object[] 
                { 
                    HttpUtility.HtmlEncode(result.Name), 
                    result.Count 
                });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// All feeds' issues
        /// </summary>
        /// <returns>as ActionResult</returns>
        public ActionResult RSS()
        {
            this.InternalRead();

            var postItems = this.IssueQuery().Where(p => p.IssueDate > DateTime.Today.AddDays(-30)).OrderByDescending(p => p.IssueDate).Take(100)
                .Select(p => new SyndicationItem(p.IssueTitle, p.IssueDescription, new Uri("https://www.windowsazurestatus.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode=" + p.FeedCode)));

            var feed = new SyndicationFeed("Windows Azure", "Microsoft Windows Azure Service Dashboard", new Uri("http://www.windowsazure.com/en-us/support/service-dashboard/"), postItems)
            {
                Copyright = new TextSyndicationContent("2009 Microsoft Corporation. All rights reserved."),
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
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
            try
            {
                //build date
                this.InternalRead();
                string builddate =  this.DashboardIssueModel.RssIssues.RetrievalDate.ToShortDateString();

                /*var issuesSorted = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                   orderby issue.ServiceName, issue.LocationName
                                   select issue;*/
                var issues = this.IssueQuery();

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
                            cell = new string[] { issue.IssueDate.ToString(@"yyyy.MM.dd HH:mm"), issue.ServiceName, issue.LocationName, issue.IssueStatus, issue.IssueTitle, issue.IssueDescription }
                        }).ToArray(),
                    retrievaldate = builddate
                };

                HttpContext.Response.AddHeader("Cache-Control", "no-cache"); 

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("ex = " + ex.InnerException);
            }

            return null;
        }

        /// <summary>
        /// Grab parameters from QueryString and build query for issues
        /// </summary>
        /// <returns>List of Issues that meet params</returns>
        private IOrderedEnumerable<Issue> IssueQuery()
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
                // DFB add Issues to the response header so it can be pulled via javascript
                HttpContext.Response.AddHeader("RetrievalDate", this.DashboardIssueModel.RssIssues.RetrievalDate.ToShortDateString());
                
                // Moved to Invoking Controller method
                //HttpContext.Response.AddHeader("Cache-Control", "no-cache"); 
          
                
                switch (majorFilter)
                {
                    case "Location":
                        var locationIssues = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                              where issue.IssueDate > DateTime.Today.AddDays(-daysToFetch)
                                              orderby issue.IssueDate descending
                                              select issue;
                        return locationIssues;
                    case "Status":
                        var statusIssues = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                            where issue.IssueDate > DateTime.Today.AddDays(-daysToFetch)
                                            orderby issue.IssueDate descending
                                            select issue;
                        return statusIssues;
                    case "Service":
                    default:
                        var serviceIssues = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                             where issue.IssueDate > DateTime.Today.AddDays(-daysToFetch)
                                             orderby issue.IssueDate descending
                                             select issue;
                        return serviceIssues;
                }
            }
            else
            {
                throw new Exception("FeedIssuesController::IssueQuery - no rss issues found");
            }
        }

        /// <summary>
        /// Grab issue xml  from Azure, save to serialized file
        /// </summary>
        private void InternalBuild()
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
        private void InternalRead()
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
