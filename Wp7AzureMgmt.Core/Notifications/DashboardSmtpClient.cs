// -----------------------------------------------------------------------
// <copyright file="DashboardSmtpClient.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.Core.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Wp7AzureMgmt.Core.Interfaces;
    
    /// <summary>
    /// SmtpClient to use in real code
    /// </summary>
    public class DashboardSmtpClient : ISmtpClient
    {
        /// <summary>
        /// Real smtpClient
        /// </summary>
        private SmtpClient smtpClient = new SmtpClient();

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
        /// Gets or sets an SmtpClient
        /// </summary>
        public SmtpClient SmtpClient
        {
            get
            {
                return this.smtpClient;
            }

            set
            {
                this.smtpClient = value;
            }
        }

        /// <summary>
        /// Send mail notification
        /// </summary>
        /// <param name="message">MailMessage of information</param>
        public void Send(MailMessage message)
        {
            // Be careful - this affects all useage
            // trying to get around Bluehost issues with how they lock down their Smtp gateway
            // I don't have a certificate so ignore cert error
            // http://stackoverflow.com/questions/777607/the-remote-certificate-is-invalid-according-to-the-validation-procedure-ple
            ServicePointManager.ServerCertificateValidationCallback += delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            
            this.smtpClient.Send(message);
        }
    }
}
