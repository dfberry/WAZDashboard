using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Wp7AzureMgmt.DashboardFeeds.Models;
using Wp7AzureMgmt.DashboardIssues.Models;

namespace Wp7AzureMgmt.DashboardIssues.Models
{
    [Serializable()]
    public class RssIssue : ISerializable
    {

        public RssFeed RssFeed { get; set; }

        public DateTime DateTime { get; set; }

        public RssIssueXml RssIssueXml { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RssIssue" /> class.
        /// </summary>
        public RssIssue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed" /> class.
        /// </summary>
        /// <param name="service">service name</param>
        /// <param name="location">location name</param>
        /// <param name="code">feed code of service</param>
        /// <param name="partialuri">uri to feed code</param>
        public RssIssue(RssFeed rssFeed, DateTime datetime, RssIssueXml rssIssueXml)
        {
            this.RssFeed = rssFeed;
            this.DateTime = datetime;
            this.RssIssueXml = RssIssueXml;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssIssue" /> class
        /// with data from serialization stream
        /// </summary>
        /// <param name="info">SerializationInfo of RssIssue</param>
        /// <param name="ctxt">StreamingContext of RssIssue</param>
        public RssIssue(SerializationInfo info, StreamingContext ctxt)
        {
            this.RssFeed = (RssFeed)info.GetValue("RssFeed", typeof(RssFeed));
            this.DateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
            this.RssIssueXml = (RssIssueXml)info.GetValue("RssIssueXml", typeof(RssIssueXml));
        }

        /// <summary>
        /// Add data to serialization string
        /// </summary>
        /// <param name="info">SerializationInfo of RssIssue</param>
        /// <param name="ctxt">StreamingContext of RssIssue</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("RssFeed", this.RssFeed);
            info.AddValue("DateTime", this.DateTime);
            info.AddValue("RssIssueXml", this.RssIssueXml);

        }

        /// <summary>
        /// hashcode of FeedCode and datestamp property
        /// </summary>
        /// <returns>int hash if FeedCode and datestamp</returns>
        public override int GetHashCode()
        {
            return this.RssFeed.GetHashCode() + this.DateTime.GetHashCode() + this.RssIssueXml.GetHashCode();

        }

        /// <summary>
        /// Used by test framework to compare objects. Only
        /// issuecode must be the same.
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(RssIssue))
            {
                return false;
            }

            if ((((RssIssue)obj).DateTime == this.DateTime)
                && (((RssIssue)obj).RssFeed.Equals(this.RssFeed))
                && (((RssIssue)obj).RssIssueXml.Equals(this.RssIssueXml)))
            {
                return true;
            }

            // date or count is not right
            return false;
        }
    }
}
