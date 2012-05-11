// -----------------------------------------------------------------------
// <copyright file="IRSSDataSource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Generic Actions Against Source of RSS Feeds
    /// </summary>
    public interface IRSSDataSource
    {
        /// <summary>
        /// Get copy of list
        /// </summary>
        /// <returns>Returns RSSFeeds ojbject</returns>
        RssFeeds Get();

        /// <summary>
        /// Set copy of list
        /// </summary>
        void Set();
    }
}
