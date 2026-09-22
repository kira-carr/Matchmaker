using ClosedXML.Excel;
using Matchmaker;

internal class AsBuiltLoader : IExcelLoader<AsBuiltEntry>
{
    private static int RequireColumn(IXLWorksheet ws, string header)
    {
        var cell = ws.Search(header).FirstOrDefault();
        if (cell == null)
            throw new ApplicationException($"Required column '{header}' was not found in the worksheet.");
        return cell.Address.ColumnNumber;
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