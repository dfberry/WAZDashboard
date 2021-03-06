﻿// -----------------------------------------------------------------------
// <copyright file="IssueMgrTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using NUnit.Framework;
    using Wp7AzureMgmt.Core;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using Wp7AzureMgmt.DashboardIssues.Utiliites;

    /// <summary>
    /// Tests for IssueMgr.
    /// </summary>
    [TestFixture]
    public class IssueMgrTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssueMgrTest" /> class.
        /// </summary>
        public IssueMgrTest()
        {
            HttpContextBase httpContext = null;
            this.Configuration = new IssueConfiguration(httpContext);
        }

        /// <summary>
        /// Gets or sets IssueConfiguration for App.Config
        /// settings. 
        /// </summary>
        private IssueConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets DashboardHttp object for 
        /// web requests. 
        /// </summary>
        private DashboardHttp DashboardHttp { get; set; }

        /// <summary>
        /// A test for DashboardMgr Constructor
        /// </summary>
        [Test]
        public void DashboardMgrConstructorTest()
        {
            // act
            HttpContextBase context = null;
            IssueMgr target = new IssueMgr(context);

            // assert
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// DFB:TBD fix this - this test hits the live Azure site - 
        /// </summary>
        [Test]
        public void SetRssIssuesFromUriTest()
        {
            // arrange
            Setup.RunBeforeTests_FeedListFile();

            HttpContextBase context = null;
            string pathToFiles = Setup.GetDataPath();
            IssueMgr issueMgr = new IssueMgr(context);
            IssueConfiguration issueConfig = new IssueConfiguration(context);

            if (File.Exists(pathToFiles + issueConfig.SerializedIssueListFile))
            {
                File.Delete(pathToFiles + issueConfig.SerializedIssueListFile);
            }

            // act
            issueMgr.SetRssIssuesFromUri(pathToFiles);

            // assert
            Assert.IsTrue(File.Exists(pathToFiles + issueConfig.SerializedIssueListFile));

            // cleanup
            File.Delete(pathToFiles + issueConfig.SerializedIssueListFile);
        }

        /// <summary>
        /// A test for GetStoredRssIssues 
        /// DFB:TBD fix this - this test hits the live Azure site - 
        /// </summary>
        [Test]
        public void GetStoredRssIssuesTest()
        {
            // arrange
            HttpContextBase context = null; // TODO: Initialize to an appropriate value
            IssueMgr target = new IssueMgr(context); // TODO: Initialize to an appropriate value
            string pathToFiles = Setup.GetDataPath();

            if (!File.Exists(pathToFiles + "IssueFileDatasource"))
            {
                Setup.RunBeforeTests_IssueListFile();
            }

            RssIssues actual;

            // act
            actual = target.GetStoredRssIssues(pathToFiles);

            // assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Issues);

            // DFB:TBD - this is not a great test - need to figure out what
            // the lower level tests aren't going to catch and then make sure this one does
            // example: datetime diffs

            // cleanup
            File.Exists(pathToFiles + "IssueFileDatasource");
        }
    }
}