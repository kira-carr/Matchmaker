using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;


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
