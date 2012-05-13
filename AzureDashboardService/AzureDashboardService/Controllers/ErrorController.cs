// -----------------------------------------------------------------------
// <copyright file="DashboardSmtpClient.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Management;
    using AzureDashboardService.Notifications;
    
    /// <summary>
    /// 
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Currently handles all errors
        /// </summary>
        /// <param name="message"></param>
        public ActionResult Error(string message)
        {
            new NotificationLog(message);
            return View();
        } 

    }
}
