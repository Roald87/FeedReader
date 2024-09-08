using CodeHollow.FeedReader.Parser;

namespace CodeHollow.FeedReader;

/// <summary>
/// The static FeedReader class which allows to read feeds from a given url. Use it to
/// parse a feed from an url <see cref="Read(string)"/>, a file <see cref="ReadFromFile(string)"/> or <see cref="ReadFromFileAsync(string)"/>, a byte array <see cref="ReadFromByteArray(byte[])"/>
/// or a string <see cref="ReadFromString(string)"/>. If the feed url is not known, <see cref="ParseFeedUrlsFromHtml(string)"/>
/// returns all feed links on a given page.
/// </summary>
/// <example>
/// var links = FeedReader.ParseFeedUrlsFromHtml("https://codehollow.com");
/// var firstLink = links.First();
/// var feed = FeedReader.Read(firstLink.Url);
/// Console.WriteLine(feed.Title);
/// </example>
public static class FeedReader
{
    /// <summary>
    /// gets a url (with or without http) and returns the full url
    /// </summary>
    /// <param name="url">url with or without http</param>
    /// <returns>full url</returns>
    /// <example>GetUrl("codehollow.com"); => returns https://codehollow.com</example>
    public static string GetAbsoluteUrl(string url)
    {
        return new UriBuilder(url).ToString();
    }

    /// <summary>   
    /// Returns the absolute url of a link on a page. If you got the feed links via
    /// GetFeedUrlsFromUrl(url) and the url is relative, you can use this method to get the full url.
    /// </summary>
    /// <param name="pageUrl">the original url to the page</param>
    /// <param name="feedLink">a referenced feed (link)</param>
    /// <returns>a feed link</returns>
    /// <example>GetAbsoluteFeedUrl("codehollow.com", myRelativeFeedLink);</example>
    public static HtmlFeedLink GetAbsoluteFeedUrl(string pageUrl, HtmlFeedLink feedLink)
    {
        string tmpUrl = feedLink.Url.HtmlDecode();
        pageUrl = GetAbsoluteUrl(pageUrl);

        if (tmpUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || tmpUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return feedLink;

        if (tmpUrl.StartsWith("//", StringComparison.OrdinalIgnoreCase)) // special case
            tmpUrl = "http:" + tmpUrl;

        if (Uri.TryCreate(tmpUrl, UriKind.RelativeOrAbsolute, out Uri finalUri))
        {
            if (finalUri.IsAbsoluteUri)
            {
                return new HtmlFeedLink(feedLink.Title.HtmlDecode(), finalUri.ToString(), feedLink.FeedType);
            }
            else if (Uri.TryCreate(pageUrl + '/' + tmpUrl.TrimStart('/'), UriKind.Absolute, out finalUri))
                return new HtmlFeedLink(feedLink.Title.HtmlDecode(), finalUri.ToString(), feedLink.FeedType);
        }
        
        throw new UrlNotFoundException($"Could not get the absolute url out of {pageUrl} and {feedLink.Url}");
    }

    /// <summary>
    /// Parses RSS links from html page and returns all links
    /// </summary>
    /// <param name="htmlContent">the content of the html page</param>
    /// <returns>all RSS/feed links</returns>
    public static IEnumerable<HtmlFeedLink> ParseFeedUrlsFromHtml(string htmlContent)
    {
        // left the method here for downward compatibility
        return Helpers.ParseFeedUrlsFromHtml(htmlContent);
    }

    /// <summary>
    /// reads a feed from a file
    /// </summary>
    /// <param name="filePath">the path to the feed file</param>
    /// <returns>parsed feed</returns>
    public static Feed ReadFromFile(string filePath)
    {
        var feedContent = System.IO.File.ReadAllBytes(filePath);
        return ReadFromByteArray(feedContent);
    }

    /// <summary>
    /// reads a feed from a file
    /// </summary>
    /// <param name="filePath">the path to the feed file</param>
    /// <param name="cancellationToken">token to cancel operation</param>
    /// <returns>parsed feed</returns>
    public static async Task<Feed> ReadFromFileAsync(string filePath, CancellationToken cancellationToken)
    {
        byte[] result;
        using (var stream = System.IO.File.Open(filePath, System.IO.FileMode.Open))
        {
            result = new byte[stream.Length];
            await stream.ReadAsync(result, 0, (int)stream.Length, cancellationToken).ConfigureAwait(false);
        }
        return ReadFromByteArray(result);
    }

    /// <summary>
    /// reads a feed from a file
    /// </summary>
    /// <param name="filePath">the path to the feed file</param>
    /// <returns>parsed feed</returns>
    public static Task<Feed> ReadFromFileAsync(string filePath)
    {
        return ReadFromFileAsync(filePath, CancellationToken.None);
    }

    /// <summary>
    /// reads a feed from the <paramref name="feedContent" />
    /// </summary>
    /// <param name="feedContent">the feed content (xml)</param>
    /// <returns>parsed feed</returns>
    public static Feed ReadFromString(string feedContent)
    {
        return FeedParser.GetFeed(feedContent);
    }

    /// <summary>
    /// reads a feed from the bytearray <paramref name="feedContent"/>
    /// This could be useful if some special encoding is used.
    /// </summary>
    /// <param name="feedContent"></param>
    /// <returns></returns>
    public static Feed ReadFromByteArray(byte[] feedContent)
    {
        return FeedParser.GetFeed(feedContent);
    }
}
