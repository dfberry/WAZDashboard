// -----------------------------------------------------------------------
// <copyright file="HTMLParser.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HtmlAgilityPack;
    using System.Diagnostics;
    using Wp7AzureMgmt.Dashboard.Models;
    //using Microsoft.WindowsAzure;
    using System.Threading.Tasks;
   

    /// <summary>
    /// 
    /// </summary>
    internal class HTMLParser 
    {
        protected String Attribute = "id";

        protected List<HTMLParserFeedItemDefintion> Definitions { get; set; }
        protected ParallelOptions Options = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // no limit to parallelism
        protected RSSFeedFactory RssFeedFactory;
        public String Html;
        public string feedsURI; 

        public HTMLParser(string FeedsURI)
        {
            this.feedsURI = FeedsURI;
            Trace.TraceInformation("HTMLParser::HTMLParser begin b");
            BuildDefinition();
            RssFeedFactory = new RSSFeedFactory(this.feedsURI);
            Trace.TraceInformation("HTMLParser::HTMLParser end b");
        }

        public void BuildDefinition()
        {
            Trace.TraceInformation("HTMLParser::BuildDefinition begin "); 
            
            Definitions = new List<HTMLParserFeedItemDefintion>();

            Definitions.Add(new HTMLParserFeedItemDefintion() { Tag = "span", AttributeValue = "lblServiceName", ReturnAttributeValue = String.Empty, Name = HTMLParserFeedItemType.ServiceName });
            Definitions.Add(new HTMLParserFeedItemDefintion() { Tag = "span", AttributeValue = "lblRegionName", ReturnAttributeValue = String.Empty, Name = HTMLParserFeedItemType.LocationName });
            Definitions.Add(new HTMLParserFeedItemDefintion() { Tag = "a", AttributeValue = "hyperlinkRSS", ReturnAttributeValue = "href", Name = HTMLParserFeedItemType.RSSLink });
            Definitions.Add(new HTMLParserFeedItemDefintion() { Tag = "input", AttributeValue = "hdnRSSFeedCode", ReturnAttributeValue = "value", Name = HTMLParserFeedItemType.RSSCode });

            Trace.TraceInformation("HTMLParser::BuildDefinition end ");
        }



        public IEnumerable<RSSFeed> GetAndAddDashboardUrls(Uri uri)
        {
            Trace.TraceInformation("HTMLParser::GetDashboardUrls begin ");

            if (uri == null)
            {
                Trace.TraceWarning("HTMLParser::GetDashboardUrls (uri == null ");
                return null;
            }

            IEnumerable<RSSFeed> results = null;

            String html = GetHTMLOfDashboardPage(uri);

            results = ParseHtmlForUrls(html);
            
            Trace.TraceInformation("HTMLParser::GetDashboardUrls end ");

            return results;
        }

        public string GetHTMLOfDashboardPage(Uri uri)
        {
            Trace.TraceInformation("HTMLParser::GetHTMLOfDashboardPage begin ");

            String results = String.Empty;
            
            if (uri == null)
            {
                Trace.TraceWarning("HTMLParser::GetHTMLOfDashboardPage (uri == null ");
                return null;
            }

            HTTP context = new HTTP(uri);

            results =  context.RequestGET();

            Trace.TraceInformation("HTMLParser::GetHTMLOfDashboardPage end, results=" + results);

            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public IEnumerable<RSSFeed> ParseHtmlForUrls(String html, bool addToDB=true)
        {
            Trace.TraceInformation("HTMLParser::ParseHtmlForUrls begin ");

            // if param is empty, don't do any work
            if ((String.Empty==html) || (html==null))
            {
                return null;
            }

            List<RSSFeed> urls = new List<RSSFeed>();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);

            //Parallel.ForEach(doc.DocumentNode.SelectNodes("//tr"), this.Options, trNode =>
            foreach (HtmlAgilityPack.HtmlNode trNode in doc.DocumentNode.SelectNodes("//tr"))
            {
                List<HTMLParserFeedItem> returnedlist = new List<HTMLParserFeedItem>();

                foreach (HtmlNode tdNode in trNode.ChildNodes)
                //Parallel.ForEach(trNode.ChildNodes, this.Options, tdNode =>
                {

                    if ((tdNode.Name == "td")
                        && (tdNode.Attributes["class"] != null)
                        && (tdNode.Attributes["class"].Value != String.Empty)
                        && ((tdNode.Attributes["class"].Value == "cellStyleTodayStatus") || (tdNode.Attributes["class"].Value == "cellStyleTodayService")))
                    {
                        if (tdNode != null)
                        {
                            List<HTMLParserFeedItem> partiallist = ProcessTDNode(tdNode);

                            //foreach (HTMLParserFeedItem item in partiallist) ;
                            if (partiallist != null)
                            {
                                returnedlist.AddRange(partiallist);
                            }

                            // need all 6 items to continue
                            if ((returnedlist!=null) && (returnedlist.Count == 6))
                            {
                                RSSFeed thisItem = ConvertHTMLParserFeedItemList(returnedlist);

                                if (thisItem != null) 
                                {
                                    urls.Add(thisItem);
                                }

                            }
                        }
                    }
                    //});
                }
                //});
            }

            Trace.TraceInformation("HTMLParser::ParseHtmlForUrls end ");

            return urls;

        }
        public List<HTMLParserFeedItem> ProcessTDNode(HtmlNode tdNode)
        {
            Trace.TraceInformation("HTMLParser::ProcessTDNode begin ");

            if (tdNode == null)
                return null;

            List<HTMLParserFeedItem> list = new List<HTMLParserFeedItem>();

            //Parallel.ForEach(tdNode.ChildNodes, this.Options, node =>
            foreach (HtmlNode node in tdNode.ChildNodes)
            {
                Trace.TraceInformation("HTMLParser::ProcessTDNode node.Name=" + node.Name);

                HTMLParserFeedItem item = null;
                // Allow all items to run in parallel


                switch (node.Name.ToLower())
                {
                    case "div":

                        Trace.TraceInformation("HTMLParser::ProcessTDNode 'div'");
                        HtmlNodeCollection divNodeChildNodes = node.ChildNodes;

                        //Parallel.ForEach(divNodeChildNodes, this.Options, divChildNode =>
                        foreach (HtmlNode divChildNode in divNodeChildNodes)
                        {
                            Trace.TraceInformation("HTMLParser::ProcessTDNode divChildNode.Name=" + divChildNode.Name);

                            if ((divChildNode.Name == "span") || (divChildNode.Name == "a"))
                            {


                                HTMLParserFeedItem item2 = ParseNode(divChildNode);
                                if (item2 != null)
                                {
                                    list.Add(item2);
                                }
                            }
                        }
                        //});
                        break;
                    case "input":
                        Trace.TraceInformation("HTMLParser::ProcessTDNode 'input'");
                        item = ParseNode(node);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                        break;
                    default:
                        Trace.TraceInformation("HTMLParser::ProcessTDNode default");
                        break;
                }

                Trace.TraceInformation("HTMLParser::ProcessTDNode list.count=" + list.Count);
                }
            //});
            Trace.TraceInformation("HTMLParser::ProcessTDNode end ");

            return list;
        }
        public RSSFeed ConvertHTMLParserFeedItemList(List<HTMLParserFeedItem> list)
        {
            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList begin ");

            RSSFeed result = null;

            if (list == null)
            {
                Trace.TraceError("HTMLParser::ConvertHTMLParserFeedItemList list == null "); 
                return null;
            }

            if (list.Count != 6)
            {
                Trace.TraceError("HTMLParser::ConvertHTMLParserFeedItemList list.Count != 6 ");
                return null;
            }
         
            
            String RssLink = FindParseFeedItemValue(list, HTMLParserFeedItemType.RSSLink);
            String FeedCode = FindParseFeedItemValue(list, HTMLParserFeedItemType.RSSCode);
            String ServiceName = FindParseFeedItemValue(list, HTMLParserFeedItemType.ServiceName);
            String ServiceId = FindParseFeedItemValue(list, HTMLParserFeedItemType.ServiceId);
            String RegionLocation = FindParseFeedItemValue(list, HTMLParserFeedItemType.RegionName);
            String RegionId = FindParseFeedItemValue(list, HTMLParserFeedItemType.RegionId);

            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList RssLink=" + RssLink);
            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList FeedCode" + FeedCode);
            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList ServiceName=" + ServiceName);
            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList ServiceId=" + ServiceId);
            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList RegionLocation=" + RegionLocation);
            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList RegionId=" + RegionId);

            if (String.IsNullOrEmpty(RssLink)
                || (String.IsNullOrEmpty(FeedCode))
                || (String.IsNullOrEmpty(ServiceName))
                || (String.IsNullOrEmpty(ServiceId))
                || (String.IsNullOrEmpty(RegionLocation))
                || (String.IsNullOrEmpty(RegionId)))
            {
                Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList required element is empty");
                return null;
            }

            //create object
            result = RssFeedFactory.Create(RssLink, FeedCode, ServiceName, ServiceId, RegionLocation, RegionId);

            Trace.TraceInformation("HTMLParser::ConvertHTMLParserFeedItemList end ");

            return result;
        }
        public String FindParseFeedItemValue(List<HTMLParserFeedItem> list, HTMLParserFeedItemType type)
        {

            Trace.TraceInformation("HTMLParser::FindParseFeedItemValue begin ");

            if (list == null)
            {
                Trace.TraceInformation("HTMLParser::FindParseFeedItemValue list == null ");
                return String.Empty;
            }

            String result = String.Empty;

            result = list.Find(item => item.Name == type).Value;

            Trace.TraceInformation("HTMLParser::FindParseFeedItemValue end, result=" + result);

            return result;

        }

        public HTMLParserFeedItem ParseNode(HtmlNode node)
        {
            Trace.TraceInformation("HTMLParser::ParseNode begin ");

            HTMLParserFeedItem result = null;

            if (node == null)
            {
                Trace.TraceError("HTMLParser::ParseNode node == null ");
                return null;
            }

            if (node.Name == String.Empty)
            {
                Trace.TraceError("HTMLParser::ParseNode node.Name == String.Empty ");
                return null;
            }

            if (node.Attributes == null)
            {
                Trace.TraceError("HTMLParser::ParseNode node.Attributes == null "); 
                return null;
            }

            if (this.Attribute == String.Empty)
            {
                Trace.TraceError("HTMLParser::ParseNode this.Attribute == String.Empty "); 
                return null;
            }

            if (node.Attributes[this.Attribute] == null)
            {
                Trace.TraceError("HTMLParser::ParseNode node.Attributes[this.Attribute] == null ");
                return null;
            }

            if (node.Attributes[this.Attribute].Value == String.Empty)
            {
                Trace.TraceError("HTMLParser::ParseNode node.Attributes[this.Attribute].Value == String.Empty "); 
                return null;
            }

            if (this.Definitions == null)
            {
                Trace.TraceError("HTMLParser::ParseNode this.Definitions == null"); 
                return null;
            }

            String returnValue = String.Empty;
            HTMLParserFeedItem htmlParsedFeedItem = new HTMLParserFeedItem();

            HTMLParserFeedItemDefintion htmlFeedItemDefinition = FindDefinition(this.Definitions, node.Attributes[this.Attribute].Value);

            if (htmlFeedItemDefinition == null)
            {
                Trace.TraceError("HTMLParser::ParseNode htmlFeedItemDefinition == null"); 
                return null;
            }

            htmlParsedFeedItem.Name = htmlFeedItemDefinition.Name;
            Trace.TraceError("HTMLParser::ParseNode htmlFeedItemDefinition.Name=" + htmlFeedItemDefinition.Name);
          
            // Get Attribute Name to figure out which item we need to find
            String attributeValue = node.Attributes[this.Attribute].Value;
            Trace.TraceError("HTMLParser::ParseNode attributeValue=" + attributeValue);

            switch (htmlParsedFeedItem.Name)
            {
                case HTMLParserFeedItemType.RegionName:
                case HTMLParserFeedItemType.ServiceName:
                    // Find cooresponding value for this item
                    returnValue = ParseIndividualTagForInnerHTML(node, Attribute, htmlFeedItemDefinition.AttributeValue);
                    break;
                case HTMLParserFeedItemType.RSSLink:
                case HTMLParserFeedItemType.ServiceId:
                case HTMLParserFeedItemType.RSSCode:
                case HTMLParserFeedItemType.RegionId:
                    // Find cooresponding value for this item
                    returnValue = ParseIndividualTagForAttributeValue(node, Attribute, htmlFeedItemDefinition.AttributeValue, htmlFeedItemDefinition.ReturnAttributeValue);
                    break;
            }

            if (String.IsNullOrEmpty(returnValue))
            {
                Trace.TraceError("HTMLParser::ParseNode returnValue == String.Empty"); 
                return null;
            }

            htmlParsedFeedItem.Value = returnValue;

            Trace.TraceInformation("HTMLParser::ParseNode end, returnValue=" + returnValue); 

            return htmlParsedFeedItem;

        }
        public HTMLParserFeedItemDefintion FindDefinition(List<HTMLParserFeedItemDefintion> list, String itemAttribute)
        {
            Trace.TraceInformation("HTMLParser::FindDefinition begin");

            HTMLParserFeedItemDefintion result = null;

            if (list == null)
            {
                Trace.TraceInformation("HTMLParser::FindDefinition list == null");
                return null;
            }

            if (String.IsNullOrEmpty(itemAttribute))
            {
                Trace.TraceInformation("HTMLParser::FindDefinition String.IsNullOrEmpty(itemAttribute)");
                return null;
            }
            
            result = list.Find(item => itemAttribute.Contains(item.AttributeValue));
 
            Trace.TraceInformation("HTMLParser::FindDefinition end");

            return result;

        }

        public String ParseIndividualTagForAttributeValue(HtmlNode node, String AttributeName, String AttributeNameValue, String ReturnAttributeName)
        {
            Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue begin");

            if (node == null)
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue node == null");
                return String.Empty;
            }

            if (String.IsNullOrEmpty(AttributeName))
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue String.IsNullOrEmpty(AttributeName)");
                return String.Empty;
            }

            if (String.IsNullOrEmpty(AttributeNameValue))
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue String.IsNullOrEmpty(AttributeNameValue)");
                return String.Empty;
            }

            if (String.IsNullOrEmpty(ReturnAttributeName))
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue String.IsNullOrEmpty(ReturnAttributeName)");
                return String.Empty;
            }

            String result = String.Empty;
            
            if (node.Attributes[AttributeName].Value.Contains(AttributeNameValue))
            {
                result = node.Attributes[ReturnAttributeName].Value;
            }

            Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue result=" + result);

            return result;
        
        }
        public String ParseIndividualTagForInnerHTML(HtmlNode node, String AttributeName, String AttributeNameValue)
        {
            Trace.TraceInformation("HTMLParser::ParseIndividualTagForInnerHTML begin");

            if (node == null)
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue node == null");
                return String.Empty;
            }

            if (String.IsNullOrEmpty(AttributeName))
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue String.IsNullOrEmpty(AttributeName)");
                return String.Empty;
            }

            if (String.IsNullOrEmpty(AttributeNameValue))
            {
                Trace.TraceInformation("HTMLParser::ParseIndividualTagForAttributeValue String.IsNullOrEmpty(AttributeNameValue)");
                return String.Empty;
            }


            String result = String.Empty;

            if (node.Attributes[AttributeName].Value.Contains(AttributeNameValue))
            {
                result = node.InnerText;
            }

            Trace.TraceInformation("HTMLParser::ParseIndividualTagForInnerHTML result=" + result);

            return result;

        }
    }
}
