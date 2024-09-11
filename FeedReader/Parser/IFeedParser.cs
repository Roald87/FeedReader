using Roald87.FeedReader.Feeds;

namespace Roald87.FeedReader.Parser
{
    internal interface IFeedParser
    {
        BaseFeed Parse(string feedXml);
    }
}
