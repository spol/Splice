using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WinPlexServer
{
    public class ImageResponse : PlexResponse
    {
        public string FilePath { get; set; }

        public override void Send(HttpListenerResponse response)
        {
            FileInfo info = new FileInfo(FilePath);
            if (!info.Exists)
            {
                throw new Exception("File specifed for response does not exist.");
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";

            switch (info.Extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    response.ContentType = "image/jpeg";
                    break;
                case ".png":
                    response.ContentType = "image/png";
                    break;
                case ".gif":
                    response.ContentType = "image/gif";
                    break;
                default:
                    response.ContentType = "application/octet-stream";
                    break;
            }
            try
            {
                byte[] data = File.ReadAllBytes(FilePath);
                BinaryWriter writer = new BinaryWriter(response.OutputStream);
                response.ContentLength64 = Convert.ToInt64(data.Length);
                writer.Write(data, 0, data.Length);
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
