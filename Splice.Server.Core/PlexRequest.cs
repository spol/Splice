using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Splice.Server
{
    public class PlexRequest
    {
        public string AbsolutePath
        {
            get
            {
                if (request.Url.AbsolutePath.EndsWith("/"))
                {

                    return request.Url.AbsolutePath;
                }
                else
                {
                    return request.Url.AbsolutePath + "/";
                }
            }
        }

        public string[] PathSegments
        {
            get
            {
                return AbsolutePath.Trim('/').Split('/');
            }
        }

        public bool IsRoot
        {
            get
            {
                return AbsolutePath == "/";
            }
        }

        public string Controller
        {
            get
            {
                if (IsRoot)
                {
                    return null;
                }
                else
                {
                    return AbsolutePath.Split('/')[1];
                }
            }
        }

        public string ControllerPath
        {
            get
            {
                return AbsolutePath.Substring(Controller.Length + 1);
            }
        }

        public NameValueCollection Headers
        {
            get
            {
                return request.Headers;
            }
        }

        public String Method
        {
            get
            {
                return request.HttpMethod;
            }
        }

        //private List<?> _Files;
        private Dictionary<String, MimePostField> _Files = new Dictionary<string, MimePostField>();
        public Dictionary<String, MimePostField> Files
        {
            get
            {
                return _Files;
            }
        }
        private List<PostField> _Fields = new List<PostField>();
        private NameValueCollection _PostData;
        public NameValueCollection PostData
        {
            get
            {
                if (_PostData == null)
                {
                    ParsePostData();
                }
                return _PostData;
            }
        }

        private void ParsePostData()
        {
            if (!request.HasEntityBody)
            {
                return;
            }
            else
            {
                _PostData = new NameValueCollection();
                StreamReader Reader = new StreamReader(request.InputStream);
                String Data = Reader.ReadToEnd();

                NameValueCollection Fields = new NameValueCollection();

                if (request.ContentType.StartsWith("multipart/form-data;"))
                {
                    Regex R = new Regex("boundary=(-+[a-f0-9]+)$", RegexOptions.Multiline);

                    Match M = R.Match(request.ContentType);

                    String Boundary = "--" + M.Groups[1].Captures[0].Value;

                    Regex FieldRegex = new Regex("^" + Boundary + @"\s+\n(.*?)(?=" + Boundary + ")", RegexOptions.Multiline | RegexOptions.Singleline);

                    MatchCollection FieldMatches = FieldRegex.Matches(Data);

                    List<MimePostField> PostFields = new List<MimePostField>();
                    foreach (Match Field in FieldMatches)
                    {
                        MimePostField F = new MimePostField(Field.Groups[1].Captures[0].Value);
                        if (F.IsFile)
                        {
                            _Files.Add(F.Name, F);
                        }
                        else {
                            _PostData.Add(F.Name, F.Value);
                        }
                        _Fields.Add(F);
                    }

                }
                else
                {
                    String[] KeyValues = Data.Split('&');
                    foreach (String KeyValue in KeyValues)
                    {
                        String[] Parts = KeyValue.Split('=');
                        _PostData.Add(Parts[0], Parts[1]);
                    }
                }

                //_PostData = Fields;
            
    
            }
        }

        private HttpListenerRequest request;


        public PlexRequest(HttpListenerRequest req)
        {

            request = req;
        }
    }
}
