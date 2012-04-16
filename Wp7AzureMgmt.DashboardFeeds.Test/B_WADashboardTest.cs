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
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Factories;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// This is a test class for WADashboardTest and is intended
    /// to contain all WADashboardTest Unit Tests
    /// </summary>
    [TestFixture]
    public class B_WADashboardTest 
    {
        /// <summary>
        /// Regardless of where feeds are from, how many should there be
        /// </summary>
        private int lastknownRSSFeedCount;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="B_WADashboardTest" /> class.
        /// </summary>
        public B_WADashboardTest()
        {
            DashboardConfiguration = new DashboardConfiguration();
            DashboardHttp = new DashboardHttp(new Uri(DashboardConfiguration.GetAzureUri));
            DashboardFileFactory = new DashboardFileFactory();
            DashboardFile = new DashboardFile();

            // set file name
            DashboardFile.FileName = DashboardConfiguration.GetTestFileName;
        }

        /// <summary>
        /// Gets or sets DashboardHttp object for 
        /// web requests. 
        /// </summary>
        private DashboardHttp DashboardHttp { get; set; }

        /// <summary>
        /// Gets or sets DashboardConfiguration for App.Config
        /// settings. 
        /// </summary>
        private DashboardConfiguration DashboardConfiguration { get; set; }

        /// <summary>
        /// Gets or sets DashboardFile
        /// </summary>
        private DashboardFile DashboardFile { get; set; }

        /// <summary>
        /// Gets or sets DashboardFileFactory
        /// </summary>
        private DashboardFileFactory DashboardFileFactory { get; set; }  
        
        /// <summary>
        /// Get html content and count of expected feeds
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            this.TestContent();
            this.lastknownRSSFeedCount = DashboardConfiguration.GetFeedCount;

            Uri getUri = new Uri(DashboardConfiguration.GetDefaultUri);
            DashboardHttp target = new DashboardHttp(getUri); 
        }

         /// <summary>
        /// Depends on finding correct TD Node 
        /// </summary>
        /// <returns>int as count of feeds</returns>
        public int FindFeedCountByRegEx()
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            MatchCollection matches = Regex.Matches(this.DashboardFile.FileContents, "<td class=\"cellStyleTodayService\">", options);

            return matches.Count;
        }

        /// <summary>
        /// A test for Dashboard Constructor
        /// </summary>
        [Test]
        public void WADashboardConstructorUriTest()
        {
            // arrange
            Uri uri = new Uri(DashboardConfiguration.GetDefaultUri);

            // act
            Dashboard target = new Dashboard(uri);

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual(DateTime.MinValue, target.BuildDate());
        }

        /// <summary>
        /// A test for Dashboard Constructor
        /// </summary>
        [Test]
        public void WADashboardConstructorStringTest()
        {
            // arrange
            string html = "hello there";

            // act
            Dashboard target = new Dashboard(html);

            // assert
            Assert.IsNotNull(target);
            Assert.AreEqual(DateTime.MinValue, target.BuildDate());
        }

        /// <summary>
        /// A test for Dashboard Constructor
        /// </summary>
        [Test]
        public void WADashboardConstructorDefaultTest()
        {
            // act
            Dashboard target = new Dashboard();

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            int configfileCount = DashboardConfiguration.GetFeedCount;

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents); 
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnRSSFeedCode";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = ContentTag.AttributeValue };
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_lblServiceName";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = ContentTag.InnerHtml };
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hyperlinkRSS";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = ContentTag.AttributeValue };
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

            List<HTMLParserFeedItemDefinition> definitions = this.GetHTMLParserFeedItemDefintionList();

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_lblRegionName";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = ContentTag.InnerHtml };
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);

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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents); 
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents); 
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            HtmlNode trnode = doc.CreateElement("tr");
            doc.OptionUseIdAttribute = true;
            trnode.Name = "tr";
            trnode.InnerHtml = RSSFeedResponseResource.TestTRFail1;

            // setup Test Object
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            HtmlNode trnode = doc.CreateElement("tr");
            doc.OptionUseIdAttribute = true;
            trnode.Name = "tr";
            trnode.InnerHtml = RSSFeedResponseResource.TestTRSuccess1;

            // setup Test Object
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDSuccess_cellStyleTodayDetails;

            // setup Test Object
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDSuccess_cellStyleTodayService;

            // setup Test Object
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDFail1;

            // setup Test Object
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
            string html = this.DashboardFile.FileContents;
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("a");
            node.SetAttributeValue("href", "RSSFeed.aspx?RSSFeedCode=NSACSEA");

            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("input");
            node.SetAttributeValue("value", "NSACSSEA");

            Dashboard target = new Dashboard(this.DashboardFile.FileContents);
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
            doc.LoadHtml(this.DashboardFile.FileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("span");
            node.InnerHtml = "Access Control";

            Dashboard target = new Dashboard(this.DashboardFile.FileContents); 
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

            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = ContentTag.InnerHtml });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = ContentTag.InnerHtml });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = ContentTag.AttributeValue });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = ContentTag.AttributeValue });

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
        /// Determine if it is web or file.
        /// If web, grab copy to file so rest
        /// of testing can go off file instead of
        /// hitting Azure for remaining tests.
        /// </summary>
        private void TestContent()
        {
            string tempcontentfrom = DashboardConfiguration.GetContentFrom;
            if (string.IsNullOrEmpty(tempcontentfrom))
            {
                throw new Exception("Expect config file to have AppSettings[\"ContentFrom\"] but not found");
            }

            string filename = DateTime.Now.Ticks.ToString() + ".html";

            switch (tempcontentfrom)
            {
                case "File":
                    // for file: open & read contents to test framework
                    this.DashboardFile.FileContents = this.DashboardFileFactory.Read(DashboardFile);
                    this.DashboardFile.FileName = filename;

                    // set count of Feeds for test
                    this.SetFeedListCount();

                    break;
                case "Uri":
                    // for web: open, read, set contents to test framework
                    this.DashboardFile.FileName = filename;
                    this.DashboardFile.FileContents = this.DashboardHttp.GetRequest();
                    this.DashboardFileFactory.Save(this.DashboardFile);
                    this.DashboardConfiguration.ChangeAppSettingsConfiguration("TestFile", this.DashboardFile.FileName);

                    // set count of Feeds for test
                    this.SetFeedListCount();

                    break;
                default:
                    // for web: open, read, set contents to test framework
                    this.DashboardFile.FileName = filename;
                    this.DashboardFile.FileContents = this.DashboardHttp.GetRequest();
                    this.DashboardFileFactory.Save(this.DashboardFile);
                    this.DashboardConfiguration.ChangeAppSettingsConfiguration("TestFile", this.DashboardFile.FileName);

                    // set count of Feeds for test
                    this.SetFeedListCount();

                    break;
            }
        }

        /// <summary>
        /// Parse feeds (assuming this works), get count of list,
        /// set list count to App.Config[LastKnownRSSFeedCount].
        /// Tests will grab count to verify correctness of parse.
        /// Recognize that this depends on successful parsing. If
        /// parsing fails, count fails, so test parsing will fail and 
        /// test verification will fail. 
        /// TBD: Not the best way to do this. Think of something else.
        /// </summary>
        private void SetFeedListCount()
        {
            // not keeping this around - just want to get the count from file
            Dashboard dashboard = new Dashboard(this.DashboardFile.FileContents);
            List<RSSFeed> list = dashboard.FindFeeds().ToList();
            DashboardConfiguration.ChangeAppSettingsConfiguration("LastKnownRSSFeedCount", list.Count().ToString());
        }
    }
}