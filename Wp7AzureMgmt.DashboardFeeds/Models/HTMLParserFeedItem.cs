// -----------------------------------------------------------------------
// <copyright file="HTMLParserFeedItem.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Dashboard.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// 4 datapoints that define Dashboard Service
    /// </summary>
    internal enum HTMLParserFeedItemType
    {
        /// <summary>
        /// Link to the Windows Azure Dashboard issues for a particular service and location
        /// </summary>
        RSSLink,

        /// <summary>
        /// Code defining feed service and location
        /// </summary>
        RSSCode,

        /// <summary>
        /// Global location of data warehouse
        /// </summary>
        LocationName,

        /// <summary>
        /// Windows Azure Service
        /// </summary>
        ServiceName
    }

    /// <summary>
    /// Places to search for html tag content
    /// </summary>
    internal enum TagContent
    {
        /// <summary>
        /// Named attribute inside of tag bracket
        /// </summary>
        AttributeValue,

        /// <summary>
        /// Inner html of tag
        /// </summary>
        InnerHtml
    }

    /// <summary>
    /// Is content found by pulling from Uri or opening file 
    /// built from pulling from Uri.
    /// Meant to reduce the pulls from Azure for testing and to only
    /// pull when necessary.
    /// </summary>
    internal enum FindBy
    {
        /// <summary>
        /// No location
        /// </summary>
        notset,
        
        /// <summary>
        /// Uri provided 
        /// </summary>
        uriprovided,

        /// <summary>
        /// String/String from file
        /// </summary>
        stringprovided
    }

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

        /// <summary>
        /// Used by test framework to compare objects
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(HTMLParserFeedItemDefinition))
            {
                return false;
            }

            return ((HTMLParserFeedItemDefinition)obj).Name == this.Name;
        }
    }
}
