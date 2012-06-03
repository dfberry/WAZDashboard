// -----------------------------------------------------------------------
// <copyright file="FakeDatasource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardIssues.DataSources
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Wp7AzureMgmt.DashboardIssues.Interfaces;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// This fake datasource is used for testing both library
    /// and mvc app.
    /// </summary>
    internal class FakeDatasource : IRssIssueDataSource
    {
        /// <summary>
        /// Default Parallel Options to no limit
        /// </summary>
#if DEBUG
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
#else  
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
#endif

        public rssChannelImage GetrssChannelImage(int i)
        {
            rssChannelImage temp = new rssChannelImage();

            temp.url = "url_" + i.ToString();
            temp.link = "link_" + i.ToString();
            temp.title = "title_" + i.ToString();

            return temp;
        }
        public rssChannelImage[] GetrssChannelImageArray(int iMaxItems)
        {
            List<rssChannelImage> tempCollection = new List<rssChannelImage>();
            int i = iMaxItems;

            while (i > 0)
            {
                rssChannelImage tempItem = GetrssChannelImage(i);

                tempCollection.Add(tempItem);
                i--;
            }

            return tempCollection.ToArray();
        }

        public rssChannelItem GetrssChannelItem(int i)
        {
            rssChannelItem temp = new rssChannelItem();

            temp.pubDate = "pubDate_" + i.ToString();
            temp.description = "description_" + i.ToString();
            temp.title = "title_" + i.ToString();
            temp.status = "status_" + i.ToString();

            return temp;
        }

        public rssChannelItem[] GetrssChannelItemArray(int iMaxItems)
        {
            List<rssChannelItem> tempCollection = new List<rssChannelItem>();
            int i = iMaxItems;

            while (i > 0)
            {
                rssChannelItem tempItem = GetrssChannelItem(i);

                tempCollection.Add(tempItem);
                i--;
            }

            return tempCollection.ToArray();
        }
        
        
        public rssChannel GetrssChannel(int i)
        {
            rssChannel temp = new rssChannel();

            temp.pubDate = "pubDate_" + i.ToString();
            temp.description = "description_" + i.ToString();
            temp.title = "title_" + i.ToString();
            temp.link = "link_" + i.ToString();
            temp.language = "language_" + i.ToString();
            temp.lastBuildDate = "lastBuildDate" + i.ToString();
            temp.copyright = "copyright" + i.ToString();

            temp.image = this.GetrssChannelImageArray(i);
            temp.item = this.GetrssChannelItemArray(i);

            return temp;
        }

        public rssChannel[] GetrssChannelArray(int iMaxItems)
        {
            List<rssChannel> tempCollection = new List<rssChannel>();
            int i = iMaxItems;

            while (i > 0)
            {
                rssChannel tempItem = GetrssChannel(i);

                tempCollection.Add(tempItem);
                i--;
            }

            return tempCollection.ToArray();
        }

        public RssIssueXml GetRssIssueXml(int iMaxItems)
        {
            RssIssueXml temp = new RssIssueXml();

            temp.channel = this.GetrssChannelArray(iMaxItems);
            temp.version = "version_" + iMaxItems;

            return temp;

        }

        public RssIssue GetRssIssue(int iMaxItems)
        {
            RssIssue temp = new RssIssue();

            temp.DateTime = DateTime.UtcNow;
            temp.RssIssueXml = this.GetRssIssueXml(iMaxItems);
            temp.RssFeed = new RssFeed() { FeedCode = "testFeedCode", RSSLink = "testRssLink", LocationName = "testLocationName", ServiceName = "testServiceName" };

            return temp;
        }

        public RssIssues GetRssIssues(int iMaxItems)
        {
            RssIssues temp = new RssIssues();
            List<RssIssue> tempCollection = new List<RssIssue>();
            int i = iMaxItems;

            temp.RetrievalDate = DateTime.UtcNow;

            while (i > 0)
            {
                RssIssue tempItem = GetRssIssue(i);

                tempCollection.Add(tempItem);
                i--;
            }

            temp.Issues = tempCollection.ToArray();

            return temp;
        }

        /// <summary>
        /// Create 5 of item
        /// </summary>
        int maxCount = 5;

        /// <summary>
        /// Location (path and file) of Windows Azure Rss issues as html page
        /// </summary>
        private string htmlFileName = @"..\..\..\Wp7AzureMgmt.DashboardIssues.Test\rssissuecontent.html";

        public FakeDatasource()
        {
            this.rssIssues = this.GetRssIssues(maxCount);
        }

        /// <summary>
        /// New RssIssues with UtcNow datetime and RssIssue array.
        /// </summary>
        private RssIssues rssIssues { get; set; }

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
        /// Gets RssIssues. 
        /// </summary>
        public RssIssues RssIssues
        {
            get
            {
                return this.rssIssues;
            }
        }

        /// <summary>
        /// Internal mechanics of parsing HTML to build list
        /// </summary>
        /// <returns>IEnumerable of RSSFeed</returns>
        public RssIssues Get()
        {
            return this.rssIssues;
        }

        /// <summary>
        /// Set can't be done back to the Azure Uri.
        /// </summary>
        public void Set()
        {
            return;
        }

        /// <summary>
        /// Open file and return text as string
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
