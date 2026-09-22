using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class LoaderFactory : ILoaderFactory
    {
        public IExcelLoader<InovarBomEntry> CreateInovarBomLoader()
            => new InovarBomLoader();

        public IExcelLoader<AsBuiltEntry> CreateAsBuiltLoader()
            => new AsBuiltLoader();


        public CBomLoader CreateCBomLoader()
            => new CBomLoader();

        public FAReportLoader CreateFAReportLoader()
            => new FAReportLoader();
    }
}
