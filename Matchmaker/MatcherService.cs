using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class MatcherService
    {
        public MatchResult EvaluateMatch(FindNumberRecord rec) {

            // if information missing, mismatch
            if (rec.AsBuiltMpn is null) return Mismatch(rec);
            if(rec.DesignPn is null) return Mismatch(rec);

            // if perfect match
            if (rec.AsBuiltMpn == rec.DesignPn) {
                return Exact(rec);
            }
            // if acceptable alternate
            if (rec.ApprovedAlternates is not null && rec.ApprovedAlternates.Contains(rec.AsBuiltMpn)){
                return Alt(rec);
            }
            // otherwise
            return Mismatch(rec);
        }


        private MatchResult Exact(FindNumberRecord rec) {
            return new MatchResult
            {
                FindNumber = rec.FindNumber,
                AsBuiltMpn = rec.AsBuiltMpn,
                FindNumberUnique = rec.FindNumberUnique,
                Supplier = rec.Supplier,
                DesignPn = rec.DesignPn,
                LtNumber = rec.LtNumber,
                ValidAlternates = rec.ApprovedAlternates,
                Rir = rec.Rir,
                Specification = rec?.Specification,
                MatchType = "Exact",
                Notes = "As-built matches design part."
            };        
        }

        private MatchResult Alt(FindNumberRecord rec)
        {
            return new MatchResult
            {
                FindNumber = rec.FindNumber,
                AsBuiltMpn = rec.AsBuiltMpn,
                FindNumberUnique = rec.FindNumberUnique,
                Supplier = rec.Supplier,
                DesignPn = rec.DesignPn,
                LtNumber = rec.LtNumber,
                ValidAlternates = rec.ApprovedAlternates,
                Rir = rec.Rir,
                Specification = rec?.Specification,
                MatchType = "Alternate",
                Notes = "As-built matches an approved alternate."
            };
        }

        private MatchResult Mismatch(FindNumberRecord rec)
        {
            return new MatchResult
            {
                FindNumber = rec.FindNumber,
                AsBuiltMpn = rec.AsBuiltMpn,
                Supplier = rec.Supplier,
                FindNumberUnique = rec.FindNumberUnique,
                DesignPn = rec.DesignPn,
                LtNumber = rec.LtNumber,
                ValidAlternates = rec.ApprovedAlternates,
                Rir = rec.Rir,
                Specification = rec?.Specification,
                MatchType = "Mismatch",
                Notes = "Variance required - part not approved."
            };
        }

    }
}

