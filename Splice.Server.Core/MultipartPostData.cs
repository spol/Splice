using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Splice.Server
{
    public class MultipartPostData : PostData
    {
        private String Boundary;

        public MultipartPostData(HttpListenerRequest Request)
        {
            if (!Request.ContentType.StartsWith("multipart/form-data;"))
            {
                throw new Exception("Not a Multipart Post Request");
            }

            MemoryStream MemStream = new MemoryStream();
            Byte[] BinaryData = new Byte[512];
            int ReadCount = 0;
            while ((ReadCount = Request.InputStream.Read(BinaryData, 0, BinaryData.Length)) > 0)
            {
                MemStream.Write(BinaryData, 0, ReadCount);
            }

            Regex R = new Regex("boundary=(-+[a-f0-9]+)$", RegexOptions.Multiline);
            Match M = R.Match(Request.ContentType);
            Boundary = "--" + M.Groups[1].Captures[0].Value;

            MemStream.Seek(0, SeekOrigin.Begin);
            Int64 LastBoundary = -1;
            while (true)
            {
                Int32 DataByte = MemStream.ReadByte();
                if (DataByte == -1)
                {
                    throw new Exception("Reached end of stream before data ended.");
                }
                if (DataByte == (Int32)'-')
                {
                    MemStream.Seek(-1, SeekOrigin.Current);
                    Byte[] Data = new Byte[Boundary.Length + 2];
                    MemStream.Read(Data, 0, Boundary.Length + 2);
                    String Text = Encoding.UTF8.GetString(Data);

                    if (Text.Length >= Boundary.Length && Text.Substring(0, Boundary.Length) == Boundary)
                    {
                        // Start of a new part.
                        if (LastBoundary != -1)
                        {
                            Int64 NewBoundary = MemStream.Position - (Boundary.Length + 2);
                            Int64 Length = NewBoundary - 2 - LastBoundary; // -2 for trailing \r\n

                            MemStream.Seek(LastBoundary, SeekOrigin.Begin);
                            Byte[] FieldData = new Byte[Length];
                            MemStream.Read(FieldData, 0, Convert.ToInt32(Length));
                            MemStream.Seek(NewBoundary + Boundary.Length + 2, SeekOrigin.Begin);

                            // Process Field Data.
                            PostField Field = new PostField(FieldData);

                            Fields.Add(Field.Name, Field);

                        }

                        LastBoundary = MemStream.Position;


                        if (Text == Boundary + "--")
                        {
                            // Final Boundary
                            break;
                        }
                    }

                }
            }
        }
    }
}
