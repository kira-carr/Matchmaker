using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal interface IExcelLoader<T>
    {
        public List<T> Load(string filePath);
    }
}
