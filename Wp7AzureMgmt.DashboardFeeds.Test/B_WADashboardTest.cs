// -----------------------------------------------------------------------
// <copyright file="B_WADashboardTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;
    using NUnit.Framework;
    using Wp7AzureMgmt.Dashboard;
    using Wp7AzureMgmt.Dashboard.DataSources;
    using Wp7AzureMgmt.Dashboard.Models;
    using Wp7AzureMgmt.DashboardFeeds.Models;    

    /// <summary>
    /// This is a test class for WADashboardTest and is intended
    /// to contain all WADashboardTest Unit Tests
    /// </summary>
    [TestFixture]
    public class B_WADashboardTest
    {
        /// <summary>
        /// Known good version of Windows Azure Dashboard RSS Feeds html page
        /// </summary>
        private string testFileContents = null;

        /// <summary>
        /// Regardless of where feeds are from, how many should there be
        /// </summary>
        private int lastknownRSSFeedCount = 0;
        
        /// <summary>
        /// Get html content and count of expected feeds
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            this.ContentFrom();
            this.lastknownRSSFeedCount = DashboardTestUtilities.ConfigFeedCount();
        }

         /// <summary>
        /// Depends on finding correct TD Node 
        /// </summary>
        /// <returns>int as count of feeds</returns>
        public int FindFeedCountByRegEx()
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            MatchCollection matches = Regex.Matches(this.testFileContents, "<td class=\"cellStyleTodayService\">", options);

            return matches.Count;
        }

        /// <summary>
        /// A test for WADashboard Constructor
        /// </summary>
        [Test]
        public void WADashboardConstructorUriTest()
        {
            // arrange
            Uri uri = new Uri(DashboardTestUtilities.GrabDefaultUriFromConfig()); 

            // act
            WADashboard target = new WADashboard(uri);

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual(DateTime.MinValue, target.BuildDate());
        }

        /// <summary>
        /// A test for WADashboard Constructor
        /// </summary>
        [Test]
        public void WADashboardConstructorStringTest()
        {
            // arrange
            string html = "hello there";

            // act
            WADashboard target = new WADashboard(html);

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual(DateTime.MinValue, target.BuildDate());
        }

        /// <summary>
        /// A test for WADashboard Constructor
        /// </summary>
        [Test]
        public void WADashboardConstructorDefaultTest()
        {
            // act
            WADashboard target = new WADashboard();

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual(DateTime.MinValue, target.BuildDate());
        }

        /// <summary>
        /// A test for BuildDate
        /// </summary>
        [Test]
        public void BuildDateTestMinValue()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            DateTime actual;
            
            // act
            actual = target.BuildDate();

            // assert
            Assert.AreEqual(DateTime.MinValue, actual);
        }

        /// <summary>
        /// A test for BuildDate
        /// </summary>
        [Test]
        public void BuildDateTestNotMinValue()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            DateTime now = DateTime.Now;
            DateTime actual;
            target.List();

            // act
            actual = target.BuildDate();

            // assert
            Assert.LessOrEqual(now.Ticks, actual.Ticks);
        }

        /// <summary>
        /// Down and dirt regex to see if count is good
        /// </summary>
        [Test] public void ListCountVersusKnownCount()
        {
            // arrange
            int regexCount = this.FindFeedCountByRegEx();
            int configfileCount = DashboardTestUtilities.ConfigFeedCount();

            // act/assert
            Assert.AreEqual(configfileCount, regexCount);
        }

        /// <summary>
        /// A test for ConvertFeedItemListToRSSFeed
        /// </summary>
        [Test]
        public void ConvertFeedItemListToRSSFeedTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItem> list = this.GetHTMLParserFeedItemList();

            // act
            RSSFeed actual = target.ConvertFeedItemListToRSSFeed(list);

            // arrange
            Assert.IsNotNull(actual);
            Assert.AreEqual("RSSCode", actual.FeedCode);
            Assert.AreEqual("LocationName", actual.LocationName);
            Assert.AreEqual("RSSLink", actual.RSSLink);
            Assert.AreEqual("ServiceName", actual.ServiceName);
        }

        /// <summary>
        /// A test for FindFeeds
        /// </summary>
        [Test]
        public void FindFeedsTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            // act
            List<RSSFeed> actual = target.FindFeeds().ToList();

            // arrange
            Assert.IsNotNull(actual);
            Assert.GreaterOrEqual(actual.Count, this.lastknownRSSFeedCount);
        }

        /// <summary>
        /// A test for FindParseFeedItemValue
        /// </summary>
        [Test]
        public void FindParseFeedItemValueLocationNameTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> list = this.GetHTMLParserFeedItemList();

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.LocationName;

            // act
            string actual = target.FindParseFeedItemValue(list, expectedType);

            // assert
            Assert.AreEqual(expectedType.ToString(), actual);
        }

        /// <summary>
        /// A test for FindParseFeedItemValue
        /// </summary>
        [Test]
        public void FindParseFeedItemValueRSSCodeTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> list = this.GetHTMLParserFeedItemList();

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.RSSCode;

            // act
            string actual = target.FindParseFeedItemValue(list, expectedType);

            // assert
            Assert.AreEqual(expectedType.ToString(), actual);
        }

        /// <summary>
        /// A test for FindParseFeedItemValue
        /// </summary>
        [Test]
        public void FindParseFeedItemValueRSSLinkTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> list = this.GetHTMLParserFeedItemList();

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.RSSLink;

            // act
            string actual = target.FindParseFeedItemValue(list, expectedType);

            // assert
            Assert.AreEqual(expectedType.ToString(), actual);
        }

        /// <summary>
        /// A test for FindParseFeedItemValue
        /// </summary>
        [Test]
        public void FindParseFeedItemValueServiceNameTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> list = this.GetHTMLParserFeedItemList(); 

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.ServiceName;

            // act
            string actual = target.FindParseFeedItemValue(list, expectedType);

            // assert
            Assert.AreEqual(expectedType.ToString(), actual);
        }

        /// <summary>
        /// A test for GetRSSFeedProperty
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents); 
            HtmlNode node = null; 
            List<HTMLParserFeedItemDefinition> definitionList = null; 
            HTMLParserFeedItem expected = null; 
            HTMLParserFeedItem actual;

            // act
            actual = target.GetRSSFeedProperty(node, definitionList);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// example Id: "ctl00_MainContent_gvStatusToday_ctl02_hdnRSSFeedCode"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Success1()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnRSSFeedCode";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = TagContent.AttributeValue };
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_lblServiceName"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Success2()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_lblServiceName";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = TagContent.InnerHtml };
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hyperlinkRSS"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Success3()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hyperlinkRSS";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = TagContent.AttributeValue };
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_lblRegionName"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Success4()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_lblRegionName";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = TagContent.InnerHtml };
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hdnServiceId"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure1()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnServiceId";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hdnRegionId"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure2()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnRegionId";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hdnStatusColor"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure3()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnStatusColor";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_imgStatus"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure4()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_imgStatus";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_Panel2"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure5()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_Panel2";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.IsNull(actual);
        } 
           
        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_lblDetails"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure6()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_lblDetails";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitions, itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }             
        
        /// <summary>
        /// A test for List - Public/External collection of RSSFeeds
        /// </summary>
        [Test]
        public void ListTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents); 
            IEnumerable<RSSFeed> actual;

            // act
            actual = target.List();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(this.lastknownRSSFeedCount, actual.Count());
        }

        /// <summary>
        /// A test for OPML
        /// </summary>
        [Test]
        public void OPMLTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents); 
            string actual;

            // act
            actual = target.OPML();

            // assert
            Assert.IsNotNullOrEmpty(actual);

            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            MatchCollection matches = Regex.Matches(actual, "<outline", options);

            Assert.IsTrue(actual.Contains("<outline"));
            Assert.GreaterOrEqual(matches.Count, this.lastknownRSSFeedCount);
        }

        /// <summary>
        /// A test for ParseFeedNode
        /// </summary>
        [Test]
        public void ParseFeedNodeTest_Fail1()
        {
            // arrange

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            HtmlNode trnode = doc.CreateElement("tr");
            doc.OptionUseIdAttribute = true;
            trnode.Name = "tr";
            trnode.InnerHtml = RSSFeedResponseResource.TestTRFail1;

            // setup Test Object
            WADashboard target = new WADashboard(this.testFileContents);
            RSSFeed actual;

            // act
            actual = target.ParseFeedNode(trnode);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for ParseFeedNode
        /// </summary>
        [Test]
        public void ParseFeedNodeTest_Success()
        {
            // arrange

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            HtmlNode trnode = doc.CreateElement("tr");
            doc.OptionUseIdAttribute = true;
            trnode.Name = "tr";
            trnode.InnerHtml = RSSFeedResponseResource.TestTRSuccess1;

            // setup Test Object
            WADashboard target = new WADashboard(this.testFileContents);
            RSSFeed actual;

            // act
            actual = target.ParseFeedNode(trnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.IsNotNullOrEmpty(actual.FeedCode);
            Assert.IsNotNullOrEmpty(actual.LocationName);
            Assert.IsNotNullOrEmpty(actual.RSSLink);
            Assert.IsNotNullOrEmpty(actual.ServiceName);
        }

        /// <summary>
        /// A test for ParseFeedNodeItems
        /// </summary>
        [Test]
        public void ParseFeedNodeItemsTest_Success1()
        {
            // arrange

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDSuccess_cellStyleTodayDetails;

            // setup Test Object
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.RSSLink && item.Value == "RSSFeed.aspx?RSSFeedCode=DSSSCU").Count());
        }

        /// <summary>
        /// A test for ParseFeedNodeItems
        /// </summary>
        [Test]
        public void ParseFeedNodeItemsTest_Success2()
        {
            // arrange

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDSuccess_cellStyleTodayService;

            // setup Test Object
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.ServiceName && item.Value == "SQL Azure Data Sync").Count());
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.LocationName && item.Value == "South Central US").Count());
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.RSSCode && item.Value == "DSSSCU").Count());
        }

        /// <summary>
        /// A test for ParseFeedNodeItems
        /// </summary>
        [Test]
        public void ParseFeedNodeItemsTest_Fail1()
        {
            // arrange

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDFail1;

            // setup Test Object
            WADashboard target = new WADashboard(this.testFileContents);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for ParseHtmlForUrls
        /// </summary>
        [Test]
        public void ParseHtmlForUrlsTest()
        {
            // arrange
            WADashboard target = new WADashboard(this.testFileContents);
            string html = this.testFileContents;
            IEnumerable<RSSFeed> actual;

            // act
            actual = target.ParseHtmlForUrls(html);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(this.lastknownRSSFeedCount, actual.Count());
        }

        /// <summary>
        /// A test for ParseTagForAttributeValue
        /// Example Id:
        /// <code><a id="ctl00_MainContent_gvStatusToday_ctl02_hyperlinkRSS" href="RSSFeed.aspx?RSSFeedCode=NSACSEA" target="_parent"><img src="Images/Rss_New.png" border="0" /></a></code>
        /// </summary>
        [Test]
        public void ParseTagForAttributeValueTest_a_href()
        {
            // arrange
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("a");
            node.SetAttributeValue("href", "RSSFeed.aspx?RSSFeedCode=NSACSEA");

            WADashboard target = new WADashboard(this.testFileContents);
            string returnAttributeName = "href";
            string expected = "RSSFeed.aspx?RSSFeedCode=NSACSEA";
            string actual;

            // act
            actual = target.ParseTagForAttributeValue(node, /*attributeNameValue,*/ returnAttributeName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for ParseTagForAttributeValue
        /// <code><input type="hidden" name="ctl00$MainContent$gvStatusToday$ctl06$hdnRSSFeedCode" id="ctl00_MainContent_gvStatusToday_ctl06_hdnRSSFeedCode" value="NSACSSEA" /></code>
        /// </summary>
        [Test]
        public void ParseTagForAttributeValueTest_input()
        {
            // arrange
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("input");
            node.SetAttributeValue("value", "NSACSSEA");

            WADashboard target = new WADashboard(this.testFileContents);
            string returnAttributeName = "value";
            string expected = "NSACSSEA";
            string actual;

            // act
            actual = target.ParseTagForAttributeValue(node, /*attributeNameValue,*/ returnAttributeName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for ParseTagForInnerHTML
        /// </summary>
        [Test]
        public void ParseTagForInnerHTMLTest()
        {
            // arrange
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.testFileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("span");
            node.InnerHtml = "Access Control";

            WADashboard target = new WADashboard(this.testFileContents); 
            string expected = "Access Control";
            string actual;

            // act
            actual = target.InnerHTML(node/*, attributeNameValue*/);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Definition of html tags to parse and what to get 
        /// from each one.
        /// </summary>
        /// <returns>List of HTMLParserFeedItemDefintion</returns>
        internal List<HTMLParserFeedItemDefinition> GetHTMLParserFeedItemDefintionList()
        {
            List<HTMLParserFeedItemDefinition> definitions = new List<HTMLParserFeedItemDefinition>();

            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = TagContent.InnerHtml });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = TagContent.InnerHtml });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = TagContent.AttributeValue });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = TagContent.AttributeValue });

            return definitions;
        }

        /// <summary>
        /// List of Feed data items
        /// </summary>
        /// <returns>List of HTMLParserFeedItem</returns>
        internal List<HTMLParserFeedItem> GetHTMLParserFeedItemList()
        {
            List<HTMLParserFeedItem> list = new List<HTMLParserFeedItem>();

            list.Add(new HTMLParserFeedItem { Name = HTMLParserFeedItemType.LocationName, Value = "LocationName" });
            list.Add(new HTMLParserFeedItem { Name = HTMLParserFeedItemType.RSSCode, Value = "RSSCode" });
            list.Add(new HTMLParserFeedItem { Name = HTMLParserFeedItemType.RSSLink, Value = "RSSLink" });
            list.Add(new HTMLParserFeedItem { Name = HTMLParserFeedItemType.ServiceName, Value = "ServiceName" });

            return list;
        }

         /// <summary>
        /// Determine if it is web or file
        /// </summary>
        private void ContentFrom()
        {
            string tempcontentfrom = DashboardTestUtilities.ConfigContentFrom();
            if (string.IsNullOrEmpty(tempcontentfrom))
            {
                throw new Exception("Expect config file to have AppSettings[\"ContentFrom\"] but not found");
            }

            switch (tempcontentfrom)
            {
                case "File":
                    this.testFileContents = DashboardTestUtilities.TestFileContents();

                    // not keeping this around - just want to get the count from file
                    WADashboard dashboard = new WADashboard(this.testFileContents);
                    List<RSSFeed> list = dashboard.FindFeeds().ToList();
                    DashboardTestUtilities.ChangeConfiguration("LastKnownRSSFeedCount", list.Count().ToString());
                    break;
                case "Mix":
                    break;
                case "Uri":
                    this.testFileContents = DashboardTestUtilities.AZUriContents();

                    // not keeping this around - just want to get the count from file
                    WADashboard dashboard2 = new WADashboard(this.testFileContents);
                    List<RSSFeed> list2 = dashboard2.FindFeeds().ToList();
                    DashboardTestUtilities.ChangeConfiguration("LastKnownRSSFeedCount", list2.Count().ToString());

                    break;
            }
        }
    }
}