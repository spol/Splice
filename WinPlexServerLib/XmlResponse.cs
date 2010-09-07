using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace WinPlexServer
{
    public class XmlResponse
    {
        private XmlDocument _xml;
        private HttpListenerResponse _response;
        private HttpStatusCode _statusCode = HttpStatusCode.OK;
        private string _statusDesc = "OK";
        private string _contentType = "text/xml;charset=utf-8";

        //public string Content
        //{
        //    set { _content = value; }
        //    get { return _content; }
        //}

        public XmlDocument XmlDoc
        {
            get
            {
                return _xml;
            }
            set
            {
                _xml = value;
            }
        }

        public XmlResponse(HttpListenerResponse response)
        {
            _response = response;
        }

        public void Send()
        {
            string content = _xml.OuterXml;
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            _response.ProtocolVersion = System.Net.HttpVersion.Version10;
            _response.Headers.Add("X-Plex-Protocol", "1.0");
            _response.StatusCode = (int)_statusCode;
            _response.StatusDescription = _statusDesc;
            _response.ContentType = _contentType;
            _response.ContentEncoding = Encoding.UTF8;
            _response.ContentLength64 = buffer.Length;
            _response.OutputStream.Write(buffer, 0, buffer.Length);
            _response.OutputStream.Close();
        }
    }
}
