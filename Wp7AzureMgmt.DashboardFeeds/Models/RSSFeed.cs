// -----------------------------------------------------------------------
// <copyright file="RSSFeed.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Models
{
    using System;

    /// <summary>
    /// This defines the RSS Feed for each Dashboard Service
    /// </summary>
    public class RSSFeed 
    {
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

        /// <summary>
        /// Gets or sets URI as string where Dashboard Service Issues can be fetched
        /// </summary>
        public string RSSLink { get; set; }

        /// <summary>
        /// hashcode of FeedCode property
        /// </summary>
        /// <returns>int hash if FeedCode</returns>
        public override int GetHashCode()
        {
            return this.FeedCode.GetHashCode();
        }

        /// <summary>
        /// Used by test framework to compare objects
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(RSSFeed))
            {
                return false;
            }

            return ((RSSFeed)obj).FeedCode == this.FeedCode;
        }
    }
}
