using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class FindNumberRecord
    {
        public int FindNumber { get; set; }
        public string? AsBuiltMpn { get; set; }
        public string? FindNumberUnique { get; set; }
        public string? LtNumber { get; set; }
        public string? DesignPn { get; set; }
        public string? InovarBomMpn { get; set; }
        public List<string>? ApprovedAlternates { get; set; } = new();
        public string? Supplier { get; set; }
        public string? Rir { get; set; }
        public string? Specification { get; set; }
    }
}
