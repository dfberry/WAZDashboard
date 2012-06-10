// -----------------------------------------------------------------------
// <copyright file="DashboardIssueModel.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDashboardService.Models
{
    using System;

    /// <summary>
    /// Flattened issue model 
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Azure service name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Azure location name
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Azure feedcode, combination of service and location
        /// </summary>
        public string FeedCode { get; set; }

        /// <summary>
        /// Azure Issue date (utc?)
        /// </summary>
        public string IssueDate { get; set; }

        /// <summary>
        /// Azure Issue Title (raw)
        /// </summary>
        public string IssueTitle { get; set; }

        /// <summary>
        /// Azure Issue Description (deal with encoded/unencoded html)
        /// </summary>
        public string IssueDescription { get; set; }

        /// <summary>
        /// Azure Issue Status
        /// </summary>
        public string IssueStatus { get; set; }

    }
}
