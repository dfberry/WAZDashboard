// -----------------------------------------------------------------------
// <copyright file="IRSSDataSource.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Dashboard.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.Dashboard.Models;

    /// <summary>
    /// Generic Actions Against Source of RSS Feeds
    /// </summary>
    public interface IRSSDataSource
    {
        /// <summary>
        /// Get copy of list
        /// </summary>
        /// <returns>Returns IEnumerable of RSS Feeds</returns>
        IEnumerable<RSSFeed> List();

        /// <summary>
        /// Returns importable file as string for Google Reader
        /// </summary>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        string OPML();

        /// <summary>
        /// DateTime list was fetched from Datasource
        /// </summary>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        DateTime BuildDate();
    }
}
