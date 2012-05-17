// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="DFBerry">
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
    using AzureDashboardService.Models;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    
    /// <summary>
    /// Home controller page of the web site
    /// </summary>
    public class HomeController : DashboardBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        public HomeController()
            : base()
        {
        }

        /// <summary>
        /// Default Home page
        /// </summary>
        /// <returns>Home page content as ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Default About page
        /// </summary>
        /// <returns>about page as ActionResult</returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Contact page
        /// </summary>
        /// <returns>contact page as ActionResult</returns>
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Default Download page
        /// </summary>
        /// <returns>download page as ViewResult</returns>
        public ViewResult Download()
        {
            return View();
        }
    }
}