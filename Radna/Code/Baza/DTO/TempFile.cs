using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public static class TempFile
    {
        public class TempFileHelper
        {
            public static string CreateTmpFile(bool throwException = true)
            {
                string fileName = string.Empty;

                try
                {
                    // Get the full name of the newly created Temporary file. 
                    // Note that the GetTempFileName() method actually creates
                    // a 0-byte file and returns the name of the created file.
                    fileName = Path.GetTempFileName();

                    // Craete a FileInfo object to set the file's attributes
                    var fileInfo = new FileInfo(fileName);

                    // Set the Attribute property of this file to Temporary. 
                    // Although this is not completely necessary, the .NET Framework is able 
                    // to optimize the use of Temporary files by keeping them cached in memory.
                    fileInfo.Attributes = FileAttributes.Temporary;
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        throw new Exception("Unable to create TEMP file or set its attributes: " + ex.Message, ex);
                    }
                }

                return fileName;
            }

            public static void DeleteTmpFile(string tmpFile, bool throwException = true)
            {
                try
                {
                    // Delete the temp file (if it exists)
                    if (File.Exists(tmpFile))
                    {
                        File.Delete(tmpFile);
                    }
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        throw new Exception("Error deleteing TEMP file: " + ex.Message, ex);
                    }
                }
            }
        }
    }
}
