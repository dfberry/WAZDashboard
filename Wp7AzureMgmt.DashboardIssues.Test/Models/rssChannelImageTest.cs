using Wp7AzureMgmt.DashboardIssues.Models;
using NUnit.Framework;
using System;
using System.Reflection;
using Wp7AzureMgmt.DashboardIssues.DataSources;

namespace Wp7AzureMgmt.DashboardIssues.Test
{
    
    
    /// <summary>
    ///This is a test class for rssChannelImageTest and is intended
    ///to contain all rssChannelImageTest Unit Tests
    ///</summary>
    [TestFixture]
    public class rssChannelImageTest
    {
        /// <summary>
        ///A test for Equals
        ///</summary>
        [Test]
        public void EqualsTest_empty()
        {
            rssChannelImage target = new rssChannelImage();
            rssChannelImage obj = new rssChannelImage();

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

            rssChannelImage target = fake.GetrssChannelImage(5);
            rssChannelImage obj = fake.GetrssChannelImage(5);
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

            rssChannelImage target = fake.GetrssChannelImage(5);
            rssChannelImage obj = fake.GetrssChannelImage(4);

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

            rssChannelImage target = fake.GetrssChannelImage(5);
            rssChannelImage obj = fake.GetrssChannelImage(5);

            obj.title = "asdfasdfasdf";

            bool actual;

            // act
            actual = target.Equals(obj);

            // assert
            Assert.IsFalse(actual);
        }
    }
}
