using ClosedXML.Excel;

namespace Matchmaker
{
    internal class CBomLoader
    {
        public List<EBomEntry> LoadEBom(string filePath) {


            var list = new List<EBomEntry>();


            try
            {
                using (var wb = new XLWorkbook(filePath)) {
                    var ws = wb.Worksheet("BOM");   // find correct sheet in workbook

                    // confirm file formatting
                    var headerRow = ws.Row(2);

                    var requiredHeaders = new[]
                                        {
                        "Find No",
                        "Design Part Number",
                        "Specification",
                        "Approved ALT 1 Design PN",
                        "Approved ALT 2 Design PN"
                    };

                    // Scan header row and build lookup
                    var headerLookup =
                        headerRow.Cells()
                                 .Where(c => !string.IsNullOrWhiteSpace(c.GetValue<string>()))
                                 .ToDictionary(
                                     cell => cell.GetValue<string>().Trim(),
                                     cell => cell.Address.ColumnNumber,
                                     StringComparer.OrdinalIgnoreCase);

                    // Check for missing columns
                    var missing = requiredHeaders
                        .Where(req => !headerLookup.ContainsKey(req))
                        .ToList();

                    if (missing.Any())
                    {
                        var msg =
                            "Your CBOM is missing required columns:\n\n" +
                            string.Join("\n", missing) +
                            "\n\nPlease verify that you uploaded the correct CBOM file.";

                        MessageBox.Show(msg, "Invalid CBOM File", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                        throw new ApplicationException("CBOM missing required columns.");
                    }

                    // Dynamically map column indices
                    int colFindNo = headerLookup["Find No"];
                    int colDesignPn = headerLookup["Design Part Number"];
                    int colSpec = headerLookup["Specification"];
                    int colAlt1 = headerLookup["Approved ALT 1 Design PN"];
                    int colAlt2 = headerLookup["Approved ALT 2 Design PN"];

                    // skip 1st and 2nd rows of BOM
                    foreach (var row in ws.RowsUsed().Skip(2))
                    {
                        var entry = new EBomEntry
                        {
                            FindNumber = row.Cell(1).GetValue<int>(),
                            DesignPn = row.Cell(4).GetValue<string>(),
                            Specification = row.Cell(6).GetValue<string>(),
                        };

                        var alt1 = row.Cell(7).GetValue<string>();
                        if (!string.IsNullOrWhiteSpace(alt1))   // if cell has content, add to alts
                            entry.ApprovedAlternatePns.Add(alt1);

                        var alt2 = row.Cell(8).GetValue<string>();
                        if (!string.IsNullOrWhiteSpace(alt2))   // if cell has content, add to alts
                            entry.ApprovedAlternatePns.Add(alt2);

                        list.Add(entry);
                    }
                }

                return list;
            }
            catch (IOException ex)
            {                    // if file is open/locked

                throw new ApplicationException($"The file '{filePath}' is currently open or locked. Please close the file and try again.", ex);

            }
            catch (UnauthorizedAccessException ex)
            {    // if locked during autosave
                throw new ApplicationException($"The file '{filePath}' could not be accessed. Please close the file and try again.", ex);

            }
            catch (Exception ex)
            {    // other exceptions
                throw new ApplicationException($"An error occurred while loading '{filePath}': {ex.Message}", ex);

            }
        }





        public List<MpnAlternateEntry> LoadMpnAlternates(string filePath) {

            var list = new List<MpnAlternateEntry>();

            try
            {
                using (var wb = new XLWorkbook(filePath))
                {
                    if (!wb.TryGetWorksheet("MPN", out var ws)) // if there's not an MPN, no biggie
                        return list;


                    var headerRow = ws.Row(5);
                    var requiredHeaders = new[]
                                        {
                        "Find No",
                        "Design Part",
                        "Specification",
                        "Approved ALT 1 Design PN",
                        "Approved ALT 2 Design PN"
                    };

                    // Scan header row and build lookup
                    var headerLookup = headerRow.Cells()
                                 .Where(c => !string.IsNullOrWhiteSpace(c.GetValue<string>()))
                                 .ToDictionary(
                                     cell => cell.GetValue<string>().Trim(),
                                     cell => cell.Address.ColumnNumber,
                                     StringComparer.OrdinalIgnoreCase);

                    // Check for missing columns
                    var missing = requiredHeaders
                        .Where(req => !headerLookup.ContainsKey(req))
                        .ToList();

                    if (missing.Any())
                    {
                        var msg =
                            "Your CBOM is missing required columns:\n\n" +
                            string.Join("\n", missing) +
                            "\n\nPlease verify that you uploaded the correct CBOM file.";

                        MessageBox.Show(msg, "Invalid CBOM File", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                        throw new ApplicationException("CBOM missing required columns.");
                    }

                    // Dynamically map column indices
                    int colFindNo = headerLookup["Find No"];
                    int colDesignPn = headerLookup["Design Part"];
                    int colSpec = headerLookup["Specification"];
                    int colAlt1 = headerLookup["Approved ALT 1 Design PN"];
                    int colAlt2 = headerLookup["Approved ALT 2 Design PN"];


                    // data doesn't start until row 6
                    foreach (var row in ws.RowsUsed().Skip(5))
                    {
                        list.Add(new MpnAlternateEntry
                        {
                            DesignPn = row.Cell(1).GetValue<string>(),
                            ManufacturerMpn = row.Cell(9).GetValue<string>()
                        });
                    }
                }

                return list;
            }
            catch (IOException ex)
            {                    // if file is open/locked

                throw new ApplicationException($"The file '{filePath}' is currently open or locked. Please close the file and try again.", ex);

            }
            catch (UnauthorizedAccessException ex)
            {    // if locked during autosave
                throw new ApplicationException($"The file '{filePath}' could not be accessed. Please close the file and try again.", ex);

            }
            catch (Exception ex)
            {    // other exceptions
                throw new ApplicationException($"An error occurred while loading '{filePath}': {ex.Message}", ex);

            }

        }
    }
}






