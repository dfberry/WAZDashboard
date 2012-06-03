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
        public static string RunBeforeTests()
        {

            HttpContextBase httpContext = null;
            DashboardMgr dashboardMgr = new DashboardMgr(httpContext);

            dashboardMgr.SetRssFeedsFromUri(GetDataPath());

            return string.Empty;
        }

        /// <summary>
        /// Gets data path so tests look for data files in correct place
        /// </summary>
        /// <returns>string including post pend slash</returns>
        public static string GetDataPath()
        {
            string binPath = new System.Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;

            // everything before the bin directory
            Regex matchPattern = new Regex("bin");

            // grab matches
            string[] splitString = matchPattern.Split(binPath);

            if (splitString != null)
            {
                return splitString[0] + "App_Data/";
            }

            return string.Empty;
        }
    }
}
