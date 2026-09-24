using ClosedXML.Excel;
using Matchmaker;

internal class AsBuiltLoader : IExcelLoader<AsBuiltEntry>
{
    private static int RequireColumn(IXLWorksheet ws, string header)
    {

        var headerRow = ws.Row(1);

        // Scan header row only
        var headerLookup =
            headerRow.Cells()
                     .Where(c => !string.IsNullOrWhiteSpace(c.GetValue<string>()))
                     .ToDictionary(
                         cell => cell.GetValue<string>().Trim(),
                         cell => cell.Address.ColumnNumber,
                         StringComparer.OrdinalIgnoreCase);

        if (!headerLookup.TryGetValue(header, out int col))
        {
            MessageBox.Show(
                $"Required column '{header}' was not found in the uploaded As-Built file.\n\n" +
                $"Please make sure you selected the correct As-Built export.",
                "Invalid As-Built File",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            throw new ApplicationException($"Required column '{header}' was not found.");
        }

        return col;

    }

    public List<AsBuiltEntry> Load(string filePath)
    {
        var results = new List<AsBuiltEntry>();

        try
        {
            using (var wb = new XLWorkbook(filePath))
            {
                var ws = wb.Worksheet(1);

                int LtNumCol = RequireColumn(ws, "Part #");
                int AsBuiltMpnCol = RequireColumn(ws, "Description");
                int SupplierCol = RequireColumn(ws, "Supplier");
                int RirCol = RequireColumn(ws, "RIR #");

                foreach (var row in ws.RowsUsed().Skip(1))
                {
                    var entry = new AsBuiltEntry
                    {
                        LtNumber = row.Cell(LtNumCol).GetValue<string>(),
                        AsBuiltMpn = row.Cell(AsBuiltMpnCol).GetValue<string>(),
                        Supplier = row.Cell(SupplierCol).GetValue<string>(),
                        Rir = row.Cell(RirCol).GetValue<string>()
                    };

                    results.Add(entry);
                }
            }

            return results;
        }
        catch (IOException ex)
        {
            throw new ApplicationException(
                $"The file '{filePath}' is currently open or locked. Please close the file and try again.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new ApplicationException(
                $"The file '{filePath}' could not be accessed. Please close the file and try again.", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(
                $"An error occurred while loading '{filePath}': {ex.Message}", ex);
        }
    }
}