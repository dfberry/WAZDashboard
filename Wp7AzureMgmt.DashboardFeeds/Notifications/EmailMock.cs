

// -----------------------------------------------------------------------
// <copyright file="EmailMock.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    
    /// <summary>
    /// Mock email to use during testing
    /// </summary>
    internal class MockEmailService : INotification 
    {
        /// <summary>
        /// Message title
        /// </summary>
        private string sentTitle;

        /// <summary>
        /// Message text
        /// </summary>
        private string sentText;

        /// <summary>
        /// Gets SentTitle
        /// </summary>
        public string SentTitle
        { 
            get 
            {
                return this.sentTitle; 
            } 
        }

        /// <summary>
        /// Gets SentText
        /// </summary>
        public string SentText
        {
            get
            {
                return this.sentText;
            }
        }

        /// <summary>
        /// Simulates email message being sent. 
        /// Sets SentTitle and SentTest.
        /// </summary>
        /// <param name="title">mock title for email</param>
        /// <param name="text">mock text for eamil</param>
        public void Notify(string title, string text) 
        {
            this.sentTitle = title;
            this.sentText = text;
        } 
    }
}
