using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal interface IServiceFactory
    {
        public MatcherService CreateMatcherService();
        public KeyBuilderService CreateKeyBuilderService();
        public NormalizerService CreateNormalizerService();
    }
}
