namespace Roald87.FeedReader.Tests
{
    [TestClass]
    public class HelpersTest
    {
        [TestMethod]
        public void TestCodeHollowLinkTag01()
        {
            string input = "<link rel=\"alternate\" type=\"application/rss+xml\" title=\"codehollow &raquo; Feed\" href=\"https://codehollow.com/feed/\" />";
            TestLinkTagParse(input, new HtmlFeedLink("codehollow » Feed", "https://codehollow.com/feed/", FeedType.Rss));
        }

        [TestMethod]
        public void TestCodeHollowLinkTag01Reordered1()
        {
            string input = "<link title=\"codehollow &raquo; Feed\" rel=\"alternate\" type=\"application/rss+xml\" href=\"https://codehollow.com/feed/\" />";
            TestLinkTagParse(input, new HtmlFeedLink("codehollow » Feed", "https://codehollow.com/feed/", FeedType.Rss));
        }

        [TestMethod]
        public void TestCodeHollowLinkTag01Reordered2()
        {
            string input = "<link type=\"application/rss+xml\"   href=\"https://codehollow.com/feed/\" title=\"codehollow &raquo; Feed\" rel=\"alternate\" />";
            TestLinkTagParse(input, new HtmlFeedLink("codehollow » Feed", "https://codehollow.com/feed/", FeedType.Rss));
        }

        [TestMethod]
        public void TestCodeHollowLinkTagWhitespaces()
        {
            string input = "<link        rel  = \"alternate\"   type= \"application/rss+xml\"         title=\"codehollow &raquo; Feed\"      href=\"https://codehollow.com/feed/\" />";
            TestLinkTagParse(input, new HtmlFeedLink("codehollow » Feed", "https://codehollow.com/feed/", FeedType.Rss));
        }

        [TestMethod]
        public void TestCodeHollowLinkTagNewLine()
        {
            string input = $"<link rel=\"alternate\" " +
                "type=\"application/rss+xml\" title=\"codehollow &raquo; Feed\" href=\"https://codehollow.com/feed/\" />";
            TestLinkTagParse(input, new HtmlFeedLink("codehollow » Feed", "https://codehollow.com/feed/", FeedType.Rss));
        }

        [TestMethod]
        public void TestUnquotedAttributes()
        {
            string input = "<link href=https://shkspr.mobi/blog/feed rel=alternate title=\"RSS Feed.\" type=application/rss+xml>";
            TestLinkTagParse(input, new HtmlFeedLink("RSS Feed.", "https://shkspr.mobi/blog/feed", FeedType.Rss));
        }

        private static void TestLinkTagParse(string input, HtmlFeedLink expectedResult)
        {
            var res = Helpers.GetFeedLinkFromLinkTag(input);
            Assert.AreEqual(expectedResult.Title, res.Title);
            Assert.AreEqual(expectedResult.Url, res.Url);
            Assert.AreEqual(expectedResult.FeedType, res.FeedType);
        }

        [TestMethod]
        [DataRow("2020-01-01", 2020, 1, 1, 0, 0, 0)]
        [DataRow("2024-03-01T13:26:09+00:00", 2024, 3, 1, 13, 26, 09)]
        [DataRow("2017-01-07T09:00:01-05:00", 2017, 1, 7, 14, 0, 1)]
        [DataRow("Sat, 07 Jan 2017 10:19:44 -0500", 2017, 1, 7, 15, 19, 44)]
        [DataRow("2019-04-27T14:25:30Z", 2019, 4, 27, 14, 25, 30)]
        public void TestDateTimeParse(string input, int year, int month, int day, int hour, int minute, int second)
        {
            var res = Helpers.TryParseDateTime(input);
            Assert.AreEqual(new DateTime(year, month, day, hour, minute, second), res);
        }

        #region ParseFeedUrlsFromHtml Test -  test full html feed parse
        [TestMethod]
        public void ParseFeedsCodeHollow()
        {
            TestHtmlLinkParse("Html/codehollow.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("codehollow » Feed", "https://codehollow.com/feed/", FeedType.Rss),
                new HtmlFeedLink("codehollow » Comments Feed", "https://codehollow.com/comments/feed/", FeedType.Rss)
            });

        }

        [TestMethod]
        public void ParseFeedsHeise()
        {

            TestHtmlLinkParse("Html/heise.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("Aktuelle News von heise online", "https://www.heise.de/rss/heise-atom.xml", FeedType.Atom),
                new HtmlFeedLink("Aktuelle News von heise online (für ältere RSS-Reader)", "https://www.heise.de/rss/heise.rdf", FeedType.Rss)
            });

        }

        [TestMethod]
        public void ParseFeedsJapanTimes()
        {

            TestHtmlLinkParse("Html/japantimes.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("Japan Times RSS Feed - Top Stories", "https://www.japantimes.co.jp/feed/topstories", FeedType.Rss),
            });

        }
        [TestMethod]
        public void ParseFeedsOrfAt()
        {

            TestHtmlLinkParse("Html/orf.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("Newsfeed - news.ORF.at", "https://rss.orf.at/news.xml", FeedType.Rss),
            });

        }
        [TestMethod]
        public void ParseFeedsStackOverflow()
        {

            TestHtmlLinkParse("Html/stackoverflow.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("Feed of recent questions", "/feeds", FeedType.Atom),
            });

        }
        [TestMethod]
        public void ParseFeedsStadtfeuerwehrWeiz()
        {

            TestHtmlLinkParse("Html/stadtfeuerwehrweiz.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("Stadtfeuerwehr Weiz - Einsätze", "http://www.stadtfeuerwehr-weiz.at/rss/einsaetze.xml", FeedType.Rss),
            });

        }
        [TestMethod]
        public void ParseFeedsTheVerge()
        {

            TestHtmlLinkParse("Html/theverge.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("The Verge", "/rss/index.xml", FeedType.Rss),
                new HtmlFeedLink("Front Page", "https://www.theverge.com/rss/front-page/index.xml", FeedType.Rss)
            });

        }

        [TestMethod]
        public void ParseFeedsShksprMobi()
        {
            TestHtmlLinkParse("Html/shksprmobi.html", new List<HtmlFeedLink>()
            {
                new HtmlFeedLink("Atom Feed.", "https://shkspr.mobi/blog/feed/atom", FeedType.Atom),
                new HtmlFeedLink("RSS Feed.", "https://shkspr.mobi/blog/feed", FeedType.Rss),
            });
        }

        private static void TestHtmlLinkParse(string path, IEnumerable<HtmlFeedLink> expectedLinks)
        {
            var content = System.IO.File.ReadAllText(path);

            var links = Helpers.ParseFeedUrlsFromHtml(content);
            Assert.AreEqual(expectedLinks.Count(), links.Count());

            foreach (var l in links)
            {
                expectedLinks.First(e => e.FeedType == l.FeedType && e.Title == l.Title && e.Url == l.Url); // throws exception if link doesn't exist
            }
        }
        #endregion
    }
}
