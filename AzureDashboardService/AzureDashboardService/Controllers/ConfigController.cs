// -----------------------------------------------------------------------
// <copyright file="ConfigController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Displays the web.config settings read from DashboardConfiguration
    /// </summary>
    public class ConfigController : DashboardBaseController
    {
        /// <summary>
        /// Displays all config settings
        /// </summary>
        /// <returns>ConfigSettings view</returns>
        public ActionResult Index()
        {
            NameValueCollection configSettings = this.DashboardConfiguration.GetAll;

            return View("ConfigSettings", configSettings);
        }

        /// <summary>
        /// Set Email config settings
        /// </summary>
        /// <returns>Email View</returns>
        public ActionResult Email()
        {
            string emailLogon = this.DashboardConfiguration.EmailLogon;
            string emailPassword = this.DashboardConfiguration.EmailPassword;

            // if the config is already set, don't show form again
            if ((!string.IsNullOrEmpty(emailLogon))
                && (!string.IsNullOrEmpty(emailPassword)))
            {
                return this.SaveEmail(null);
            }

            return View();
        }

        /// <summary>
        /// Save email config settings
        /// </summary>
        /// <param name="collection">Collection of config settings</param>
        /// <returns>listing of config settings</returns>
        public ActionResult SaveEmail(FormCollection collection)
        {
            if (collection != null)
            {
                string emailLogon = collection["emailLogon"];
                string emailPassword = collection["emailPassword"];

                // if values passed in, save them
                if ((!string.IsNullOrEmpty(emailLogon))
                    && (!string.IsNullOrEmpty(emailPassword)))
                {
                    this.DashboardConfiguration.EmailLogon = emailLogon;
                    this.DashboardConfiguration.EmailPassword = emailPassword;
                }
            }

            return this.Index();
        }
    }
}
