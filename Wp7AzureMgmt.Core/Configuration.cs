// -----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Configuration;
    using System.Web;
    using System.Web.Configuration;

    /// <summary>
    /// Manages Config file AppSettings values.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// HttpContext determines where to get config settings. Web app looks in 
        /// web config.
        /// </summary>
        private HttpContextBase httpContext;

        /// <summary>
        /// Configuration file of current loading assembly
        /// </summary>
        private System.Configuration.Configuration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        public Configuration(HttpContextBase httpContext)
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
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// configFileName is any "other" config file besides the default.
        /// For example: MainSettings.config
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        /// <param name="configFileName">name of non-default config file</param>
        public Configuration(HttpContextBase httpContext, string configFileName)
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
        /// General method to fetch config setting
        /// value 
        /// </summary>
        /// <param name="key">name of appSettings key</param>
        /// <returns>value of appSettings Key</returns>
        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("string.IsNullOrEmpty(key)");
            }

            return this.config.AppSettings.Settings[key].Value;
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

