// -----------------------------------------------------------------------
// <copyright file="WADashboard.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Dashboard.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using HtmlAgilityPack;
    using Wp7AzureMgmt.Dashboard.Interfaces;
    using Wp7AzureMgmt.Dashboard.Models;
    using Wp7AzureMgmt.DashboardFeeds;
    using Wp7AzureMgmt.DashboardFeeds.Models;

    /// <summary>
    /// This datasource fetches the list from Windows Azure Dashboard Service web page
    /// </summary>
    internal class WADashboard : IRSSDataSource
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
        /// DateTime Feed list was returned from source
        /// </summary>
        private DateTime feedBuildDate = DateTime.MinValue;

        ///// <summary>
        ///// Defintiions of what to parse out of html
        ///// </summary>
        //private List<HTMLParserFeedItemDefinition> htmlDefinitions = null;

        /// <summary>
        /// Feedlist of Windows Azure Dashboard
        /// </summary>
        private IEnumerable<RSSFeed> feedList = null;

        /// <summary>
        /// URI containing Feedlist
        /// </summary>
        private Uri dashboardURI = null;

        /// <summary>
        /// Http response content pulled from URI
        /// </summary>
        private string uricontent = null;

        /// <summary>
        /// Location type (file or uri) to get content from
        /// </summary>
        private FindBy findby = FindBy.notset;

        /// <summary>
        /// Initializes a new instance of the <see cref="WADashboard" /> class.
        /// /// find default Uri at 
        /// ConfigurationManager.AppSettings["AzureDashboardServiceURL"]
        /// </summary>
        public WADashboard()
        {
            string tempUri = ConfigurationManager.AppSettings["AzureDashboardServiceURL"];

            if (string.IsNullOrEmpty(tempUri))
            {
                throw new ArgumentNullException();
            }

            this.dashboardURI = new Uri(tempUri);
            this.findby = FindBy.uriprovided;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WADashboard" /> class.
        /// /// RSS Feed URI
        /// </summary>
        /// <param name="uri">Uri of Windows Azure Dashboard Feeds</param>
        public WADashboard(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("URI is null");
            }
            else
            {
                this.dashboardURI = uri;
                this.findby = FindBy.uriprovided;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WADashboard" /> class.
        /// /// Http Response Content as string
        /// </summary>
        /// <param name="uriresponescontent">html content of the page</param>
        public WADashboard(string uriresponescontent)
        {
            if (uriresponescontent == null)
            {
                throw new ArgumentNullException("uriresponescontent is null");
            }
            else
            {
                this.uricontent = uriresponescontent;
                this.findby = FindBy.stringprovided;
            }
        }

        ///// <summary>
        ///// Gets or sets HtmlDefinitions
        ///// </summary>
        ////public List<HTMLParserFeedItemDefinition> HtmlDefinitions
        ////{
        ////    get
        ////    {
        ////        return this.htmlDefinitions;
        ////    }

        ////    set
        ////    {
        ////        this.htmlDefinitions = value;
        ////    }
        ////}

        /// <summary>
        /// Get copy of list
        /// </summary>
        /// <returns>Returns IEnumerable of RSS Feeds</returns>
        public IEnumerable<RSSFeed> List()
        {
            if (this.feedList == null)
            {
                this.feedList = this.FindFeeds();
            }

            return this.feedList;
        }

        /// <summary>
        /// Returns importable file as string for Google Reader
        /// </summary>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        public string OPML()
        {
            string opml = null;

            if (this.feedList == null)
            {
                this.feedList = this.FindFeeds();
            }

            opml = DashboardResource.OPMLMaster;
            string opmlOutline = DashboardResource.OPMLMasterOutline;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (RSSFeed feed in this.feedList)
            {
                string feedOutline = opmlOutline.Replace("$$name", HttpUtility.HtmlEncode(feed.ServiceName));
                feedOutline = feedOutline.Replace("$$location", HttpUtility.HtmlEncode(feed.LocationName));
                feedOutline = feedOutline.Replace("$$code", HttpUtility.HtmlEncode(feed.FeedCode));

                stringBuilder.Append(feedOutline);
            }

            opml = opml.Replace("$$content", stringBuilder.ToString());

            return opml;
        }

        /// <summary>
        /// DateTime list was fetched from Datasource
        /// </summary>
        /// <returns>Returns OPML formatted string of RSS Feeds</returns>
        public DateTime BuildDate()
        {
            return this.feedBuildDate;
        }

        /// <summary>
        /// Internal mechanics of parsing HTML to build list
        /// </summary>
        /// <returns>IEnumerable of RSSFeed</returns>
        public IEnumerable<RSSFeed> FindFeeds()
        {
            IEnumerable<RSSFeed> feeds = null;

            // grab html
            if ((this.findby == FindBy.uriprovided) && (this.dashboardURI != null))
            {
                this.uricontent = this.FindFeedsCallingURI();
                feeds = this.ParseHtmlForUrls(this.uricontent);
            }
            else if ((this.findby == FindBy.stringprovided) && (!string.IsNullOrEmpty(this.uricontent)))
            {
                // parse html
                feeds = this.ParseHtmlForUrls(this.uricontent);
            }
            else
            {
                throw new Exception("FindFeeds - missing valid findby enum and findby value");
            }

            // set build date
            this.feedBuildDate = DateTime.Now;

            return feeds;
        }

        /// <summary>
        /// Hunt for containing tags and grab RSS feed details
        /// </summary>
        /// <param name="html">entire html page content</param>
        /// <returns>IEnumerable of RSSFeed</returns>
        public IEnumerable<RSSFeed> ParseHtmlForUrls(string html)
        {
            // if param is empty, don't do any work
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            List<RSSFeed> urls = new List<RSSFeed>();

            //if (this.HtmlDefinitions == null)
            //{
            //    this.HtmlDefinitions = this.BuildDefinition();
            //    if (this.HtmlDefinitions == null)
            //    {
            //        throw new Exception("BuildDefintion is null");
            //    }
            //}

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            // Assuming top tr nodes are what we want
            Parallel.ForEach(
                doc.DocumentNode.SelectNodes("//tr"),
                this.options,
                trNode =>
                {
                    RSSFeed rssfeed = ParseFeedNode(trNode);

                    if (rssfeed != null)
                    {
                        urls.Add(rssfeed);
                    }
                });

            return urls;
        }

        /// <summary>
        /// Parse the top level TR nodes
        /// </summary>
        /// <param name="trnode">html TR node</param>
        /// <returns>RSSFeed parsed from TR node</returns>
        public RSSFeed ParseFeedNode(HtmlNode trnode)
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

                    List<HTMLParserFeedItemDefinition> defintionlist = buildDefinitions.ToList();

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
        public RSSFeed ConvertFeedItemListToRSSFeed(List<HTMLParserFeedItem> list)
        {
            if (list != null)
            {
                return new RSSFeed
                {
                    FeedCode = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.RSSCode),
                    LocationName = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.LocationName),
                    RSSLink = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.RSSLink),
                    ServiceName = this.FindParseFeedItemValue(list, HTMLParserFeedItemType.ServiceName)
                };
            }

            return null;
        }

        /// <summary>
        /// Given a list, return the feedItemType in it
        /// </summary>
        /// <param name="list">list of HTMLParserFeedItems</param>
        /// <param name="feedItemType">which data type is needed from list</param>
        /// <returns>string of feedItemType's data item value</returns>
        public string FindParseFeedItemValue(List<HTMLParserFeedItem> list, HTMLParserFeedItemType feedItemType)
        {
            if ((list != null) && (list.Count > 0))
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
        /// <param name="node">html node</param>
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
                        case TagContent.AttributeValue:
                            feedItem.Value = this.ParseTagForAttributeValue(node, /*definition.AttributeValue,*/ definition.ReturnAttributeName);
                            return feedItem;
                        case TagContent.InnerHtml:
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
        /// <param name="node">html node</param>
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
        /// <param name="node">html node</param>
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

        /// <summary>
        /// Call Uri and get Response html content
        /// </summary>
        /// <returns>html content as string</returns>
        private string FindFeedsCallingURI()
        {
            HTTP httpRequest = new HTTP(this.dashboardURI);
            return httpRequest.RequestGET();
        }

        /// <summary>
        /// HTML tags holding various pieces of RSSFeed
        /// </summary>
        /// <returns>List of HTMLParserFeedItemDefintion</returns>
        private IEnumerable<HTMLParserFeedItemDefinition> BuildDefinition()
        {
            List<HTMLParserFeedItemDefinition> definitions = new List<HTMLParserFeedItemDefinition>();

            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = TagContent.InnerHtml });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = TagContent.InnerHtml });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = TagContent.AttributeValue });
            definitions.Add(new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = TagContent.AttributeValue });

            return definitions;
        }

        /// <summary>
        /// Static array built at compile time
        /// </summary>
        private HTMLParserFeedItemDefinition[] buildDefinitions = new HTMLParserFeedItemDefinition[] 
        {
            new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblServiceName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.ServiceName, ContentType = TagContent.InnerHtml },
            new HTMLParserFeedItemDefinition() { Tag = "span", AttributeName = "lblRegionName", ReturnAttributeName = null, Name = HTMLParserFeedItemType.LocationName, ContentType = TagContent.InnerHtml },
            new HTMLParserFeedItemDefinition() { Tag = "a", AttributeName = "hyperlinkRSS", ReturnAttributeName = "href", Name = HTMLParserFeedItemType.RSSLink, ContentType = TagContent.AttributeValue },
            new HTMLParserFeedItemDefinition() { Tag = "input", AttributeName = "hdnRSSFeedCode", ReturnAttributeName = "value", Name = HTMLParserFeedItemType.RSSCode, ContentType = TagContent.AttributeValue } 
        };
    }
}
