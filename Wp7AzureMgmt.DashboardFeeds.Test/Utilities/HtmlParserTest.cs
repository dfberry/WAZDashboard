﻿// -----------------------------------------------------------------------
// <copyright file="HtmlParserTest.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HtmlAgilityPack;
    using NUnit.Framework;
    using Wp7AzureMgmt.DashboardFeeds.DataSources;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Tests for HtmlParser
    /// </summary>
    [TestFixture]
    public class HtmlParserTest
    {
        /// <summary>
        /// Raw html file contents
        /// </summary>
        private string htmlFileContents;

        /// <summary>
        /// Last known value of feed count
        /// </summary>
        private int currentFeedCount = 88;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParserTest" /> class.
        /// </summary>
        public HtmlParserTest()
        {
            // get test string to parse
            FakeDatasource fakeDatasource = new FakeDatasource();
            fakeDatasource.HtmlFileName = Setup.RunBeforeTests();
            this.htmlFileContents = fakeDatasource.GetHtml();
        }

        /// <summary>
        /// A test for ConvertFeedItemListToRSSFeed
        /// </summary>
        [Test]
        public void ConvertFeedItemListToRSSFeedTest()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            // act
            RssFeed actual = target.ConvertFeedItemListToRSSFeed(target.HtmlParserFeedItems.ToList());

            // arrange
            Assert.IsNotNull(actual);
            Assert.AreEqual("RSSCode", actual.FeedCode);
            Assert.AreEqual("LocationName", actual.LocationName);
            Assert.AreEqual(@"<a href='https://www.windowsazurestatus.com/windowsazure/support/status/RSSLink'>rss</a>", actual.RSSLink);
            Assert.AreEqual("ServiceName", actual.ServiceName);
        }

        /// <summary>
        /// A test for FindParseFeedItemValue
        /// </summary>
        [Test]
        public void FindParseFeedItemValueLocationNameTest()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.LocationName;

            // act
            string actual = target.FindParseFeedItemValue(target.HtmlParserFeedItems.ToList(), expectedType);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.RSSCode;

            // act
            string actual = target.FindParseFeedItemValue(target.HtmlParserFeedItems.ToList(), expectedType);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.RSSLink;

            // act
            string actual = target.FindParseFeedItemValue(target.HtmlParserFeedItems.ToList(), expectedType);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            HTMLParserFeedItemType expectedType = HTMLParserFeedItemType.ServiceName;

            // act
            string actual = target.FindParseFeedItemValue(target.HtmlParserFeedItems.ToList(), expectedType);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix); 
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
        public void GetRSSFeedPropertyDefinitionTest_FeedCode_Success()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "MainContent_tcStatus_tpAllStatusToday_gvAllStatusToday_hdnRSSFeedCode_0";
            HTMLParserFeedItemDefinition[] definitionList = target.HtmlDefinitions;
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitionList.ToList(), itemAttributeName);

            // assert
            Assert.AreEqual(definitionList[5], actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_lblServiceName"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_ServiceName_Success()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "MainContent_tcStatus_tpAllStatusToday_gvAllStatusToday_lblServiceName_0";
            HTMLParserFeedItemDefinition[] definitionList = target.HtmlDefinitions; 
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(definitionList.ToList(), itemAttributeName);

            // assert
            Assert.AreEqual(definitionList[2], actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hyperlinkRSS"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_hyperlinkRS_Success()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "MainContent_tcStatus_tpAllStatusToday_gvAllStatusToday_hyperlinkRSS_0";
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

            // assert
            Assert.AreEqual(target.HtmlDefinitions[4], actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_lblRegionName"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_RegionName_Success4()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "MainContent_tcStatus_tpAllStatusToday_gvAllStatusToday_lblRegionName_0";
            HTMLParserFeedItemDefinition expected = new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "RegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = ContentTag.InnerHtml };
            HTMLParserFeedItemDefinition actual;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

            // assert
            Assert.AreEqual(target.HtmlDefinitions[3], actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hdnServiceId"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure1()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnTerviceId";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for GetRSSFeedPropertyDefinition
        /// Example Id: "ctl00_MainContent_gvStatusToday_ctl02_hdnTegionId"
        /// </summary>
        [Test]
        public void GetRSSFeedPropertyDefinitionTest_Failure2()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "MainContent_tcStatus_tpAllStatusToday_gvAllStatusToday_hdnTegionId_0";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_hdnStatusColor";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_imgStatus";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_Panel2";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

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
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);

            string itemAttributeName = "ctl00_MainContent_gvStatusToday_ctl02_lblDetails";
            HTMLParserFeedItemDefinition actual = null;

            // act
            actual = target.GetRSSFeedPropertyDefinition(target.HtmlDefinitions.ToList(), itemAttributeName);

            // assert
            Assert.IsNull(actual);
        }             

        /// <summary>
        /// A test for ParseFeedNodeTest_Fail1. 
        /// Test string contains no "td" nodes,
        /// only "th" so the test fails.
        /// </summary>
        [Test]
        public void ParseFeedNodeTest_Fail1()
        {
            // arrange

            // setup HTML Model
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);
            HtmlDocument doc = new HtmlDocument();

            // load 
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode trnode = doc.CreateElement("tr");
            doc.OptionUseIdAttribute = true;
            trnode.Name = "tr";
            trnode.InnerHtml = RSSFeedResponseResource.TestTRFail1;

            // act
            RssFeed actual = target.ParseFeedNode(trnode);

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
            string uriPrefix = string.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode trnode = doc.CreateElement("tr");
            doc.OptionUseIdAttribute = true;
            trnode.Name = "tr";
            trnode.InnerHtml = RSSFeedResponseResource.TestTRSuccess1;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            RssFeed actual;

            // act
            actual = target.ParseFeedNode(trnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.FeedCode);
            Assert.IsNotNull(actual.LocationName);
            Assert.IsNotNull(actual.RSSLink);
            Assert.IsNotNull(actual.ServiceName);
        }

        /// <summary>
        /// A test for ParseFeedNodeItems
        /// </summary>
        [Test]
        public void ParseFeedNodeItemsTest_ServiceName_Success()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.ParseFeedNodeItems1;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.ServiceName && item.Value == "Access Control 1.0").Count());
        }

        [Test]
        public void ParseFeedNodeItemsTest_RegionName_Success()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.ParseFeedNodeItems2;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.LocationName && item.Value == "East Asia").Count());
        }

        public void ParseFeedNodeItemsTest_ServiceId_Success()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.ParseFeedNodeItems3;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.ServiceId && item.Value == "6").Count());
        }

        public void ParseFeedNodeItemsTest_RegionId_Success()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.ParseFeedNodeItems4;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.ServiceId && item.Value == "7").Count());
        }

        public void ParseFeedNodeItemsTest_Feedcode_Success()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.ParseFeedNodeItems5;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.RSSCode && item.Value == "NSACSEA").Count());
        }

        public void ParseFeedNodeItemsTest_hyperlinkRSS_Success()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.ParseFeedNodeItems6;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Where(item => item.Name == HTMLParserFeedItemType.RSSCode && item.Value == "RSSFeed.aspx?RSSFeedCode=NSACSEA").Count());
        }

        /// <summary>
        /// A test for ParseFeedNodeItems. 
        /// Test string doesn't contain a "div"
        /// or "input" so there is nothing to parse.
        /// </summary>
        [Test]
        public void ParseFeedNodeItemsTest_Fail1()
        {
            // arrange
            string uriPrefix = string.Empty;

            // setup HTML Model
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            HtmlNode tdnode = doc.CreateElement("td");
            doc.OptionUseIdAttribute = true;
            tdnode.Name = "td";
            tdnode.InnerHtml = RSSFeedResponseResource.TestTDFail1;

            // setup Test Object
            HtmlParser target = new HtmlParser(uriPrefix);
            List<HTMLParserFeedItem> actual;

            // act
            actual = target.ParseFeedNodeItems(tdnode);

            // assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// A test for ParseHtmlForFeeds
        /// </summary>
        [Test]
        public void ParseHtmlForUrlsTest()
        {
            // arrange
            string uriPrefix = string.Empty;
            HtmlParser target = new HtmlParser(uriPrefix);
            string html = this.htmlFileContents;
            RssFeeds actual;

            // act
            actual = target.ParseHtmlForFeeds(html);

            // assert
            Assert.IsNotNull(actual);

            // DFB? where is a good place to put this test number
            Assert.AreEqual(this.currentFeedCount, actual.Feeds.Count());
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
            string uriPrefix = string.Empty;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("a");
            node.SetAttributeValue("href", "RSSFeed.aspx?RSSFeedCode=NSACSEA");

            HtmlParser target = new HtmlParser(uriPrefix);
            string returnAttributeName = "href";
            string expected = "RSSFeed.aspx?RSSFeedCode=NSACSEA";
            string actual;

            // act
            actual = target.ParseTagForAttributeValue(node, returnAttributeName);

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
            string uriPrefix = string.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("input");
            node.SetAttributeValue("value", "NSACSSEA");

            HtmlParser target = new HtmlParser(uriPrefix);
            string returnAttributeName = "value";
            string expected = "NSACSSEA";
            string actual;

            // act
            actual = target.ParseTagForAttributeValue(node, returnAttributeName);

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
            string uriPrefix = string.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.htmlFileContents);
            doc.OptionUseIdAttribute = true;
            HtmlNode node = doc.CreateElement("span");
            node.InnerHtml = "Access Control";

            HtmlParser target = new HtmlParser(uriPrefix);
            string expected = "Access Control";
            string actual;

            // act
            actual = target.InnerHTML(node);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
