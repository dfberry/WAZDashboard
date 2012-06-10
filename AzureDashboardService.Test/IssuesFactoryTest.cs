// -----------------------------------------------------------------------
// <copyright file="IssuesFactoryTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureDashboardService.Factories;
    using AzureDashboardService.Models;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardIssues;
    using Wp7AzureMgmt.DashboardIssues.Models;

    /// <summary>
    /// This is a test class for IssuesFactoryTest and is intended
    /// to contain all IssuesFactoryTest Unit Tests
    /// </summary>
    [TestFixture()]
    public class IssuesFactoryTest
    {
        /// <summary>
        /// Test model transformation.
        /// </summary>
        [Test]
        public void ToIssueModelTest()
        {
            // arrange
            IssueMgr issueMgr = new IssueMgr(null);
            RssIssues rssIssues = issueMgr.GetStoredRssIssues(Setup.GetDataPath());
            IEnumerable<Issue> actual;

            // act
            actual = IssuesFactory.ToIssueModel(rssIssues).ToList(); 

            // assert - DFB: todo - terrible tests - redo these!
            Assert.IsNotNull(actual);
            Assert.GreaterOrEqual(actual.Count(), 1);
        }
    }
}
