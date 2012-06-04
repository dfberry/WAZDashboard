﻿// -----------------------------------------------------------------------
// <copyright file="DashboardMgrTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardIssues.Test
{
    using Wp7AzureMgmt.DashboardIssues.DataSources;
    using NUnit.Framework;
    using System;
    using System.Web;
    using System.Linq;
    using Wp7AzureMgmt.DashboardIssues.Models;
    using System.Collections.Generic;
    using System.IO;
    
    /// <summary>
    ///This is a test class for FileDatasourceTest and is intended
    ///to contain all FileDatasourceTest Unit Tests
    ///</summary>
    [TestFixture]
    public class FileDatasourceTest
    {
        /// <summary>
        ///A test for FileDatasource Constructor
        ///</summary>
        [Test]
        public void FileDatasourceConstructorTest()
        {
            // arrange
            string pathToFilename = string.Empty; 
            HttpContextBase httpContext = null; 

            // act
            FileDatasource target = new FileDatasource(pathToFilename, httpContext);

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual("IssueFileDatasource", target.FileName);
            Assert.IsNull(target.RssIssues);
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [Test]
        public void GetTest()
        {
            // arrange
            string pathToFilename = Setup.GetDataPath(); 
            HttpContextBase httpContext = null; 
            FileDatasource target = new FileDatasource(pathToFilename, httpContext);
 
            RssIssues actual;

            // act
            actual = target.Get();

            // assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(target.RssIssues);
            Assert.IsNotNull(target.RssIssues.Issues);
            Assert.IsTrue((target.RssIssues.RetrievalDate - DateTime.UtcNow).Days < 1);

            List<RssIssue> issuesList = target.RssIssues.Issues.ToList();
            
            Assert.GreaterOrEqual(issuesList.Count(), 0);
        }

        /// <summary>
        /// A test for GetIssues - make sure any filename can be used for
        /// serialized data source file name. Rename file using FileInfo.MoveTo.
        ///</summary>
        [Test]
        public void GetTest_params()
        {
            // arrange

            // copy file so it has different name - leave it in app_data
            // so if it is ever propped to server, it doesn't have permission
            // problems
            string serializedFile = Setup.GetDataPath() + "GetTest_params";
            
            FileInfo fileInfo = new FileInfo(Setup.GetDataPath() + "IssueFileDatasource");
            if (!fileInfo.Exists)
            {
                Assert.Fail("file doesn't exist");
            }

            fileInfo.MoveTo(serializedFile);

            string pathToFilename = Setup.GetDataPath();
            HttpContextBase httpContext = null; 
            FileDatasource target = new FileDatasource(pathToFilename, httpContext);
            RssIssues actual;

            // act - want to pass both path and file name
            actual = target.Get(serializedFile);

            // assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(target.RssIssues);
            Assert.IsNotNull(target.RssIssues.Issues);
            Assert.IsTrue((target.RssIssues.RetrievalDate - DateTime.UtcNow).Days < 1);

            List<RssIssue> issuesList = target.RssIssues.Issues.ToList();

            Assert.GreaterOrEqual(issuesList.Count(), 0);

            // clean up
            fileInfo.MoveTo(serializedFile);
        }

        /// <summary>
        ///A test for Set. Use a fake here.
        ///</summary>
        [Test]
        public void SetTest()
        {
            // arrange
            string pathToFilename = Setup.GetDataPath(); 
            HttpContextBase httpContext = null; 
            FileDatasource target = new FileDatasource(pathToFilename, httpContext); 

            // grab a fake
            FakeDatasource fake = new FakeDatasource();

            RssIssues issues = fake.RssIssues; 
            string filename = "FileDatasource_SetTest";
            string fullpathandfilename = pathToFilename + filename;

            // actls
            target.Set(issues, fullpathandfilename);

            // assert
            File.Exists(fullpathandfilename);

            // make sure I can deserialize
            RssIssues deserialized = target.Get(fullpathandfilename);
            Assert.AreSame(issues, deserialized);
            
            // cleanup
            File.Delete(fullpathandfilename);
        }

        /// <summary>
        ///A test for Set
        ///</summary>
        [Test]
        public void SetTest1()
        {
            string pathToFilename = string.Empty; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            FileDatasource target = new FileDatasource(pathToFilename, httpContext); // TODO: Initialize to an appropriate value
            target.Set();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FileName
        ///</summary>
        [Test]
        public void FileNameTest()
        {
            string pathToFilename = string.Empty; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            FileDatasource target = new FileDatasource(pathToFilename, httpContext); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FileName = expected;
            actual = target.FileName;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RssIssues
        ///</summary>
        [Test]
        public void RssIssuesTest()
        {
            string pathToFilename = string.Empty; // TODO: Initialize to an appropriate value
            HttpContextBase httpContext = null; // TODO: Initialize to an appropriate value
            FileDatasource target = new FileDatasource(pathToFilename, httpContext); // TODO: Initialize to an appropriate value
            RssIssues expected = null; // TODO: Initialize to an appropriate value
            RssIssues actual;
            target.RssIssues = expected;
            actual = target.RssIssues;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
