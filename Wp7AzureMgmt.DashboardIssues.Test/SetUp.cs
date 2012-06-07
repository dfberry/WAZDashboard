// -----------------------------------------------------------------------
// <copyright file="Setup.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardIssues.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.Core;
    //using Wp7AzureMgmt.DashboardFeeds.Utilities;

    /// <summary>
    /// Grab RSS feeds page from Windows Azure
    /// only once a day and datestamp the file
    /// name during serialization.
    /// http://stackoverflow.com/questions/10491996/nunit-not-running-setup-method-in-visual-studio-debug-mode
    /// </summary>
    public static class Setup
    {
        ///// <summary>
        ///// Pull Html file from Windows Azure only once, store it,
        ///// save file name.
        ///// </summary>
        ///// <returns>filename as string</returns>
        public static string RunBeforeTests_FeedListFile()
        {

            HttpContextBase httpContext = null;
            DashboardMgr dashboardMgr = new DashboardMgr(httpContext);

            dashboardMgr.SetRssFeedsFromUri(GetDataPath());

            return string.Empty;
        }

        ///// <summary>
        ///// Pull Html file from Windows Azure only once, store it,
        ///// save file name.
        ///// </summary>
        ///// <returns>filename as string</returns>
        public static string RunBeforeTests_IssueListFile()
        {
            HttpContextBase httpContext = null;
            IssueMgr issueMgr = new IssueMgr(httpContext);

            issueMgr.SetRssIssuesFromUri(GetDataPath());

            return string.Empty;
        }

        /// <summary>
        /// Gets data path so tests look for data files in correct place
        /// </summary>
        /// <returns>string including post pend slash</returns>
        public static string GetDataPath()
        {
            string finalPath = string.Empty;
            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] splitString = null;
            string basePath = string.Empty;

            // everything before the bin directory
            if (binPath.Contains("bin"))
            {
                Regex matchPattern = new Regex("bin");
                splitString = matchPattern.Split(binPath);
                basePath = splitString[0];
                finalPath = Path.Combine(basePath, @"App_Data\");
            }
            else if (binPath.Contains("TestResults"))
            {
                Regex matchPattern = new Regex("TestResults");
                splitString = matchPattern.Split(binPath);
                basePath = splitString[0];
                finalPath = Path.Combine(basePath, "XmlTestProject", @"App_Data\");
            }
            else
            {
                // don't know where the path is at this point
            }

            return finalPath;
        }
    }
}
