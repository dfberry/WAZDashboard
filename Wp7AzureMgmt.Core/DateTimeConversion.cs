// -----------------------------------------------------------------------
// <copyright file="DateTimeConversion.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    
    /// <summary>
    /// Class for any mucking about with or to DateTime
    /// </summary>
    public static class DateTimeConversion
    {
        /// <summary>
        /// Given a pubdate string like "", return the 
        /// equivalent DateTime. Used to convert RSS PubDates. If
        /// the string can't be converted, return DateTime.MinValue.
        /// </summary>
        /// <param name="rssPubDate">string of PubDate</param>
        /// <returns>DateTime of rssPubDate</returns>
        public static DateTime FromRssPubDateToDateTime(string rssPubDate)
        {
            String tempDateTime = rssPubDate;
            string format1 = "yyyy-MM-dd HH:mm";

            if (!String.IsNullOrEmpty(tempDateTime))
            {
                // DFB: this is the final format I want but it isn't the format that comes back from azure

                tempDateTime = tempDateTime.Replace("T", " ");
                tempDateTime = tempDateTime.Substring(0, 16); //cut off the seconds

                // DFB: not sure if the cultureinfo.invariantculture is the right choice
                return DateTime.ParseExact(tempDateTime, format1, CultureInfo.InvariantCulture);
            }

            return DateTime.MinValue;
        }
    }
}
