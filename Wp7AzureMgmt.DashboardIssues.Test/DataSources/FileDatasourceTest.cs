// -----------------------------------------------------------------------
// <copyright file="FileDatasourceTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardIssues.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardIssues.DataSources;
    using Wp7AzureMgmt.DashboardIssues.Models;
    
    /// <summary>
    /// This is a test class for FileDatasourceTest and is intended
    /// to contain all FileDatasourceTest Unit Tests
    /// </summary>
    [TestFixture]
    public class FileDatasourceTest
    {
        /// <summary>
        /// A test for FileDatasource Constructor
        /// </summary>
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
        /// A test for Get
        /// </summary>
        [Test]
        public void GetTest()
        {
            // arrange
            string pathToFilename = Setup.GetDataPath(); 
            HttpContextBase httpContext = null; 
            FileDatasource target = new FileDatasource(pathToFilename, httpContext);

            if (!File.Exists(pathToFilename + "IssueFileDatasource"))
            {
                Setup.RunBeforeTests_IssueListFile();
            }

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

            // Cleanup
            File.Delete(pathToFilename + "IssueFileDatasource");
        }

        /// <summary>
        /// A test for GetIssues - make sure any filename can be used for
        /// serialized data source file name. Rename file using FileInfo.MoveTo.
        /// </summary>
        [Test]
        public void GetTest_params()
        {
            // arrange

            // copy file so it has different name - leave it in app_data
            // so if it is ever propped to server, it doesn't have permission
            // problems
            string serializedFile = Setup.GetDataPath() + "GetTest_params";

            if (File.Exists(Setup.GetDataPath() + "GetTest_params"))
            {
                File.Delete(Setup.GetDataPath() + "GetTest_params");
            }

            if (!File.Exists(Setup.GetDataPath() + "IssueFileDatasource"))
            {
                Setup.RunBeforeTests_IssueListFile();
            }

            FileInfo fileInfo = new FileInfo(Setup.GetDataPath() + "IssueFileDatasource");
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

            // cleanup
            File.Delete(pathToFilename + "GetTest_params");
            File.Delete(pathToFilename + "IssueFileDatasource");
        }

        /// <summary>
        /// A test for Set. Use a fake here.
        /// </summary>
        [Test]
        public void SetTest_wParams()
        {
            // arrange
            string pathToFilename = Setup.GetDataPath(); 
            HttpContextBase httpContext = null; 
            FileDatasource target = new FileDatasource(pathToFilename, httpContext); 

            // grab a fake
            FakeDatasource fake = new FakeDatasource();

            RssIssues issues = fake.RssIssues;
            string filename = "FileDatasource_SetTest_wParams";
            string fullpathandfilename = pathToFilename + filename;

            // act
            target.Set(issues, fullpathandfilename);

            // assert
            File.Exists(fullpathandfilename);

            // make sure I can deserialize
            RssIssues deserialized = target.Get(fullpathandfilename);
            Assert.AreEqual(issues, deserialized);
            
            // cleanup
            File.Delete(fullpathandfilename);
        }

        /// <summary>
        /// A test for Set
        /// </summary>
        [Test]
        public void SetTest()
        {
            // arrange
            string pathToFilename = Setup.GetDataPath() + "FileDatasource_SetTest";
            HttpContextBase httpContext = null;
            FileDatasource target = new FileDatasource(pathToFilename, httpContext);

            // grab a fake
            FakeDatasource fake = new FakeDatasource();
            RssIssues issues = fake.RssIssues;
            target.RssIssues = issues;
            target.FileName = pathToFilename;

            // act
            target.Set();

            // assert
            File.Exists(pathToFilename);

            // make sure I can deserialize
            RssIssues deserialized = target.Get(pathToFilename);
            Assert.AreEqual(issues, deserialized);

            // cleanup
            File.Delete(pathToFilename);
        }
    }
}
