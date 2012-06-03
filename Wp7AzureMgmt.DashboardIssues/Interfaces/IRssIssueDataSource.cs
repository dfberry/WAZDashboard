// -----------------------------------------------------------------------
// <copyright file="IRssIssueDataSource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.DashboardIssues.Models;

    /// <summary>
    /// Generic Actions Against Source of RSS Issues
    /// </summary>
    public interface IRssIssueDataSource
    {
        /// <summary>
        /// Get copy of list
        /// </summary>
        /// <returns>Returns RSSIssues ojbject</returns>
        RssIssues Get();

        /// <summary>
        /// Set copy of list
        /// </summary>
        void Set();
    }
}
