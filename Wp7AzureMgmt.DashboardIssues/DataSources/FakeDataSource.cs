// -----------------------------------------------------------------------
// <copyright file="FakeDatasource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardIssues.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using Wp7AzureMgmt.DashboardIssues.Interfaces;
    using Wp7AzureMgmt.DashboardIssues.Models;

    /// <summary>
    /// This fake datasource is used for testing both library
    /// and mvc app.
    /// </summary>
    public class FakeDatasource : IRssIssueDataSource
    {
        /// <summary>
        /// Default Parallel Options to no limit
        /// </summary>
#if DEBUG
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
#else  
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
#endif

        /// <summary>
        /// New RssIssues with UtcNow datetime and RssIssue array.
        /// </summary>
        private RssIssues rssIssues;

        /// <summary>
        /// Create 5 of item
        /// </summary>
        private int maxCount = 5;

        /// <summary>
        /// Location (path and file) of Windows Azure Rss issues as html page
        /// </summary>
        private string htmlFileName = @"..\..\..\Wp7AzureMgmt.DashboardIssues.Test\rssissuecontent.html";

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDatasource" /> class.
        /// </summary>
        public FakeDatasource()
        {
            this.rssIssues = this.GetRssIssues(this.maxCount);
        }

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
        /// Gets rssChannelImage with fake data.
        /// </summary>
        /// <param name="i">postpends i to property value</param>
        /// <returns>rssChannelImage filled with fake values</returns>
        public rssChannelImage GetrssChannelImage(int i)
        {
            rssChannelImage temp = new rssChannelImage();

            temp.url = "url_" + i.ToString();
            temp.link = "link_" + i.ToString();
            temp.title = "title_" + i.ToString();

            return temp;
        }

        /// <summary>
        /// Gets rssChannelImageArray object.
        /// </summary>
        /// <param name="maxItems">fill array with maxItems</param>
        /// <returns>rssChannelImage[] filled with fake values</returns>
        public rssChannelImage[] GetrssChannelImageArray(int maxItems)
        {
            List<rssChannelImage> tempCollection = new List<rssChannelImage>();
            int i = maxItems;

            while (i > 0)
            {
                rssChannelImage tempItem = this.GetrssChannelImage(i);

                tempCollection.Add(tempItem);
                i--;
            }

            return tempCollection.ToArray();
        }

        /// <summary>
        /// Gets rssChannelItem.
        /// </summary>
        /// <param name="i">Postpends i to property value.</param>
        /// <returns>rssChannelItem filled with fake values</returns>
        public rssChannelItem GetrssChannelItem(int i)
        {
            rssChannelItem temp = new rssChannelItem();

            temp.pubDate = "pubDate_" + i.ToString();
            temp.description = "description_" + i.ToString();
            temp.title = "title_" + i.ToString();
            temp.status = "status_" + i.ToString();

            return temp;
        }

        /// <summary>
        /// Gets rssChannelItem[] object.
        /// </summary>
        /// <param name="maxItems">array length will be maxItems</param>
        /// <returns>rssChannelItem[] filled with fake values</returns>
        public rssChannelItem[] GetrssChannelItemArray(int maxItems)
        {
            List<rssChannelItem> tempCollection = new List<rssChannelItem>();
            int i = maxItems;

            while (i > 0)
            {
                rssChannelItem tempItem = this.GetrssChannelItem(i);

                tempCollection.Add(tempItem);
                i--;
            }

            return tempCollection.ToArray();
        }

        /// <summary>
        /// Gets rssChannel object. 
        /// </summary>
        /// <param name="i">i used as postpend data and passed down as array length for image and item</param>
        /// <returns>rssChannel filled with fake values</returns>
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

        /// <summary>
        /// Gets rssChannel[].
        /// </summary>
        /// <param name="maxItems">creates array with maxItems length</param>
        /// <returns>rssChannel[] filled with fake values</returns>
        public rssChannel[] GetrssChannelArray(int maxItems)
        {
            List<rssChannel> tempCollection = new List<rssChannel>();
            int i = maxItems;

            while (i > 0)
            {
                rssChannel tempItem = this.GetrssChannel(i);

                tempCollection.Add(tempItem);
                i--;
            }

            return tempCollection.ToArray();
        }

        /// <summary>
        /// Gets a preloaded RssIssueXml object.
        /// </summary>
        /// <param name="maxItems">max items for Channel</param>
        /// <returns>RssIssueXml filled with fake values</returns>
        public RssIssueXml GetRssIssueXml(int maxItems)
        {
            RssIssueXml temp = new RssIssueXml();

            temp.channel = this.GetrssChannelArray(maxItems);
            temp.version = "version_" + maxItems;

            return temp;
        }

        /// <summary>
        /// Gets a single RssIssue object.
        /// </summary>
        /// <param name="maxItems">item count to pass to child object creators</param>
        /// <returns>RssIssue filled with fake values</returns>
        public RssIssue GetRssIssue(int maxItems)
        {
            RssIssue temp = new RssIssue();

            temp.DateTime = DateTime.UtcNow;
            temp.RssIssueXml = this.GetRssIssueXml(maxItems);
            temp.RssFeed = new RssFeed() { FeedCode = "testFeedCode", RSSLink = "testRssLink", LocationName = "testLocationName", ServiceName = "testServiceName" };

            return temp;
        }

        /// <summary>
        /// Gets a single RssIssues object
        /// </summary>
        /// <param name="maxItems">item count of RssIssue</param>
        /// <returns>RssIssues filled with fake values</returns>
        public RssIssues GetRssIssues(int maxItems)
        {
            RssIssues temp = new RssIssues();
            List<RssIssue> tempCollection = new List<RssIssue>();
            int i = maxItems;

            temp.RetrievalDate = DateTime.UtcNow;

            while (i > 0)
            {
                RssIssue tempItem = this.GetRssIssue(i);

                tempCollection.Add(tempItem);
                i--;
            }

            temp.Issues = tempCollection.ToArray();

            return temp;
        }

        /// <summary>
        /// Gets fake RssIssues object.
        /// </summary>
        /// <returns>RssIssues filled with fake values</returns>
        public RssIssues Get()
        {
            return this.rssIssues;
        }

        /// <summary>
        /// Set can't be done back to the Azure Uri, return nothing.
        /// </summary>
        public void Set()
        {
            return;
        }

        /// <summary>
        /// Open this.htmlFileName and return text as string.
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
