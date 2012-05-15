// -----------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Management;
    using System.Web.Mvc;
    using AzureDashboardService.Notifications;
    
    /// <summary>
    /// Error controller handles all errors generated from MVC
    /// app - display something meaningful to browser and 
    /// alert IT.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Currently handles all errors
        /// </summary>
        /// <param name="exception">Web exception from Global.asa</param>
        /// <returns>Error view</returns>
        public ActionResult GeneralError(Exception exception)
        {
            string exceptionMsg = this.NoteErrors(exception);
            new NotificationLog(exceptionMsg);
            return View(Server.HtmlEncode(exceptionMsg));
        }

        /// <summary>
        /// Build up string containing error information so
        /// I can track down error. 
        /// </summary>
        /// <param name="exception">Exception object containing error info</param>
        /// <returns>string of error properties strung together</returns>
        protected string NoteErrors(Exception exception)
        {
            string moreInfo = string.Empty;

            if (Request != null)
            {
                if (Request.HttpMethod != null)
                {
                    moreInfo += "\n Request.Url:" + Request.HttpMethod + "\n****************************\n";
                }

                if (Request.UserLanguages != null)
                {
                    moreInfo += "\n Request.UserLanguages:\n";

                    foreach (string language in Request.UserLanguages)
                    {
                        moreInfo += language + "\n";
                    }

                    moreInfo += "****************************\n";
                }

                if (Request.Url != null)
                {
                    moreInfo += "\n Request.Url:" + Request.Url + "\n****************************\n";
                }

                if (Request.FilePath != null)
                {
                    moreInfo += "\n Request.FilePath:" + Request.FilePath + "\n****************************\n";
                }

                if (Request.PhysicalApplicationPath != null)
                {
                    moreInfo += "\n PhysicalApplicationPath:" + Request.PhysicalApplicationPath + "\n****************************\n";
                }

                if (Request.Headers != null)
                {
                    moreInfo += "\n PhysicalApplicationPath:" + Request.Headers + "\n****************************\n";
                }

                if (Request.UserAgent != null)
                {
                    moreInfo += "\n UserAgent:" + Request.UserAgent + "\n****************************\n";
                }

                if (Request.ServerVariables != null)
                {
                    moreInfo += "\n Request.ServerVariables:\n";

                    foreach (string sv in Request.ServerVariables)
                    {
                        moreInfo += sv + ":" + Request.ServerVariables[sv] + "\n";
                    }

                    moreInfo += "****************************\n";
                }
            }

            if (exception != null)
            {
                if (exception.InnerException != null)
                {
                    moreInfo += "\n InnerException:" + exception.InnerException + "\n****************************\n";
                }

                if (exception.Message != null)
                {
                    moreInfo += "\n Exception.Message: " + exception.Message + "\n****************************\n";
                }

                if (exception.StackTrace != null)
                {
                    moreInfo += "Exception.StackTrace: " + exception.StackTrace + "\n****************************\n";
                }
            }

            return moreInfo;
        }
    }
}
