// -----------------------------------------------------------------------
// <copyright file="ContentTag.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Enums
{
    using System;
    
    /// <summary>
    /// Places to search for html tag content
    /// </summary>
    internal enum ContentTag  
    {
        /// <summary>
        /// Named attribute inside of tag bracket
        /// </summary>
        AttributeValue,

        /// <summary>
        /// Inner html of tag
        /// </summary>
        InnerHtml
    }
}
