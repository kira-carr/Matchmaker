using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class ServiceFactory
    {
        public MatcherService CreateMatcherService()
            => new MatcherService();
        public KeyBuilderService CreateKeyBuilderService()
            =>new KeyBuilderService();
        public NormalizerService CreateNormalizerService()
            => new NormalizerService();

    }
}
