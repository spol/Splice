using System;
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

        public static XmlResponse Created()
        {
            XmlResponse resp = new XmlResponse();
            resp._contentType = "text/xml";
            resp._statusCode = HttpStatusCode.Created;

            XmlDocument xml = new XmlDocument();
            XmlElement ResponseElement = xml.CreateElement("response");
            xml.AppendChild(ResponseElement);
            XmlElement Message = xml.CreateElement("message");
            Message.InnerText = "Entity Created Successfully";

            ResponseElement.AppendChild(Message);

            resp.XmlDoc = xml;
            return resp;
        }

        public static XmlResponse MethodNotAllowed()
        {
            XmlResponse resp = new XmlResponse();
            resp._contentType = "text/html";
            resp._statusCode = HttpStatusCode.MethodNotAllowed;

            XmlDocument xml = new XmlDocument();
            XmlElement html = xml.CreateElement("html");
            xml.AppendChild(html);
            XmlElement head = xml.CreateElement("head");
            XmlElement title = xml.CreateElement("title");
            title.InnerText = "Not Found";
            head.AppendChild(title);
            XmlElement body = xml.CreateElement("body");
            XmlElement h1 = xml.CreateElement("h1");
            h1.InnerText = "405 Method Not Allowed";
            body.AppendChild(h1);

            html.AppendChild(head);
            html.AppendChild(body);

            resp.XmlDoc = xml;
            return resp;
        }

        public static XmlResponse BadRequest()
        {
            XmlResponse resp = new XmlResponse();
            resp._contentType = "text/html";
            resp._statusCode = HttpStatusCode.BadRequest;

            XmlDocument xml = new XmlDocument();
            XmlElement html = xml.CreateElement("html");
            xml.AppendChild(html);
            XmlElement head = xml.CreateElement("head");
            XmlElement title = xml.CreateElement("title");
            title.InnerText = "Not Found";
            head.AppendChild(title);
            XmlElement body = xml.CreateElement("body");
            XmlElement h1 = xml.CreateElement("h1");
            h1.InnerText = "400 Bad Request";
            body.AppendChild(h1);

            html.AppendChild(head);
            html.AppendChild(body);

            resp.XmlDoc = xml;
            return resp;
        }

        public static XmlResponse OK()
        {
            return OK("Operation completed successfully.");
        }

        public static XmlResponse OK(String Message)
        {
            XmlResponse Response = new XmlResponse();
            Response._contentType = "application/xml";
            Response._statusCode = HttpStatusCode.OK;

            XmlDocument Xml = new XmlDocument();
            XmlElement ResponseElement = Xml.CreateElement("Response");
            Xml.AppendChild(ResponseElement);
            XmlElement MessageElement = Xml.CreateElement("Message");
            MessageElement.InnerText = Message;

            Response.XmlDoc = Xml;
            return Response;
        }
    }
}
