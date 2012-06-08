// -----------------------------------------------------------------------
// <copyright file="UriDatasource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Wp7AzureMgmt.Core;
    using Wp7AzureMgmt.Core.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using Wp7AzureMgmt.DashboardIssues.Interfaces;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using Wp7AzureMgmt.DashboardIssues.Utiliites;
    
    /// <summary>
    /// This datasource fetches the issue list from Windows Azure Dashboard Service web page
    /// </summary>
    public class UriDatasource : IRssIssueDataSource
    {
        /// <summary>
        /// Default Parallel Options to no limit
        /// </summary>
#if DEBUG
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 1 }; // no parallelism
#else  
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
#endif
        
        /// <summary>
        /// HttpContext determines where to get config settings. Web app looks in 
        /// web config.
        /// </summary>
        private HttpContextBase configurationContext;

        /// <summary>
        /// Configuration settings
        /// </summary>
        private IssueConfiguration configuration = null;

        /// <summary>
        /// RssIssues that is the data
        /// </summary>
        private RssIssues rssIssues;

        /// <summary>
        /// IDashboardHttp object
        /// </summary>
        private IDashboardHttp httpRequest;

        /// <summary>
        /// Path and File name 
        /// </summary>
        private string serializedFilename;

        /// <summary>
        /// URI containing Feedlist
        /// </summary>
        private Uri dashboardURI;

        /// <summary>
        /// Path to serialized feeds file - should be in /App_Data
        /// </summary>
        private string pathToSerializedFeeds;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriDatasource" /> class.
        /// </summary>
        /// <param name="http">DashboardHttp object containing request</param>
        /// <param name="httpContext">Http web context or null if not a web request</param>
        /// <param name="pathToFeedsFileSource">Path but not file name</param>
        public UriDatasource(DashboardHttp http, HttpContextBase httpContext, string pathToFeedsFileSource)
        {
            this.configurationContext = httpContext;
            this.httpRequest = http;
            this.pathToSerializedFeeds = pathToFeedsFileSource;
            this.GetConfigSettings();
        }

        /// <summary>
        /// Gets or sets DashboardUri
        /// </summary>
        public Uri DashboardUri
        {
            get
            {
                return this.dashboardURI;
            }

            set
            {
                this.dashboardURI = value;
            }
        }

        /// <summary>
        /// Gets or sets RssIssues
        /// </summary>
        public RssIssues RssIssues
        {
            get
            {
                return this.rssIssues;
            }

            set
            {
                this.rssIssues = value;
            }
        }

        /// <summary>
        /// Gets or sets SerializedFilename
        /// </summary>
        public string SerializedFilename
        {
            get
            {
                return this.serializedFilename;
            }

            set
            {
                this.serializedFilename = value;
            }
        }

        /// <summary>
        /// Gets or sets DashboardHttp
        /// </summary>
        public IDashboardHttp DashboardHttp
        {
            get
            {
                return this.httpRequest;
            }

            set
            {
                this.httpRequest = value;
            }
        }

        /// <summary>
        /// Get RssIssues from uris specified in RssFeeds 
        /// </summary>
        /// <returns>RssIssues from parsed html from uri</returns>
        public RssIssues Get()
        {
            RssFeeds rssFeeds = this.GetRssFeeds();
            List<RssIssue> issues = new List<RssIssue>();

            Parallel.ForEach(
                rssFeeds.Feeds, 
                this.options, 
                feed =>
            {
                if (!string.IsNullOrEmpty(feed.FeedCode))
                {
                    Uri uri = new Uri(String.Format("http://www.microsoft.com/windowsazure/support/status/RSSFeed.aspx?RSSFeedCode={0}", feed.FeedCode));

                    DashboardHttp http = new Core.DashboardHttp(uri);

                    RssIssue rssIssue = new RssIssue()
                    {
                        RssIssueXml = http.GetXmlRequest<RssIssueXml>(),
                        RssFeed = feed,
                        DateTime = DateTime.UtcNow
                    };

                    issues.Add(rssIssue);
                }
            });

            if ((issues != null) && (issues.Count > 0))
            {
                this.rssIssues = new RssIssues();
                this.rssIssues.Issues = issues;
                this.rssIssues.RetrievalDate = DateTime.UtcNow;
            }

            return this.rssIssues;
        }

        /// <summary>
        /// Set can't be done back to the Azure Uri.
        /// Throws NotImplementedException.
        /// </summary>
        public void Set()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all config settings. Sets 
        /// dashboardURI. Creates new DashboardHttp
        /// for httpRequest. Calls BuildHttpGet. All
        /// of these can be overwritten/called again
        /// later.
        /// </summary>
        private void GetConfigSettings()
        {
            this.configuration = new IssueConfiguration(this.configurationContext);
            this.serializedFilename = this.configuration.SerializedIssueListFile;
        }

        /// <summary>
        /// Gets RssFeeds from web based on Uri in config file.
        /// </summary>
        /// <returns>RssFeeds from the web</returns>
        private RssFeeds GetRssFeeds()
        {
            // get RssFeeds
            FeedConfiguration feedConfiguration = new FeedConfiguration(this.configurationContext);
            DashboardMgr feedMgr = new DashboardMgr(this.configurationContext);
            return feedMgr.GetStoredRssFeeds(this.pathToSerializedFeeds);
        }
    }
}
