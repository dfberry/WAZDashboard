// -----------------------------------------------------------------------
// <copyright file="DashboardModel.cs" company="DFBerry">
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
        public RssFeeds Feeds { get; set; }

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