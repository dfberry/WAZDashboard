// -----------------------------------------------------------------------
// <copyright file="IssueConfiguration.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.Utiliites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    
    /// <summary>
    /// Configuration class for Issues. 
    /// </summary>
    public class IssueConfiguration : Wp7AzureMgmt.Core.Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssueConfiguration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        public IssueConfiguration(HttpContextBase httpContext)
            : base(httpContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueConfiguration" /> class.
        /// If httpContext is null, calls ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        /// otherwise calls WebConfigurationManager.OpenWebConfiguration("~");
        /// configFileName is any "other" config file besides the default.
        /// For example: MainSettings.config
        /// </summary>
        /// <param name="httpContext">httpContext of calling assembly</param>
        /// <param name="configFileName">name of non-default config file</param>
        public IssueConfiguration(HttpContextBase httpContext, string configFileName)
            : base(httpContext, configFileName)
        {
        }

        /// <summary>
        /// Gets or sets SerializedIssueListFile stored in config file.
        /// </summary>
        public string SerializedIssueListFile
        {
            get
            {
                return this.Get("SerializedIssueListFile");
            }

            set
            {
                this.Save("SerializedIssueListFile", value);
            }
        }
    }
}
