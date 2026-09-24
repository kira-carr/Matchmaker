namespace Matchmaker
{
    internal interface IExcelLoader<T>
    {
        public List<T> Load(string filePath);
    }
}
