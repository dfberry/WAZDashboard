using Wp7AzureMgmt.DashboardIssues.DataSources;
using NUnit.Framework;
using System;
using Wp7AzureMgmt.Core;
using System.Web;
using Wp7AzureMgmt.DashboardIssues.Models;
using Wp7AzureMgmt.DashboardFeeds.Models;
using Wp7AzureMgmt.Core.Interfaces;

namespace Wp7AzureMgmt.DashboardIssues.Test
{
    
    
    /// <summary>
    ///This is a test class for UriDatasourceTest and is intended
    ///to contain all UriDatasourceTest Unit Tests
    ///</summary>
    [TestFixture]
    public class UriDatasourceTest
    {
        /// <summary>
        ///A test for UriDatasource Constructor
        ///</summary>
        [Test]
        public void UriDatasourceConstructorTest()
        {
            DashboardHttp http = null; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            string pathToFeedsFileSource = string.Empty; // TODO: Initialize to an appropriate value
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [Test]
        public void GetTest()
        {
            // arrange
            DashboardHttp http = null; 
            HttpContextBase httpContext = null; 
            string pathToFeedsFileSource = Setup.GetDataPath(); 
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource); 
            RssIssues expected = null; 
            RssIssues actual;

            // act
            actual = target.Get();

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Set
        ///</summary>
        [Test]
        public void SetTest()
        {
            DashboardHttp http = null; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            string pathToFeedsFileSource = string.Empty; // TODO: Initialize to an appropriate value
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource); // TODO: Initialize to an appropriate value
            target.Set();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DashboardHttp
        ///</summary>
        [Test]
        public void DashboardHttpTest()
        {
            DashboardHttp http = null; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            string pathToFeedsFileSource = string.Empty; // TODO: Initialize to an appropriate value
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource); // TODO: Initialize to an appropriate value
            IDashboardHttp expected = null; // TODO: Initialize to an appropriate value
            IDashboardHttp actual;
            target.DashboardHttp = expected;
            actual = target.DashboardHttp;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DashboardUri
        ///</summary>
        [Test]
        public void DashboardUriTest()
        {
            DashboardHttp http = null; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            string pathToFeedsFileSource = string.Empty; // TODO: Initialize to an appropriate value
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource); // TODO: Initialize to an appropriate value
            Uri expected = null; // TODO: Initialize to an appropriate value
            Uri actual;
            target.DashboardUri = expected;
            actual = target.DashboardUri;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RssIssues
        ///</summary>
        [Test]
        public void RssIssuesTest()
        {
            DashboardHttp http = null; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            string pathToFeedsFileSource = string.Empty; // TODO: Initialize to an appropriate value
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource); // TODO: Initialize to an appropriate value
            RssIssues expected = null; // TODO: Initialize to an appropriate value
            RssIssues actual;
            target.RssIssues = expected;
            actual = target.RssIssues;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SerializedFilename
        ///</summary>
        [Test]
        public void SerializedFilenameTest()
        {
            DashboardHttp http = null; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            string pathToFeedsFileSource = string.Empty; // TODO: Initialize to an appropriate value
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SerializedFilename = expected;
            actual = target.SerializedFilename;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
