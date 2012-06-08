using Wp7AzureMgmt.DashboardIssues.Models;
using System;
using NUnit.Framework;
using Wp7AzureMgmt.DashboardIssues.DataSources;
using System.Reflection;


namespace Wp7AzureMgmt.DashboardIssues.Test
{
    
    
    /// <summary>
    ///This is a test class for rssChannelTest and is intended
    ///to contain all rssChannelTest Unit Tests
    ///</summary>
    [TestFixture]
    public class rssChannelTest
    {
        /// <summary>
        ///A test for Equals
        ///</summary>
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
