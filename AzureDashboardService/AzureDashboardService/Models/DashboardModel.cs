// -----------------------------------------------------------------------
// <copyright file="DashboardModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Dashboard Model
    /// </summary>
    public class DashboardModel
    {
        /// <summary>
        /// Gets or sets feedlist
        /// </summary>
        public List<RSSFeed> FeedList { get; set; }

        /// <summary>
        /// Gets or sets feeddate (date feeds were generated from html)
        /// </summary>
        public DateTime FeedDate { get; set; }

        /// <summary>
        /// Gets or sets URI to pull feeds from
        /// </summary>
        public string LibraryFeedURI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether DEBUG
        /// is the build configuration.
        /// </summary>
        public bool IsDebug { get; set; }
    }
}