// -----------------------------------------------------------------------
// <copyright file="Serializer.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// http://www.switchonthecode.com/tutorials/csharp-tutorial-serialize-objects-to-a-file
// http://www.codeproject.com/KB/cs/SerializeUtility.aspx
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml.Serialization;
    
    /// <summary>
    /// Serializes objects to files.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serialize object to file. Filename includes path.
        /// If file exists, it will be overwritten. If file doesn't exist,
        /// it will be created. If path is not specified, path becomes bin 
        /// directory.
        /// Example call: FileSerializer.Serialize(@"C:\Car1.dat", car1);
        /// /// </summary>
        /// <param name="filename">path and filename to store object</param>
        /// <param name="objectToSerialize">object to store</param>
        public static void Serialize(string filename, object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentNullException("objectToSerialize cannot be null");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename cannot be empty or null");
            }

            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                if (stream != null)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, objectToSerialize);
                }
            }
        }

        /// <summary>
        /// Deserialize object from file. Filename includes path.
        /// Does not check if file exists.
        /// If file not found, throws FileNotFoundException.
        /// If filename is null, throws ArgumentNullException.
        /// Example call: Car savedCar2 = FileSerializer.DeSerialize(@"C:\Car2.dat");
        /// </summary>
        /// <typeparam name="T">return object type</typeparam>
        /// <param name="filename">path and filename to deserialize</param>
        /// <returns>T as object from serialization file</returns>
        public static T Deserialize<T>(string filename)
        {
            T objectToSerialize;

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename cannot be empty or null");
            }

            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                objectToSerialize = (T)formatter.Deserialize(stream);
            }

            return objectToSerialize;
        }

        /// <summary>
        /// Deserializes xml string into model, based on encoding provided.
        /// </summary>
        /// <typeparam name="T">generic model of Xml</typeparam>
        /// <param name="xml">Xml as string text</param>
        /// <param name="encoding">encoding found in top of xml string</param>
        /// <returns>Xml deserialized into object T</returns>
        public static T XmlDeserialize<T>(string xml, Encoding encoding)
        {
            try
            {
                T obj = Activator.CreateInstance<T>();

                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml)))
                {
                    T temp = (T)serializer.Deserialize(memoryStream);
                    return temp;
                }
            }
            catch 
            {
                // DFB: XML was malformed 
                // return default object so code can keep going
                return default(T);
            }
        }
    }
}
