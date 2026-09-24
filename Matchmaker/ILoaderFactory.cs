namespace Matchmaker
{
    internal interface ILoaderFactory
    {
        public IExcelLoader<InovarBomEntry> CreateInovarBomLoader();
        public IExcelLoader<AsBuiltEntry> CreateAsBuiltLoader();
       // public IExcelLoader<EBomEntry> CreateCBomLoader();
  
    }
}
