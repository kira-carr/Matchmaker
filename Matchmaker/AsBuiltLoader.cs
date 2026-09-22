using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Matchmaker;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class AsBuiltLoader : IExcelLoader<AsBuiltEntry>
    {
        public List<AsBuiltEntry> Load(string filePath)
        {
            var results = new List<AsBuiltEntry>();
            try
            {
                using (var wb = new XLWorkbook(filePath))
                {

                    var ws = wb.Worksheet(1);

                    // column order depends on inonet user's settings; need dynamic column indices
                    int LtNumCol = ws.Search("Part #").FirstOrDefault().Address.ColumnNumber;
                    int AsBuiltMpnCol = ws.Search("Description").FirstOrDefault().Address.ColumnNumber;
                    int SupplierCol = ws.Search("Supplier").FirstOrDefault().Address.ColumnNumber;
                    int RirCol = ws.Search("RIR #").FirstOrDefault().Address.ColumnNumber;

                    // skip row 1 headers
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


