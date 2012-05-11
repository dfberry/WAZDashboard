// -----------------------------------------------------------------------
// <copyright file="DashboardSmtpClient.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    
    /// <summary>
    /// SmtpClient to use in real code
    /// </summary>
    internal class DashboardSmtpClient : ISmtpClient
    {
        /// <summary>
        /// Real smtpClient
        /// </summary>
        private SmtpClient smtpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardSmtpClient" /> class.
        /// </summary>
        public DashboardSmtpClient()
        {
            this.smtpClient = new SmtpClient();
        }

        /// <summary>
        /// Sets NetworkCredential
        /// </summary>
        public NetworkCredential Credentials 
        { 
            set
            {
                this.smtpClient.Credentials = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether EnableSsl
        /// </summary>
        public bool EnableSsl
        {
            get
            {
                return this.smtpClient.EnableSsl; 
            }

            set
            {
                this.smtpClient.EnableSsl = value;
            }
        }

        /// <summary>
        /// Gets or sets Host
        /// </summary>
        public string Host
        {
            get
            {
                return this.smtpClient.Host;
            }

            set
            {
                this.smtpClient.Host = value;
            }
        }

        /// <summary>
        /// Gets or sets Port
        /// </summary>
        public int Port
        {
            get
            {
                return this.smtpClient.Port;
            }

            set
            {
                this.smtpClient.Port = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UseDefaultCredentials
        /// </summary>
        public bool UseDefaultCredentials
        {
            get
            {
                return this.smtpClient.UseDefaultCredentials;
            }

            set
            {
                this.smtpClient.UseDefaultCredentials = value;
            }
        }

        /// <summary>
        /// Send mail notification
        /// </summary>
        /// <param name="message">MailMessage of information</param>
        public void Send(MailMessage message)
        {
            this.smtpClient.Send(message);
        }
    }
}
