// -----------------------------------------------------------------------
// <copyright file="FileDatasource.cs" company="DFBerry">
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
    using System.Threading.Tasks;
    using System.Web;
    using HtmlAgilityPack;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Factories;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// This datasource fetches the feed list from a file on disk
    /// </summary>
    internal class FileDatasource : IRSSDataSource
    {
        /// <summary>
        /// HttpContext determines where to get config settings. Web app looks in 
        /// web config.
        /// </summary>
        private HttpContextBase configurationContext;
      
        /// <summary>
        /// Path and filename of RssFeeds datasource. 
        /// </summary>
        private string fileName;

        /// <summary>
        /// RssFeeds built from filename
        /// </summary>
        private RssFeeds feeds = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDatasource" /> class.
        /// Can be overwritten with
        /// FileName property. 
        /// </summary>
        /// <param name="pathToFilename">full path to filename including postpend slash</param>
        /// <param name="httpContext">HttpContextBase - used to determine config file location</param>
        public FileDatasource(string pathToFilename, HttpContextBase httpContext)
        {
            this.configurationContext = httpContext;

            // set this once in constructor
            DashboardConfiguration config = new DashboardConfiguration(this.configurationContext);

            this.FileName = pathToFilename + config.SerializedFeedListFile;

            if (string.IsNullOrEmpty(this.FileName))
            {
                throw new NullReferenceException("FileName");
            }
        }

        /// <summary>
        /// Gets or sets RssFeeds built from filename
        /// </summary>
        public RssFeeds RssFeeds
        {
            get
            {
                return this.feeds;
            }

            set
            {
                this.feeds = value;
            }
        }

        /// <summary>
        /// Gets or sets FileName
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
            }
        }

        /// <summary>
        /// Returns importable file as string for Google Reader
        /// based on feeds passed in.
        /// </summary>
        /// <param name="rssFeeds">RssFeeds to convert to opml</param>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        public string OPML(RssFeeds rssFeeds)
        {
            if (rssFeeds == null)
            {
                throw new ArgumentNullException("RssFeeds rssFeeds");
            }

            return rssFeeds.OPML();
        }

        /// <summary>
        /// Returns importable file as string for Google Reader
        /// </summary>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        public string OPML()
        {
            return this.OPML(this.feeds);
        }

        /// <summary>
        /// Serialize RssFeeds to FileName.
        /// </summary>
        public void Set()
        {
            this.Set(this.feeds, this.fileName);
        }

        /// <summary>
        /// Serialize the feeds to a filename. Neither can be 
        /// null.
        /// </summary>
        /// <param name="feeds">list of feeds</param>
        /// <param name="filename">filename and path</param>
        public void Set(RssFeeds feeds, string filename)
        {
            if (feeds == null)
            {
                throw new ArgumentNullException("RssFeeds feeds");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("string filename");
            }

            try
            {
                Serializer.Serialize(filename, feeds);
            }
            catch
            {
                throw new Exception("FileDatasource::Set - serialization of FileDatasource failed.");
            }
        }

        /// <summary>
        /// Deserialize the feeds from FileName. 
        /// </summary>
        /// <returns>RssFeeds from Serialized file</returns>
        public RssFeeds Get()
        {
            return this.GetFeeds(this.fileName);
        }

        /// <summary>
        /// Deserialize RssFeeds from serializedFile
        /// </summary>
        /// <param name="serializedFile">path and filename</param>
        /// <returns>RssFeeds from file</returns>
        public RssFeeds GetFeeds(string serializedFile)
        {
            if (string.IsNullOrEmpty(serializedFile))
            {
                throw new ArgumentNullException("string serializedFile");
            }

            try
            {
                if (File.Exists(serializedFile))
                {
                    this.feeds = Serializer.Deserialize<RssFeeds>(serializedFile);
                    return this.feeds;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new Exception("FileDatasource::GetFeeds - deserialization of FileDatasource failed.");
            }
        }
    }
}
