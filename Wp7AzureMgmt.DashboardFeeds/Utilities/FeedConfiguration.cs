// -----------------------------------------------------------------------
// <copyright file="FeedConfiguration.cs" company="DFBerry">
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
    using Wp7AzureMgmt.Core;

    /// <summary>
    /// Manages Config file AppSettings values.
    /// </summary>
    public class FeedConfiguration : Wp7AzureMgmt.Core.Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedConfiguration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        public FeedConfiguration(HttpContextBase httpContext)
            : base(httpContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedConfiguration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// configFileName is any "other" config file besides the default.
        /// For example: MainSettings.config
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        /// <param name="configFileName">name of non-default config file</param>
        public FeedConfiguration(HttpContextBase httpContext, string configFileName)
            : base(httpContext, configFileName)
        {
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
    }
}
