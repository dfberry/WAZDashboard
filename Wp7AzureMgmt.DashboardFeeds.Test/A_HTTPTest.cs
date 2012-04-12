// -----------------------------------------------------------------------
// <copyright file="A_HTTPTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.IO;
    using System.Net;
    using NUnit.Framework;
    using Wp7AzureMgmt.Dashboard;
    
    /// <summary>
    /// This is a test class for HTTPTest and is intended
    /// to contain all HTTPTest Unit Tests
    /// </summary>
    [TestFixture] 
    public class A_HTTPTest
    {
        /// <summary>
        /// A test for HTTP Constructor
        /// </summary>
        [Test] public void HTTPConstructorTest_Success()
        {
            // arrange
            Uri getUri = new Uri(DashboardTestUtilities.GrabDefaultUriFromConfig()); 

            // act
            HTTP target = new HTTP(getUri);

            // assert
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for RequestGET
        /// </summary>
        [Test] public void RequestGETTest()
        {
            // arrange
            Uri getUri = new Uri(DashboardTestUtilities.GrabDefaultUriFromConfig());
            HTTP target = new HTTP(getUri); 
            string actual = string.Empty; 

            // act
            actual = target.RequestGET();

            // assert
            Assert.IsNotNullOrEmpty(actual);
        }

        /// <summary>
        /// A test for HttpWebRequest
        /// </summary>
        [Test] public void HttpWebRequestTest()
        {
            // arrange
            Uri getUri = new Uri(DashboardTestUtilities.GrabDefaultUriFromConfig());
            HTTP http = new HTTP(getUri); 

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
            Uri getUri = new Uri(DashboardTestUtilities.GrabDefaultUriFromConfig());
            HTTP target = new HTTP(getUri);
            string filename = DateTime.Now.Ticks + ".html";

            string responsecontent = target.RequestGET();

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
