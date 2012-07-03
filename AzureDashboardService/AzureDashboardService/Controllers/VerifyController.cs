// -----------------------------------------------------------------------
// <copyright file="VerifyController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections.Specialized;
    using AzureDashboardService.Models;
    using AzureDashboardService.Factories;
    
    /// <summary>
    /// Used to verify build
    /// </summary>
    public class VerifyController : DashboardBaseController
    {
        public ViewResult Issues()
        {
            return View();
        }
        
        public ViewResult Ajax()
        {
            return View();
        }
        public ViewResult jqGrid()
        {
            return View();
        }
        public ViewResult jqPlot()
        {
            return View();
        }

        public ViewResult HighChart()
        {
            var majorFilter = Request.QueryString["majorfilter"];
            var minorFilter = Request.QueryString["minorfilter"];


            return View();
        }

        /// <summary>
        /// Test returning Json 2D array of data
        /// </summary>
        /// <returns>JsonResult 2D array</returns>
        public JsonResult JsonTest()
        {

            var json = new[] { 
                new object[] {"Pending", 1 }, 
                new object[] {"Completed", 5 } 
            }; 

            return Json(json,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index to show basic build requirements
        /// </summary>
        /// <returns>ActionResult for Index view</returns>
        public ActionResult Index()
        {
            // if 'build' querystring key found, ignore value and build
            if (Request.QueryString["build"] != null)
            {
                if (Request.QueryString["feeds"] != null)
                {
                    this.DashboardMgr.SetRssFeedsFromUri(this.PathToFiles);
                }

                if (Request.QueryString["issues"] != null)
                {
                    this.IssueMgr.SetRssIssuesFromUri(this.PathToFiles);
                }
            }

            string filePathFeeds = this.PathToFiles + this.DashboardMgr.DatasourceFilename;
            string filePathIssues = this.PathToFiles + this.IssueMgr.DatasourceFilename;

            // Fully qualify File so it doesn't use MVC version
            bool fileExistsFeeds = System.IO.File.Exists(filePathFeeds);
            bool fileExistsIssues = System.IO.File.Exists(filePathIssues);

            if (fileExistsFeeds)
            {
                FileInfo infoFeeds = new FileInfo(filePathFeeds);

                ViewData["Feeds-CreationTime"] = infoFeeds.CreationTime;
                ViewData["Feeds-FullName"] = infoFeeds.FullName;
                ViewData["Feeds-LastWriteTime"] = infoFeeds.LastWriteTime;
                ViewData["Feeds-Length"] = infoFeeds.Length;
            }

            ViewData["Feeds-FilePath"] = filePathFeeds;

            if (fileExistsIssues)
            {
                FileInfo infoIssues = new FileInfo(filePathIssues);

                ViewData["Issues-CreationTime"] = infoIssues.CreationTime;
                ViewData["Issues-FullName"] = infoIssues.FullName;
                ViewData["Issues-LastWriteTime"] = infoIssues.LastWriteTime;
                ViewData["Issues-Length"] = infoIssues.Length;
            }

            return View();
        }

        /// <summary>
        /// Adds test to tracefile. Underlying Trace call depends on Trace config settings.
        /// </summary>
        /// <returns>ActionResult of Trace View</returns>
        public ActionResult TraceTest()
        {
            if (Request.QueryString["test"] != null)
            {
                string test = Request.QueryString["test"];
            }

            return this.Trace();
        }

        /// <summary>
        /// Print trace file contents to web page. View handles html coding between
        /// newline for text file and new line for html file.
        /// </summary>
        /// <returns>ActionResult of Trace View</returns>
        public ActionResult Trace()
        {
            //string traceData = TraceLogToFile.Get(this.PathToFiles + this.DashboardConfiguration.TraceLogFileName);
            //ViewData["TraceData"] = traceData;
            return View();
        }

        /// <summary>
        /// Deletes existing tracefile.
        /// </summary>
        /// <returns>ActionResult of Trace View</returns>
        public ActionResult DeleteTraceFile()
        {
            return this.Trace();
        }

        /// <summary>
        /// Send email with Server Variables and Request header information
        /// </summary>
        /// <returns>ActionResult containing both sets of information</returns>
        public ActionResult Email()
        {
            string toReturn = Request.HttpMethod + " " + Request.RawUrl + " " + Request.ServerVariables["SERVER_PROTOCOL"] + "\n\n"; 
                
            toReturn += "***\nServer Variables\n";

            foreach (string var in Request.ServerVariables)
            {  
                toReturn += var + " " + Request[var] + "\n";
            }

            toReturn += "***\n\nRequest Headers\n";

            foreach (string var in Request.Headers.AllKeys)
            {
                toReturn += var + " " + Request.Headers[var] + "\n";
            }

            ViewData["State"] = toReturn;

            this.Notify("/Verify/Email", toReturn);

            return View();
        }

        /// <summary>
        /// Show all loaded Rss issues
        /// </summary>
        /// <returns>ActionResult showing all issues</returns>
        public ActionResult AllIssues()
        {
            if (System.IO.File.Exists(this.PathToFiles + this.IssueMgr.DatasourceFilename))
            {
                if (this.DashboardIssueModel.RssIssues == null)
                {
                    this.DashboardIssueModel.RssIssues = this.IssueMgr.GetStoredRssIssues(this.PathToFiles);
                }
            }
            var flattenedIssues = (from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                   where issue.IssueDate > DateTime.Today.AddDays(-30)
                                   orderby issue.IssueDate descending, issue.ServiceName, issue.LocationName
                                  select issue);

            return View(flattenedIssues);
        }
    }
} 