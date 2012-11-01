// -----------------------------------------------------------------------
// <copyright file="RssFeed.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Models
{
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Permissions;

    /// <summary>
    /// This defines the RSS Feed for each Dashboard Service
    /// </summary>
    [Serializable()]
    public class RssFeed : ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed" /> class.
        /// </summary>
        public RssFeed()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed" /> class.
        /// </summary>
        /// <param name="service">service name</param>
        /// <param name="location">location name</param>
        /// <param name="code">feed code of service</param>
        /// <param name="partialuri">uri to feed code</param>
        public RssFeed(string service, string location, string code, string partialuri)
        {
            this.ServiceName = service;
            this.LocationName = location;
            this.FeedCode = code;
            this.RSSLink = partialuri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeed" /> class
        /// with data from serialization stream
        /// </summary>
        /// <param name="info">SerializationInfo of RssFeed</param>
        /// <param name="ctxt">StreamingContext of RssFeed</param>
        public RssFeed(SerializationInfo info, StreamingContext ctxt)
        {
            this.ServiceName = info.GetString("ServiceName");
            this.LocationName = info.GetString("LocationName");
            this.FeedCode = info.GetString("FeedCode");
            this.RSSLink = info.GetString("RSSLink");
        }

        /// <summary>
        /// Gets or sets Windows Azure Service
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets Location of Data Center (specific or general)
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets Code distinguishing containing both service and location
        /// </summary>
        public string FeedCode { get; set; }

        public String ServiceId { get; set; }
        public String RegionId { get; set; }

        /// <summary>
        /// Gets or sets URI as string where Dashboard Service Issues can be fetched
        /// </summary>
        public string RSSLink { get; set; }
        public DateTime DateStamp { get; set; }

        /// <summary>
        /// Add data to serialization string
        /// </summary>
        /// <param name="info">SerializationInfo of RssFeed</param>
        /// <param name="ctxt">StreamingContext of RssFeed</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ServiceName", this.ServiceName);
            info.AddValue("LocationName", this.LocationName);
            info.AddValue("FeedCode", this.FeedCode);
            info.AddValue("RSSLink", this.RSSLink);
        }

        /// <summary>
        /// hashcode of FeedCode property
        /// </summary>
        /// <returns>int hash if FeedCode</returns>
        public override int GetHashCode()
        {
            return this.FeedCode.GetHashCode();
        }

        /// <summary>
        /// Used by test framework to compare objects. Only
        /// feedcode must be the same.
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(RssFeed))
            {
                return false;
            }

            return ((RssFeed)obj).FeedCode == this.FeedCode;
        }
    }
}
