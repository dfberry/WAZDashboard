// -----------------------------------------------------------------------
// <copyright file="RssFeeds.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    
    /// <summary>
    /// Main Data Class containing information of the dashboard feeds
    /// </summary>
    [Serializable()]
    public class RssFeeds : ISerializable
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
        /// Where to find Azure Rss Feeds - base of Uri. Rest of Uri is in 
        /// each RssFeed.
        /// </summary>
        private string uriPrefix;

        /// <summary>
        /// Internal feeds list
        /// </summary>
        private IEnumerable<RssFeed> feeds;

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeeds" /> class.
        /// Need a zero param constructor.
        /// </summary>
        public RssFeeds()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeeds" /> class.
        /// </summary>
        /// <param name="info">SerializationInfo of RssFeeds</param>
        /// <param name="ctxt">StreamingContext of RssFeeds</param>
        public RssFeeds(SerializationInfo info, StreamingContext ctxt)
        {
            this.feeds = (IEnumerable<RssFeed>)info.GetValue("Feeds", typeof(IEnumerable<RssFeed>));
            this.FeedDate = (DateTime)info.GetValue("FeedDate", typeof(DateTime));
            this.UriPrefix = (string)info.GetValue("UriPrefix", typeof(string));
        }

        /// <summary>
        /// Gets or sets date of generation of Feed list - when the list was
        /// generated from the Azure Service Dashboard website.
        /// </summary>
        /// <returns>DateTime of Feed list generation</returns>
        public DateTime FeedDate { get; set; }

        /// <summary>
        /// Gets or sets Feeds list
        /// </summary>
        public IEnumerable<RssFeed> Feeds
        {
            get { return this.feeds; }
            set { this.feeds = value; }
        }

        /// <summary>
        /// Gets or sets Feeds list
        /// </summary>
        public string UriPrefix
        {
            get { return this.uriPrefix; }
            set { this.uriPrefix = value; }
        }        

        /// <summary>
        /// Get object from serialized data
        /// </summary>
        /// <param name="info">SerializationInfo of RssFeeds</param>
        /// <param name="ctxt">StreamingContext of RssFeeds</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Feeds", this.feeds);
            info.AddValue("FeedDate", this.FeedDate);
            info.AddValue("UriPrefix", this.UriPrefix);
        }

        /// <summary>
        /// Uses the RssFeed item values: service, location, feedcode. Doesn't use rsslink.
        /// </summary>
        /// <returns>string of OPML text to save as OPML file, to import into RSS reader</returns>
        public string OPML()
        {
            string opml = null;

            opml = DashboardResource.OPMLMaster;

            string opmlOutline = DashboardResource.OPMLMasterOutline;

            StringBuilder stringBuilder = new StringBuilder();

            Parallel.ForEach(
                this.feeds,
                this.options,
                feed =>
                {
                    string feedOutline = opmlOutline.Replace("$$name", HttpUtility.HtmlEncode(feed.ServiceName));
                    feedOutline = feedOutline.Replace("$$location", HttpUtility.HtmlEncode(feed.LocationName));
                    feedOutline = feedOutline.Replace("$$code", HttpUtility.HtmlEncode(feed.FeedCode));

                    stringBuilder.Append(feedOutline);
                });

            opml = opml.Replace("$$content", stringBuilder.ToString());

            return opml;
        }

        /// <summary>
        /// Returns combined hashcode for properties that define object
        /// </summary>
        /// <returns>int as hashcode</returns>
        public override int GetHashCode()
        {
            // date + count + concatenated codes
            string stringToHash = this.FeedDate.ToString() + this.Feeds.Count().ToString();

            Parallel.ForEach(
                this.feeds,
                this.options,
                feed =>
                {
                    stringToHash += feed.FeedCode;
                });

            return stringToHash.GetHashCode();
        }

        /// <summary>
        /// Used by test framework to compare objects. Only
        /// feedcode must be the same.
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(RssFeeds))
            {
                return false;
            }

            if ((((RssFeeds)obj).FeedDate == this.FeedDate)
                && (((RssFeeds)obj).Feeds.Count() == this.Feeds.Count()))
            {
                RssFeed[] thisList = this.Feeds.ToArray();
                RssFeed[] testList = ((RssFeeds)obj).Feeds.ToArray();

                for (int i = 0; i < this.Feeds.Count(); i++)
                {
                    if (!thisList[i].Equals(testList[i]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // date or count is not right
                return false;
            }

            return true;
        }
    }
}
