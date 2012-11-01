// -----------------------------------------------------------------------
// <copyright file="IssuesFactory.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDashboardService.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureDashboardService.Models;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using Wp7AzureMgmt.Core;

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
                                            IssueDate = Wp7AzureMgmt.Core.DateTimeConversion.FromRssPubDateToDateTime(item.pubDate),
                                            IssueTitle = item.title,
                                            IssueDescription = item.description,
                                            IssueStatus = item.status
                                        }).ToList();

            return issuesFlattened;
        }
        /// <summary>
        /// Need to return DashboardRequest object
        /// </summary>
        /// <param name="rssIssues"></param>
        /// <returns></returns>
        public static DashboardResponse ToPhoneModel(IEnumerable<RssIssue> issues, int issueage)
        {
            if (issues == null)
            {
                throw new Exception("issues == null");
            }

            DashboardResponse response = new DashboardResponse(); //List<DashboardItem>

            List<DashboardItem> listDashboardItems = (from issue in issues
                                  select new DashboardItem()
                                  {
                                      FeedDefintion = new AzureDashboardService.RssFeed
                                      {
                                          ServiceName = issue.RssFeed.ServiceName,
                                          RegionLocation = issue.RssFeed.LocationName,
                                          FeedCode = issue.RssFeed.FeedCode,
                                          RSSLink = issue.RssFeed.RSSLink
                                      }
                                      ,

                                      FeedIssues = (from channel in issue.RssIssueXml.channel
                                                    where channel.item != null
                                                    from item in channel.item
                                                    where DateTimeConversion.FromRssPubDateToDateTime(item.pubDate) >= DateTime.UtcNow.AddDays(-issueage)
                                                    select new RSSFeedItemDetail
                                            {
                                                Title = item.title,
                                                Description = item.description,
                                                PubDate = item.pubDate,
                                                Status = item.status
                                            }).ToList()
                                  }
                                 ).ToList();


            response.List = listDashboardItems;
            return response;
        }
    }
}