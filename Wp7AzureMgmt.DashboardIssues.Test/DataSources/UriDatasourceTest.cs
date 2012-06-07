using Wp7AzureMgmt.DashboardIssues.DataSources;
using NUnit.Framework;
using System;
using Wp7AzureMgmt.Core;
using System.Web;
using Wp7AzureMgmt.DashboardIssues.Models;
using Wp7AzureMgmt.DashboardFeeds.Models;
using Wp7AzureMgmt.Core.Interfaces;
using System.Linq;

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
            // arrange
            DashboardHttp http = null; 
            HttpContextBase httpContext = null;
            string pathToFeedsFileSource = string.Empty; Setup.GetDataPath();

            // act
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource);

            // assert
            Assert.IsNotNull(target);
            Assert.IsNull(target.DashboardHttp);
            Assert.IsNull(target.DashboardUri);
            Assert.IsNull(target.RssIssues);
            Assert.IsNotNull(target.SerializedFilename);
            Assert.That(target.SerializedFilename.Contains("IssueFileDatasource"));
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
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Issues);
            Assert.IsTrue((actual.RetrievalDate - DateTime.UtcNow).Days < 1);
            Assert.GreaterOrEqual(actual.Issues.ToList().Count, 70);
        }

        /// <summary>
        ///A test for Set
        ///</summary>
        [Test]
        public void SetTest()
        {
            // arrange
            DashboardHttp http = null; 
            HttpContextBase httpContext = null; 
            string pathToFeedsFileSource = string.Empty; 
            UriDatasource target = new UriDatasource(http, httpContext, pathToFeedsFileSource);

            // act
            try
            {
                target.Set();
                Assert.Fail("exception not thrown");
            }
            catch (NotImplementedException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            } 
            
        }
    }
}
