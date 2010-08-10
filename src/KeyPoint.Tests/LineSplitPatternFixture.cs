using NUnit.Framework;

namespace KeyPoint.Tests
{
    [TestFixture]
    public class LineSplitPatternFixture
    {
        [Test]
        public void SplitEndingWithLineBreak()
        {
            const string input = "First line\r\nSecond line\n";
            var lines = Parser.LineSplitRegex.Split(input);

            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual(0, lines[lines.Length - 1].Length);
        }

        [Test]
        public void SplitEndingWithLine()
        {
            const string input = "First line\r\nSecond line\nThird line";
            var lines = Parser.LineSplitRegex.Split(input);

            Assert.AreEqual(3, lines.Length);
        }
    }
}