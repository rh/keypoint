using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace KeyPoint
{
    public class PdfDocument : IDisposable
    {
        private readonly Parser parser;
        private readonly Document document;
        private readonly Font font = FontFactory.GetFont(FontFactory.HELVETICA, 48f, BaseColor.WHITE);
        private List list;

        public PdfDocument(Stream output)
        {
            parser = new Parser();
            parser.PageStarted += OnPageStarted;
            parser.PageEnded += OnPageEnded;
            parser.Line += OnLine;
            parser.EmptyLine += OnEmptyLine;
            parser.ListItem += OnListItem;

            var rectangle = new Rectangle(PageSize.A4).Rotate();
            rectangle.BackgroundColor = BaseColor.BLACK;
            document = new Document(rectangle);
            PdfWriter.GetInstance(document, output);
            document.Open();
        }

        public void ReadFrom(string input)
        {
            parser.Parse(input);
        }

        private void OnPageStarted(object sender, EventArgs e)
        {
            document.NewPage();
        }

        private void OnPageEnded(object sender, EventArgs e)
        {
            Flush();
        }

        private void OnLine(object sender, LineEventArgs e)
        {
            FlushList();
            document.Add(new Paragraph(e.Text, font));
        }

        private void OnEmptyLine(object sender, EventArgs e)
        {
            document.Add(new Paragraph(Chunk.NEWLINE));
            Flush();
        }

        private void OnListItem(object sender, ListItemEventArgs e)
        {
            if (list == null)
            {
                list = new List();
                list.ListSymbol = new Chunk("- ");
            }

            list.Add(new ListItem(e.Text, font));
        }

        private void FlushList()
        {
            if (list != null)
            {
                document.Add(list);
                list = null;
            }
        }

        private void Flush()
        {
            FlushList();
        }

        public void Dispose()
        {
            if (document != null && document.IsOpen())
            {
                document.Close();
            }
        }
    }
}