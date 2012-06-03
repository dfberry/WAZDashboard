// -----------------------------------------------------------------------
// <copyright file="SerializerTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Wp7AzureMgmt.Core;

    /// <summary>
    /// Tests the Serializer class
    /// </summary>
    [TestFixture]
    public class SerializerTest
    {
        /// <summary>
        /// Serialize using valid filename and object
        /// </summary>
        [Test]
        public void Serialize_Success()
        {
            // arrange
            string filename = "Serialize_Success.txt";
            string objectToSerialize = "This is a test of the Emergency Broadcast System. This is only a test.";

            // act
            Serializer.Serialize(filename, objectToSerialize);

            // assert
            Assert.IsTrue(File.Exists(filename));

            // cleanup
            File.Delete(filename);
        }

        /// <summary>
        /// Serialize - argument 1 is the filename
        /// </summary>
        [Test]
        public void Serialize_Failure_Arg1Null()
        {
            string filename = null;
            string objectToSerialize = "This is a test of the Emergency Broadcast System. This is only a test.";

            // act
            try
            {
                Serializer.Serialize(filename, objectToSerialize);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Argument 2 is the object to serialize
        /// </summary>
        [Test]
        public void Serialize_Failure_Arg2Null()
        {
            string filename = "Serialize_Failure_Arg2Null.txt";
            string objectToSerialize = null;

            // act
            try
            {
                Serializer.Serialize(filename, objectToSerialize);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Deserialize using valid filename 
        /// </summary>
        [Test]
        public void Deserialize_Success()
        {
            // arrange
            string filename = "Deserialize_Success.txt";
            string objectToSerialize = "This is a test of the Emergency Broadcast System. This is only a test.";
            Serializer.Serialize(filename, objectToSerialize);

            // act
            string actual = Serializer.Deserialize<string>(filename);

            // assert
            Assert.AreEqual(objectToSerialize, actual);

            // cleanup
            File.Delete(filename);
        }

        /// <summary>
        /// Deserialize - arg 1 is the filename
        /// </summary>
        [Test]
        public void Deserialize_Failure_Arg1Null()
        {
            // arrange
            string filename = null;

            // act
            try
            {
                string actual = Serializer.Deserialize<string>(filename);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }
    }
}
