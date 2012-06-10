// -----------------------------------------------------------------------
// <copyright file="IssuesFactory.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDashboardService.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using AzureDashboardService.Models;
    using Wp7AzureMgmt.DashboardIssues.Models;

    /// <summary>
    /// Takes RssIssues and converts into List.
    /// </summary>
    public static class IssuesFactory
    {
        /// <summary>
        /// Test that model transformation happens correctly.
        /// </summary>
        /// <param name="rssIssues">Model to flatten</param>
        /// <returns>List of Issue</returns>
        public static IEnumerable<Issue> ToIssueModel(RssIssues rssIssues)
        {
            if (rssIssues == null)
            {
                return new Issue[0];
            }

            // Order data by service and location
            var issuesFlattened = (from issue in rssIssues.Issues
                                    from channel in issue.RssIssueXml.channel
                                    where channel.item != null
                                    from item in channel.item
                                    select new Issue()
                                        {
                                            ServiceName = issue.RssFeed.ServiceName,
                                            LocationName = issue.RssFeed.LocationName,
                                            FeedCode = issue.RssFeed.FeedCode,
                                            IssueDate = item.pubDate,
                                            IssueTitle = item.title,
                                            IssueDescription = item.description,
                                            IssueStatus = item.status
                                        }).ToList();

            return issuesFlattened;
        }
    }
}