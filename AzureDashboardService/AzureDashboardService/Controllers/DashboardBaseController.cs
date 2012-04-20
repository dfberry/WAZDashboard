// -----------------------------------------------------------------------
// <copyright file="DashboardBaseController.cs" company="Microsoft">
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

    /// <summary>
    /// BaseController for all controllers
    /// in this web app.
    /// </summary>
    public class DashboardBaseController : Controller
    {
        /// <summary>
        /// Dashboard data model 
        /// </summary>
        private DashboardModel model;

        /// <summary>
        /// Dashboard factory manager 
        /// </summary>
        private DashboardMgr dashboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardBaseController" /> class.
        /// Builds model for web app that most/all
        /// controllers use.
        /// </summary>
        public DashboardBaseController()
        {
            this.dashboard = new DashboardMgr();
            this.model = new DashboardModel();

            this.model.FeedList = this.dashboard.Feeds().ToList();
            this.model.FeedDate = this.dashboard.FeedDate();
            this.model.LibraryFeedURI = this.dashboard.AzureDashboardLocation();

#if DEBUG
            this.model.IsDebug = true;
#else
            this.model.IsDebug = false;
#endif
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
    }
}
