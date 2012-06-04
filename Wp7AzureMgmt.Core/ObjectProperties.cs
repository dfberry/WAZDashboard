using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Wp7AzureMgmt.Core
{
    public static class ObjectProperties
    {
        /// <summary>
        /// Based on any object, get properties and types.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjectPropertyList(Object obj)
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
