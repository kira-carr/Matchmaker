using System.Text.RegularExpressions;

namespace Matchmaker
{
    internal class MatchmakerEngine
    {
        private readonly LoaderFactory loaderFactory;
        private readonly ServiceFactory serviceFactory;

        public MatchmakerEngine(LoaderFactory loaderFactory, ServiceFactory serviceFactory) {

            this.loaderFactory = loaderFactory;
            this.serviceFactory = serviceFactory;
        }

        public List<MatchResult> Process(string InovarBomPath, string asBuiltPath, string cBomPath, string faReportPath) {

            // create concrete loaders
            var inovarBomLoader = loaderFactory.CreateInovarBomLoader();
            var asBuiltLoader = loaderFactory.CreateAsBuiltLoader();
            var cBomLoader = loaderFactory.CreateCBomLoader();
            var faLoader = loaderFactory.CreateFAReportLoader();

            // create concrete sevices
            var normalizerService = serviceFactory.CreateNormalizerService();
            var keyBuilderService = serviceFactory.CreateKeyBuilderService();
            var matcherService = serviceFactory.CreateMatcherService();

            // load input data
            var inovarBom = inovarBomLoader.Load(InovarBomPath);
            var asBuilt = asBuiltLoader.Load(asBuiltPath);
            var eBom = cBomLoader.LoadEBom(cBomPath);
            var mpnAlternates = cBomLoader.LoadMpnAlternates(cBomPath);
            var faEntries = faLoader.Load(faReportPath);

            // Merge alternates into eBom
            foreach (var entry in eBom) {

                var matches = mpnAlternates.FindAll(x => x.DesignPn == entry.DesignPn);

                foreach (var alt in matches) {
                    if (alt.ManufacturerMpn is not null) {
                        entry.ApprovedAlternatePns.Add(alt.ManufacturerMpn);
                    }
                }
            }


            // Build an index for FA entries (normalized keys)
            var ns = normalizerService;

            // Index by (MPN+Supplier)
            var faByMpnSupplier = faEntries
                .Where(f => !string.IsNullOrWhiteSpace(f.ManufacturerMpn) && !string.IsNullOrWhiteSpace(f.Supplier))
                .GroupBy(f => ns.NormalizeMpn(f.ManufacturerMpn) + "|" + ns.NormalizeSupplier(f.Supplier))
                .ToDictionary(g => g.Key, g => g.First());

            // Index by Spec
            var faBySpec = faEntries
                .Where(f => !string.IsNullOrWhiteSpace(f.Specification))
                .GroupBy(f => ns.NormalizeSpec(f.Specification))
                .ToDictionary(g => g.Key, g => g.First());

            // Index by MPN only
            var faByMpnOnly = faEntries
                .Where(f => !string.IsNullOrWhiteSpace(f.ManufacturerMpn))
                .GroupBy(f => ns.NormalizeMpn(f.ManufacturerMpn))
                .ToDictionary(g => g.Key, g => g.First());


            // cycle through BOM to join and match
            var results = new List<MatchResult>();
            foreach (var part in inovarBom) {
                var rec = CreateFindNumberRecord(part, asBuilt, eBom);
                if (rec is null) {
                    continue;   // move on without adding if not in as built
                }

                var match = matcherService.EvaluateMatch(rec);
                results.Add(match);

                // NEW: enrich with FA data if Form 2 candidate
                if (IsForm2Candidate(match))
                {
                    var asBuiltMpnNorm = ns.NormalizeMpn(match.AsBuiltMpn ?? "");
                    var supplierNorm = ns.NormalizeSupplier(match.Supplier ?? "");
                    var specNorm = ns.NormalizeSpec(match.Specification ?? "");

                    FAReportEntry? fa = null;

                    // Priority order
                    if (!string.IsNullOrEmpty(asBuiltMpnNorm) && !string.IsNullOrEmpty(supplierNorm))
                    {
                        faByMpnSupplier.TryGetValue(asBuiltMpnNorm + "|" + supplierNorm, out fa);
                    }
                    if (fa is null && !string.IsNullOrEmpty(specNorm))
                    {
                        faBySpec.TryGetValue(specNorm, out fa);
                    }
                    if (fa is null && !string.IsNullOrEmpty(asBuiltMpnNorm))
                    {
                        faByMpnOnly.TryGetValue(asBuiltMpnNorm, out fa);
                    }

                    if (fa is not null)
                    {
                        //match.CofCNumber = fa.CofCNumber;
                        match.LotCode = fa.LotCode;
                    }
                }

                results.Add(match);
            }

            return results;
        }




    private static bool IsForm2Candidate(MatchResult r)
    {
        // Mirror your TemplateManager logic:
        return (r.LtNumber?.StartsWith("LT437") == true && !(r.DesignPn?.StartsWith("H") == true))
            || r.LtNumber?.StartsWith("710") == true
            || r.LtNumber?.StartsWith("001") == true;
    }

    private FindNumberRecord? CreateFindNumberRecord(InovarBomEntry inovarBom, List<AsBuiltEntry> asBuilt, List<EBomEntry> eBom) {
            
            // match using normalized LT part number
            NormalizerService ns = new NormalizerService();
            var ltInovar = ns.NormalizeLt(inovarBom.LtNumber);
            var asBuiltRec = asBuilt.Find(x => ns.NormalizeLt(x.LtNumber) == ltInovar);

            // if no entry for part number in as built, return early
            if (asBuiltRec is null) {
                return null;
            }

            var eBomRec = eBom.Find(x => x.FindNumber == inovarBom.FindNumber);

            return new FindNumberRecord
            {
                FindNumber = inovarBom.FindNumber,
                LtNumber = inovarBom.LtNumber,
                InovarBomMpn = inovarBom.InovarBomMpn,
                AsBuiltMpn = asBuiltRec?.AsBuiltMpn,    // if asBuiltRec is null, use a new empty list
                DesignPn = eBomRec?.DesignPn,
                ApprovedAlternates = eBomRec?.ApprovedAlternatePns,
                Supplier = asBuiltRec?.Supplier,
                Rir = asBuiltRec?.Rir,
                Specification = eBomRec?.Specification
            };
        }
    }
}
