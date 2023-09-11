namespace Task_1.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        string _path;
        public FileLoggerProvider(string path)
        {
            this._path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }

        public void Dispose() { }
    }
}
