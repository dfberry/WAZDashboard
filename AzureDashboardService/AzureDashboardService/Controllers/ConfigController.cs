﻿// -----------------------------------------------------------------------
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
    using System.Net.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using Wp7AzureMgmt.DashboardFeeds;

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
                string host = collection["host"];
                string portString = collection["port"];
                int port;

                DashboardConfiguration mailSettingsConfig = new DashboardConfiguration(this.HttpContext);

                SmtpSection smtpSection = new SmtpSection();

                smtpSection.Network.UserName = emailLogon;
                smtpSection.Network.Password = emailPassword;
                smtpSection.Network.Host = host;

                if (int.TryParse(portString, out port) == true)
                {
                    smtpSection.Network.Port = port;
                }

                this.DashboardConfiguration.SmtpSection = smtpSection;
            }

            return this.Index();
        }
    }
}
