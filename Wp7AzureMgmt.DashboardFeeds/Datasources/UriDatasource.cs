// -----------------------------------------------------------------------
// <copyright file="UriDatasource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using HtmlAgilityPack;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Factories;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using Wp7AzureMgmt.DashboardFeeds.Utilities;
    
    /// <summary>
    /// This datasource fetches the feed list from Windows Azure Dashboard Service web page
    /// </summary>
    internal class UriDatasource : IRSSDataSource
    {
        /// <summary>
        /// HttpContext determines where to get config settings. Web app looks in 
        /// web config.
        /// </summary>
        private HttpContextBase configurationContext;

        /// <summary>
        /// Configuration settings
        /// </summary>
        private DashboardConfiguration configuration = null;

        /// <summary>
        /// RssFeeds that is the data
        /// </summary>
        private RssFeeds rssFeeds;

        /// <summary>
        /// IDashboardHttp object
        /// </summary>
        private IDashboardHttp httpRequest;

        /// <summary>
        /// Path and File name 
        /// </summary>
        private string serializedFilename;

        /// <summary>
        /// URI containing Feedlist
        /// </summary>
        private Uri dashboardURI = null;

        /// <summary>
        /// Http response content pulled from URI
        /// </summary>
        private string uricontent = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriDatasource" /> class.
        /// </summary>
        /// <param name="http">DashboardHttp object containing request</param>
        /// <param name="httpContext">Http web context or null if not a web request</param>
        public UriDatasource(DashboardHttp http, HttpContextBase httpContext)
        {
            this.configuration = new DashboardConfiguration(httpContext);
            this.configurationContext = httpContext;
            this.httpRequest = http;
            this.GetConfigSettings();
        }

        /// <summary>
        /// Gets or sets DashboardUri
        /// </summary>
        public Uri DashboardUri
        {
            get
            {
                TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::DashboardUri - this.dashboardURI=" + this.dashboardURI);
                return this.dashboardURI;
            }

            set
            {
                this.dashboardURI = value;
            }
        }

        /// <summary>
        /// Gets or sets RssFeeds
        /// </summary>
        public RssFeeds RssFeeds
        {
            get
            {
                return this.rssFeeds;
            }

            set
            {
                this.rssFeeds = value;
            }
        }

        /// <summary>
        /// Gets or sets SerializedFilename
        /// </summary>
        public string SerializedFilename
        {
            get
            {
                TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::SerializedFilename - this.serializedFilename=" + this.serializedFilename);
                return this.serializedFilename;
            }

            set
            {
                this.serializedFilename = value;
            }
        }

        /// <summary>
        /// Gets or sets DashboardHttp
        /// </summary>
        public IDashboardHttp DashboardHttp
        {
            get
            {
                return this.httpRequest;
            }

            set
            {
                this.httpRequest = value;
            }
        }
        
        /// <summary>
        /// Get RssFeeds from uri specified in config file
        /// </summary>
        /// <returns>RssFeeds from parsed html from uri</returns>
        public RssFeeds Get()
        {
            // get raw html
            this.uricontent = this.GetHtml();
            TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::Get - this.uricontent=" + this.uricontent);

            string uriPrefix = this.configuration.AzureFeedUriPrefix;

            // parse content into object
            TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::Get - call into parser");
            TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::Get - uriPrefix=" + uriPrefix);
            HtmlParser htmlParser = new HtmlParser(uriPrefix);
            this.rssFeeds = htmlParser.ParseHtmlForFeeds(this.uricontent);

            return this.rssFeeds;
        }

        /// <summary>
        /// Get Html from uri. Sets object's Uri and requests
        /// from that Uri.
        /// </summary>
        /// <param name="uri">uri to grab html from </param>
        /// <returns>Html of request</returns>
        public string GetHtml(Uri uri)
        {
            if (this.httpRequest == null)
            {
                throw new NullReferenceException("httpRequest");
            }

            this.dashboardURI = uri; 
            this.httpRequest.SetUri(this.dashboardURI);
            this.httpRequest.BuildHttpGet();
            this.uricontent = this.httpRequest.GetRequest();

            TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::GetHtml(Uri uri) - this.uricontent=" + this.uricontent);

            return this.uricontent;
        }

        /// <summary>
        /// Get Html from uri specified in config file
        /// </summary>
        /// <returns>Html of Rss feeds as string</returns>
        public string GetHtml()
        {
            if (this.httpRequest == null)
            {
                throw new ArgumentNullException();
            }

            this.uricontent = this.httpRequest.GetRequest();
            TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::GetHtml() - this.uricontent=" + this.uricontent);

            return this.uricontent;
        }

        /// <summary>
        /// Set can't be done back to the Azure Uri.
        /// Throws NotImplementedException.
        /// </summary>
        public void Set()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets html from uri and saves (without any changes)
        /// to filename. Works for text files only.
        /// </summary>
        /// <param name="filename">save html to this path and filename</param>
        public void GetAndSaveHtml(string filename)
        {
            if (string.IsNullOrEmpty(this.dashboardURI.ToString()))
            {
                throw new NullReferenceException("dashboardUri");
            }

            if (string.IsNullOrEmpty(filename.ToString()))
            {
                throw new NullReferenceException("filename");
            }

            string html = this.GetHtml(this.DashboardUri);
            TraceLogToFile.Trace(this.configuration.FullTraceLogFilePathAndName, "UriDatasource::GetAndSaveHtml() - html=" + html);

            File.WriteAllText(filename, html);
        }

        /// <summary>
        /// Get all config settings. Sets 
        /// dashboardURI. Creates new DashboardHttp
        /// for httpRequest. Calls BuildHttpGet. All
        /// of these can be overwritten/called again
        /// later.
        /// </summary>
        private void GetConfigSettings()
        {
            this.configuration = new DashboardConfiguration(this.configurationContext);

            // need an http requester - this can always be overwritten
            this.dashboardURI = new Uri(this.configuration.AzureUri);
            this.httpRequest = new DashboardHttp(this.dashboardURI);
        }
    }
}
