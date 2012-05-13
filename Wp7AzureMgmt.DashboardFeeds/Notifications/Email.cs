// -----------------------------------------------------------------------
// <copyright file="Email.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;

    /// <summary>
    /// Sending Emails
    /// http://stackoverflow.com/questions/1058041/how-do-i-make-a-mockup-of-system-net-mail-mailmessage
    /// </summary>
    public class EmailNotification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotification" /> class.
        /// </summary>
        /// <param name="stamp">Add text to email message with DateTime</param>
        /// <param name="smtpClient">SmtpClient to send email with</param>
        public EmailNotification(bool stamp, ISmtpClient smtpClient)
        {
            this.StampEmailWithTime = stamp;
            this.NotificationSent = false;

            if (smtpClient == null)
            {
                throw new ArgumentNullException("iSmtpClient");
            }

            this.SmtpClient = smtpClient;
        }

        /// <summary>
        /// Gets or sets a value indicating whether NotificationSent
        /// </summary>
        public bool NotificationSent { get; set; }

        /// <summary>
        /// Gets or sets From
        /// </summary>
        public MailAddress From { get; set; }

        /// <summary>
        /// Gets or sets SmtpClient
        /// </summary>
        public ISmtpClient SmtpClient { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether StampEmailWithTime
        /// </summary>
        public bool StampEmailWithTime { get; set; }

        /// <summary>
        /// Gets or sets To
        /// </summary>
        public MailAddress To { get; set; }

        /// <summary>
        /// Gets or sets Message
        /// </summary>
        public MailMessage Message { get; set; }

        /// <summary>
        /// Set username and password for network
        /// </summary>
        /// <param name="username">username as string</param>
        /// <param name="password">password as string</param>
        public void SetNetworkCredentials(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            this.SmtpClient.Credentials = new System.Net.NetworkCredential(username, password);
        }

        /// <summary>
        /// Sets the Smtp host/port and from email/name.
        /// </summary>
        /// <param name="fromEmail">From email address</param>
        /// <param name="fromEmailName">From email name</param>
        /// <param name="host">mail host as string</param>
        /// <param name="port">mail host port as string</param>
        public void SetSmtpClient(string fromEmail, string fromEmailName, string host, string port)
        {
            if (string.IsNullOrEmpty(fromEmail))
            {
                throw new ArgumentNullException("fromEmail");
            }

            if (string.IsNullOrEmpty(fromEmailName))
            {
                throw new ArgumentNullException("fromEmailName");
            }

            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            if (string.IsNullOrEmpty(port))
            {
                throw new ArgumentNullException("port");
            }

            this.SmtpClient.Host = host;
            this.SmtpClient.Port = int.Parse(port);
            this.SmtpClient.UseDefaultCredentials = true;
            this.SmtpClient.EnableSsl = true;

            this.From = new MailAddress(fromEmail, fromEmailName);
        }

        /// <summary>
        /// Build new Message with to and from. If name is empty, 
        /// just use email address as name.
        /// </summary>
        /// <param name="toEmail">email address of recipient</param>
        /// <param name="toName">name of recipient</param>
        public void SetReceiver(string toEmail, string toName)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                throw new ArgumentNullException("toEmail");
            }

            if (string.IsNullOrEmpty(toName))
            {
                toName = toEmail;
            }

            this.To = new MailAddress(toEmail, toName);
        }

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="title">Email message subject</param>
        /// <param name="text">Email message text</param>
        public void Notify(string title, string text)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (this.From == null)
            {
                throw new NullReferenceException("From");
            }

            if (this.To == null)
            {
                throw new NullReferenceException("To");
            }

            this.Message = new MailMessage(this.From, this.To);

            if (this.Message == null)
            {
                throw new NullReferenceException("Message");
            }            
            
            this.Message.Subject = title;

            if (this.StampEmailWithTime)
            {
                this.Message.Body = DateTime.Now.ToString() + "\n" + text + "\n";
            }
            else
            {
                this.Message.Body = text;
            }

            this.SmtpClient.Send(this.Message);

            this.NotificationSent = true;
        }
    }
}
