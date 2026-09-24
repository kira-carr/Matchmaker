namespace Matchmaker
{

    public class FAReportEntry
    {
        public string? MaterialName { get; set; }     // e.g. "EPOXY", "ADHESIVE" (human readable section header)
        public string? ManufacturerMpn { get; set; }  // if present in FA report
        public string? Supplier { get; set; }         // supplier/vendor/manufacturer
        public string? Specification { get; set; }    // AMS/MIL/ASTM or internal spec
        //public string? CofCNumber { get; set; }       // "CoC", "Certificate #", etc.
        public string? LotCode { get; set; }          // "Lot", "Batch", "Heat", etc.—can hold multiple lot codes
        public string? Notes { get; set; }            // freeform, optional
    }

}
