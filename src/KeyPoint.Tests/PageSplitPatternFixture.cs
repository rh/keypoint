using System.Collections.Generic;
using NUnit.Framework;

namespace KeyPoint.Tests
{
    [TestFixture]
    public class PageSplitPatternFixture
    {
        [Test]
        public void Empty()
        {
            const string input = "";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsFalse(matched);
        }

        [Test]
        public void OneDash()
        {
            const string input = "-";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsTrue(matched);
        }

        [Test]
        public void MultipleDashes()
        {
            const string input = "---";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsTrue(matched);
        }

        [Test]
        public void OneDashWithLeadingWhitespace()
        {
            const string input = "  -";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsTrue(matched);
        }

        [Test]
        public void OneDashWithTrailingWhitespace()
        {
            const string input = "-  ";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsTrue(matched);
        }

        [Test]
        public void MultipleDashesWithLeadingWhitespace()
        {
            const string input = "  ---";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsTrue(matched);
        }

        [Test]
        public void MultipleDashesWithTrailingWhitespace()
        {
            const string input = "---  ";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsTrue(matched);
        }

        [Test]
        public void OneDashPlusIllegalCharacter()
        {
            const string input = "-1";
            var matched = Parser.PageSplitRegex.IsMatch(input);
            Assert.IsFalse(matched);
        }

        [Test]
        public void OnePage()
        {
            const string input = "Page one";
            var pages = Parser.PageSplitRegex.Split(input);
            Assert.AreEqual(1, pages.Length);
        }

        [Test]
        public void ManyPages()
        {
            const string input = "Page one\nLine two of page one\n-----\nPage two";
            var pages = new List<string>();

            foreach (var page in Parser.SplitPages(input))
            {
                pages.Add(page);
            }

            Assert.AreEqual(2, pages.Count);
            Assert.AreEqual("Page one\nLine two of page one", pages[0]);
            Assert.AreEqual("Page two", pages[1]);
        }

        [Test]
        public void IgnoreEmptyPages()
        {
            const string input = "Page one\n-----\n-----\nPage two";
            var pages = new List<string>();

            foreach (var page in Parser.SplitPages(input))
            {
                pages.Add(page);
            }

            Assert.AreEqual(2, pages.Count);
            Assert.AreEqual("Page one", pages[0]);
            Assert.AreEqual("Page two", pages[1]);
        }
    }
}