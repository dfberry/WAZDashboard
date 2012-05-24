﻿// -----------------------------------------------------------------------
// <copyright file="DashboardBaseController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using AzureDashboardService.Models;
    using AzureDashboardService.Notifications;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Utilities;

    /// <summary>
    /// BaseController for all controllers
    /// in this web app.
    /// </summary>
    public class DashboardBaseController : Controller
    {
        /// <summary>
        /// Path to files used for app
        /// </summary>
        private string pathToFiles;

        /// <summary>
        /// Dashboard data model 
        /// </summary>
        private DashboardModel model;

        /// <summary>
        /// Dashboard factory manager 
        /// </summary>
        private DashboardMgr dashboard;

        /// <summary>
        /// DashboardConfiguration - entry into web.config appSettings
        /// </summary>
        private DashboardConfiguration dbconfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardBaseController" /> class.
        /// Builds model for web app that most/all
        /// controllers use.
        /// </summary>
        public DashboardBaseController()
        {
            this.dbconfig = new DashboardConfiguration(this.HttpContext);

            this.dashboard = new DashboardMgr(this.HttpContext);
            this.model = new DashboardModel();

            // DFB-todo: set this only once on app start up or check at each request?
            // answer: for now - check at each request
            this.pathToFiles = this.HttpContext.Server.MapPath("~/App_Data") + @"\";
        }

        /// <summary>
        /// Gets HttpContext to pass into Dashboard library.
        /// http://stackoverflow.com/questions/223317/httpcontext-on-instances-of-controllers-are-null-in-asp-net-mvc
        /// </summary>
        public new HttpContextBase HttpContext
        {
            get
            {
                HttpContextWrapper context =
                    new HttpContextWrapper(System.Web.HttpContext.Current);
                return (HttpContextBase)context;
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        protected string PathToFiles
        {
            get
            {
                return this.pathToFiles;
            }

            set
            {
                this.pathToFiles = value;
            }
        }

        /// <summary>
        /// Gets or sets the only DashboardModel.
        /// </summary>
        protected DashboardModel DashboardModel
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
            }
        }

        /// <summary>
        /// Gets or sets the only DashboardMgr.
        /// </summary>
        protected DashboardMgr DashboardMgr
        {
            get
            {
                return this.dashboard;
            }

            set
            {
                this.dashboard = value;
            }
        }

        /// <summary>
        /// Gets or sets DashboardConfiguration.
        /// </summary>
        protected DashboardConfiguration DashboardConfiguration
        {
            get
            {
                return this.dbconfig;
            }

            set
            {
                this.dbconfig = value;
            }
        }

        /// <summary>
        /// Only used to send internal IT-ish notices
        /// </summary>
        /// <param name="subject">Email subject</param>
        /// <param name="text">Email text</param>
        protected void Notify(string subject, string text)
        {
            bool stamp = true;

            var smtpClient = new DashboardSmtpClient();
            EmailNotification email = new EmailNotification(stamp, smtpClient);

            email.SetFromMailAddress(
                this.DashboardConfiguration.EmailFromAddress,
                this.DashboardConfiguration.EmailFromName);

            email.SetReceiver(
                this.DashboardConfiguration.EmailToAddress,
                this.DashboardConfiguration.EmailToName);

            email.Notify(subject, text);
        }
    }
}
