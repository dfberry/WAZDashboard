// -----------------------------------------------------------------------
// <copyright file="FileDatasource.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Wp7AzureMgmt.DashboardIssues.Interfaces;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using Wp7AzureMgmt.DashboardIssues.Utiliites;

    /// <summary>
    /// This datasource fetches the issue list from a file on disk
    /// </summary>
    public class FileDatasource : IRssIssueDataSource
    {
        /// <summary>
        /// DashboardConfiguration is required as class object for tracing
        /// </summary>
        private IssueConfiguration config;

        /// <summary>
        /// HttpContext determines where to get config settings. Web app looks in 
        /// web config.
        /// </summary>
        private HttpContextBase configurationContext;

        /// <summary>
        /// Path and filename of RssIssues datasource. 
        /// </summary>
        private string fileName;

        /// <summary>
        /// RssIssues built from filename
        /// </summary>
        private RssIssues issues = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDatasource" /> class.
        /// Can be overwritten with
        /// FileName property. 
        /// </summary>
        /// <param name="pathToFilename">full path to filename including postpend slash</param>
        /// <param name="httpContext">HttpContextBase - used to determine config file location</param>
        public FileDatasource(string pathToFilename, HttpContextBase httpContext)
        {
            // set this once in constructor
            this.configurationContext = httpContext;
            this.config = new IssueConfiguration(this.configurationContext);

            this.FileName = pathToFilename + this.config.SerializedIssueListFile;

            if (string.IsNullOrEmpty(this.FileName))
            {
                throw new NullReferenceException("FileDatasource::FileDatasource - FileName is null or empty");
            }
        }

        /// <summary>
        /// Gets or sets RssIssues built from filename
        /// </summary>
        public RssIssues RssIssues
        {
            get
            {
                return this.issues;
            }

            set
            {
                this.issues = value;
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
        /// Serialize RssIssues to FileName.
        /// </summary>
        public void Set()
        {
            this.Set(this.issues, this.fileName);
        }

        /// <summary>
        /// Serialize the issues to a filename. Neither can be 
        /// null.
        /// </summary>
        /// <param name="issues">list of issues</param>
        /// <param name="filename">filename and path</param>
        public void Set(RssIssues issues, string filename)
        {
            if (issues == null)
            {
                throw new ArgumentNullException("FileDatasource::Set - " + "RssIssues issues");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("FileDatasource::Set - " + "string filename");
            }

            Wp7AzureMgmt.Core.Serializer.Serialize(filename, issues);
        }

        /// <summary>
        /// Deserialize the issues from FileName. 
        /// </summary>
        /// <returns>RssIssues from Serialized file</returns>
        public RssIssues Get()
        {
            return this.Get(this.fileName);
        }

        /// <summary>
        /// Deserialize RssIssues from serializedFile
        /// </summary>
        /// <param name="serializedFile">path and filename</param>
        /// <returns>RssIssues from file</returns>
        public RssIssues Get(string serializedFile)
        {
            if (string.IsNullOrEmpty(serializedFile))
            {
                throw new ArgumentNullException("FileDatasource::GetIssues - " + "string serializedFile");
            }

            if (File.Exists(serializedFile))
            {
                this.issues = Wp7AzureMgmt.Core.Serializer.Deserialize<RssIssues>(serializedFile);
                return this.issues;
            }
            else
            {
                throw new ArgumentNullException("FileDatasource::GetIssues - " + "serializedFile doesn't exist - " + serializedFile);
            }
        }
    }
}
