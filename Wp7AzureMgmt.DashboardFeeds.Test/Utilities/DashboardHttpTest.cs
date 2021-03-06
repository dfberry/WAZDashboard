﻿// -----------------------------------------------------------------------
// <copyright file="DashboardHttpTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;
    using NUnit.Framework;
    using Wp7AzureMgmt.Core;
    using Wp7AzureMgmt.DashboardFeeds;

    /// <summary>
    /// This is a test class for HTTPTest and is intended
    /// to contain all HTTPTest Unit Tests
    /// </summary>
    [TestFixture]
    public class DashboardHttpTest 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardHttpTest" /> class.
        /// </summary>
        public DashboardHttpTest()
        {
            HttpContextBase context = null;

            this.DashboardConfiguration = new FeedConfiguration(context);
            this.DashboardHttp = new DashboardHttp(new Uri(this.DashboardConfiguration.DefaultUri));
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
        private FeedConfiguration DashboardConfiguration { get; set; }

        /// <summary>
        /// A test for HTTP Constructor
        /// </summary>
        [Test]public void DashboardHttpConstructorTest_Success()
        {
            // arrange
            FeedConfiguration dashboardConfiguration = new FeedConfiguration(null);
            Uri getUri = new Uri(dashboardConfiguration.DefaultUri);

            // act
            DashboardHttp target = new DashboardHttp(getUri);

            // assert
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for RequestGET
        /// 
        /// this test just makes sure an Http Get returns an Html string successfully
        /// 
        /// using a random url 
        /// 
        /// </summary>
        [Test] public void RequestGETTest()
        {
            // arrange
            HttpContextBase context = null;

            FeedConfiguration dashboardConfiguration = new FeedConfiguration(context);
            Uri getUri = new Uri(@"http://www.microsoft.com");
            DashboardHttp target = new DashboardHttp(getUri);
            string actual = string.Empty; 

            // act
            actual = target.GetRequest();

            // assert
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// A test for HttpWebRequest
        /// </summary>
        [Test] public void HttpWebRequestTest()
        {
            // arrange
            Uri getUri = new Uri(this.DashboardConfiguration.DefaultUri);
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

            // this number will be wrong if the file used is different, 
            // ie, there are more, less, or different Rss feeds
            // at Windows Azure
            int fileLength = 14;
                        
            Uri getUri = new Uri(this.DashboardConfiguration.DefaultUri);
            DashboardHttp target = new DashboardHttp(getUri);
            target.responseContent = "this is a test";
            
            string filename = DateTime.Now.Ticks + ".html";

            // act
            filename = target.SaveResponseContentToFile(filename);

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(filename));

            FileInfo info = new FileInfo(filename);

            Assert.IsTrue(info.Exists);
            Assert.AreEqual(info.Length, fileLength);

            // cleanup
            File.Delete(filename);
        }
    }
}
