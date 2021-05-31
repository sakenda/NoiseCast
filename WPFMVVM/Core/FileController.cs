using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NoiseCast.Core
{
    public static class FileController
    {
        /// <summary>
        /// <see cref="File.WriteAllText(string, string?)"/> implementation with path and content checking
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void WriteAllText(string path, string content)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(content))
                return;

            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Download and save image
        /// </summary>
        /// <param name="webPath">Webpath to file</param>
        /// <param name="localPath">Localpath to save to</param>
        /// <returns></returns>
        public async static Task<string> DownloadImageAndSave(string webPath, string localPath)
        {
            if (string.IsNullOrWhiteSpace(webPath) && string.IsNullOrWhiteSpace(localPath)) return null;

            if (!File.Exists(localPath))
            {
                using (var client = new HttpClient())
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(webPath);
                    await File.WriteAllBytesAsync(localPath, imageBytes);
                }
            }

            return localPath;
        }

        /// <summary>
        /// Delete single file
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }

        /// <summary>
        /// Delete multiple files
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!File.Exists(filePath)) continue;

                File.Delete(filePath);
            }
        }
    }
}