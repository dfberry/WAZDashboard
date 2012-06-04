using Wp7AzureMgmt.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wp7AzureMgmt.Core.Test
{
    public class TestObject
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }
    
    /// <summary>
    ///This is a test class for ObjectPropertiesTest and is intended
    ///to contain all ObjectPropertiesTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ObjectPropertiesTest
    {
        /// <summary>
        /// A test for ObjectPropertyList. Crappy test because
        /// the types of string and System.String are different
        /// but because one has to be convert to object, it uses
        /// the System class. 
        ///</summary>
        [Test]
        public void ObjectPropertyListTest()
        {

            // arrange
            TestObject obj = new TestObject();

            Dictionary<string, string> expected = new Dictionary<string, string>();
            expected.Add("Property1", "System.String");
            expected.Add("Property2", "System.Int32");

            Dictionary<string, string> actual;

            // act
            actual = ObjectProperties.ObjectPropertyList(obj);

            // assert
            Assert.AreEqual(expected.Count, actual.Count);

            // http://stackoverflow.com/questions/43500/is-there-a-built-in-method-to-compare-collections-in-c
            Assert.IsTrue(actual.OrderBy(kvp => kvp.Key).SequenceEqual(expected.OrderBy(kvp => kvp.Key))); 

        }
    }
}
