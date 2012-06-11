// -----------------------------------------------------------------------
// <copyright file="HtmlParser.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Wp7AzureMgmt.DashboardFeeds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using Wp7AzureMgmt.DashboardFeeds.Enums;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// Main parsing class that moves data between an HTML file into RssFeeds
    /// </summary>
    internal class HtmlParser
    {
        /// <summary>
        /// Default Parallel Options to no limit
        /// </summary>
#if DEBUG
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
#else
        private ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
#endif

        /// <summary>
        /// Where to find Azure Rss Feeds - base of Uri. Rest of Uri is in 
        /// each RssFeed.
        /// </summary>
        private string uriPrefix;

        /// <summary>
        /// Build definitions - how to parse html
        /// </summary>
        private HTMLParserFeedItemDefinition[] htmlDefinitions = new HTMLParserFeedItemDefinition[] 
        {
            new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = ContentTag.InnerHtml },
            new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = ContentTag.InnerHtml },
            new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = ContentTag.AttributeValue },
            new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = ContentTag.AttributeValue } 
        };

        /// <summary>
        /// Build rssFeedArray for testing
        /// </summary>
        private HTMLParserFeedItem[] htmlParserFeedItems = new HTMLParserFeedItem[] 
        {
            new HTMLParserFeedItem() { Name = HTMLParserFeedItemType.LocationName, Value = "LocationName" },
            new HTMLParserFeedItem() { Name = HTMLParserFeedItemType.RSSCode, Value = "RSSCode" },
            new HTMLParserFeedItem() { Name = HTMLParserFeedItemType.RSSLink, Value = "RSSLink" },
            new HTMLParserFeedItem() { Name = HTMLParserFeedItemType.ServiceName, Value = "ServiceName" },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParser" /> class.
        /// </summary>
        /// <param name="uriPrefix">prefix of Uri to rss issues</param>
        public HtmlParser(string uriPrefix)
        {
            this.uriPrefix = uriPrefix;
        }

        /// <summary>
        /// Gets HtmlDefinitions
        /// </summary>
        public HTMLParserFeedItemDefinition[] HtmlDefinitions
        {
            get
            {
                return this.htmlDefinitions;
            }
        }

        /// <summary>
        /// Gets HtmlParserFeedItems
        /// </summary>
        public HTMLParserFeedItem[] HtmlParserFeedItems
        {
            get
            {
                return this.htmlParserFeedItems;
            }
        }

        /// <summary>
        /// Sets uriPrefix
        /// </summary>
        public string UriPrefix
        {
            set
            {
                this.uriPrefix = value;
            }
        }

        /// <summary>
        /// Hunt for containing tags and grab RSS feed details
        /// </summary>
        /// <param name="html">entire html page content</param>
        /// <returns>IEnumerable of RSSFeed</returns>
        public RssFeeds ParseHtmlForFeeds(string html)
        {
            // if param is empty, don't do any work
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException("html string is null");
            }

            RssFeeds rssFeeds = null;
            List<RssFeed> feeds = new List<RssFeed>();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            // Assuming top tr nodes are what we want
            Parallel.ForEach(
                doc.DocumentNode.SelectNodes("//tr"),
                this.options,
                trNode =>
                {
                    if (trNode == null)
                    {
                        throw new Exception("trNode == null");
                    }

                    RssFeed rssfeed = ParseFeedNode(trNode);

                    if (rssfeed != null)
                    {
                        feeds.Add(rssfeed);
                    }
                });

            if ((feeds != null) && (feeds.Count > 0))
            {
                rssFeeds = new RssFeeds();
                rssFeeds.Feeds = feeds;
                rssFeeds.FeedDate = DateTime.UtcNow;
                rssFeeds.UriPrefix = this.uriPrefix;
            }

            return rssFeeds;
        }

        /// <summary>
        /// Parse the top level TR nodes
        /// </summary>
        /// <param name="trnode">html TR node</param>
        /// <returns>RSSFeed parsed from TR node</returns>
        public RssFeed ParseFeedNode(HtmlNode trnode)
        {
            List<HTMLParserFeedItem> returnedlist = new List<HTMLParserFeedItem>();

            Parallel.ForEach(
                trnode.ChildNodes,
                this.options,
                tdNode =>
                {
                    if ((tdNode != null)
                        && (tdNode.Name == "td")
                        && (tdNode.Attributes["class"] != null)
                        && (tdNode.Attributes["class"].Value != string.Empty)
                        && ((tdNode.Attributes["class"].Value == "cellStyleTodayStatus") || (tdNode.Attributes["class"].Value == "cellStyleTodayService")))
                    {
                        List<HTMLParserFeedItem> innerlist = ParseFeedNodeItems(tdNode);
                        if ((innerlist != null) && (innerlist.Count > 0))
                        {
                            returnedlist.AddRange(innerlist);
                        }
                    }
                });

            if ((returnedlist != null) && (returnedlist.Count > 0))
            {
                return this.ConvertFeedItemListToRSSFeed(returnedlist);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse the TD nodes
        /// </summary>
        /// <param name="tdnode">contains all data points to parse</param>
        /// <returns>List of HTMLParserFeedItem</returns>
        public List<HTMLParserFeedItem> ParseFeedNodeItems(HtmlNode tdnode)
        {
            if (tdnode == null)
            {
                return null;
            }

            List<HTMLParserFeedItem> list = new List<HTMLParserFeedItem>();

            Parallel.ForEach(
                tdnode.ChildNodes,
                this.options,
                node =>
                {
                    HTMLParserFeedItem item = null;

                    List<HTMLParserFeedItemDefinition> defintionlist = htmlDefinitions.ToList();

                    switch (node.Name.ToLower())
                    {
                        case "div":
                            HtmlNodeCollection divNodeChildNodes = node.ChildNodes;

                            Parallel.ForEach(
                                divNodeChildNodes,
                                this.options,
                                divChildNode =>
                                {
                                    if ((divChildNode.Name == "span") || (divChildNode.Name == "a"))
                                    {
                                        HTMLParserFeedItem item2 = GetRSSFeedProperty(divChildNode, defintionlist);
                                        if (item2 != null)
                                        {
                                            list.Add(item2);
                                        }
                                    }
                                });
                            break;
                        case "input":
                            item = GetRSSFeedProperty(node, defintionlist);
                            if (item != null)
                            {
                                list.Add(item);
                            }

                            break;
                        default:
                            break;
                    }
                });

            if ((list != null) && (list.Count > 0))
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convert from List of HTMLParserFeedItem into RSSFeed
        /// </summary>
        /// <param name="list">list of HTMLParserFeedItems</param>
        /// <returns>RSSFeed converted from Feed data items</returns>
        public RssFeed ConvertFeedItemListToRSSFeed(List<HTMLParserFeedItem> list)
        {
            if ((list != null) && (list.Count == 4))
            {
                return new RssFeed
                {
                    FeedCode = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.RSSCode),
                    LocationName = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.LocationName),
                    RSSLink = this.BuildRSSFeed(this.FindParseFeedItemValue(list, HTMLParserFeedItemType.RSSLink)),
                    ServiceName = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.ServiceName)
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Add uri prefex (domain and subdirs) to 
        /// rss feed uri to get complete uri.
        /// </summary>
        /// <param name="feedsuri">feed uri as returned by Azure</param>
        /// <returns>complete uri as discovered on azure</returns>
        public string BuildRSSFeed(string feedsuri)
        {
            string temp = this.uriPrefix + feedsuri;
            return "<a href='" + temp + "'>rss</a>";
        }

        /// <summary>
        /// Given a list, return the feedItemType in it
        /// </summary>
        /// <param name="list">list of HTMLParserFeedItems</param>
        /// <param name="feedItemType">which data type is needed from list</param>
        /// <returns>string of feedItemType's data item value</returns>
        public string FindParseFeedItemValue(List<HTMLParserFeedItem> list, HTMLParserFeedItemType feedItemType)
        {
            if ((list != null) && (list.Count == 4))
            {
                return list.Find(item => item.Name == feedItemType).Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Given a node and a definition list, generate/pull out
        /// the data elements and build an HTMLParserFeedItem
        /// </summary>
        /// <param name="node">html node to parse</param>
        /// <param name="definitionList">Definition of html item containing Feed data item</param>
        /// <returns>HTMLParserFeedItem of feed data item values</returns>
        public HTMLParserFeedItem GetRSSFeedProperty(HtmlNode node, List<HTMLParserFeedItemDefinition> definitionList)
        {
            if ((node != null)
            && (!string.IsNullOrEmpty(node.Name))
            && (node.Attributes != null)
            && (node.Attributes["id"] != null)
            && (!string.IsNullOrEmpty(node.Attributes["id"].Value)))
            {
                HTMLParserFeedItemDefinition definition = this.GetRSSFeedPropertyDefinition(definitionList, node.Attributes["id"].Value);

                if (definition != null)
                {
                    HTMLParserFeedItem feedItem = new HTMLParserFeedItem();
                    feedItem.Name = definition.Name;

                    switch (definition.ContentType)
                    {
                        case ContentTag.AttributeValue:
                            feedItem.Value = this.ParseTagForAttributeValue(node, /*definition.AttributeValue,*/ definition.ReturnAttributeName);
                            return feedItem;
                        case ContentTag.InnerHtml:
                            feedItem.Value = this.InnerHTML(node/*, definition.AttributeValue*/);
                            return feedItem;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Given a list, return the requested item in the list
        /// This is tricky because the Id of the node isn't exact 
        /// so it needs a string.Contains instead of a string==
        /// </summary>
        /// <param name="list">Definition of html item containing Feed data item</param>
        /// <param name="itemAttribute">html tag attribute</param>
        /// <returns>itemAttribute's HTMLParserFeedItemDefintion 
        /// </returns>
        public HTMLParserFeedItemDefinition GetRSSFeedPropertyDefinition(List<HTMLParserFeedItemDefinition> list, string itemAttribute)
        {
            if ((list != null)
                && (!string.IsNullOrEmpty(itemAttribute)))
            {
                return list.Find(item => itemAttribute.Contains(item.AttributeName));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Given a html node and a named attribute of the node, 
        /// return the named attributes, value
        /// </summary>
        /// <param name="node">html node of tag</param>
        /// <param name="returnAttributeName">attribute of html node</param>
        /// <returns>value of tag's attribute as string</returns>
        public string ParseTagForAttributeValue(HtmlNode node, string returnAttributeName)
        {
            if ((node != null)
            && (!string.IsNullOrEmpty(returnAttributeName)))
            {
                return node.Attributes[returnAttributeName].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Given a html node, return the inner html
        /// Given "<span id="ctl00_MainContent_gvStatusToday_ctl03_lblServiceName">Access Control</span>"
        /// Return "Access Control"
        /// </summary>
        /// <param name="node">html node for inner html</param>
        /// <returns>value of tag's inner html as string</returns>
        public string InnerHTML(HtmlNode node)
        {
            if (node != null)
            {
                return node.InnerText;
            }
            else
            {
                return null;
            }
        }
    }
}
