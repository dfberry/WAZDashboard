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
                return this.Get("Default200Uri");
            }
        }

        /// <summary>
        /// Gets GetAzureFeedUriPrefix. This is the Uri prefix for the Feeds.
        /// </summary>
        /// <returns>AppSettings["AzureDashboardServiceFeedPrefix"] as string</returns>
        public string GetAzureFeedUriPrefix
        {
            get
            {
                return this.Get("AzureDashboardServiceFeedPrefix");
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
                return this.Get("AzureDashboardServiceURL");
            }
        }

        /// <summary>
        /// Gets FeedCount. Regardless of where html comes from, 
        /// how many RSS feeds should there be. Throws
        /// FormatException if can't parse value into int.
        /// </summary>
        /// <returns>AppSettings["LastKnownRSSFeedCount"] as int</returns>
        public int GetFeedCount
        {
            get
            {
                int result = 0;

                string tempCount = this.Get("LastKnownRSSFeedCount");

                if (int.TryParse(tempCount, out result))
                {
                    return result;
                }
                else
                {
                    throw new FormatException();
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
                return this.Get("ContentFrom");
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
                return this.Get("TestFile");
            }
        }

        /// <summary>
        /// General method to fetch config setting
        /// value 
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <returns>value of appSettings Key</returns>
        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("name");
            }

            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Save determines if update or create is necessary,
        /// and performs that action. Throws ArgumentNullException
        /// if params are null. Throws NullReferenceException if
        /// Configuration object can't be created. 
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <param name="value">value assocated with key</param>
        public void Save(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (configFile == null)
            {
                throw new NullReferenceException("Configuration");
            }

            foreach (string strKey in configFile.AppSettings.Settings.AllKeys)
            {
                if (strKey == key)
                {
                    this.Update(key, value, configFile);
                    return;
                }
            }

            // key not found so create it
            this.Create(key, value, configFile);
        }

        /// <summary>
        /// Create new appSettings key/value pair
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <param name="value">value assocated with key</param>
        /// <param name="configFile">config file containing settings</param>
        public void Create(string key, string value, Configuration configFile)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (configFile == null)
            {
                throw new ArgumentNullException("ConfigFile");
            }

            configFile.AppSettings.Settings.Add(key, value);
            configFile.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
        
        /// <summary>
        /// Update existing appSettings value given key
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <param name="value">value assocated with key</param>
        /// <param name="configFile">config file containing settings</param>
        public void Update(string key, string value, Configuration configFile)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (configFile == null)
            {
                throw new ArgumentNullException("ConfigFile");
            }

            configFile.AppSettings.Settings[key].Value = value;
            configFile.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Clear key from appSettings
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Returns true if key exists
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <returns>bool if key exists</returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (ConfigurationManager.AppSettings.AllKeys.ToList().Contains(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }    
    }
}
