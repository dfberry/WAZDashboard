// -----------------------------------------------------------------------
// <copyright file="RssChannelImageTest.cs" company="DFBerry">
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
    /// This is a test class for rssChannelImageTest and is intended
    /// to contain all rssChannelImageTest Unit Tests
    /// </summary>
    [TestFixture]
    public class RssChannelImageTest
    {
        /// <summary>
        /// A test for Equals
        /// </summary>
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

        /// <summary>
        /// Tests if two just-created objects are the equal (not the same).
        /// </summary>
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

        /// <summary>
        /// Expect two objects created with different lengths to 
        /// not be equal. Tests object's Equal method.
        /// </summary>
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
