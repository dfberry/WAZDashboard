// -----------------------------------------------------------------------
// <copyright file="ISmtpClient.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    /// <summary>
    /// Interface for SmtpClient
    /// </summary>
    public interface ISmtpClient
    {
        /// <summary>
        /// Sets Network security credentials.
        /// </summary>
        NetworkCredential Credentials { set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enablessl 
        /// </summary>
        bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets Host as string property
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets Port as integer property
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether UseDefaultCredentials as boolean property
        /// </summary>
        bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Send email message.
        /// </summary>
        /// <param name="message">message as MailMessage</param>
        void Send(MailMessage message);
    }
}
