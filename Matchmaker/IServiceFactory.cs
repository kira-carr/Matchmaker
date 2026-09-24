namespace Matchmaker
{
    internal interface IServiceFactory
    {
        public MatcherService CreateMatcherService();
        public KeyBuilderService CreateKeyBuilderService();
        public NormalizerService CreateNormalizerService();
    }
}
