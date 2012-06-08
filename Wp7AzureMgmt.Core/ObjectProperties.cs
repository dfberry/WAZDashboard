// -----------------------------------------------------------------------
// <copyright file="ObjectProperties.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    
    /// <summary>
    /// ObjectProperties uses reflection to determine properties of any object.
    /// </summary>
    public static class ObjectProperties
    {
        /// <summary>
        /// Based on any object, get properties and types.
        /// </summary>
        /// <param name="obj">object of any type with properties</param>
        /// <returns>Dictionary of propertyname and propertytype</returns>
        public static Dictionary<string, string> ObjectPropertyList(object obj)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                dict.Add(property.Name, property.PropertyType.ToString());
            }

            return dict;
        }
    }
}
