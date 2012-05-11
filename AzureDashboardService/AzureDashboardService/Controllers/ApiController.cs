// -----------------------------------------------------------------------
// <copyright file="ApiController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Wp7AzureMgmt.DashboardFeeds;

    /// <summary>
    /// Api Mgmt Page
    /// </summary>
    public class ApiController : DashboardBaseController
    {
        /// <summary>
        /// Return documentation for using Api
        /// </summary>
        /// <returns>ActionResult of html with textual instructions</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Grab Azure html, parse to RssFeeds, serialize.
        /// </summary>
        /// <returns>download page as ViewResult</returns>
        public ViewResult BuildRSSFeedsOnSchedule()
        {
            this.DashboardMgr.SetRssFeedsFromUri(this.PathToFiles);

            return null;
        }
    }
}
