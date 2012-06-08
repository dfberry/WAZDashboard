// -----------------------------------------------------------------------
// <copyright file="DashboardHttpFake.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Wp7AzureMgmt.Core.Interfaces;
    
    /// <summary>
    /// Stub/Fake Http requests for testing
    /// </summary>
    public class DashboardHttpFake : IDashboardHttp
    {
        /// <summary>
        /// fake uri - not used
        /// </summary>
        private Uri uri;

        /// <summary>
        /// Pretends to make Uri request and returns mocked response from file.
        /// </summary>
        /// <returns>string as mock html response</returns>
        public string GetRequest()
        {
            string filename = @"..\..\..\Wp7AzureMgmt.DashboardFeeds.Test\servicedashboardcontent.html";
            string response = string.Empty;

            if (File.Exists(filename))
            {
                using (StreamReader rdr = File.OpenText(filename))
                {
                    response = rdr.ReadToEnd();
                }
            }
            else
            {
                throw new NullReferenceException("file doesn't exist");
            }

            return response;
        }

        /// <summary>
        /// Throws NotImplementedException
        /// </summary>
        /// <param name="filename">path and filename</param>
        /// <returns>nothing to return</returns>
        public string SaveResponseContentToFile(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException
        /// </summary>
        /// <returns>nothing to return</returns>
        public HttpWebRequest BuildHttpGet()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented. 
        /// </summary>
        /// <typeparam name="T">generic T of Xml model</typeparam>
        /// <returns>exceptions only for now</returns>
        /// <exception cref="NotImplementedException">This method currently not implemented.</exception>
        public T GetXmlRequest<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set Uri for http request
        /// </summary>
        /// <param name="uri">Uri to build http get request from</param>
        public void SetUri(Uri uri)
        {
            this.uri = uri;
        }
    }
}
