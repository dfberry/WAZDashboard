// -----------------------------------------------------------------------
// <copyright file="DashboardMgr.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using System.Threading;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DashboardMgr 
    {
        /// <summary>
        /// Current list of RSS feeds
        /// </summary>
        private IEnumerable<RSSFeed> feedList;

        /// <summary>
        /// Uri to Windows Azure Dashboard RSS list
        /// </summary>
        private string azureDashboardURI;

        /// <summary>
        /// Date feedlist is built
        /// </summary>
        private DateTime buildDate;

        /// <summary>
        /// Datasource of feeds following IRSSDataSource rules
        /// </summary>
        private IRSSDataSource datasource; 

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardMgr" /> class.
        /// If AzureDashboardRSSFeedURI is null, last known good URI used. 
        /// </summary>
        /// <param name="azureDashboardServiceURI">string AzureDashboardRSSFeedURI optional</param>
        public DashboardMgr(string azureDashboardServiceURI = null)
        {
            if (azureDashboardServiceURI == null)
            {
                this.azureDashboardURI = ConfigurationManager.AppSettings["AzureDashboardServiceURL"];
                if (string.IsNullOrEmpty(this.azureDashboardURI))
                {
                    throw new Exception("AzureDashboardServiceURL string is null");
                }
            }
            else
            {
                this.azureDashboardURI = azureDashboardServiceURI;
            }

            this.datasource = new Dashboard(new Uri(this.azureDashboardURI));
            this.feedList = new List<RSSFeed>();
            this.buildDate = DateTime.MinValue;
        }

        /// <summary>
        /// If the param is null, code uses last known good URI when library was built
        /// </summary>
        /// <returns>string of OPML text to save as OPML file, to import into RSS reader</returns>
        public string OPML()
        {
            if (this.feedList == null)
            {
                this.feedList = this.Feeds();
            }

            return this.datasource.OPML();
        }

        /// <summary>
        /// Building or rebuilding the list means another trip across the wire to Azure.
        /// </summary>
        /// <param name="forceRebuild">If list exists but param==true, rebuild list.</param>
        /// <returns>IEnumerable of RSSFeed</returns>
        public IEnumerable<RSSFeed> Feeds(bool forceRebuild = false)
        {
            if ((this.feedList == null) || (this.feedList.Count() == 0) || (forceRebuild == true))
            {
                this.feedList = this.datasource.List();

                // DFB TBD: Fix this - need better way to verify forceRebuid in tests
                // wait 1 second before setting field
                Thread.Sleep(1000);

                this.buildDate = DateTime.Now;
            }

            return this.feedList;
        }

        /// <summary>
        /// Date of generation of Feed list
        /// </summary>
        /// <returns>DateTime of Feed list generation</returns>
        public DateTime FeedDate()
        {
            return this.buildDate;
        }

        /// <summary>
        /// Uri of Windows Azure Dashboard feeds to pull content from
        /// </summary>
        /// <returns>string of WA Uri</returns>
        public string AzureDashboardLocation()
        {
            return this.azureDashboardURI;
        }
    }
}