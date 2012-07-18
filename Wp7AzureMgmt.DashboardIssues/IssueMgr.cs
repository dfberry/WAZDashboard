// -----------------------------------------------------------------------
// <copyright file="IssueMgr.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Wp7AzureMgmt.Core;
    using Wp7AzureMgmt.DashboardIssues.DataSources;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using Wp7AzureMgmt.DashboardIssues.Utiliites;

    /// <summary>
    /// Entry point for Issue management from MVC app.
    /// </summary>
    public class IssueMgr
    {
        /// <summary>
        /// Default Parallel Options to no limit
        /// </summary>
#if DEBUG
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
#else  
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
#endif

        /// <summary>
        /// Http context which shouldn't be null if called
        /// from MVC app
        /// </summary>
        private HttpContextBase httpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueMgr" /> class.
        /// </summary>
        /// <param name="context">HttpContextBase tells library where to get config settings</param>
        public IssueMgr(HttpContextBase context)
        {
            if (context != null)
            {
                this.httpContext = context;
            }
        }

        /// <summary>
        /// Gets Issues file name in /App_Data
        /// </summary>
        public string DatasourceFilename
        {
            get
            {
                IssueConfiguration issueConfiguration = new IssueConfiguration(this.httpContext);
                return issueConfiguration.SerializedIssueListFile;
            }
        }

        /// <summary>
        /// Deserialize RssIssues from disk
        /// </summary>
        /// <param name="pathToFiles">location on disk for serialized files</param>
        /// <returns>RssIssues from file</returns>
        public RssIssues GetStoredRssIssues(string pathToFiles)
        {
            FileDatasource fileDatasource = new FileDatasource(pathToFiles, this.httpContext);

            // get from file
            return fileDatasource.Get();
        }

        /// <summary>
        /// Serialize RssIssues to disk. Make request across
        /// wire to Uri, grab response into RssIssues,
        /// serialize RssIssues.
        /// </summary>
        /// <param name="pathToFiles">Path to serialized files</param>
        public void SetRssIssuesFromUri(string pathToFiles)
        {
            DashboardHttp httpRequest = new DashboardHttp();

            // DFB - this is a dummy uri not intended to work 
            httpRequest.Uri = new Uri("http://localhost");

            UriDatasource uriDataSource = new UriDatasource(httpRequest, this.httpContext, pathToFiles);
            RssIssues issues = uriDataSource.Get();
            FileDatasource fileDatasource = new FileDatasource(pathToFiles, this.httpContext);
            fileDatasource.RssIssues = issues;
            fileDatasource.Set();
        }
    }
}
