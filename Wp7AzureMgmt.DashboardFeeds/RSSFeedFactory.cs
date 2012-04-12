using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp7AzureMgmt.Dashboard.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;


namespace Wp7AzureMgmt.Dashboard
{
    /// <summary>
    /// There are dual functions because one uses the same context and allows 
    /// for transactions if need. The other is used for non-transaction calls.
    /// </summary>
    internal class RSSFeedFactory 
    {
        protected DateTime FeedBuildDate = DateTime.MinValue;

        protected List<RSSFeed> FeedList=null;

        protected string azureDashboardURI;
 
        public DateTime dateOfEntry = DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AzureDashboardServiceURI">Current Full URI for Azure Dashboard Service RSS Feeds</param>
        public RSSFeedFactory(string AzureDashboardServiceURI)
        {
            if (String.IsNullOrEmpty(AzureDashboardServiceURI))
            {
                throw new ArgumentNullException("AzureDashboardServiceURI is null");
            }
            this.azureDashboardURI = AzureDashboardServiceURI;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rssurl">such as RSSFeed.aspx?RSSFeedCode={0}</param>
        /// <param name="rsscode">NSACSEA</param>
        /// <param name="servicename">Access Control</param>
        /// <param name="serviceid">1</param>
        /// <param name="locationname">[East Asia] </param>
        /// <param name="locationid">1</param>
        /// <returns>RSSFeed</returns>
        public RSSFeed Create(string rssurl, string rsscode, string servicename, string serviceid, string locationname, string locationid)
        {
            Trace.TraceInformation("RSSFeedFactory::Create begin");

            RSSFeed entity = new RSSFeed(DateTime.UtcNow, rsscode)
            {
                RSSLink = rssurl,
                FeedCode = rsscode,
                ServiceName = servicename,
                ServiceId = serviceid,
                LocationName = locationname,
                RegionId = locationid

            };

            Trace.TraceInformation("RSSFeedFactory::Create end");

            return (entity);
        }
        /// <summary>
        /// Return Feed list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RSSFeed> List()
        {
            if (this.FeedList == null)
            {
                this.FeedList = this.FindFeeds();
            }
            return this.FeedList;
        }
        /// <summary>
        /// Assuming feed list is built, return datetime of build.
        /// </summary>
        /// <returns>datetime FeedBuildDate</returns>
        public DateTime BuildDate()
        {
            if (this.FeedList == null)
            {
                this.FeedList = this.FindFeeds();
            } 
            return this.FeedBuildDate;
        }
        /// <summary>
        /// Return feed list as OPML file. Use file to Import feeds into Google Reader.
        /// </summary>
        /// <returns>string OPMLFile</returns>
        public string OPML()
        {
            string opml = null;

            if (this.FeedList == null)
            {
                FeedList = this.FindFeeds();
            }

            opml = DashboardResource.OPMLMaster;
            string opmlOutline = DashboardResource.OPMLMasterOutline;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (RSSFeed feed in this.FeedList)
            {
                string feedOutline = opmlOutline.Replace("$$name", HttpUtility.HtmlEncode(feed.ServiceName));
                feedOutline = feedOutline.Replace("$$location", HttpUtility.HtmlEncode(feed.LocationName));
                feedOutline = feedOutline.Replace("$$code", HttpUtility.HtmlEncode(feed.FeedCode));

                stringBuilder.Append(feedOutline);
            }

            opml = opml.Replace("$$content", stringBuilder.ToString());


            return opml;

        }
        public List<RSSFeed> FindFeeds()
        {
            Trace.TraceInformation("RSSFeedFactory::FindFeeds begin");

            HTMLParser parser = new HTMLParser(this.azureDashboardURI);

            // Grab current list of Dashboard RSS Url data
            List<RSSFeed> listOfNewUrls = parser.GetAndAddDashboardUrls(new Uri(azureDashboardURI)).ToList();

            Trace.TraceInformation("RSSFeedFactory::FindFeeds end countOfNewUrls=" + listOfNewUrls.Count, "Information");

            return listOfNewUrls;
        }
    }
}
