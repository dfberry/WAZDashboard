// -----------------------------------------------------------------------
// <copyright file="A_DashboardHttpTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.IO;
    using System.Net;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds;
    
    /// <summary>
    /// This is a test class for HTTPTest and is intended
    /// to contain all HTTPTest Unit Tests
    /// </summary>
    [TestFixture]
    public class A_DashboardHttpTest 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="A_DashboardHttpTest" /> class.
        /// </summary>
        public A_DashboardHttpTest()
        {
            DashboardConfiguration = new DashboardConfiguration();
            DashboardHttp = new DashboardHttp(new Uri(DashboardConfiguration.GetDefaultUri));
        }

        /// <summary>
        /// Gets or sets DashboardHttp object for 
        /// web requests. 
        /// </summary>
        private DashboardHttp DashboardHttp { get; set; }

        /// <summary>
        /// Gets or sets dashboardConfiguration for App.Config
        /// settings. 
        /// </summary>
        private DashboardConfiguration DashboardConfiguration { get; set; }

        /// <summary>
        /// A test for HTTP Constructor
        /// </summary>
        [Test]public void DashboardHttpConstructorTest_Success()
        {
            // arrange
            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Uri getUri = new Uri(dashboardConfiguration.GetDefaultUri);

            // act
            DashboardHttp target = new DashboardHttp(getUri);

            // assert
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for RequestGET
        /// </summary>
        [Test] public void RequestGETTest()
        {
            // arrange
            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Uri getUri = new Uri(dashboardConfiguration.GetDefaultUri);
            DashboardHttp target = new DashboardHttp(getUri);
            string actual = string.Empty; 

            // act
            actual = target.GetRequest();

            // assert
            Assert.IsNotNullOrEmpty(actual);
        }

        /// <summary>
        /// A test for HttpWebRequest
        /// </summary>
        [Test] public void HttpWebRequestTest()
        {
            // arrange
            Uri getUri = new Uri(DashboardConfiguration.GetDefaultUri);
            DashboardHttp http = new DashboardHttp(getUri); 

            // act
            HttpWebRequest target = http.HttpWebRequest;

            // assert
            Assert.AreEqual(getUri, target.RequestUri);
            Assert.AreEqual("GET", target.Method);
            Assert.AreEqual("text/html", target.ContentType);
        }

        /// <summary>
        /// A test for SaveToFile.
        /// File exists and has a size > 0
        /// </summary>
        [Test] public void SaveResponseContentToFileTest()
        {
            // arrange
            Uri getUri = new Uri(DashboardConfiguration.GetDefaultUri);
            DashboardHttp target = new DashboardHttp(getUri); 
            string filename = DateTime.Now.Ticks + ".html";

            string responsecontent = target.GetRequest();

            // act
            filename = target.SaveResponseContentToFile(filename);

            // assert
            Assert.IsNotNullOrEmpty(filename);

            FileInfo info = new FileInfo(filename);

            Assert.IsTrue(info.Exists);
            Assert.GreaterOrEqual(info.Length, 1);
        }
    }
}
