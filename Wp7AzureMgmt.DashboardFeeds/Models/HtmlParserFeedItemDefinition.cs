// -----------------------------------------------------------------------
// <copyright file="HtmlParserFeedItemDefinition.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.Dashboard.Models;
    
    /// <summary>
    /// After grabbed the parent object, this controls
    /// how to divid the inner data of that object
    /// </summary>
    internal class HTMLParserFeedItemDefinition
    {
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public HTMLParserFeedItemType Name { get; set; }

        /// <summary>
        /// Gets or sets Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets ContentType
        /// </summary>
        public TagContent ContentType { get; set; }

        /// <summary>
        /// Gets or sets ChildTag
        /// </summary>
        public string ChildTag { get; set; }

        /// <summary>
        /// Gets or sets AttributeName
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets or sets ReturnAttributeName
        /// </summary>
        public string ReturnAttributeName { get; set; }

        /// <summary>
        /// hashcode built from Tag and AttributeName
        /// </summary>
        /// <returns>hascode as int</returns>
        public override int GetHashCode()
        {
            return (this.Tag ?? string.Empty).GetHashCode() +
            (this.AttributeName ?? string.Empty).GetHashCode();
        }

        /// <summary>
        /// Used by test framework to compare objects
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(HTMLParserFeedItemDefinition))
            {
                return false;
            }

            return (((HTMLParserFeedItemDefinition)obj).Tag == this.Tag) && (((HTMLParserFeedItemDefinition)obj).AttributeName == this.AttributeName);
        }
    }
}
