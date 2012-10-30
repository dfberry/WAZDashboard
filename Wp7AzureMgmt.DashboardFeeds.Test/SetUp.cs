// -----------------------------------------------------------------------
// <copyright file="Setup.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
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
    using Wp7AzureMgmt.Core;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Grab RSS feeds page from Windows Azure
    /// only once a day and datestamp the file
    /// name during serialization.
    /// http://stackoverflow.com/questions/10491996/nunit-not-running-setup-method-in-visual-studio-debug-mode
    /// </summary>
    public static class Setup
    {
        /// <summary>
        /// Pull Html file from Windows Azure only once, store it,
        /// save file name.
        /// </summary>
        /// <returns>filename as string</returns>
        public static string RunBeforeTests()
        {
            Trace.TraceInformation("RunBeforeAnyTests a");

            // if there is a file with today's date, don't do anything
            string fileName = @"..\..\..\Wp7AzureMgmt.DashboardFeeds.Test\Html_634871865298370572.html";
            HttpContextBase httpContext = null;

            // file built today not found so go build it
            // runs inside same day can use today's html 
            if (!File.Exists(fileName))
            {
                Trace.TraceInformation("RunBeforeAnyTests b");

                // initialize Uri
                UriDatasource uriDatasource = new UriDatasource(null, httpContext);

                DashboardHttp http = new DashboardHttp(uriDatasource.DashboardUri);

                uriDatasource.DashboardHttp = http;

                // get html and save to serialized file
                // set filename in config file as test
                uriDatasource.GetAndSaveHtml(fileName);

                // Save filename to config file so test run can use it for parsing
                FeedConfiguration dbconf = new FeedConfiguration(httpContext);
                dbconf.TestFileName = fileName;

                Trace.TraceInformation("RunBeforeAnyTests c");
            }

            Trace.TraceInformation("RunBeforeAnyTests d");

            return fileName;
        }

        /// <summary>
        /// Gets data path so tests look for data files in correct place
        /// </summary>
        /// <returns>string including post pend slash</returns>
        public static string GetDataPath()
        {
            string finalPath = string.Empty;

            RssFeeds feeds = new RssFeeds();

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
