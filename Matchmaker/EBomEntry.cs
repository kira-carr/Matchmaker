namespace Matchmaker
{
    internal class EBomEntry
    {
        public int FindNumber { get; set; }
        public string? Specification { get; set; }
        public string? DesignPn { get; set; }
        public List<string> ApprovedAlternatePns { get; set; } = new();
    }
}
