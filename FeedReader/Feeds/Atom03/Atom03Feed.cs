namespace Roald87.FeedReader.Feeds
{
    using System;
    using System.Xml.Linq;

    public class Atom03Feed : BaseFeed
    {
        /// <summary>
        /// The "author" element
        /// </summary>
        public AtomPerson Author { get; set; }

        /// <summary>
        /// The "modified" element as string
        /// </summary>
        public string ModifiedString { get; set; }

        /// <summary>
        /// The "updated" element as DateTime. Null if parsing failed of modifiedString is empty.
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// The "generator" element
        /// </summary>
        public string Generator { get; set; }
        
        /// <summary>
        /// The "id" element
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The "tagline" element
        /// </summary>
        public string Tagline { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Atom03Feed"/> class.
        /// default constructor (for serialization)
        /// </summary>
        public Atom03Feed()
            : base()
        {
        }

        public Atom03Feed(string feedXml, XElement feed)
            : base(feedXml, feed)
        {
            this.Link = feed.GetElement("link")?.Attribute("href")?.Value;
            this.Author = new AtomPerson(feed.GetElement("author"));
            this.Tagline = feed.GetElement("tagline")?.Value;
            this.Id = feed.GetValue("id");
            this.ModifiedString = feed.GetElement("modified")?.Value;
            this.Modified = Helpers.TryParseDateTime(this.ModifiedString);
            this.Generator = feed.GetElement("generator")?.Value;

            var items = feed.GetElements("entry");
            foreach (var item in items)
            {
                this.Items.Add(new Atom03FeedItem(item));
            }
        }

        /// <summary>
        /// Creates the base <see cref="Feed"/> element out of this feed.
        /// </summary>
        /// <returns>feed</returns>
        public override Feed ToFeed()
        {
            Feed f = new Feed(this)
            {
                Copyright = null,
                Description = null,
                ImageUrl = null,
                Language = null,
                LastUpdatedDate = this.Modified,
                LastUpdatedDateString = this.ModifiedString,
                Type = FeedType.Atom_0_3
            };
            return f;
        }
    }
}