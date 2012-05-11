// -----------------------------------------------------------------------
// <copyright file="DashboardFileFactoryTest.cs" company="DFBerry">
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
    using Wp7AzureMgmt.DashboardFeeds.Factories;

    /// <summary>
    /// Tests for DashboardFileFactory
    /// </summary>
    [TestFixture]
    public class DashboardFileFactoryTest
    {
        /// <summary>
        /// Contents of file for test
        /// </summary>
        private string contents = "test is a test.";

        /// <summary>
        /// Filename for test
        /// </summary>
        private string filename = "test.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardFileFactoryTest" /> class.
        /// </summary>
        public DashboardFileFactoryTest()
        {
            // make sure file isn't there
            File.Delete(this.filename);
        }

        /// <summary>
        /// Test for CreateFile() that returns
        /// correct DashboardFile object.
        /// </summary>
        [Test]
        public void CreateFileTest_Success()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();

            // act
            DashboardFile file = target.CreateFile(this.contents, this.filename);

            // assert
            Assert.IsNotNull(file);
            Assert.AreSame(this.contents, file.FileContents);
            Assert.AreSame(this.filename, file.FileName);
        }

        /// <summary>
        /// Test for CreateFile() that returns
        /// correct DashboardFile object.
        /// </summary>
        [Test]
        public void CreateFileTest_SuccessNoContent()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();

            // act
            DashboardFile file = target.CreateFile(null, this.filename);

            // assert
            Assert.IsNotNull(file);
            Assert.IsNull(file.FileContents);
            Assert.AreSame(this.filename, file.FileName);
        }

        /// <summary>
        /// Tests null value of filename throws null 
        /// arg exception.
        /// </summary>
        [Test]
        public void CreateFileTest_ParamFailureFilename()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();
            string temp = null;

            // act
            try
            {
                DashboardFile file = target.CreateFile(this.contents, temp);
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
        /// Test for correctly saving content to file on disk.
        /// </summary>
        [Test]
        public void SaveTest_Success()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();
            DashboardFile file = target.CreateFile(this.contents, this.filename);

            // act
            target.Save(file);

            // assert
            Assert.IsTrue(File.Exists(file.FileName));

            // cleanup
            File.Delete(file.FileName);
        }

        /// <summary>
        /// Test for saving fails by throwing exception for null
        /// param.
        /// </summary>
        [Test]
        public void SaveTest_ParamFailure()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();

            // act
            try
            {
                target.Save(null);
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
        /// Test that Read method returns correct
        /// contents.
        /// </summary>
        [Test]
        public void ReadTest_Success()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();
            DashboardFile file = target.CreateFile(this.contents, this.filename);
            target.Save(file);

            // act
            string actual = target.Read(file);

            // assert
            Assert.AreEqual(this.contents, actual);

            // cleanup
            File.Delete(file.FileName);
        }

        /// <summary>
        /// Test for read fails based on file not found.
        /// </summary>
        [Test]
        public void ReadTest_FailureFileNotFound()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();
            DashboardFile file = target.CreateFile(this.contents, "bogusname.txt");

            // act
            try
            {
                target.Read(file);
                Assert.Fail("exception not thrown");
            }
            catch (FileNotFoundException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests null value of file object throws null 
        /// arg exception.
        /// </summary>
        [Test]
        public void ReadTest_FailureParamFile()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();

            // act
            try
            {
                target.Read(null);
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
        /// Test success for file exists on disk.
        /// </summary>
        [Test]
        public void ExistsTest_SuccessTrue()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();
            DashboardFile file = target.CreateFile(this.contents, this.filename);
            target.Save(file);

            // act
            string actual = target.Read(file);

            // assert
            Assert.AreEqual(this.contents, actual);

            // cleanup
            File.Delete(file.FileName);
        }

        /// <summary>
        /// Test that Exists correctly returns false
        /// if file doesn't exist
        /// </summary>
        [Test]
        public void ExistsTest_SuccessFalse()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();
            DashboardFile file = target.CreateFile(this.contents, this.filename);

            // act
            bool actual = target.Exists(file);

            // assert
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Tests null param throws exception
        /// </summary>
        [Test]
        public void ExistsTest_FailureParamFile()
        {
            // arrange
            DashboardFileFactory target = new DashboardFileFactory();

            // act
            try
            {
                target.Exists(null);
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
