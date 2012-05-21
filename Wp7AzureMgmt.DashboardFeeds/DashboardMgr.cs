// -----------------------------------------------------------------------
// <copyright file="DashboardMgr.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;
    using System.Web;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Main entry point in dashboard Mgr
    /// </summary>
    public class DashboardMgr 
    {
        /// <summary>
        /// Http context which shouldn't be null if called
        /// from MVC app
        /// </summary>
        private HttpContextBase httpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardMgr" /> class.
        /// </summary>
        /// <param name="context">HttpContextBase tells library where to get config settings</param>
        public DashboardMgr(HttpContextBase context)
        {
            if (context != null)
            {
                this.httpContext = context;
            }
        }

        /// <summary>
        /// Deserialize RssFeeds from disk
        /// </summary>
        /// <param name="pathToFiles">location on disk for serialized files</param>
        /// <returns>RssFeeds from file</returns>
        public RssFeeds GetStoredRssFeeds(string pathToFiles)
        {
            FileDatasource fileDatasource = new FileDatasource(pathToFiles, this.httpContext);
            RssFeeds feeds = null;

            // get from file
            feeds = fileDatasource.Get();

            return feeds;
        }

        /// <summary>
        /// Serialize RssFeeds to disk. Make request across
        /// wire to Uri, grab response into RssFeeds,
        /// serialize RssFeeds.
        /// </summary>
        /// <param name="pathToFiles">Path to serialized files</param>
        public void SetRssFeedsFromUri(string pathToFiles)
        {
            DashboardHttp httpRequest = new DashboardHttp();

            UriDatasource uriDataSource = new UriDatasource(httpRequest, this.httpContext);
            RssFeeds feeds = uriDataSource.Get();
            FileDatasource fileDatasource = new FileDatasource(pathToFiles, this.httpContext);
            fileDatasource.RssFeeds = feeds;
            fileDatasource.Set();
        }
    }
}