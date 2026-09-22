using Matchmaker;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

internal class FAReportLoader
{
    public List<FAReportEntry> Load(string pdfPath)
    {
        var entries = new List<FAReportEntry>();

        var processPattern = new Regex(@"First Article Inspection Report for\s+(?<proc>[^\r\n]+)", RegexOptions.IgnoreCase);
        var woPattern = new Regex(@"\bWO\s*:\s*(?<wo>\S+)", RegexOptions.IgnoreCase);
        var snPattern = new Regex(@"\bSerial\s+Number\s*:\s*(?<sn>[^\r\n]+)", RegexOptions.IgnoreCase);

        var lotPromptPattern = new Regex(
            @"Record\s+(?:theto)?\s*lot\s+code\s+of\s+(?:the\s+)?(?<material>pastechip\s+bonderepoxyunderfill)\b[^\r\n]*\r?\n\s*(?<code>(?:N/?An/?a[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12[A-Za-z0-9\-]{6,}))",
            RegexOptions.IgnoreCase);

        using var pdf = PdfDocument.Open(pdfPath);

        foreach (var page in pdf.GetPages())
        {
            string pageText = page.Text;

            // Page context
            string process = MatchOrEmpty(processPattern, pageText, "proc");
            string wo = MatchOrEmpty(woPattern, pageText, "wo");
            string sn = MatchOrEmpty(snPattern, pageText, "sn");

            foreach (Match m in lotPromptPattern.Matches(pageText))
            {
                var material = m.Groups["material"].Value.Trim();
                var code = m.Groups["code"].Value.Trim();

                // Normalize material for downstream matching, e.g., "chip bonder" -> "CHIP_BONDER"
                string materialNorm = NormalizeMaterial(material);

                entries.Add(new FAReportEntry
                {
                    MaterialName = materialNorm,   // e.g., "PASTE", "CHIP_BONDER", "EPOXY", "UNDERFILL"
                    LotCode = code.Equals("N/A", StringComparison.OrdinalIgnoreCase) ? "" : code,
                    // Optional notes for traceability
                    Notes = BuildNotes(process, wo, sn)
                });
            }
        }

        return entries;
    }

    private static string MatchOrEmpty(Regex rx, string text, string groupName)
    {
        var m = rx.Match(text);
        return m.Success ? m.Groups[groupName].Value.Trim() : "";
    }

    private static string NormalizeMaterial(string s)
    {
        s = s.Trim().ToUpperInvariant();
        s = Regex.Replace(s, @"\s+", "_"); // "chip bonder" -> "CHIP_BONDER"
        return s;
    }

    private static string BuildNotes(string process, string wo, string sn)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(process)) parts.Add($"Process: {process}");
        if (!string.IsNullOrWhiteSpace(wo)) parts.Add($"WO: {wo}");
        if (!string.IsNullOrWhiteSpace(sn)) parts.Add($"SN: {sn}");
        return string.Join(" | ", parts);
    }
}