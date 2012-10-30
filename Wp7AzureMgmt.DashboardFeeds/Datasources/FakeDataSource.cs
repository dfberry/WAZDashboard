// -----------------------------------------------------------------------
// <copyright file="FakeDatasource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.DataSources
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// This fake datasource is used for testing both library
    /// and mvc app.
    /// </summary>
    internal class FakeDatasource : IRSSDataSource
    {
        /// <summary>
        /// RssFeed array for testing
        /// </summary>
        private static RssFeed[] rssFeedArray = new RssFeed[] 
        {
            new RssFeed() { ServiceName = "s1", LocationName = "l1", FeedCode = "c1",  RSSLink = "u1" },
            new RssFeed() { ServiceName = "s2", LocationName = "l2", FeedCode = "c2",  RSSLink = "u2" },
            new RssFeed() { ServiceName = "s3", LocationName = "l3", FeedCode = "c3",  RSSLink = "u3" },
        };
        
        /// <summary>
        /// Location (path and file) of Windows Azure Rss feeds as html page
        /// </summary>
        private string htmlFileName = @"..\..\..\Wp7AzureMgmt.DashboardFeeds.Test\Html_634871865298370572.html";

        /// <summary>
        /// Location (path and file) of opml file
        /// </summary>
        private string opmlFileName = @"..\..\WazServiceDashboardOpml.xml";

        /// <summary>
        /// New RssFeeds with UtcNow datetime and RssFeed array.
        /// </summary>
        private RssFeeds rssFeeds = new RssFeeds
        {
            Feeds = rssFeedArray.Cast<Wp7AzureMgmt.DashboardFeeds.Models.RssFeed>(),
            FeedDate = DateTime.UtcNow
        };

        /// <summary>
        /// Gets or sets HtmlFileName. 
        /// </summary>
        public string HtmlFileName
        {
            get
            {
                return this.htmlFileName;
            }

            set
            {
                this.htmlFileName = value;
            }        
        }

        /// <summary>
        /// Gets RssFeeds. 
        /// </summary>
        public RssFeeds RssFeeds
        {
            get
            {
                return this.rssFeeds;
            }
        }

        /// <summary>
        /// Returns importable file as string for Google Reader
        /// </summary>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        public string OPML()
        {
            string testOpmlContent = string.Empty;

            if (File.Exists(this.opmlFileName))
            {
                using (StreamReader rdr = File.OpenText(this.opmlFileName))
                {
                    testOpmlContent = rdr.ReadToEnd();
                }
            }

            return testOpmlContent;
        }

        /// <summary>
        /// Internal mechanics of parsing HTML to build list
        /// </summary>
        /// <returns>IEnumerable of RSSFeed</returns>
        public RssFeeds Get()
        {
            return this.rssFeeds;
        }

        /// <summary>
        /// Set can't be done back to the Azure Uri.
        /// </summary>
        public void Set()
        {
            return;
        }

        /// <summary>
        /// Call Uri and get Response html content
        /// </summary>
        /// <returns>html content as string</returns>
        public string GetHtml()
        {
            using (StreamReader reader = File.OpenText(this.htmlFileName))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
