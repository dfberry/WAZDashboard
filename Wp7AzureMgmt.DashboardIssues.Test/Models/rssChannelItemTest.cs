
namespace Wp7AzureMgmt.DashboardIssues.Test
{
    using Wp7AzureMgmt.DashboardIssues.Models;
    using System;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardIssues.DataSources;
    using System.Reflection;

    
    
    /// <summary>
    ///This is a test class for rssChannelItemTest and is intended
    ///to contain all rssChannelItemTest Unit Tests
    ///</summary>
    [TestFixture]
    public class rssChannelItemTest
    {
        /// <summary>
        ///A test for Equals
        ///</summary>
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
        /// Only testing one field since all fields should be
        /// used to determine eqaulity
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
