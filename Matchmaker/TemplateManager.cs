using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;  // allows for queries
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
//using UglyToad.PdfPig.DocumentLayoutAnalysis;

namespace Matchmaker
{
    internal class TemplateManager
    {
        public XLWorkbook LoadTemplate(string templateFilePath) {
            return new XLWorkbook(templateFilePath);
        }

        public void PopulateTemplate(XLWorkbook wb, List<MatchResult> results)
        {
            var ws = wb.Worksheet("Form 1");
            List<MatchResult> form2Entries = new List<MatchResult>(); 

            // Locate the header row containing "15. Part Number".
            var startCell = ws.Search("15. Part Number").FirstOrDefault() ?? ws.Search("15. Part Number:").FirstOrDefault();

            if (startCell == null) {
                throw new InvalidOperationException("Header not found in template; template corrupted. Everything is broken.");
            }

            int startRow = startCell.Address.RowNumber + 1;

            // Queries to results to eliminate duplicates
            NormalizerService ns = new NormalizerService();

            var distinctResults = results
                .Where(r => !string.IsNullOrWhiteSpace(r.LtNumber)) // use only entries with populated LtNumber
                .GroupBy(r => ns.NormalizeLt(r.LtNumber))
                .Select(g => g
                    .OrderByDescending(r => CompletenessScore(r))
                    .ThenBy(r=> r.DesignPn ?? string.Empty)
                    .First())
                .ToList();

            // debug results
            //WriteDebugFile("debug_output.txt", distinctResults);

            const int firstCol = 2;
            const int lastCol = 6;

            // Write to excel file while preserving original form's formatting
            int lastUsedRow = ws.LastRowUsed()?.RowNumber() ?? startRow;
            if (lastUsedRow < startRow) {
                lastUsedRow = startRow;
            } 
            

            // clear data
            for (int r = startRow; r <= lastUsedRow; r++) {
                for (int c = firstCol;c <= lastCol; c++){

                    ws.Cell(r, c).Value = "";               // clear only values, not cell itself
                }
            }

            // write data
            int writingRow = startRow;

            foreach (var r in distinctResults)
            {
                
                if ((r.LtNumber?.StartsWith("LT437") == true && !(r.DesignPn?.StartsWith("H") == true)) || r.LtNumber?.StartsWith("710") == true || r.LtNumber?.StartsWith("001") == true) {
                    // if non-COTS consumable or CC/potting or serial labels
                    form2Entries.Add(r);
                }
                else if (r.LtNumber?.StartsWith("831") == true || r.LtNumber?.StartsWith("100") == true ||r.LtNumber?.StartsWith("700") == true) {
                    // if bag/box labels, boxes ("miscellaneous parts") or bags
                    continue; // N/A
                }
                else {
                    bool isSubassembly = IsSubassemblyLt(r.LtNumber);
                    bool isPcb = IsPcbLt(r.LtNumber);

                    // fill new row // make first two completely blank if single thing is null
                    ws.Cell(writingRow, 2).Value = r.DesignPn != null ? "FN " + r.FindNumber + "-" + r.DesignPn : "";    // R drawing number
                    ws.Cell(writingRow, 3).Value = r.AsBuiltMpn != null ? r.AsBuiltMpn : "";                             // description from asbuilt
                    ws.Cell(writingRow, 4).Value = isSubassembly ? "Sub-assembly" : "COTS (or equivalent)";                               
                    ws.Cell(writingRow, 5).Value = r.Supplier ?? "";                                                     // supplier from asBuilt supplier (or unknown of blank)
                    ws.Cell(writingRow, 6).Value = r.LtNumber != null ? r.LtNumber: "";                                  // FAIR Identifier = LT part number
                    ws.Cell(writingRow, 7).Value = (isSubassembly && !isPcb)? "" : (r.Rir ?? "");                        // Rir# or blank if none
                    writingRow++;
                }
            }

            // send form 2 items to appropriate sheet
            ProcessForForm2(wb, form2Entries);
        }


