using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureDashboardService
{
    public class DashboardRequest 
    {
        /// <summary>
        /// Gets or sets the service dashboard returned from request
        /// </summary>
        public DashboardResponse DashboardResponse { get; set; }

        /// <summary>
        /// Request [1 == trial, -1 == not trial]
        /// Response [0=expired trial, >0 nonexpired trial, -1 = not trial]
        /// </summary>
        public int TrialRemaining { get; set; }

        public string AppVersion { get; set; }

        public int IssueAge { get; set; }

        public int FetchAllIncludingEmpties { get; set; }

        public String UserId { get; set; }

        public String PhoneId { get; set; }

        public String PhoneMaker { get; set; }

        /// <summary>
        /// Summary of data
        /// true == count of items since & most recent item - this is used by background agent for toast
        /// false == all items since - this is used by app
        /// </summary>
        public bool Summary { get; set; }

    }

    public class RssFeed 
    {
        public String ServiceName { get; set; }
        public String RegionLocation { get; set; }
        public String ServiceId { get; set; }
        public String RegionId { get; set; }
        public String FeedCode { get; set; }
        public String RSSLink { get; set; }
        public DateTime DateStamp { get; set; }
    }
    public class RSSFeedItemDetail 
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PubDate { get; set; }

        public string Status { get; set; }
    }
    public class DashboardResponse 
    {
        public List<DashboardItem> List { get; set; }
    }
    public class DashboardItem 
    {
        public RssFeed FeedDefintion { get; set; }
        public List<RSSFeedItemDetail> FeedIssues { get; set; }
    }
}
