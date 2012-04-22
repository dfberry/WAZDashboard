// -----------------------------------------------------------------------
// <copyright file="HTMLParserFeedItem.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Feed data item defined as name and value
    /// </summary>
    internal class HTMLParserFeedItem
    {
        /// <summary>
        /// Gets or sets Name: 1 of 4 datapoints that can be retrieved from WA Dashboard web page
        /// </summary>
        public HTMLParserFeedItemType Name { get; set; }
        
        /// <summary>
        /// Gets or sets Value assocated with datapoint
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// hashcode of Name property
        /// </summary>
        /// <returns>hashcode as int</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
