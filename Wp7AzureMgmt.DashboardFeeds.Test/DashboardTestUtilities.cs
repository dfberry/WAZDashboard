// -----------------------------------------------------------------------
// <copyright file="DashboardTestUtilities.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.Dashboard;
    
    /// <summary>
    /// Utility methods needed to resolve tests
    /// </summary>
    internal static class DashboardTestUtilities
    {
        /// <summary>
        /// This is any Uri that will return 200
        /// suggest "http://localhost"
        /// </summary>
        /// <returns>AppSettings["Default200Uri"] as string</returns>
        public static string GrabDefaultUriFromConfig()
        {
            return ConfigurationManager.AppSettings["Default200Uri"];
        }

        /// <summary>
        /// This is the Uri for Windows Azure Dashboard RSSFeeds
        /// </summary>
        /// <returns>AppSettings["AzureDashboardServiceURL"] as string</returns>
        public static string ConfigAZDashboardUriAsString()
        {
            return ConfigurationManager.AppSettings["AzureDashboardServiceURL"];
        }

        /// <summary>
        /// Regardless of where html comes from, how many RSS feeds should there be
        /// </summary>
        /// <returns>AppSettings["LastKnownRSSFeedCount"] as int</returns>
        public static int ConfigFeedCount()
        {
            string tempCount = ConfigurationManager.AppSettings["LastKnownRSSFeedCount"];
            if (string.IsNullOrEmpty(tempCount))
            {
                throw new ArgumentException("AppSettings[LastKnownRSSFeedCount] is empty");
            }
            else
            {
                return int.Parse(tempCount);
            }
        }

        /// <summary>
        /// Grab html from web or file
        /// </summary>
        /// <returns>AppSettings["ContentFrom"] as string</returns>
        public static string ConfigContentFrom()
        {
            return ConfigurationManager.AppSettings["ContentFrom"];
        }

        /// <summary>
        /// Test file name to open and treat and contents of Dashboard
        /// to parse for feeds. 
        /// </summary>
        /// <returns>AppSettings["TestFile"] as string</returns>
        public static string ConfigTestFileName()
        {
            return ConfigurationManager.AppSettings["TestFile"];
        }

        /// <summary>
        /// Contents of test file to parse for feeds. 
        /// </summary>
        /// <returns>file contents as string</returns>
        public static string TestFileContents()
        {
            string testFile = ConfigTestFileName();
            if ((!string.IsNullOrEmpty(testFile)) && File.Exists(testFile))
            {
                using (StreamReader sr = File.OpenText(testFile))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                throw new ArgumentException("AppSettings[TestFile] is empty");
            }
        }

        /// <summary>
        /// Http response content
        /// </summary>
        /// <returns>response content as string</returns>
        public static string AZUriContents()
        {
            string url = ConfigAZDashboardUriAsString();
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("no file content to parse");
            }
            else
            {
                HTTP http = new HTTP(new Uri(url));
                return http.RequestGET();
            }
        }

        /// <summary>
        /// Save response content as html file
        /// </summary>
        public static void SaveWADashboardContentToFile()
        {
            Uri getUri = new Uri(DashboardTestUtilities.ConfigAZDashboardUriAsString());
            HTTP http = new HTTP(getUri);

            string filename = DateTime.Now.Ticks + ".html";
            string responsecontent = http.RequestGET();

            http.SaveResponseContentToFile(filename);
        }

        /// <summary>
        /// Plan to update feed count and file name
        /// </summary>
        /// <param name="key">Name of config/appsettings item</param>
        /// <param name="value">New value of config/appsettings item</param>
        public static void ChangeConfiguration(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");

            appSettings.Settings[key].Value = value; 

            config.Save();
        }
    }
}
