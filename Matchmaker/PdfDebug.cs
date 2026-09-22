using UglyToad.PdfPig;
using System.Text;

public static class PdfDebug
{
    public static string DumpRawText(string pdfPath, string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        var sb = new StringBuilder();
        using var pdf = PdfDocument.Open(pdfPath);

        foreach (var page in pdf.GetPages())
        {
            sb.AppendLine($"--- Page {page.Number} ---");
            sb.AppendLine(page.Text);
        }

        string outPath = Path.Combine(outputDir, $"FAReport_RawText_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
        return outPath;
    }
}