namespace Roald87.FeedReader.Feeds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Atom 1.0 feed item object according to specification: https://support.google.com/merchants/answer/14991618?hl=en
    /// </summary>
    public class Atom03FeedItem : BaseFeedItem
    {
        /// <summary>
        /// The "author" element
        /// </summary>
        public AtomPerson Author { get; set; }

        /// <summary>
        /// The "created" date as string
        /// </summary>
        public string CreatedDateString { get; set; }

        /// <summary>
        /// The "created" element as DateTime. Null if parsing failed or published is empty.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// The "issued" date as string
        /// </summary>
        public string IssuedDateString { get; set; }

        /// <summary>
        /// The "issued" element as DateTime. Null if parsing failed or published is empty.
        /// </summary>
        public DateTime? IssuedDate { get; set; }

        /// <summary>
        /// The "id" element
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The "summary" element
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The "modified" element
        /// </summary>
        public string ModifiedDateString { get; set; }

        /// <summary>
        /// The "modified" element as DateTime. Null if parsing failed or updated is empty
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// All "link" elements
        /// </summary>
        public ICollection<AtomLink> Links { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeedItem"/> class.
        /// default constructor (for serialization)
        /// </summary>
        public Atom03FeedItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomFeedItem"/> class.
        /// Reads an atom feed based on the xml given in item
        /// </summary>
        /// <param name="item">feed item as xml</param>
        public Atom03FeedItem(XElement item)
            : base(item)
        {
            this.Link = item.GetElement("link")?.Attribute("href")?.Value;
            this.Author = new AtomPerson(item.GetElement("author"));
            var categories = item.GetElements("category");
            this.Id = item.GetValue("id");
            this.CreatedDateString = item.GetValue("created");
            this.CreatedDate = Helpers.TryParseDateTime(this.CreatedDateString);
            this.IssuedDateString = item.GetValue("issued");
            this.IssuedDate = Helpers.TryParseDateTime(this.IssuedDateString);
            this.Links = item.GetElements("link").Select(x => new AtomLink(x)).ToList();
            this.Summary = item.GetValue("summary");
            this.ModifiedDateString = item.GetValue("modified");
            this.ModifiedDate = Helpers.TryParseDateTime(this.ModifiedDateString);
        }

        /// <inheritdoc/>
        internal override FeedItem ToFeedItem()
        {
            FeedItem fi = new FeedItem(this)
            {
                Author = this.Author?.ToString(),
                Categories = null,
                Content = null,
                Description = this.Summary,
                Id = this.Id,
                PublishingDate = this.CreatedDate,
                PublishingDateString = this.CreatedDateString,
            };
            return fi;
        }
    }
}
