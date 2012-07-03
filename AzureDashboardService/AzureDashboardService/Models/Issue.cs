// -----------------------------------------------------------------------
// <copyright file="Issue.cs" company="DFBerry">
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
        /// Gets or sets Azure service name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets Azure location name
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets Azure feedcode, combination of service and location
        /// </summary>
        public string FeedCode { get; set; }

        /// <summary>
        /// Gets or sets Azure Issue date (utc?)
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Gets or sets Azure Issue Title (raw)
        /// </summary>
        public string IssueTitle { get; set; }

        /// <summary>
        /// Gets or sets Azure Issue Description (deal with encoded/unencoded html)
        /// </summary>
        public string IssueDescription { get; set; }

        /// <summary>
        /// Gets or sets Azure Issue Status
        /// </summary>
        public string IssueStatus { get; set; }
    }
}
