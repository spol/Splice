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
//            string content = _xml.OuterXml;
//            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ProtocolVersion = System.Net.HttpVersion.Version10;
            response.Headers.Add("X-Plex-Protocol", "1.0");
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.ContentType = "application/octet-stream";
            
            response.ContentLength64 = info.Length;
            FileStream fs = File.OpenRead(FilePath);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0) {
                response.OutputStream.Write(buffer, 0, bytesRead);
            }
            response.OutputStream.Close();
        }
    }
}
