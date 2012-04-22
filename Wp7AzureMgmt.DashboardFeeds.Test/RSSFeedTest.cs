// -----------------------------------------------------------------------
// <copyright file="RSSFeedTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Factories;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Tests for RSSFeed object
    /// </summary>
    [TestFixture]
    public class RSSFeedTest
    {
        /// <summary>
        /// Test for HashCode must return same hash for two
        /// different objects as long as the feedcode is the same.
        /// </summary>
        [Test]
        public void GetHashCode_Success()
        {
            // arrange
            string feedcode = "zY1%&njasdf_";

            // make sure other properties of object don't affect hash
            // also make sure order properties are set doesn't affect hash
            RSSFeed diversion = new RSSFeed();
            diversion.ServiceName = "test service name";
            diversion.LocationName = "test location name";
            diversion.RSSLink = "test rss link";
            diversion.FeedCode = feedcode;
            int diversionHash = diversion.GetHashCode();

            // only setting feedcode - other properties are empty
            RSSFeed target = new RSSFeed();
            target.FeedCode = feedcode;

            // act
            int actual = target.GetHashCode();

            // assert
            Assert.AreEqual(diversionHash, actual);
        }

        /// <summary>
        /// Test for equality - only feedcode matters
        /// </summary>
        [Test]
        public void Equals_Success()
        {
            // arrange
            string feedcode = "zY1%&njasdf_";

            // make sure other properties of object don't affect hash
            // also make sure order properties are set doesn't affect hash
            RSSFeed diversion = new RSSFeed();
            diversion.ServiceName = "test service name";
            diversion.LocationName = "test location name";
            diversion.RSSLink = "test rss link";
            diversion.FeedCode = feedcode;

            // only setting feedcode - other properties are empty
            RSSFeed target = new RSSFeed();
            target.FeedCode = feedcode;

            // act
            bool actual = target.Equals(diversion);

            // assert
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Test for failure of equality - only feedcode matters
        /// </summary>
        [Test]
        public void Equals_Failure()
        {
            // arrange
            RSSFeed diversion = new RSSFeed();
            diversion.FeedCode = "diversion";

            // only setting feedcode - other properties are empty
            RSSFeed target = new RSSFeed();
            target.FeedCode = "target";

            // act
            bool actual = target.Equals(diversion);

            // assert
            Assert.IsFalse(actual);
        }
    }
}
