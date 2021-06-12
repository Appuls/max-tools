using System;
using System.IO;

namespace MaxToolsUi.Utilities
{
    public static class FSUtilities
    {
        public static void CreateFileDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException(nameof(filePath));

            var dirName = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(dirName))
                throw new Exception("Directory name is empty.");

            var dirInfo = new DirectoryInfo(dirName);
            dirInfo.Create();
        }
    }
}
