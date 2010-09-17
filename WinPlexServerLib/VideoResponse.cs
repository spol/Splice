using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WinPlexServer
{
    class VideoResponse : PlexResponse
    {
        public string FilePath { get; set; }

        public Int64 Start { get; set; }
        public Nullable<Int64> End { get; set; }

        public override void Send(HttpListenerResponse response)
        {
            if (FilePath == null)
            {
                throw new Exception("No File specifed for response.");
            }
            FileInfo info = new FileInfo(FilePath);
            if (!info.Exists)
            {
                throw new Exception("File specifed for response does not exist.");
            }

            response.ProtocolVersion = System.Net.HttpVersion.Version10;
            response.Headers.Add("X-Plex-Protocol", "1.0");
            if (Start > 0 || End != null)
            {
                response.StatusCode = (int)HttpStatusCode.PartialContent;
                response.StatusDescription = "Partial Content";
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
            }
            response.ContentType = "application/octet-stream";

            Int64 length = info.Length;
            length = length - Start;
            response.ContentLength64 = length;
            FileStream fs = File.OpenRead(FilePath);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            int totalBytesRead = 0;
            int totalBytesWritten = 0;
            fs.Seek(Start, SeekOrigin.Begin);

            BinaryWriter writer = new BinaryWriter(response.OutputStream);
            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0) {
                totalBytesRead += bytesRead;
                writer.Write(buffer);
                totalBytesWritten += bytesRead;
            }
            writer.Close();
            response.OutputStream.Flush();
            response.OutputStream.Close();
        }
    }
}
