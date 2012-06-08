// -----------------------------------------------------------------------
// <copyright file="RssChannelTest.cs" company="DFBerry">
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
    /// This is a test class for rssChannelTest and is intended
    /// to contain all rssChannelTest Unit Tests
    /// </summary>
    [TestFixture]
    public class RssChannelTest
    {
        /// <summary>
        /// A test for Equals
        /// </summary>
        [Test]
        public void EqualsTest_empty()
        {
            rssChannel target = new rssChannel();
            rssChannel obj = new rssChannel();

            // act
            bool actual = target.Equals(obj);

            // assert
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Test that object's Equal method works 
        /// when two objects are equal (not same).
        /// </summary>
        [Test]
        public void EqualsTest_equal()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();

            rssChannel target = fake.GetrssChannel(5);
            rssChannel obj = fake.GetrssChannel(5);
            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Tests that two objects that contain arrays of different
        /// length are not equal. Uses object's Equal method.
        /// </summary>
        [Test]
        public void EqualsTest_notequallength()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();

            rssChannel target = fake.GetrssChannel(5);
            rssChannel obj = fake.GetrssChannel(4);

            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Only testing one field since all fields should be
        /// used to determine eqaulity
        /// </summary>
        [Test]
        public void EqualsTest_notequalcontent()
        {
            // arrange
            FakeDatasource fake = new FakeDatasource();

            rssChannel target = fake.GetrssChannel(5);
            rssChannel obj = fake.GetrssChannel(5);

            obj.description = "asdfasdfasdf";

            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsFalse(actual);
        }
    }
}
