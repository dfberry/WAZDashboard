// -----------------------------------------------------------------------
// <copyright file="DashboardFileFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Factories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Wp7AzureMgmt.DashboardFeeds;
    
    /// <summary>
    /// Internal file object to manage Azure Dashboard Feed listing html
    /// for parsing later. 
    /// </summary>
    internal class DashboardFileFactory
    {
        /// <summary>
        /// CreateFile creates a Wp7AzureMgmt.DashboardFeeds.DashboardFile
        /// object.
        /// </summary>
        /// <param name="content">usually html content</param>
        /// <param name="filename">filename including path</param>
        /// <returns>DashboardFile with content and filename set</returns>
        public Wp7AzureMgmt.DashboardFeeds.DashboardFile CreateFile(string content, string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            Wp7AzureMgmt.DashboardFeeds.DashboardFile file = new Wp7AzureMgmt.DashboardFeeds.DashboardFile();

            file.FileContents = content;
            file.FileName = filename;

            return file;
        }

        /// <summary>
        /// Create new file with write access then save content.
        /// </summary>
        /// <param name="file">DashboardFile with filename and contents</param>
        public void Save(Wp7AzureMgmt.DashboardFeeds.DashboardFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            
            if (string.IsNullOrEmpty(file.FileName))
            {
                throw new ArgumentNullException("filename is empty or null");
            }

            using (FileStream stream = new FileStream(file.FileName, FileMode.CreateNew, FileAccess.Write))
            {
                using (StreamWriter outfile = new StreamWriter(stream))
                {
                    outfile.Write(file.FileContents);
                }
            }
        }

        /// <summary>
        /// Read file and return as string
        /// </summary>
        /// <param name="file">DashboardFile to act on</param>
        /// <returns>String contents of file</returns>
        internal string Read(DashboardFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            using (StreamReader reader = File.OpenText(file.FileName))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Checks to see if file exists. file.FileName 
        /// must include path.
        /// </summary>
        /// <param name="file">DashboardFile with valid filename</param>
        /// <returns>Boolean true if file exists.</returns>
        internal bool Exists(Wp7AzureMgmt.DashboardFeeds.DashboardFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            } 
            
            return File.Exists(file.FileName);
        }
    }
}
