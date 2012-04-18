// -----------------------------------------------------------------------
// <copyright file="FeedListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Feed List (rss feeds) api controller
    /// </summary>
    public class FeedListController : ApiController
    {
        /// <summary>
        /// Dashboard factory manager 
        /// </summary>
        private DashboardMgr dashboard = null;
        
        /// <summary>
        /// GET /api/feedlist
        /// </summary>
        /// <returns>IEnumerable of RSSFeed</returns>
        public IEnumerable<RSSFeed> Get()
        {
            this.dashboard = new DashboardMgr();

            return this.dashboard.Feeds();
        }
    }
}
