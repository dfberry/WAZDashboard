// -----------------------------------------------------------------------
// <copyright file="RssIssue.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.Models
{
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using Wp7AzureMgmt.DashboardIssues.Models;
    
    /// <summary>
    /// Model of RssIssue with can be serialized and 
    /// deserialized.
    /// </summary>
    [Serializable()]
    public class RssIssue : ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RssIssue" /> class.
        /// </summary>
        public RssIssue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssIssue" /> class.
        /// </summary>
        /// <param name="rssFeed">rssFeed value</param>
        /// <param name="datetime">datetime value</param>
        /// <param name="rssIssueXml">rssIssueXml value</param>
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
        /// Gets or sets RssFeed, defines the issue category.
        /// </summary>
        public RssFeed RssFeed { get; set; }

        /// <summary>
        /// Gets or sets DateTime of issue construction.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets RssIssueXml, defines issues.
        /// </summary>
        public RssIssueXml RssIssueXml { get; set; }

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
                && ((RssIssue)obj).RssFeed.Equals(this.RssFeed)
                && ((RssIssue)obj).RssIssueXml.Equals(this.RssIssueXml))
            {
                return true;
            }

            // date or count is not right
            return false;
        }
    }
}
