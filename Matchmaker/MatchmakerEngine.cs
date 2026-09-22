using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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

            // create concrete sevices
            var normalizerService = serviceFactory.CreateNormalizerService();
            var keyBuilderService = serviceFactory.CreateKeyBuilderService();
            var matcherService = serviceFactory.CreateMatcherService();

            // load input data
            var inovarBom = inovarBomLoader.Load(InovarBomPath);
            var asBuilt = asBuiltLoader.Load(asBuiltPath);
            var eBom = cBomLoader.LoadEBom(cBomPath);
            var mpnAlternates = cBomLoader.LoadMpnAlternates(cBomPath);

            // Merge alternates into eBom
            foreach (var entry in eBom) {

                var matches = mpnAlternates.FindAll(x => x.DesignPn == entry.DesignPn);

                foreach (var alt in matches) {
                    if (alt.ManufacturerMpn is not null) {
                        entry.ApprovedAlternatePns.Add(alt.ManufacturerMpn);
                    }
                }
            }

            // cycle through BOM to join and match
            var results = new List<MatchResult>();
            foreach (var part in inovarBom) {
                var rec = CreateFindNumberRecord(part, asBuilt, eBom);
                if (rec is null) {
                    continue;   // move on without adding if not in as built
                }

                var match = matcherService.EvaluateMatch(rec);
                results.Add(match);
            }

            return results;
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
