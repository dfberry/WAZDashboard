// -----------------------------------------------------------------------
// <copyright file="TraceLogToFile.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    
    /// <summary>
    /// Grab trace. If using AppHarbor, and Trace config setting is true
    /// then trace to file. Otherwise trace to debug output window.
    /// </summary>
    public static class TraceLogToFile
    {
        /// <summary>
        /// Grab trace to file
        /// </summary>
        /// <param name="pathAndFilename">full path and file name of trace file</param>
        /// <param name="msg">test to append to file</param>
        public static void Trace(string pathAndFilename, string msg)
        {
#if Trace
            msg = " ******************************\n" + DateTime.UtcNow + "\n" + msg + "\n";

            if (!File.Exists(pathAndFilename))
            {
                using (StreamWriter sw = File.CreateText(pathAndFilename))
                {
                    sw.Write(msg);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(pathAndFilename))
                {
                    sw.Write(msg);
                } 
            }
#else
            System.Diagnostics.Trace.TraceInformation(msg);
#endif
        }

        /// <summary>
        /// Return Trace file as string
        /// </summary>
        /// <param name="pathAndFilename">location of trace file</param>
        /// <returns>trace file contents as string</returns>
        public static string Get(string pathAndFilename)
        {
            using (StreamReader reader = File.OpenText(pathAndFilename))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Delete trace file
        /// </summary>
        /// <param name="pathAndFilename">full path and tracefile name </param>
        public static void Delete(string pathAndFilename)
        {
            if (File.Exists(pathAndFilename))
            {
                File.Delete(pathAndFilename);
            }
        }
    }
}
