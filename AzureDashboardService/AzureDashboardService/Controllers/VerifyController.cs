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
            string filePath = this.PathToFiles + this.DashboardConfiguration.SerializedFeedListFile;

            // Fully qualify File so it doesn't use MVC version
            bool fileExists = System.IO.File.Exists(filePath);

            ViewData["FilePath"] = filePath;
            ViewData["DataFileExists"] = fileExists.ToString();

            return View();
        }
    }
}
