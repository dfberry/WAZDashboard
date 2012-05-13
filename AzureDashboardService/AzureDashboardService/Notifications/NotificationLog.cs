namespace AzureDashboardService.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Management;
    
    /// <summary>
    /// 
    /// </summary>
    public class NotificationLog : WebRequestErrorEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NotificationLog(string message)
            : base(null, null, 100001, new Exception(message))
        {        
        }
    }
}