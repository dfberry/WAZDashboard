// -----------------------------------------------------------------------
// <copyright file="HtmlParserFeedItemDefinitionTest.cs" company="DFBerry">
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
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Models;
    
    /// <summary>
    /// Test for HtmlParserFeedItemDefinition
    /// </summary>
    [TestFixture]
    public class HtmlParserFeedItemDefinitionTest
    {
        /// <summary>
        /// Tet for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCode_Success()
        {
            // arrange
            HTMLParserFeedItemDefinition definition = new HTMLParserFeedItemDefinition();

            definition.Tag = "GetHashCode_Success";
            definition.AttributeName = "AttributeName";

            int expected = definition.Tag.GetHashCode() + definition.AttributeName.GetHashCode();

            // act
            int actual = definition.GetHashCode();

            // assert
            Assert.IsTrue(expected == actual);
        }

        /// <summary>
        /// Test for GetHashCode with partial information missing
        /// </summary>
        [Test]
        public void GetHashCode_JustTag()
        {
            // arrange
            HTMLParserFeedItemDefinition definition = new HTMLParserFeedItemDefinition();
            definition.Tag = "GetHashCode_JustTag";
            int expected = definition.Tag.GetHashCode() + string.Empty.GetHashCode();

            // act
            int actual = definition.GetHashCode();

            // assert
            Assert.IsTrue(expected == actual);
        }

        /// <summary>
        /// Test for GetHashCode with partial information missing
        /// </summary>
        [Test]
        public void GetHashCode_JustAttributeName()
        {
            // arrange
            HTMLParserFeedItemDefinition definition = new HTMLParserFeedItemDefinition();
            definition.AttributeName = "AttributeName";
            int expected = definition.AttributeName.GetHashCode() + string.Empty.GetHashCode();

            // act
            int actual = definition.GetHashCode();

            // assert
            Assert.IsTrue(expected == actual);
        }     

        /// <summary>
        /// Test for object equality 
        /// </summary>
        [Test]
        public void Equals_Success()
        {
            // arrange
            HTMLParserFeedItemDefinition definition = new HTMLParserFeedItemDefinition();
            HTMLParserFeedItemDefinition duplicate = new HTMLParserFeedItemDefinition(); 
            string tag = "Equals_Success";
            string attributeName = "Attribute Name";

            definition.Tag = tag;
            definition.AttributeName = attributeName;

            duplicate.Tag = tag.ToUpper();
            duplicate.AttributeName = attributeName.ToUpper();

            // act
            bool actual = definition.Equals(duplicate);

            // assert
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Test for object equality where the two objects
        /// are of different types
        /// </summary>
        [Test]
        public void Equals_Failure_ObjectType()
        {
            // arrange
            HTMLParserFeedItemDefinition definition = new HTMLParserFeedItemDefinition();
            object failureObject = new object();

            // act
            bool actual = definition.Equals(failureObject);

            // assert
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Test for equality when objects are not equal
        /// </summary>
        [Test]
        public void Equals_Failure()
        {
            // arrange
            HTMLParserFeedItemDefinition definition = new HTMLParserFeedItemDefinition();
            HTMLParserFeedItemDefinition duplicate = new HTMLParserFeedItemDefinition();
            string tag = "Equals_Failure";
            string attributeName = "Attribute Equals_Failure";

            definition.Tag = tag + "1";
            definition.AttributeName = attributeName + "1";

            duplicate.Tag = tag.ToUpper();
            duplicate.AttributeName = attributeName.ToUpper();

            // act
            bool actual = definition.Equals(duplicate);

            // assert
            Assert.IsFalse(actual);
        }
    }
}
