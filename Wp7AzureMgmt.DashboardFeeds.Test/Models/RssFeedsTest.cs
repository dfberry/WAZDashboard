// -----------------------------------------------------------------------
// <copyright file="RssFeedsTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    
    /// <summary>
    /// RssFeeds Model Test
    /// </summary>
    [TestFixture]
    public class RssFeedsTest
    {
        /// <summary>
        /// Test for Opml creation from RssFeeds listing
        /// </summary>
        [Test]
        public void OPML()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();
            RssFeeds fakeFeeds = fake.RssFeeds;

            // act
            string actual = fakeFeeds.OPML();

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("<outline text="));
            Assert.IsTrue(actual.Contains("s1"));
            Assert.IsTrue(actual.Contains("l2"));
            Assert.IsTrue(actual.Contains("c3"));
        }
    }
}
