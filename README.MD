# FeedReader
FeedReader is a .NET library used for reading and parsing RSS and ATOM feeds. Supports **RSS 0.91, 0.92, 1.0, 2.0 and ATOM**.
Developed because tested existing libraries do not work with different languages, encodings or have other issues. 
Library tested with multiple languages, encodings and feeds.

Install via nuget https://www.nuget.org/packages/Roald87.FeedReader/

This is a fork of https://github.com/arminreiter/FeedReader. In this fork I:
1. Ported to .NET 8.0.
2. Removed the `HttpClient` [because it caused issues](https://github.com/arminreiter/FeedReader/issues/58). And I couldn't get the client tests to pass.
3. Added parsing of `encoded:content` item tag for RSS 1.0

## Specifications
- RSS 0.91: http://www.rssboard.org/rss-0-9-1-netscape
- RSS 0.92: http://backend.userland.com/rss092, http://www.rssboard.org/rss-0-9-2
- RSS 1.0 : http://web.resource.org/rss/1.0/spec
- RSS 2.0 : https://validator.w3.org/feed/docs/rss2.html
- ATOM    : https://validator.w3.org/feed/docs/atom.html
