﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace OptimineLoader
{
    class VersionChecker
    {
        public string ActualHashes;
        private const bool _COMPONENTNOTFOUND = false;
        public bool DownloadHashes()
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                using (WebClient webClient = new WebClient())
                {
                    ActualHashes = webClient.DownloadString(Config.WebDir + Config.WebHashes);
                }
                return ActualHashes != string.Empty;
            }
            catch
            {
                return false;
            }
            
        }
        public bool IsLegalFile(string path)
        {
            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, (int)stream.Length);
                    byte[] checkSum = md5.ComputeHash(fileData);
                    string result = BitConverter.ToString(checkSum).Replace("-", String.Empty).ToLower();
                    return ActualHashes.Contains(result);
                }
            }
            catch { return _COMPONENTNOTFOUND; }
        }

        public bool IsLegalDir(string path)
        {
            try
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).OrderBy(p => p).ToList();
                MD5 md5 = MD5.Create();
                foreach (var file in files)
                {
                    var relativePath = file.Substring(path.Length + 1);
                    byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                    md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);
                    byte[] contentBytes = File.ReadAllBytes(file);
                    if (file != files.Last())
                        md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                    else
                        md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                }
                var result = BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToLower();
                return ActualHashes.Contains(result);
            }
            catch { return _COMPONENTNOTFOUND; }
        }
    }
}