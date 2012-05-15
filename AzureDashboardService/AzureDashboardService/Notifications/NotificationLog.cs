// -----------------------------------------------------------------------
// <copyright file="NotificationLog.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Management;
    
    /// <summary>
    /// Used to stuff errrors into AppHarbor error log
    /// </summary>
    public class NotificationLog : WebRequestErrorEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationLog" /> class.
        /// </summary>
        /// <param name="message">error message</param>
        public NotificationLog(string message)
            : base(null, null, 100001, new Exception(message))
        {        
        }
    }
}