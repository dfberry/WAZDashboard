// -----------------------------------------------------------------------
// <copyright file="FeedResult.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AzureDashboardService.Factories
{
    using System;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;

    /// <summary>
    /// Creates an RSS Feed 
    /// http://damieng.com/blog/2010/04/26/creating-rss-feeds-in-asp-net-mvc
    /// </summary>
    public class FeedResult : ActionResult
    {
        /// <summary>
        /// Syndication Feed Formatter
        /// </summary>
        private readonly SyndicationFeedFormatter feed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedResult" /> class.
        /// </summary>
        /// <param name="feed">as SyndicationFeedFormatter </param>
        public FeedResult(SyndicationFeedFormatter feed)
        {
            this.feed = feed;
        }

        /// <summary>
        /// Gets or sets ContentEncoding
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets ContentType
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets SyndicationFeedFormatter
        /// </summary>
        public SyndicationFeedFormatter Feed
        {
            get { return this.feed; }
        }
        
        /// <summary>
        /// Builds result into http context
        /// </summary>
        /// <param name="context">ControllerContext context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/rss+xml";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.feed != null)
            {
                using (var xmlWriter = new XmlTextWriter(response.Output))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    this.feed.WriteTo(xmlWriter);
                }
            }
        }
    }
}