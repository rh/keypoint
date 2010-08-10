using System.IO;

namespace KeyPoint
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filename = args[0] + ".pdf";

            using (var stream = new FileStream(filename, FileMode.Create))
            {
                using (var document = new PdfDocument(stream))
                {
                    document.ReadFrom(File.ReadAllText(args[0]));
                }
            }
        }
    }
}