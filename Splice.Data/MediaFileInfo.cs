using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public abstract class MediaFileInfo
    {
        private string _Filename;

        public Int32 Id { get; set; }
        public string Filename { get { return System.IO.Path.GetFileName(Path); } }
        public Int64 Size { get; set; }
        public string Path { get { return _Filename; } set { _Filename = value; } }
        public Int32 Duration { get; set; }

        public bool Exists
        {
            get
            {
                return File.Exists(Path);
            }
        }

        public string GetRelativePath(string Root)
        {
            if (Path.StartsWith(Root))
            {
                return Path.Remove(0, Root.Length);
            }
            else
            {
                return Path;
            }
        }

        public MediaFileInfo(string Filename)
        {
            _Filename = Filename;
        }

        public MediaFileInfo(DataRow Row)
        {
            Path = Row["path"].ToString();
            Id = Convert.ToInt32(Row["id"]);
            Size = Convert.ToInt32(Row["size"]);
            Duration = 0;
        }

        public string Hash
        {
            get
            {
                if (!Exists)
                {
                    throw new FileNotFoundException("File doesn't exists.", Path);
                }

                FileStream File = new FileStream(Path, FileMode.Open);

                byte[] Bytes;
                if (File.Length < 200)
                {
                    Bytes = new byte[File.Length];
                    File.Read(Bytes, 0, Bytes.Length);
                }
                else
                {
                    byte[] StartBuffer = new byte[100];
                    byte[] EndBuffer = new byte[100];
                    File.Read(StartBuffer, 0, StartBuffer.Length);
                    File.Seek(File.Length - EndBuffer.Length, SeekOrigin.Begin);

                    Bytes = new byte[StartBuffer.Length + EndBuffer.Length];
                    System.Buffer.BlockCopy(StartBuffer, 0, Bytes, 0, StartBuffer.Length);
                    System.Buffer.BlockCopy(EndBuffer, 0, Bytes, StartBuffer.Length, EndBuffer.Length);
                }

                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] Hash = cryptHandler.ComputeHash(Bytes);

                string HashString = "";
                foreach (byte B in Hash)
                {
                    if (B < 16)
                        HashString += "0" + B.ToString("x");
                    else
                        HashString += B.ToString("x");
                }

                File.Close();

                return HashString;
            }
        }
    }
}
