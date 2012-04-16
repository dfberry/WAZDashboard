// -----------------------------------------------------------------------
// <copyright file="C_DashboardMgrTest.cs" company="Microsoft">
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
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    
    /// <summary>
    /// This is a test class for DashboardMgrTest and is intended
    /// to contain all DashboardMgrTest Unit Tests
    /// </summary>
    [TestFixture]
    public class C_DashboardMgrTest 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="C_DashboardMgrTest" /> class.
        /// </summary>
        public C_DashboardMgrTest()
        {
            DashboardConfiguration = new DashboardConfiguration();
            DashboardHttp = new DashboardHttp(new Uri(DashboardConfiguration.GetAzureUri));
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
        private DashboardConfiguration DashboardConfiguration { get; set; }

        /// <summary>
        /// A test for DashboardMgr Constructor
        /// </summary>
        [Test]
        public void DashboardMgrConstructorTest()
        {
            // act
            DashboardMgr target = new DashboardMgr(DashboardConfiguration.GetAzureUri);

            // assert
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for AzureDashboardLocation
        /// </summary>
        [Test]
        public void AzureDashboardLocationTest()
        {
            // arrange
            string expected = DashboardConfiguration.GetAzureUri;
            string actual;
            DashboardMgr target = new DashboardMgr(DashboardConfiguration.GetAzureUri);

            // act
            actual = target.AzureDashboardLocation();

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for AzureDashboardLocation
        /// </summary>
        [Test]
        public void AzureDashboardLocationNullParamTest()
        {
            // arrange
            string expected = DashboardConfiguration.GetAzureUri; 
            DashboardMgr target = new DashboardMgr();
            string actual;

            // act
            actual = target.AzureDashboardLocation();

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for FeedDate
        /// </summary>
        [Test]
        public void FeedDateTest()
        {
            // arrange
            string azureDashboardServiceURI = DashboardConfiguration.GetAzureUri; 
            DateTime expected = DateTime.Now; 
            DateTime actual;
            DashboardMgr target = new DashboardMgr(azureDashboardServiceURI);
            List<RSSFeed> list = target.Feeds().ToList();

            // act
            actual = target.FeedDate();

            // assert
            Assert.IsNotNull(actual);
            Assert.GreaterOrEqual(actual.Ticks, expected.Ticks);
        }

        /// <summary>
        /// A test for FeedDate
        /// </summary>
        [Test]
        public void FeedDateNullTest()
        {
            // arrange
            string azureDashboardServiceURI = DashboardConfiguration.GetAzureUri;
            DateTime actual;
            DashboardMgr target = new DashboardMgr(azureDashboardServiceURI);

            // act
            actual = target.FeedDate();

            // assert
            Assert.IsNotNull(actual);
            Assert.GreaterOrEqual(actual, DateTime.MinValue);
        }

        /// <summary>
        /// A test for Feeds
        /// </summary>
        [Test]
        public void FeedsTest()
        {
            // arrange
            string azureDashboardServiceURI = DashboardConfiguration.GetAzureUri;
            DateTime buildDate = DateTime.Now;
            DashboardMgr target = new DashboardMgr(azureDashboardServiceURI); 
            bool forceRebuild = false; 

            // act
            List<RSSFeed> actual = target.Feeds(forceRebuild).ToList();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(DashboardConfiguration.GetFeedCount, actual.Count);
            Assert.Greater(target.FeedDate().Ticks, buildDate.Ticks);
        }

        /// <summary>
        /// A test for Feeds
        /// </summary>
        [Test]
        public void FeedsTestForceRebuild()
        {
            // arrange
            string azureDashboardServiceURI = DashboardConfiguration.GetAzureUri;
            DashboardMgr target = new DashboardMgr(azureDashboardServiceURI);
            bool forceRebuild = true;
            List<RSSFeed> actual1 = target.Feeds(forceRebuild).ToList();
            DateTime buildDate1 = target.FeedDate();

            // act
            List<RSSFeed> actual2 = target.Feeds(forceRebuild).ToList();
            DateTime buildDate2 = target.FeedDate();

            // assert
            Assert.IsNotNull(actual2);
            Assert.AreEqual(DashboardConfiguration.GetFeedCount, actual1.Count);
            Assert.Greater(buildDate2.Ticks, buildDate1.Ticks); 
        }

        /// <summary>
        /// A test for OPML
        /// </summary>
        [Test]
        public void OPMLTest()
        {
            // arrange
            string azureDashboardServiceURI = DashboardConfiguration.GetAzureUri;
            DashboardMgr target = new DashboardMgr(azureDashboardServiceURI);

            // act
            string actual = target.OPML();

            // assert
            Assert.IsNotNull(actual);

            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            MatchCollection matches = Regex.Matches(actual, "<outline", options);

            Assert.IsTrue(actual.Contains("<outline"));
            Assert.GreaterOrEqual(matches.Count, DashboardConfiguration.GetFeedCount);
        }
    }
}
