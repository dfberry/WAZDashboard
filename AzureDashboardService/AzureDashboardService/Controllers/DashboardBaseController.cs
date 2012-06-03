// -----------------------------------------------------------------------
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
    using Wp7AzureMgmt.Core.Notifications;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardIssues;

    //using Wp7AzureMgmt.DashboardFeeds.Utilities;

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

        private IssueMgr issueMgr;


        /// <summary>
        /// Dashboard data model 
        /// </summary>
        private DashboardModel feedModel;

        private DashboardIssueModel issueModel;

        /// <summary>
        /// Dashboard factory manager 
        /// </summary>
        private DashboardMgr feedMgr;

        /// <summary>
        /// DashboardConfiguration - entry into web.config appSettings
        /// </summary>
        private FeedConfiguration dbFeedConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardBaseController" /> class.
        /// Builds model for web app that most/all
        /// controllers use.
        /// </summary>
        public DashboardBaseController()
        {
            this.dbFeedConfig = new FeedConfiguration(this.HttpContext);

            this.feedMgr = new DashboardMgr(this.HttpContext);
            this.issueMgr = new IssueMgr(this.HttpContext);
            
            this.feedModel = new DashboardModel();
            this.issueModel = new DashboardIssueModel();


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
                return this.feedModel;
            }

            set
            {
                this.feedModel = value;
            }
        }

        /// <summary>
        /// Gets or sets the only DashboardIssueModel.
        /// </summary>
        protected DashboardIssueModel DashboardIssueModel
        {
            get
            {
                return this.issueModel;
            }

            set
            {
                this.issueModel = value;
            }
        }

        /// <summary>
        /// Gets or sets the only IssueMgr.
        /// </summary>
        protected IssueMgr IssueMgr
        {
            get
            {
                return this.issueMgr;
            }

            set
            {
                this.issueMgr = value;
            }
        }

        /// <summary>
        /// Gets or sets the only DashboardMgr.
        /// </summary>
        protected DashboardMgr DashboardMgr
        {
            get
            {
                return this.feedMgr;
            }

            set
            {
                this.feedMgr = value;
            }
        }

        /// <summary>
        /// Gets or sets DashboardConfiguration.
        /// </summary>
        protected FeedConfiguration FeedConfiguration
        {
            get
            {
                return this.dbFeedConfig;
            }

            set
            {
                this.dbFeedConfig = value;
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
                this.FeedConfiguration.EmailFromAddress,
                this.FeedConfiguration.EmailFromName);

            email.SetReceiver(
                this.FeedConfiguration.EmailToAddress,
                this.FeedConfiguration.EmailToName);

            email.Notify(subject, text);
        }
    }
}
