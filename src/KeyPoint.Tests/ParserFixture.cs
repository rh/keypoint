using NUnit.Framework;

namespace KeyPoint.Tests
{
    [TestFixture]
    public class ParserFixture
    {
        [TestCase("", 0)]
        [TestCase("Line 1 on page 1\nLine 2 on page one", 1)]
        [TestCase("Line 1 on page 1\nLine 2 on page one\n-\nLine 1 on page 2", 2)]
        public void CountPages(string input, int pageCount)
        {
            var pages = 0;
            var parser = new Parser();
            parser.PageStarted += delegate { pages++; };
            parser.Parse(input);

            Assert.AreEqual(pageCount, pages);
        }

        [TestCase("", 0)]
        [TestCase("Line 1 on page 1\nLine 2 on page one", 0)]
        [TestCase("Line 1 on page 1\nLine 2 on page one\n-\nLine 1 on page 2", 0)]
        [TestCase("Line 1 on page 1\nLine 2 on page one\n\n-\nLine 1 on page 2", 0)]
        [TestCase("Line 1 on page 1\nLine 2 on page one\n\nLine 4 on page one\n-\nLine 1 on page 2", 1)]
        public void EmptyLines(string input, int lineCount)
        {
            var lines = 0;
            var parser = new Parser();
            parser.EmptyLine += delegate { lines++; };
            parser.Parse(input);

            Assert.AreEqual(lineCount, lines);
        }

        [TestCase("", 0)]
        [TestCase("Line 1 on page 1\nLine 2 on page one", 2)]
        [TestCase("Line 1 on page 1\nLine 2 on page one\n-\nLine 1 on page 2", 3)]
        public void Lines(string input, int lineCount)
        {
            var lines = 0;
            var parser = new Parser();
            parser.Line += delegate { lines++; };
            parser.Parse(input);

            Assert.AreEqual(lineCount, lines);
        }

        [TestCase("", 0)]
        [TestCase("Line 1 on page 1\n* list item", 1)]
        [TestCase("Line 1 on page 1\nLine 2 on page one\n-\n* one\n* two", 2)]
        public void Lists(string input, int itemCount)
        {
            var items = 0;
            var parser = new Parser();
            parser.ListItem += delegate { items++; };
            parser.Parse(input);

            Assert.AreEqual(itemCount, items);
        }
    }
}