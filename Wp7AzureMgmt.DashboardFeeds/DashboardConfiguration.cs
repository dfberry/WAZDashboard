// -----------------------------------------------------------------------
// <copyright file="DashboardConfiguration.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    
    /// <summary>
    /// Manages Config file AppSettings values.
    /// </summary>
    internal class DashboardConfiguration
    {
        /// <summary>
        /// Gets DefaultUri. This is any Uri that will return 200
        /// suggest "http://localhost"
        /// </summary>
        /// <returns>AppSettings["Default200Uri"] as string</returns>
        public string GetDefaultUri
        {
            get
            {
                return ConfigurationManager.AppSettings["Default200Uri"];
            }
        }

        /// <summary>
        /// Gets AzureUri. This is the Uri for Windows Azure Dashboard RSSFeeds
        /// </summary>
        /// <returns>AppSettings["AzureDashboardServiceURL"] as string</returns>
        public string GetAzureUri
        {
            get
            {
                return ConfigurationManager.AppSettings["AzureDashboardServiceURL"];
            }
        }

        /// <summary>
        /// Gets FeedCount. Regardless of where html comes from, how many RSS feeds should there be
        /// </summary>
        /// <returns>AppSettings["LastKnownRSSFeedCount"] as int</returns>
        public int GetFeedCount
        {
            get
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
        }

        /// <summary>
        /// Gets ContentFrom setting. Grab html from web or file.
        /// </summary>
        /// <returns>AppSettings["ContentFrom"] as string</returns>
        public string GetContentFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["ContentFrom"];
            }
        }

        /// <summary>
        /// Gets test file name. Test file name to open and treat and contents of Dashboard
        /// to parse for feeds. 
        /// </summary>
        /// <returns>AppSettings["TestFile"] as string</returns>
        public string GetTestFileName
        {
            get
            {
                return ConfigurationManager.AppSettings["TestFile"];
            }
        }

        /// <summary>
        /// Plan to update feed count and file name
        /// </summary>
        /// <param name="key">Name of config/appsettings item</param>
        /// <param name="value">New value of config/appsettings item</param>
        public void ChangeAppSettingsConfiguration(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");

            appSettings.Settings[key].Value = value;

            config.Save();
        }
    }
}
