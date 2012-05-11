// -----------------------------------------------------------------------
// <copyright file="HTMLParserFeedItemTest.cs" company="DFBerry">
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
    /// Tests for HTMLParserFeedItem object. 
    /// </summary>
    [TestFixture]
    public class HTMLParserFeedItemTest
    {
        /// <summary>
        /// Test for HashCode must return same hash for two
        /// different objects as long as the name is the same.
        /// </summary>
        [Test]
        public void GetHashCode_Success()
        {
            // arrange
            HTMLParserFeedItem diversion = new HTMLParserFeedItem();
            HTMLParserFeedItemType sameName = HTMLParserFeedItemType.RSSLink;

            // make sure other properties of object don't affect hash
            // also make sure order properties are set doesn't affect hash
            diversion.Value = "this is only a test";
            diversion.Name = sameName;
            int diversionHash = diversion.GetHashCode();

            HTMLParserFeedItem target = new HTMLParserFeedItem();
            target.Name = sameName;

            // act
            int actual = target.GetHashCode();

            // assert
            Assert.AreEqual(diversionHash, actual);
        }
    }
}
