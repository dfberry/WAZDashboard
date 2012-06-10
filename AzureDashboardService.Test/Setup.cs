// -----------------------------------------------------------------------
// <copyright file="Setup.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.IO;
    
    public static class Setup
    {
        /// <summary>
        /// Gets data path so tests look for data files in correct place
        /// </summary>
        /// <returns>string including post pend slash</returns>
        public static string GetDataPath()
        {
            string finalPath = string.Empty;
            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] splitString = null;
            string basePath = string.Empty;

            // everything before the bin directory
            if (binPath.Contains("bin"))
            {
                Regex matchPattern = new Regex("bin");
                splitString = matchPattern.Split(binPath);
                basePath = splitString[0];
                finalPath = Path.Combine(basePath, @"App_Data\");
            }
            else if (binPath.Contains("TestResults"))
            {
                Regex matchPattern = new Regex("TestResults");
                splitString = matchPattern.Split(binPath);
                basePath = splitString[0];
                finalPath = Path.Combine(basePath, "XmlTestProject", @"App_Data\");
            }
            else
            {
                // don't know where the path is at this point
            }

            return finalPath;
        }
    }
}
