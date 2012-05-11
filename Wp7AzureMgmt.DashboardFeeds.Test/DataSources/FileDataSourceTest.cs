// -----------------------------------------------------------------------
// <copyright file="FileDataSourceTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Tests for FileDataSource class
    /// </summary>
    [TestFixture]
    public class FileDataSourceTest
    {
        /// <summary>
        /// Build rssFeedArray for testing
        /// </summary>
        private static RssFeed[] rssFeedArray = new RssFeed[] 
        {
            new RssFeed() { ServiceName = "s1", LocationName = "l1", FeedCode = "c1",  RSSLink = "u1" },
            new RssFeed() { ServiceName = "s2", LocationName = "l2", FeedCode = "c2",  RSSLink = "u2" },
            new RssFeed() { ServiceName = "s3", LocationName = "l3", FeedCode = "c3",  RSSLink = "u3" },
        };

        /// <summary>
        /// RssFeeds for test purposes only
        /// </summary>
        private RssFeeds rssFeeds = new RssFeeds
        {
            Feeds = rssFeedArray.Cast<Wp7AzureMgmt.DashboardFeeds.Models.RssFeed>(),
            FeedDate = DateTime.UtcNow
        };
        
        /// <summary>
        /// Tests that string is correctly formatted OPML.
        /// Using static RssFeeds for testing.
        /// </summary>
        [Test]
        public void OPML()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);
            fileDatasource.RssFeeds = this.rssFeeds;

            // act
            string actual = fileDatasource.OPML();

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("<outline text="));
            Assert.IsTrue(actual.Contains("s1"));
            Assert.IsTrue(actual.Contains("l2"));
            Assert.IsTrue(actual.Contains("c3"));
        }

        /// <summary>
        /// Tests that string is correctly formatted OPML.
        /// Using static RssFeeds for testing.
        /// </summary>
        [Test]
        public void OPML_RssFeeds()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);

            // act
            string actual = fileDatasource.OPML(this.rssFeeds);

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("<outline text="));
            Assert.IsTrue(actual.Contains("s1"));
            Assert.IsTrue(actual.Contains("l2"));
            Assert.IsTrue(actual.Contains("c3"));
        }

        /// <summary>
        /// Tests ArgumentNullException.
        /// </summary>
        [Test]
        public void OPML_ArgumentNullException()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);

            // act
            try
            {
                string actual = fileDatasource.OPML(null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Make sure the serialized file exists on disk
        /// with bogus RssFeeds.
        /// </summary>
        [Test]
        public void Set()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);
            string filename = "FileDataSourceTest_Set";
            fileDatasource.FileName = filename;
            fileDatasource.RssFeeds = this.rssFeeds;

            // act
            fileDatasource.Set();

            // assert
            Assert.IsTrue(File.Exists(filename));

            RssFeeds foundRssFeeds = Serializer.Deserialize<RssFeeds>(filename);

            Assert.IsTrue(this.rssFeeds.Equals(foundRssFeeds));

            // cleanup
            File.Delete(filename);
        }

        /// <summary>
        /// Test Set when RssFeeds and Filename are is passed in.
        /// </summary>
        [Test]
        public void Set_RssFeeds_Filename()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);
            string filename = "FileDataSourceTest_Set_RssFeeds_Filename";

            // act
            fileDatasource.Set(this.rssFeeds, filename);

            // assert
            Assert.IsTrue(File.Exists(filename));

            RssFeeds foundRssFeeds = Serializer.Deserialize<RssFeeds>(filename);

            Assert.IsTrue(this.rssFeeds.Equals(foundRssFeeds));

            // cleanup
            File.Delete(filename);
        }

        /// <summary>
        /// Test for set with a null argument
        /// </summary>
        [Test]
        public void Set_ArgumentNullException()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);
            string filename = "FileDataSourceTest_Set_ArgumentNullException";

            // act
            try
            {
                fileDatasource.Set(null, filename);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            // act
            try
            {
                fileDatasource.Set(this.rssFeeds, null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests fetch from serialized file. 
        /// </summary>
        [Test]
        public void Get()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);
            string filename = "FileDataSourceTest_Get";
            fileDatasource.FileName = filename;
            fileDatasource.RssFeeds = this.rssFeeds;
            fileDatasource.Set();

            // act
            RssFeeds actual = fileDatasource.Get();

            // assert
            Assert.IsTrue(actual.Equals(this.rssFeeds));

            // cleanup
            File.Delete(filename);
        }

        /// <summary>
        /// Tests fetch from serialized file. Assumes
        /// appSettings value is correct.
        /// </summary>
        [Test]
        public void Get_Feeds_File()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);
            string filename = "FileDataSourceTest_Get";
            fileDatasource.FileName = filename;
            fileDatasource.RssFeeds = this.rssFeeds;
            fileDatasource.Set();

            // act
            RssFeeds actual = fileDatasource.GetFeeds(filename);

            // assert
            Assert.IsNotNull(actual);

            // cleanup
            File.Delete(filename);
        }

        /// <summary>
        /// Tests fetch from serialized file. Assumes
        /// appSettings value is correct.
        /// </summary>
        [Test]
        public void Get_ArgumentNullException()
        {
            // arrange
            HttpContextBase httpContext = null;
            string pathToFilename = string.Empty;
            FileDatasource fileDatasource = new FileDatasource(pathToFilename, httpContext);

            // act
            try
            {
                fileDatasource.GetFeeds(null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }
    }
}
