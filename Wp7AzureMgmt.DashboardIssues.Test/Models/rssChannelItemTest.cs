// -----------------------------------------------------------------------
// <copyright file="RssChannelItemTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardIssues.Test
{
    using System;
    using System.Reflection;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardIssues.DataSources;
    using Wp7AzureMgmt.DashboardIssues.Models;

    /// <summary>
    /// This is a test class for rssChannelItemTest and is intended
    /// to contain all rssChannelItemTest Unit Tests
    /// </summary>
    [TestFixture]
    public class RssChannelItemTest
    {
        /// <summary>
        /// Tests that two just-created objects are equal (not same).
        /// Uses object's Equal method.
        /// </summary>
        [Test]
        public void EqualsTest_empty()
        {
            rssChannelItem target = new rssChannelItem();
            rssChannelItem obj = new rssChannelItem();

            // act
            bool actual = target.Equals(obj);

            // assert
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Tests that two just-created objects of same length are equal.
        /// Uses object's Equal method.
        /// </summary>
        [Test]
        public void EqualsTest_equal()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();

            rssChannelItem target = fake.GetrssChannelItem(5);
            rssChannelItem obj = fake.GetrssChannelItem(5);
            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Tests that two object created with different contained array lengths
        /// are not equal.
        /// Uses object's Equal method.
        /// </summary>
        [Test]
        public void EqualsTest_notequallength()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();

            rssChannelItem target = fake.GetrssChannelItem(5);
            rssChannelItem obj = fake.GetrssChannelItem(4);

            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Tests that two objects are not equal if one property value 
        /// is different.
        /// Only testing one field since all fields should be
        /// used to determine eqaulity.
        /// Uses object's Equal method.
        /// </summary>
        [Test]
        public void EqualsTest_notequalcontent()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();

            rssChannelItem target = fake.GetrssChannelItem(5);
            rssChannelItem obj = fake.GetrssChannelItem(5);

            obj.status = "asdfasdfasdf";

            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsFalse(actual);
        }
    }
}
