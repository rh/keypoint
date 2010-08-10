using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace KeyPoint
{
    public class Parser
    {
        public static Regex LineSplitRegex = new Regex(@"\r?\n");
        public static Regex PageSplitRegex = new Regex(@"^\s*-+\s*$", RegexOptions.Multiline);
        public static Regex ListItemRegex = new Regex(@"^\* (.*)$");

        public event EventHandler<EventArgs> PageStarted;
        public event EventHandler<EventArgs> PageEnded;
        public event EventHandler<EventArgs> EmptyLine;
        public event EventHandler<LineEventArgs> Line;
        public event EventHandler<ListItemEventArgs> ListItem;

        private static TextWriter debug = TextWriter.Null;

        public static TextWriter Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public void Parse(string input)
        {
            var pages = SplitPages(input);
            foreach (var page in pages)
            {
                OnPageStarted(page);

                var lines = SplitLines(page);
                foreach (var line in lines)
                {
                    if (line.Trim().Length == 0)
                    {
                        OnEmptyLine();
                    }
                    else
                    {
                        var match = ListItemRegex.Match(line);
                        if (match.Success)
                        {
                            OnListItem(match.Value.Substring(2));
                        }
                        else
                        {
                            OnLine(line);
                        }
                    }
                }

                OnPageEnded(page);
            }
        }

        protected void OnPageStarted(string text)
        {
            debug.WriteLine("Page started: <<<{0}>>>", text);

            var handler = PageStarted;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnPageEnded(string text)
        {
            debug.WriteLine("Page ended: <<<{0}>>>", text);

            var handler = PageEnded;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnEmptyLine()
        {
            debug.WriteLine("Empty line");

            var handler = EmptyLine;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnLine(string text)
        {
            debug.WriteLine("Line: <<<{0}>>>", text);

            var handler = Line;
            if (handler != null)
            {
                handler(this, new LineEventArgs(text));
            }
        }

        protected void OnListItem(string text)
        {
            debug.WriteLine("List item: <<<{0}>>>", text);

            var handler = ListItem;
            if (handler != null)
            {
                handler(this, new ListItemEventArgs(text));
            }
        }

        public static IEnumerable<string> SplitPages(string input)
        {
            var pages = PageSplitRegex.Split(input);
            foreach (var page in pages)
            {
                var trimmed = page.Trim();

                if (trimmed.Length > 0)
                {
                    yield return trimmed;
                }
            }
        }

        public static IEnumerable<string> SplitLines(string input)
        {
            var lines = LineSplitRegex.Split(input);
            foreach (var line in lines)
            {
                yield return line;
            }
        }
    }

    public class ListItemEventArgs : EventArgs
    {
        public ListItemEventArgs(string text)
        {
            this.text = text;
        }

        private readonly string text;

        public string Text
        {
            get { return text; }
        }
    }

    public class LineEventArgs : EventArgs
    {
        public LineEventArgs(string text)
        {
            this.text = text;
        }

        private readonly string text;

        public string Text
        {
            get { return text; }
        }
    }
}