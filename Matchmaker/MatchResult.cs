using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class MatchResult
    {
        public int FindNumber { get; set; }
        public string? LtNumber { get; set;  }
        public string? AsBuiltMpn { get; set; }
        public string? DesignPn { get; set; }
        public string? FindNumberUnique { get; set; }
        public List<string>? ValidAlternates { get; set; }
        public string? MatchType { get; set; }  // "Exact", "Alternate", "Mismatch"
        public string? Notes { get; set; }
        public string? Supplier { get; set; }
        public string? Rir { get; set; }
        public string? Specification { get; set; }


        // From FA Report for form 2
        public string? CofCNumber { get; set; }
        public string? LotCode { get; set; }

    }
}
