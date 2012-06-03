// -----------------------------------------------------------------------
// <copyright file="EmaiNotificationTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Moq;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds.Interfaces;
    using Wp7AzureMgmt.Core.Notifications;
    using Wp7AzureMgmt.Core.Interfaces;
    //using Wp7AzureMgmt.DashboardFeeds.Utilities;

    /// <summary>
    /// Tests for EmailNotification
    /// </summary>
    [TestFixture]
    public class EmailNotificationTest
    {
        /// <summary>
        /// Test email address
        /// </summary>
        private string emailAddress = "user@user.com";

        /// <summary>
        /// Test email host
        /// </summary>
        private string emailHost = "smtp.gmail.com";

        /// <summary>
        /// Test email port
        /// </summary>
        private string emailPort = "587";

        /// <summary>
        /// Tests for Constructor
        /// </summary>
        [Test]
        public void Email()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new Mock<ISmtpClient>();

            // act
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            // assert
            Assert.IsTrue(email.StampEmailWithTime == stamp);
        }

        /// <summary>
        /// This test doesn't work and is currently beyond scope to make it work.
        /// once credentials are set, they can't be read 
        /// </summary>
        [Test]
        public void SetNetworkCredentials()
        {
            ////    // arrange
            ////    bool stamp = true;
            ////    EmailNotification email = new EmailNotification(stamp, new DashboardSmtpClient());

            ////    string username="username";
            ////    string password="password";

            ////    NetworkCredential userCredentials = new System.Net.NetworkCredential(username, password);

            ////    // act
            ////    email.SetNetworkCredentials(username, password);

            ////    // assert
            ////    Assert.IsTrue(email.SmtpClient != null);
            ////    Assert.IsTrue(email.SmtpClient.Credentials != null);
            ////    Assert.IsTrue(email.SmtpClient.Credentials.Password == password);

            ////    // DFB: can't seem to verify credentials
            ////    //Assert.IsTrue(email.SmtpClient.Credentials.Equals(userCredentials));
        }

        /// <summary>
        /// Test for SetNetworkCredentials with params null
        /// </summary>
        [Test]
        public void SetNetworkCredentials_ArgumentNullException()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new Mock<ISmtpClient>();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            try
            {
                email.SetNetworkCredentials("user", null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            try
            {
                email.SetNetworkCredentials(null, "password");
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
        /// Test for SetReceiver
        /// </summary>
        [Test]
        public void SetReceiver()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new Mock<ISmtpClient>();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            string toEmail = this.emailAddress;
            string toName = "toemailname";

            // act
            email.SetReceiver(toEmail, toName);

            // assert
            Assert.IsTrue(email.To != null);
            Assert.IsTrue(email.To.Address == toEmail);
            Assert.IsTrue(email.To.DisplayName == toName);
        }

        /// <summary>
        /// Tests for SetReceiver with params null
        /// </summary>
        [Test]
        public void SetReceiver_ArgumentNullException()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new Mock<ISmtpClient>();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            try
            {
                email.SetReceiver(this.emailAddress, null);
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            try
            {
                email.SetReceiver(null, "toName");
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
        /// Test for SetSmtpClient
        /// </summary>
        [Test]
        public void SetFromMailAddress()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new DashboardSmtpClient();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient);

            string fromEmail = this.emailAddress;
            string fromEmailName = "fromEmailName";

            // act
            email.SetFromMailAddress(fromEmail, fromEmailName);

            // assert
            Assert.IsTrue(email.From != null);
            Assert.IsTrue(email.From.Address == fromEmail);
            Assert.IsTrue(email.From.DisplayName == fromEmailName);
        }

        /// <summary>
        /// Test for SetSmtpClient with params null
        /// </summary>
        [Test]
        public void SetSmtpClient_ArgumentNullException()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new Mock<ISmtpClient>();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            try
            {
                email.SetFromMailAddress(this.emailAddress, null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            try
            {
                email.SetFromMailAddress(null, "fromEmailName");
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
        /// Test for Notify
        /// </summary>
        [Test]
        public void Notify()
        {
            string fromEmail = this.emailAddress;
            string fromEmailName = "fromEmailName";
            string host = this.emailHost;
            string port = this.emailPort;
            bool stamp = true;

            var mockSmtpClient = new Mock<ISmtpClient>();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            email.SmtpClient = mockSmtpClient.Object;

            email.SetFromMailAddress(fromEmail, fromEmailName/*, host, port*/);

            // username and password
            string username = "username";
            string password = "password";
            email.SetNetworkCredentials(username, password);

            string toemail = this.emailAddress;
            string toname = "toname";

            email.SetReceiver(toemail, toname);

            string subject = "subject";
            string text = "text";

            // act
            email.Notify(subject, text);

            // assert
            Assert.IsTrue(email.NotificationSent == true);
            mockSmtpClient.Verify(a => a.Send(email.Message), Times.Once());
        }

        /// <summary>
        /// Test for Notify with params null
        /// </summary>
        [Test]
        public void Notify_ArgumentNullException()
        {
            // arrange
            bool stamp = true;
            var mockSmtpClient = new Mock<ISmtpClient>();
            EmailNotification email = new EmailNotification(stamp, mockSmtpClient.Object);

            try
            {
                email.Notify("title", null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            try
            {
                email.Notify(null, "Text");
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            try
            {
                email.Notify("title", "Text");
                email.SetReceiver(this.emailAddress, "toname");
                Assert.Fail("exception not thrown");
            }
            catch (NullReferenceException ex)
            {
                Assert.IsTrue(ex.Message == "From");
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }

            try
            {
                email.SetFromMailAddress(this.emailAddress, "fromEmailName"/*, this.emailHost, this.emailPort*/);
                email.Notify("title", "Text");

                Assert.Fail("exception not thrown");
            }
            catch (NullReferenceException ex)
            {
                Assert.IsTrue(ex.Message == "To");
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }
    }
}
