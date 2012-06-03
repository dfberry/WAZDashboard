using Wp7AzureMgmt.DashboardIssues.Utiliites;
using NUnit.Framework;
using System;
using System.Web;

namespace Wp7AzureMgmt.DashboardIssues.Test
{
    
    
    /// <summary>
    ///This is a test class for IssueConfigurationTest and is intended
    ///to contain all IssueConfigurationTest Unit Tests
    ///</summary>
    [TestFixture]
    public class IssueConfigurationTest
    {
        /// <summary>
        ///A test for IssueConfiguration Constructor
        ///</summary>
        [Test]
        public void IssueConfigurationConstructorTest()
        {
            // arrange
            HttpContextBase httpContext = null; 
            string configFileName = String.Empty;
            string serializedIssueListFile = "IssueFileDatasource";

            // act
            IssueConfiguration target = new IssueConfiguration(httpContext, configFileName);

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual(serializedIssueListFile, target.SerializedIssueListFile);
            Assert.IsNotNull(target.SmtpSection);
        }
    }
}
