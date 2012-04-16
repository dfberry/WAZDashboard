// -----------------------------------------------------------------------
// <copyright file="DashboardFile.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    
    /// <summary>
    /// File class to handle open, read, write. Assume any
    /// reference to file name means path + filename. 
    /// </summary>
    internal class DashboardFile
    {
        /// <summary>
        /// Gets or sets complete/entire contents of file.
        /// </summary>
        public string FileContents { get; set; }

        /// <summary>
        /// Gets or sets path + filename.
        /// </summary>
        public string FileName { get; set; }
    }
}
