using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
using Wp7AzureMgmt.DashboardFeeds;

namespace Wp7AzureMgmt.DashboardIssues.Models
{
    /// <summary>
    /// Main Data Class containing information of the dashboard issues
    /// </summary>
    [Serializable()]
    public class RssIssues : ISerializable
    {
        /// <summary>
        /// Default Parallel Options to no limit
        /// </summary>
#if DEBUG
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 1 }; // no parallelism
#else  
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
#endif

        /// <summary>
        /// Internal issues list
        /// </summary>
        private IEnumerable<RssIssue> issues;

        /// <summary>
        /// Gets or sets date of generation of Feed list - when the list was
        /// generated from the Azure Service Dashboard website.
        /// </summary>
        /// <returns>DateTime of Feed list generation</returns>
        public DateTime RetrievalDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssIssues" /> class.
        /// Need a zero param constructor.
        /// </summary>
        public RssIssues()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssIssues" /> class.
        /// </summary>
        /// <param name="info">SerializationInfo of RssIssues</param>
        /// <param name="ctxt">StreamingContext of RssIssues</param>
        public RssIssues(SerializationInfo info, StreamingContext ctxt)
        {
            this.issues = (IEnumerable<RssIssue>)info.GetValue("Issues", typeof(IEnumerable<RssIssue>));
            this.RetrievalDate = (DateTime)info.GetValue("RetrievalDate", typeof(DateTime));
        }

        /// <summary>
        /// Gets or sets Feeds list
        /// </summary>
        public IEnumerable<RssIssue> Issues
        {
            get { return this.issues; }
            set { this.issues = value; }
        }
        

        /// <summary>
        /// Get object from serialized data
        /// </summary>
        /// <param name="info">SerializationInfo of RssIssues</param>
        /// <param name="ctxt">StreamingContext of RssIssues</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Issues", this.issues);
            info.AddValue("RetrievalDate", this.RetrievalDate);

        }

        /// <summary>
        /// Returns combined hashcode for properties that define object.
        /// Defined as RetrievalDate + Concatenated strings of xml issues.
        /// </summary>
        /// <returns>int as hashcode</returns>
        public override int GetHashCode()
        {
            // date + count + concatenated codes
            string stringToHash = this.RetrievalDate.ToString() + this.issues.Count().ToString();

            Parallel.ForEach(
                this.issues,
                this.options,
                issue =>
                {
                    stringToHash += issue.RssIssueXml;
                });

            return stringToHash.GetHashCode();
        }

        /// <summary>
        /// Used by test framework to compare objects. Only
        /// issuecode must be the same.
        /// </summary>
        /// <param name="obj">object to compare against</param>
        /// <returns>equality of objects as bool </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(RssIssues))
            {
                return false;
            }

            if ((((RssIssues)obj).issues == this.issues)
                && (((RssIssues)obj).RetrievalDate == this.RetrievalDate))
            {
                RssIssue[] thisList = this.Issues.ToArray();
                RssIssue[] testList = ((RssIssues)obj).Issues.ToArray();

                for (int i = 0; i < this.Issues.Count(); i++)
                {
                    if (!thisList[i].Equals(testList[i]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // date or count is not right
                return false;
            }

            return true;
        }


    }
}
