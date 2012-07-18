// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System.Web.Mvc;

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
            return RedirectToAction("Show", "Issues");
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