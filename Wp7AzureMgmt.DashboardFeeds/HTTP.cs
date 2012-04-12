// -----------------------------------------------------------------------
// <copyright file="HTTP.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class HTTP
    {
        /// <summary>
        /// URI to get
        /// </summary>
        private Uri uri;

        /// <summary>
        /// Timeout, default is 2 minutes
        /// </summary>
        private TimeSpan timeSpan = new TimeSpan(0, 2, 0);

        /// <summary>
        /// HttpWebRequest containing call instructions
        /// </summary>
        private HttpWebRequest httpWebRequest;

        /// <summary>
        /// content found in httpWebRequest
        /// </summary>
        private string responseContent = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HTTP" /> class.
        /// Given Uri and timeout, set Uri and timeout.
        /// If getUri is null, find default at 
        /// ConfigurationManager.AppSettings["AzureDashboardServiceURL"]
        /// </summary>
        /// <param name="getUri">URI to request</param>
        public HTTP(Uri getUri)
        {
            if (getUri == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.uri = getUri;
            }

            this.httpWebRequest = this.BuildHttpGet();
        }

        /// <summary>
        /// Gets HttpWebRequest Readonly.
        /// This is built in the constructor so any call after the 
        /// constructor should have valid values.
        /// </summary>
        public HttpWebRequest HttpWebRequest
        {
            get
            {
                return this.httpWebRequest;
            }
        }

        /// <summary>
        /// Returns HTML as string from URL request
        /// don't call if expecting binary or non-text values
        /// Build HttpWebRequest: 
        ///   HttpWebRequest.Timeout = (int)timeSpan.TotalMilliseconds
        ///   HttpWebRequest.ReadWriteTimeout = (int)timeSpan.TotalMilliseconds * 100
        ///   HttpWebRequest.Method = "GET"
        ///   HttpWebRequest.ContentType = "text/html"
        /// No other properties set.
        /// Throws away http response headers.
        /// Bubbles up WebException.
        ///   </summary>
        /// <returns>string as HTTPResponse content</returns>
        public string RequestGET()
        {
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)this.httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        this.responseContent = streamReader.ReadToEnd();
                        return this.responseContent;
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (Stream responseStream = webException.Response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                Trace.TraceError(reader.ReadToEnd());
                            }
                        }
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Save response to filename in default location
        /// bin directory of this dll.
        /// If filename is empty, filename created from 
        /// DateTime.Now.Ticks.ToString() + ".txt"
        /// </summary>
        /// <param name="filename">path and filename to save http response content to</param>
        /// <returns>filename as string</returns>
        public string SaveResponseContentToFile(string filename = null)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = DateTime.Now.Ticks.ToString() + ".txt";
            }

            using (StreamWriter outfile = new StreamWriter(filename))
            {
                outfile.Write(this.responseContent);
            }

            return filename;
        }

        /// <summary>
        /// Set Http header values
        /// </summary>
        /// <returns>HttpWebRequest as request</returns>
        private HttpWebRequest BuildHttpGet()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.uri);
            request.Timeout = (int)this.timeSpan.TotalMilliseconds;
            request.ReadWriteTimeout = (int)this.timeSpan.TotalMilliseconds * 100;
            request.Method = "GET";
            request.ContentType = "text/html";

            return request;
        }
    }
}