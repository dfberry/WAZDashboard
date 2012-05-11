// -----------------------------------------------------------------------
// <copyright file="DashboardMgrTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// This is a test class for DashboardMgrTest and is intended
    /// to contain all DashboardMgrTest Unit Tests
    /// </summary>
    [TestFixture]
    public class DashboardMgrTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardMgrTest" /> class.
        /// </summary>
        public DashboardMgrTest()
        {
            HttpContextBase httpContext = null;
            this.Wp7AzureMgmtConfiguration = new DashboardConfiguration(httpContext);
            this.DashboardHttp = new DashboardHttp(new Uri(this.Wp7AzureMgmtConfiguration.AzureUri));
        }

        /// <summary>
        /// Gets or sets DashboardHttp object for 
        /// web requests. 
        /// </summary>
        private DashboardHttp DashboardHttp { get; set; }

        /// <summary>
        /// Gets or sets DashboardConfiguration for App.Config
        /// settings. 
        /// </summary>
        private DashboardConfiguration Wp7AzureMgmtConfiguration { get; set; }

        /// <summary>
        /// A test for DashboardMgr Constructor
        /// </summary>
        [Test]
        public void DashboardMgrConstructorTest()
        {
            // act
            HttpContextBase context = null;
            DashboardMgr target = new DashboardMgr(context);

            // assert
            Assert.IsNotNull(target);
        }
    }
}
