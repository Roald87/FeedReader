namespace Roald87.FeedReader.Parser
{
    using System.Xml.Linq;
    using Feeds;

    internal class Atom03Parser : AbstractXmlFeedParser
    {
        public override BaseFeed Parse(string feedXml, XDocument feedDoc)
        {
            Atom03Feed feed = new Atom03Feed(feedXml, feedDoc.Root);
            return feed;
        }
    }
}