        public void SaveModifiedTemplate(XLWorkbook wb)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Excel File|*.xlsx",
                Title = "Save Modified Template",
                FileName = "MatchmakerOutput.xlsx"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    wb.SaveAs(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file:\n" + ex.Message);
                }
            }
        }


        private bool IsSubassemblyLt(string? lt)
        {
            if (lt == null)
                return false;

            return lt.StartsWith("LT305")       // PCB
                || lt.StartsWith("B6656409")    // Isoblocks
                || lt.StartsWith("B6656398")
                || lt.StartsWith("C1170152");
        }

        private bool IsPcbLt(string? lt)
        {
            if (lt == null)
                return false;

            return lt.StartsWith("LT305");
        }


        // Helps determine which record is more complete/desireable
        private static int CompletenessScore(MatchResult r)
        {
            int score = 0;
            if (!string.IsNullOrWhiteSpace(r.DesignPn)) score += 2;
            if (!string.IsNullOrWhiteSpace(r.AsBuiltMpn)) score += 2;
            if (!string.IsNullOrWhiteSpace(r.Supplier)) score += 1;
            if (!string.IsNullOrWhiteSpace(r.Rir)) score += 1;


            return score;
        }

        
        public void ProcessForForm2(XLWorkbook wb, List<MatchResult> form2Entries) {
            // process if it needs to go in form 2 or not; if so, populate
            var ws2 = wb.Worksheet("Form 2");
            var ws3 = wb.Worksheet("Form 2 - Cont.");


            // Locate the header row.
            var startCell = ws2.Search("Type*").FirstOrDefault();
            if (startCell == null)
            {
                throw new InvalidOperationException("Header not found in template; template corrupted. Everything is broken.");
            }

            int writingRow = startCell.Address.RowNumber + 1;
            
            foreach (var entry in form2Entries) {

                // Form 2
                ws2.Cell(writingRow, 2).Value = "Material";                                                                         // Type will always be material for these items
                ws2.Cell(writingRow, 3).Value = entry.DesignPn != null ? "FN " + entry.FindNumber + "-" + entry.AsBuiltMpn : "";    // material or process name
                ws2.Cell(writingRow, 4).Value = entry.Specification ?? "";                                                          // spec num
                ws2.Cell(writingRow, 5).Value = "N/A";                                                                              // code N/A always
                ws2.Cell(writingRow, 6).Value = entry.Supplier ?? "";                                                               // supplier
                ws2.Cell(writingRow, 7).Value = "N/A";                                                                              // customer approval verification n/a always
                ws2.Cell(writingRow, 8).Value = FindCofCNumber(ws2);                                                                // CofC           

                // Form 2 continued
                // where/in what drawing do I find this number
                writingRow++;
            }

            
        }

        private string? FindCofCNumber(IXLWorksheet ws) {
            //c of c number -> reports qual wo FA reports pdf
            string text = "";
            //using (var pdf = PdfDocument.Open(path)) {
            //    foreach (var page in pdf.GetPages())
            //    {
            //        text += page.Text + "\n";
            //    }
            //}

            //var results = new List<string>();
            //string pattern = @"lot code[^A-Za-z0-9]*([A-Za-z0-9\-]+)";      // searches PDF item questions for "lot code" and filters out extra stuff

            //foreach (Match m in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
            //{
            //    results.Add(m.Groups[1].Value);
            //}

            return text;
        }

        //private void WriteDebugFile(string path, List<MatchResult> distinctResults)
        //{
        //    using (var writer = new StreamWriter(path))
        //    {
        //        foreach (var r in distinctResults)
        //        {
        //            writer.WriteLine("----- Result -----");
        //            writer.WriteLine($"FindNumber: {r.FindNumber}");
        //            writer.WriteLine($"DesignPn: {r.DesignPn}");
        //            writer.WriteLine($"AsBuiltMpn: {r.AsBuiltMpn}");

        //            string type = (r.LtNumber != null && r.LtNumber?.StartsWith("LT305") == true)
        //                ? "Subassembly"
        //                : "COTS";

        //            writer.WriteLine($"Type: {type}");
        //            writer.WriteLine($"Supplier: {r.Supplier ?? ""}");
        //            writer.WriteLine($"LtNumber: {r.LtNumber}");
        //            writer.WriteLine($"Rir: {r.Rir}");
        //            writer.WriteLine(); // blank line
        //        }
        //    }
        //}

    }
}
