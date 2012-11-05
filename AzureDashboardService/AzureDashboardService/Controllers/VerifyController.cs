// -----------------------------------------------------------------------
// <copyright file="VerifyController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDashboardService.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using AzureDashboardService.Factories;
    
    /// <summary>
    /// Used to verify build
    /// </summary>
    public class VerifyController : DashboardBaseController
    {
        /// <summary>
        /// Playing with Ajax
        /// </summary>
        /// <returns>ViewResult of default view</returns>
        public ViewResult Ajax()
        {
            return View();
        }

        /// <summary>
        /// Test returning anon Json 2D array of data
        /// </summary>
        /// <returns>JsonResult 2D array</returns>
        public JsonResult JsonTest()
        {
            var json = new[] 
            { 
                new object[] 
                { 
                    "Pending", 1 
                }, 
                new object[] 
                { 
                    "Completed", 5 
                } 
            }; 

            return Json(json, JsonRequestBehavior.AllowGet);
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

            var flattenedIssues = from issue in IssuesFactory.ToIssueModel(this.DashboardIssueModel.RssIssues)
                                   where issue.IssueDate > DateTime.Today.AddDays(-30)
                                   orderby issue.IssueDate descending, issue.ServiceName, issue.LocationName
                                  select issue;

            return View(flattenedIssues);
        }

        public ActionResult ThrowException()
        {
            throw new Exception("testing exception handling");
        }
    }
} 