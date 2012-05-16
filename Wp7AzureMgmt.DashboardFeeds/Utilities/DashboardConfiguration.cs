// -----------------------------------------------------------------------
// <copyright file="DashboardConfiguration.cs" company="DFBerry">
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
    using System.Web;
    using System.Web.Configuration;
    
    /// <summary>
    /// Manages Config file AppSettings values.
    /// </summary>
    public class DashboardConfiguration 
    {
        /// <summary>
        /// HttpContext determines where to get config settings. Web app looks in 
        /// web config.
        /// </summary>
        private HttpContextBase httpContext;

        /// <summary>
        /// Configuration file of current loading assembly
        /// </summary>
        private Configuration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardConfiguration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        public DashboardConfiguration(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;

            if (this.httpContext == null)
            {
                this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else
            {
                this.config = WebConfigurationManager.OpenWebConfiguration("~");
            }
        }

        /// <summary>
        /// Gets DefaultUri. This is any Uri that will return 200
        /// suggest "http://localhost"
        /// </summary>
        /// <returns>AppSettings["Default200Uri"] as string</returns>
        public string DefaultUri
        {
            get
            {
                return this.Get("Default200Uri");
            }
        }

        /// <summary>
        /// Gets DataFileDir. This is the location where files are saved to disk.
        /// An example is the serialized file of the FileDataSource.
        /// </summary>
        /// <returns>AppSettings["DirForDataFiles"] as string</returns>
        public string DataFileDir
        {
            get
            {
                return this.Get("DirForDataFiles");
            }
        }

        /// <summary>
        /// Gets or sets PathToWebRoot. Must be determined from calling exe
        /// (such as test environment) or web app.
        /// </summary>
        public string PathToWebRoot
        {
            get
            {
                return this.Get("PathToWebRoot");
            }

            set
            {
                this.Save("PathToWebRoot", value);
            }
        }

        /// <summary>
        /// Gets GetAzureFeedUriPrefix. This is the Uri prefix for the Feeds.
        /// </summary>
        /// <returns>AppSettings["AzureDashboardServiceFeedPrefix"] as string</returns>
        public string AzureFeedUriPrefix
        {
            get
            {
                return this.Get("AzureDashboardServiceFeedPrefix");
            }
        }

        /// <summary>
        /// Gets name of feeds file. 
        /// </summary>
        /// <returns>AppSettings["SerializedFeedListFile"] as string</returns>
        public string SerializedFeedListFile
        {
            get
            {
                return this.Get("SerializedFeedListFile");
            }
        }
        
        /// <summary>
        /// Gets tracelog filename without or without path. 
        /// </summary>
        /// <returns>AppSettings["TraceLogFileName"] as string</returns>
        public string TraceLogFileName
        {
            get
            {
                return this.Get("TraceLogFileName");
            }
        }

        /// <summary>
        /// Gets FullSerializedFileDatasourceFilePathAndName
        /// which is PathToWebRoot + DataFileDir + SerializedFeedListFile 
        /// </summary>
        public string FullSerializedFileDatasourceFilePathAndName
        {
            get
            {
                string temp = this.PathToWebRoot + this.DataFileDir + this.SerializedFeedListFile;

                return temp;
            }
        }

        /// <summary>
        /// Gets FullTraceLogFilePathAndName
        /// which is PathToWebRoot + DataFileDir + TraceLogFileName
        /// </summary>
        public string FullTraceLogFilePathAndName
        {
            get
            {
                string temp = this.PathToWebRoot + this.DataFileDir + this.TraceLogFileName;

                return temp;
            }
        }

        /// <summary>
        /// Gets AzureUri. This is the Uri for Windows Azure Dashboard RSSFeeds
        /// </summary>
        /// <returns>AppSettings["AzureDashboardServiceURL"] as string</returns>
        public string AzureUri
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
        public int FeedCount
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
        public string ContentFrom
        {
            get
            {
                return this.Get("ContentFrom");
            }
        }

        /// <summary>
        /// Gets or sets test file name. Test file name to open and treat and contents of Dashboard
        /// to parse for feeds. 
        /// </summary>
        /// <returns>AppSettings["TestFile"] as string</returns>
        public string TestFileName
        {
            get
            {
                return this.Get("TestFile");
            }

            set
            {
                this.Save("TestFile", value);
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

            string value = string.Empty;

            try
            {
                return this.config.AppSettings.Settings[key].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Save determines if update or create is necessary,
        /// and performs that action. Throws ArgumentNullException
        /// if params are null. Throws NullReferenceException if
        /// Configuration object can't be created. This will not create
        /// a multi-value key. If you need a multi-value key, call
        /// Create directly.
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

            foreach (string strKey in this.config.AppSettings.Settings.AllKeys)
            {
                if (strKey == key)
                {
                    this.Update(key, value);
                    return;
                }
            }

            // key not found so create it
            this.Create(key, value);
        }

        /// <summary>
        /// Create new appSettings key/value pair. Multiple 
        /// creates on the same key adds the value again.
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <param name="value">value assocated with key</param>
        public void Create(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (this.config == null)
            {
                throw new ArgumentNullException("ConfigFile");
            }

            this.config.AppSettings.Settings.Add(key, value);
            this.config.Save();
        }
        
        /// <summary>
        /// Update existing appSettings value given key
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <param name="value">value assocated with key</param>
        public void Update(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (this.config == null)
            {
                throw new ArgumentNullException("ConfigFile");
            }

            this.config.AppSettings.Settings[key].Value = value;
            this.config.Save();
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

            this.config.AppSettings.Settings.Remove(key);
            this.config.Save(ConfigurationSaveMode.Modified, true);
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

            if (this.config.AppSettings.Settings.AllKeys.ToList().Contains(key))
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
