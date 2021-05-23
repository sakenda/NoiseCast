using System;
using System.IO;
using System.Net;
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

        public async static Task<string> DownloadImageAndSave(string webPath, string localPath)
        {
            if (string.IsNullOrWhiteSpace(webPath) || string.IsNullOrWhiteSpace(localPath)) return null;

            using (var client = new HttpClient())
            {
                string uriWithoutQuery = new Uri(webPath).GetLeftPart(UriPartial.Path);
                string fileExtension = Path.GetExtension(uriWithoutQuery);
                string path = localPath + fileExtension;

                byte[] imageBytes = await client.GetByteArrayAsync(webPath);
                await File.WriteAllBytesAsync(path, imageBytes);

                return path;
            }
        }
    }
}