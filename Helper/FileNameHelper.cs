using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class FileNameHelper
    {

        /// <summary>
        /// hàm tạo tên file an toàn vói trình duyệt web
        /// </summary>
        /// <param name="rawName"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetSafeFileName(string rawName, string extension = ".html")
        {
            if (string.IsNullOrWhiteSpace(rawName))
                rawName = "download";

            foreach (char c in Path.GetInvalidFileNameChars())
                rawName = rawName.Replace(c, '_');

            rawName = rawName.Trim();

            if (string.IsNullOrEmpty(rawName))
                rawName = "download";

            if (!string.IsNullOrEmpty(extension) &&
                !rawName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                rawName += extension;

            return rawName;
        }

        // Encode an toàn cho HTTP Content-Disposition (RFC 5987)       
        /// <summary>
        /// chuẩn hóa tên file hỗ trợ chính xác cho URL trên trình duyệt web
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string EncodeFileNameForUrl(string fileName)
        {
            throw new NotImplementedException();
        }
    }

}
