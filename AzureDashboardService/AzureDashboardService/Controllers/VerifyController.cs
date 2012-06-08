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
    
    /// <summary>
    /// Used to verify build
    /// </summary>
    public class VerifyController : DashboardBaseController
    {
        /// <summary>
        /// Index to show basic build requirements
        /// </summary>
        /// <returns>ActionResult for Index view</returns>
        public ActionResult Index()
        {
            // if 'build' querystring key found, ignore value and build
            if (Request.QueryString["build"] != null)
            {
                this.DashboardMgr.SetRssFeedsFromUri(this.PathToFiles);
            }

            string filePath = this.PathToFiles + this.FeedConfiguration.SerializedFeedListFile;

            // Fully qualify File so it doesn't use MVC version
            bool fileExists = System.IO.File.Exists(filePath);

            if (fileExists)
            {
                FileInfo info = new FileInfo(filePath);

                ViewData["CreationTime"] = info.CreationTime;
                ViewData["FullName"] = info.FullName;
                ViewData["LastWriteTime"] = info.LastWriteTime;
                ViewData["Length"] = info.Length;
            }

            ViewData["FilePath"] = filePath;
            ViewData["DataFileExists"] = fileExists.ToString();

#if HideExceptions
            VewData["HideExceptions"] = true;
#else
            ViewData["HideExceptions"] = false;
#endif

#if Trace
            ViewData["Trace"] = true;
#else
            ViewData["Trace"] = false;
#endif

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
    }
} 