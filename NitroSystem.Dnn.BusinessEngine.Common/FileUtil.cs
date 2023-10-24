using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Data;
using System.IO;
using System.Text;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public static class FileUtil
    {
        public static string GetFileContent(string filename)
        {
            try
            {
                if (!File.Exists(filename)) return string.Empty;

                StreamReader objStreamReader = default(StreamReader);
                objStreamReader = File.OpenText(filename);
                string template = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                return template;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static bool CreateTextFile(string fileName, string content, bool deleteIsExists)
        {
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName) && deleteIsExists)
                {
                    File.Delete(fileName);
                }

                if (!Directory.Exists(Path.GetDirectoryName(fileName))) Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file    
                    Byte[] data = new UTF8Encoding(true).GetBytes(content);
                    fs.Write(data, 0, data.Length);

                    fs.Close();
                }

                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public static bool DeleteFile(string fileName, bool raiseException)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                else
                    throw new Exception("File not found!.");

                return true;
            }
            catch (Exception Ex)
            {
                if (raiseException) throw Ex;

                return false;
            }
        }
    }
}