// -----------------------------------------------------------------------
// <copyright file="UriDataSourceTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Moq;
    using NUnit.Framework;
    using UnityAutoMoq;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Factories;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    using Wp7AzureMgmt.DashboardFeeds.Utilities;
    
    /// <summary>
    /// Tests for UriDataSource
    /// </summary>
    [TestFixture]
    public class UriDataSourceTest
    {
        /// <summary>
        /// Test for Get
        /// </summary>
        [Test]
        public void Get()
        {
            // arrange
            DashboardHttp httpRequest = null;
            HttpContextBase contextBase = null;
            UriDatasource uriDatasource = new UriDatasource(httpRequest, contextBase);
            
            // Has the entire fake html string read from file
            DashboardHttpFake dashboardHttpFake = new DashboardHttpFake();
            string fakeHtml = dashboardHttpFake.GetRequest();

            // set fake string to response of GetRequest
            var mockDashboardHttp = new Mock<IDashboardHttp>();
            mockDashboardHttp.Setup(f => f.GetRequest()).Returns(fakeHtml);

            uriDatasource.DashboardHttp = mockDashboardHttp.Object;

            // act
            RssFeeds actual = uriDatasource.Get();

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Feeds.Count() == 72);

            // Simple check each field since there are other tests that do
            // deeper checking of RssFeeds
            var foundNSACSEAFeeds = actual.Feeds.Where(f => f.FeedCode == "NSACSEA");
            var foundEastAsiaFeeds = actual.Feeds.Where(f => f.LocationName == "East Asia");
            var foundAccessControl = actual.Feeds.Where(f => f.ServiceName == "Access Control");
            var foundLinks = actual.Feeds.Where(f => f.RSSLink.Contains(@"<a href"));

            // only 1 NSACSEA feed
            Assert.AreEqual(1, foundNSACSEAFeeds.Count());

            // several east asia feeds
            Assert.AreEqual(10, foundEastAsiaFeeds.Count());

            // several access control feeds
            Assert.AreEqual(10, foundEastAsiaFeeds.Count());

            // each/all should have a well-formed url
            Assert.AreEqual(72, foundLinks.Count());
        }

        /// <summary>
        /// Test for GetHtml - makes real call to Azure
        /// </summary>
        [Test]
        public void GetHtml()
        {
            // arrange
            int lastKnownLength = 300000;
            HttpContextBase httpContext = null;

            UriDatasource uriDatasource = new UriDatasource(null, httpContext);
            DashboardConfiguration config = new DashboardConfiguration(null);
            DashboardHttp http = new DashboardHttp(new Uri(config.AzureUri));
            uriDatasource.DashboardHttp = http;

            // act
            string actual = uriDatasource.GetHtml();

            // assert

            // if this length is wrong then the file Azure returns 
            // has changed since the last test
            Assert.IsTrue(lastKnownLength <= actual.Length);
        }

        /// <summary>
        /// Test for Set
        /// </summary>
        [Test]
        public void Set()
        {
            // arrange
            HttpContextBase httpContext = null;
            UriDatasource uriDatasource = new UriDatasource(null, httpContext);

            // act
            try
            {
                uriDatasource.Set();
                Assert.Fail("exception not thrown");
            }
            catch (NotImplementedException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            } 
        }
    }
}
