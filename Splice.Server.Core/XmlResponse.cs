﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Splice.Server
{
    public class XmlResponse : PlexResponse
    {
        private XmlDocument _xml;
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

        public XmlResponse()
        {
        }

        public override void Send(HttpListenerResponse response)
        {
            string content = _xml.OuterXml;
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ProtocolVersion = System.Net.HttpVersion.Version10;
            response.Headers.Add("X-Plex-Protocol", "1.0");
            response.StatusCode = (int)_statusCode;
            response.StatusDescription = _statusDesc;
            response.ContentType = _contentType;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        public static XmlResponse NotFound() {
            XmlResponse resp = new XmlResponse();
            resp._contentType = "text/html";
            resp._statusCode = HttpStatusCode.NotFound;

            XmlDocument xml = new XmlDocument();
            XmlElement html = xml.CreateElement("html");
            xml.AppendChild(html);
            XmlElement head = xml.CreateElement("head");
            XmlElement title = xml.CreateElement("title");
            title.InnerText = "Not Found";
            head.AppendChild(title);
            XmlElement body = xml.CreateElement("body");
            XmlElement h1 = xml.CreateElement("h1");
            h1.InnerText = "404 Not Found";
            body.AppendChild(h1);

            html.AppendChild(head);
            html.AppendChild(body);

            resp.XmlDoc = xml;
            return resp;
        }
    }
}