using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Matchmaker;
using System;
using System.Collections.Generic;
using System.Text;

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






