using ClosedXML.Excel;

namespace Matchmaker
{
    internal class InovarBomLoader : IExcelLoader<InovarBomEntry>
    {

        public List<InovarBomEntry> Load(string filePath)
        {
            var results = new List<InovarBomEntry>();


            try {
                using (var wb = new XLWorkbook(filePath))
                {

                    var ws = wb.Worksheet(1);


                    // Validate required columns based on header row
                    var headerRow = ws.Row(2);


                    // Required column names (case-insensitive)
                    var requiredColumns = new[]
                    {
                        "Line Item",
                        "Part #",
                        "Manufacturer Part Number"
                    };

                    // Scan all header columns and build a lookup
                    var headerLookup =
                        headerRow.Cells()
                                 .Where(c => !string.IsNullOrWhiteSpace(c.GetValue<string>()))
                                 .ToDictionary(
                                     cell => cell.GetValue<string>().Trim(),
                                     cell => cell.Address.ColumnNumber,
                                     StringComparer.OrdinalIgnoreCase);

                    // Check if required headers exist
                    var missing = requiredColumns
                        .Where(col => !headerLookup.ContainsKey(col))
                        .ToList();

                    if (missing.Any())
                    {
                        string message =
                            "Your Inovar BOM is missing required columns:\n\n" +
                            string.Join("\n", missing) +
                            "\n\nPlease make sure you uploaded the correct BOM file.";

                        MessageBox.Show(message, "Invalid BOM File", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        throw new ApplicationException("The uploaded BOM file is missing required columns.");
                    }

                    // Dynamic column lookup
                    int findNumberCol = headerLookup["Line Item"];
                    int ltNumberCol = headerLookup["Part #"];
                    int mpnCol = headerLookup["Manufacturer Part Number"];


                    // skip row 1 and 2 headers
                    foreach (var row in ws.RowsUsed().Skip(2))
                    {
                        var entry = new InovarBomEntry
                        {
                            FindNumber = row.Cell(1).GetValue<int>(),
                            LtNumber = row.Cell(4).GetValue<string>(),
                            InovarBomMpn = row.Cell(9).GetValue<string>()
                        };

                        results.Add(entry);
                    }
                }

                return results;
            }
            catch (IOException ex) {                    // if file is open/locked

                throw new ApplicationException($"The file '{filePath}' is currently open or locked. Please close the file and try again.", ex);

            }
            catch (UnauthorizedAccessException ex) {    // if locked during autosave
                throw new ApplicationException($"The file '{filePath}' could not be accessed. Please close the file and try again.", ex);

            }
            catch (Exception ex)
            {    // other exceptions
                throw new ApplicationException($"An error occurred while loading '{filePath}': {ex.Message}", ex);

            }
        }

    }
}
