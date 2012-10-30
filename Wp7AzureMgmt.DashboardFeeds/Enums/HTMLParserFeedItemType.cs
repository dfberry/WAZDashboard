// -----------------------------------------------------------------------
// <copyright file="HTMLParserFeedItemType.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
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
        /// Global location of data warehouse
        /// </summary>
        RegionId,

        /// <summary>
        /// Windows Azure Service
        /// </summary>
        ServiceName,

        /// <summary>
        /// Windows Azure Service
        /// </summary>
        ServiceId

    }
}
