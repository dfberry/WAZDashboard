using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wp7AzureMgmt.Core;
using Microsoft.WindowsAzure.ServiceRuntime;

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
        /// Named Azure local storage (not Azure Table Storage)
        /// </summary>
        private const string azureLocalStorageName = "LocalStorage1";

        /// <summary>
        /// Determine where serialized files are: app_data or azure local storage
        /// </summary>
        public StorageLocationBaseController()
        {
            // default value
            this.pathToFiles = this.HttpContext.Server.MapPath("~/App_Data") + @"\";

            // make sure we have an http context
            if (this.HttpContext != null)
            {
                config = new Configuration(this.HttpContext);

                // Web.config contains "location" key with value of "app_data" or anything else
                string webdotconfig_location = config.Get("location");

                // storage is in app_data, used for debug & testing
                if (webdotconfig_location != "app_data")
                {
                    this.pathToFiles = this.GetLocalAzureResource();
                }
            }
        }

        /// <summary>
        /// Get path to Azure Local storage
        /// </summary>
        /// <returns></returns>
        public string GetLocalAzureResource()
        {
            LocalResource localResource = RoleEnvironment.GetLocalResource(azureLocalStorageName);

            return localResource.RootPath;
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
