using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wp7AzureMgmt.Core;

namespace AzureDashboardService.Controllers
{
    public class StorageLocationBaseController : Controller
    {
        /// <summary>
        /// Access to web.config
        /// </summary>
        public Configuration config;

        /// <summary>
        /// Path to storage/serialized files for app
        /// </summary>
        public string pathToFiles;

        /// <summary>
        /// Determine where serialized files are: app_data or azure local storage
        /// </summary>
        public StorageLocationBaseController()
        {
            // default value
            this.pathToFiles = this.HttpContext.Server.MapPath("~/App_Data") + @"\";
        }

        /// <summary>
        /// Gets HttpContext to pass into Dashboard library.
        /// http://stackoverflow.com/questions/223317/httpcontext-on-instances-of-controllers-are-null-in-asp-net-mvc
        /// </summary>
        public new HttpContextBase HttpContext
        {
            get
            {
                HttpContextWrapper context =
                    new HttpContextWrapper(System.Web.HttpContext.Current);
                return (HttpContextBase)context;
            }
        }

    }
}
