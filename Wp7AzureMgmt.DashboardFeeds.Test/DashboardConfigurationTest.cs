// -----------------------------------------------------------------------
// <copyright file="DashboardConfigurationTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    
    /// <summary>
    /// Tests for DashboardConfiguration class
    /// </summary>
    [TestFixture] 
    public class DashboardConfigurationTest
    {
        /// <summary>
        /// Make sure conversion from string to int works.
        /// </summary>
        [Test]
        public void GetFeedCountTest()
        {
            // arrange
            string key = "LastKnownRSSFeedCount";
            int expected = 5;

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            dashboardConfiguration.Save(key, expected.ToString());

            // act
            int actual = dashboardConfiguration.GetFeedCount;

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// General method to test fetching config setting
        /// value 
        /// </summary>
        [Test]
        public void GetConfigSettingSuccess()
        {
            // arrange
            string key = "TestFile";
            string expected = "test.txt";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            dashboardConfiguration.Save(key, expected.ToString());

            // act
            string actual = dashboardConfiguration.Get(key);

            // assert
            Assert.AreEqual(expected, actual);

            // cleanup
            dashboardConfiguration.Remove(key);
        }

        /// <summary>
        /// General method to test fetching config setting
        /// value. 
        /// </summary>
        [Test]
        public void GetConfigSettingKeyNotFound()
        {
            // arrange
            string key = "boguskey";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            string actual = dashboardConfiguration.Get(key);

            // assert
            Assert.IsNullOrEmpty(actual);
        }

        /// <summary>
        /// Save. Key is null should throw ArgumentNullException
        /// </summary>
        [Test]
        public void Save_ArgumentNullExceptionKey()
        {
            // arrange
            string key = null;
            string value = "test";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            try
            {
                dashboardConfiguration.Save(key, value);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }        
        }
 
        /// <summary>
        /// Save. Value is null should throw ArgumentNullException
        /// </summary>
        [Test]
        public void Save_ArgumentNullExceptionValue()
        {
            // arrange
            string key = "test";
            string value = null;

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            try
            {
                dashboardConfiguration.Save(key, value);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Save (create) successful test. 
        /// </summary>
        [Test]
        public void SaveCreate_Success()
        {
            // arrange
            string key = "testkeycreate";
            string value = "testvaluecreate";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Assert.False(dashboardConfiguration.Exists(key));

            // act
            dashboardConfiguration.Save(key, value);

            // assert
            string actual = dashboardConfiguration.Get(key);
            Assert.True(dashboardConfiguration.Exists(key));
            Assert.AreEqual(value, actual);

            // cleanup
            dashboardConfiguration.Remove(key);
            Assert.False(dashboardConfiguration.Exists(key));
        }

        /// <summary>
        /// Save (update) successful test. 
        /// </summary>
        [Test]
        public void SaveUpdate_Success()
        {
            // arrange
            string key = "testkeyupdate";
            string value1 = "testvalueupdate1";
            string value2 = "testvalueupdate2";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            dashboardConfiguration.Save(key, value1); 
            Assert.True(dashboardConfiguration.Exists(key));

            // act
            dashboardConfiguration.Save(key, value2);

            // assert
            string actual = dashboardConfiguration.Get(key);
            Assert.True(dashboardConfiguration.Exists(key));
            Assert.AreEqual(value2, actual);

            // cleanup
            dashboardConfiguration.Remove(key);
            Assert.False(dashboardConfiguration.Exists(key));
        }

        /// <summary>
        /// Tests Create method, first arg is null. Should throw
        /// ArgumentNullException
        /// </summary>
        [Test]
        public void CreateFailure_Arg1Null()
        {
            // arrange
            string key = null;
            string value = "test";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // act
            try
            {
                dashboardConfiguration.Create(key, value, configFile);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests Create method, second arg is null. Should throw
        /// ArgumentNullException
        /// </summary>
        [Test]
        public void CreateFailure_Arg2Null()
        {
            // arrange
            string key = "test";
            string value = null;

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // act
            try
            {
                dashboardConfiguration.Create(key, value, configFile);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests Create method, third arg is null. Should throw
        /// ArgumentNullException
        /// </summary>
        [Test]
        public void CreateFailure_Arg3Null()
        {
            // arrange
            string key = "test";
            string value = "test";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            try
            {
                dashboardConfiguration.Create(key, value, null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Test Create method.
        /// </summary>
        [Test]
        public void CreateSuccess()
        {
            // arrange
            string key = "CreateSuccess";
            string value = "CreateSuccess_value1";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // act
            dashboardConfiguration.Create(key, value, configFile);

            // assert
            Assert.True(dashboardConfiguration.Exists(key));
            string actual = dashboardConfiguration.Get(key);
            Assert.AreEqual(value, actual);
        }

        /// <summary>
        /// Tests Update method, first arg is null. Should throw
        /// ArgumentNullException
        /// </summary>
        [Test]
        public void UpdateFailure_Arg1Null()
        {
            // arrange
            string key = null;
            string value = "test";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // act
            try
            {
                dashboardConfiguration.Update(key, value, configFile);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests Update method, second arg is null. Should throw
        /// ArgumentNullException
        /// </summary>
        [Test]
        public void UpdateFailure_Arg2Null()
        {
            // arrange
            string key = "test";
            string value = null;

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // act
            try
            {
                dashboardConfiguration.Update(key, value, configFile);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests Update method, third arg is null. Should throw
        /// ArgumentNullException
        /// </summary>
        [Test]
        public void UpdateFailure_Arg3Null()
        {
            // arrange
            string key = "test";
            string value = "test";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            try
            {
                dashboardConfiguration.Update(key, value, null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests Update method.
        /// </summary>
        [Test]
        public void UpdateSuccess()
        {
            // arrange
            string key = "UpdateSuccess";
            string value1 = "UpdateSuccess_value1";
            string value2 = "UpdateSuccess_value2";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            dashboardConfiguration.Save(key, value1);
            Assert.True(dashboardConfiguration.Exists(key));

            // act
            dashboardConfiguration.Save(key, value2);

            // assert
            string actual = dashboardConfiguration.Get(key);
            Assert.AreEqual(value2, actual);
        }

        /// <summary>
        /// Tests Remove method.
        /// </summary>
        [Test]
        public void RemoveSuccess()
        {
            // arrange
            string key = "RemoveSuccess";
            string value = "RemoveSuccess_value";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            dashboardConfiguration.Save(key, value);
            Assert.True(dashboardConfiguration.Exists(key));

            // act
            dashboardConfiguration.Remove(key);

            // assert
            Assert.False(dashboardConfiguration.Exists(key));
        }

        /// <summary>
        /// Test Remove method with key arg is null.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test]
        public void RemoveFailure_Arg1Null()
        {
            // arrange
            string key = null;

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            try
            {
                dashboardConfiguration.Remove(key);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }

        /// <summary>
        /// Tests Exists method when key does exist. Should return return. 
        /// </summary>
        [Test]
        public void ExistsSuccess_True()
        {
            // arrange
            string key = "ExistsSuccess_True";
            string value = "ExistsSuccess_True_Value";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();
            dashboardConfiguration.Save(key, value);

            // act
            bool actual = dashboardConfiguration.Exists(key);

            // assert
            Assert.True(actual);

            // cleanup
            dashboardConfiguration.Remove(key);
            Assert.False(dashboardConfiguration.Exists(key));
        }

        /// <summary>
        /// Test Exists method when key doesn't exist. Should 
        /// return false.
        /// </summary>
        [Test]
        public void ExistsSuccess_False()
        {
            // arrange
            string key = "ExistsSuccess_False";

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            bool actual = dashboardConfiguration.Exists(key);

            // assert
            Assert.False(actual);
        }

        /// <summary>
        /// Tests Exists method with key arg set to null.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test]
        public void ExistsFailure_Arg1Null()
        {
            // arrange
            string key = null;

            DashboardConfiguration dashboardConfiguration = new DashboardConfiguration();

            // act
            try
            {
                dashboardConfiguration.Exists(key);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                Assert.Fail("Invalid exception");
            }
        }      
    }
}
