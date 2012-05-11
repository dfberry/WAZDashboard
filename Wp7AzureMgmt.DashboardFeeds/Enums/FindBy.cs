// -----------------------------------------------------------------------
// <copyright file="FindBy.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Enums
{
    using System;
    
    /// <summary>
    /// Is content found by pulling from Uri or opening file 
    /// built from pulling from Uri.
    /// Meant to reduce the pulls from Azure for testing and to only
    /// pull when necessary.
    /// </summary>
    internal enum FindBy
    {
        /// <summary>
        /// No location
        /// </summary>
        notset,

        /// <summary>
        /// Uri provided 
        /// </summary>
        uriprovided,

        /// <summary>
        /// String/String from file
        /// </summary>
        stringprovided
    }
}
