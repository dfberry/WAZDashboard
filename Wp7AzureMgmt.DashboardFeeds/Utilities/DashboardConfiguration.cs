// -----------------------------------------------------------------------
// <copyright file="DashboardConfiguration.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Net.Configuration;
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
        /// Initializes a new instance of the <see cref="DashboardConfiguration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// configFileName is any "other" config file besides the default.
        /// For example: MainSettings.config
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        /// <param name="configFileName">name of non-default config file</param>
        public DashboardConfiguration(HttpContextBase httpContext, string configFileName)
        {
            this.httpContext = httpContext;

            if (this.httpContext == null)
            {
                this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else
            {
                this.config = WebConfigurationManager.OpenWebConfiguration("~", "Default Web Site", configFileName);
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
        /// Gets or sets SmtpSection in config file
        /// http://forums.asp.net/t/1704500.aspx/1
        /// http://msdn.microsoft.com/en-us/library/system.net.configuration.smtpsection.aspx
        /// http://social.msdn.microsoft.com/Forums/en/netfxbcl/thread/62b00a0a-67fc-4fd6-b240-cc55121db9a7
        /// </summary>
        public SmtpSection SmtpSection
        {
            get
            {
                return (SmtpSection)this.config.GetSection("system.net/mailSettings/smtp");
            }

            set
            {
                SmtpSection settings = (SmtpSection)this.config.GetSection("system.net/mailSettings/smtp");

                settings.Network.UserName = value.Network.UserName;
                settings.Network.Password = value.Network.Password;
                settings.Network.Host = value.Network.Host;
                settings.Network.Port = value.Network.Port;
                settings.Network.EnableSsl = true;

                this.config.Save();
            }
        }

        /// <summary>
        /// Gets or sets EmailLogon
        /// </summary>
        public string EmailLogon
        {
            get
            {
                return this.Get("EmailLogon");
            }

            set
            {
                this.Save("EmailLogon", value);
            }
        }

        /// <summary>
        /// Gets or sets EmailPassword
        /// </summary>
        public string EmailPassword
        {
            get
            {
                return this.Get("EmailPassword");
            }

            set
            {
                this.Save("EmailPassword", value);
            }
        }

        /// <summary>
        /// Gets EmailFromAddress
        /// </summary>
        public string EmailFromAddress
        {
            get
            {
                return this.Get("EmailFromAddress");
            }
        }

        /// <summary>
        /// Gets EmailFromName
        /// </summary>
        public string EmailFromName
        {
            get
            {
                return this.Get("EmailFromName");
            }
        }

        /// <summary>
        /// Gets EmailToAddress
        /// </summary>
        public string EmailToAddress
        {
            get
            {
                return this.Get("EmailToAddress");
            }
        }

        /// <summary>
        /// Gets EmailToName
        /// </summary>
        public string EmailToName
        {
            get
            {
                return this.Get("EmailToName");
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
        /// Gets FullSerializedFileDatasourceFilePathAndName
        /// which is PathToWebRoot + DataFileDir + SerializedFeedListFile 
        /// </summary>
        public string FullSerializedFileDatasourceFilePathAndName
        {
            get
            {
                string temp = this.PathToWebRoot + this.SerializedFeedListFile;

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
                string temp = this.PathToWebRoot + this.TraceLogFileName;

                return temp;
            }
        }

        /// <summary>
        /// Gets all config settings in NameValueCollection
        /// </summary>
        public NameValueCollection GetAll
        {
            get
            {
                NameValueCollection list = new NameValueCollection();

                list.Add("AzureFeedUriPrefix", this.AzureFeedUriPrefix);
                list.Add("AzureUri", this.AzureUri);
                list.Add("DefaultUri", this.DefaultUri);

                list.Add("EmailFromAddress", this.EmailFromAddress);
                list.Add("EmailFromName", this.EmailFromName);
                list.Add("EmailToAddress", this.EmailToAddress);
                list.Add("EmailToName", this.EmailToName);

                list.Add("PathToWebRoot", this.PathToWebRoot);
                list.Add("SerializedFeedListFile", this.SerializedFeedListFile);
                list.Add("TraceLogFileName", this.TraceLogFileName);

                return list;
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
