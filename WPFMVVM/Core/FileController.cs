using System;
using System.IO;

namespace WPFMVVM.Core
{
    public class FileController
    {
        public T FileExists<T>(string path, string name, Func<T> func)
        {
            if (File.Exists(path + name))
            {
                return func();
            }

            return default;
        }
    }
}