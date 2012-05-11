// -----------------------------------------------------------------------
// <copyright file="IDashboardHttp.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    
    /// <summary>
    /// Interface for all Http Requests
    /// </summary>
    public interface IDashboardHttp
    {
        /// <summary>
        /// Set Uri for Http request
        /// </summary>
        /// <param name="uri">Uri to grab Html for parsing</param>
        void SetUri(Uri uri);

        /// <summary>
        /// Http Request across the wire, return 
        /// content as a string. No ability to deal with
        /// binary response right now.
        /// </summary>
        /// <returns>html response as string</returns>
        string GetRequest();

        /// <summary>
        /// Save string to file. Filename must include full
        /// path if it is not in the current directory - usually
        /// somewhere in the \bin directory.
        /// </summary>
        /// <param name="filename">path and filename as string</param>
        /// <returns>path and filename as string - in case it was munged during method call</returns>
        string SaveResponseContentToFile(string filename);

        /// <summary>
        /// Method allows different implementations of 
        /// HttpWebRequest properties.
        /// </summary>
        /// <returns>HttpWebRequest for GetRequest() to use</returns>
        HttpWebRequest BuildHttpGet();
    }
}
