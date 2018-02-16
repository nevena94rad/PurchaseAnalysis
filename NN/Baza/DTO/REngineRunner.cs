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
    public static class REngineRunner
    {
        public static double RunFromCmd(string filePath, params string[] args)
        {
            double ret = 0;
            // Not required. But our R scripts use allmost all CPU resources if run multiple instances
            
                string result = string.Empty;
                string file1 = string.Empty;
                string file2 = string.Empty;

            try
            {
                file1 = TempFileHelper.CreateTmpFile();
                using (var streamWriter = new StreamWriter(new FileStream(file1, FileMode.Open, FileAccess.Write)))
                {
                    streamWriter.Write(args[0]);
                }
                file2 = TempFileHelper.CreateTmpFile();
                using (var streamWriter = new StreamWriter(new FileStream(file2, FileMode.Open, FileAccess.Write)))
                {
                    streamWriter.Write(args[1]);
                }
                // Get path to R
                var rCore = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-core") ??
                            Registry.CurrentUser.OpenSubKey(@"SOFTWARE\R-core");
                var is64Bit = Environment.Is64BitProcess;
                if (rCore != null)
                {
                    var r = rCore.OpenSubKey(is64Bit ? "R64" : "R");
                    var installPath = (string)r.GetValue("InstallPath");
                    var binPath = Path.Combine(installPath, "bin");
                    binPath = Path.Combine(binPath, is64Bit ? "x64" : "i386");
                    binPath = Path.Combine(binPath, "Rscript");
                    string strCmdLine = @"/c """"" + binPath + @""" """ + filePath + @""" """ + file1 + @""" """ + file2 + @"""""";
                    if (args.Any())
                    {
                        strCmdLine += " " + args[2] + " " + args[3] + " " + args[4];
                    }
                    var info = new ProcessStartInfo("cmd", strCmdLine);
                    info.RedirectStandardInput = false;
                    info.RedirectStandardOutput = true;
                    info.UseShellExecute = false;
                    info.CreateNoWindow = true;
                    using (var proc = new Process())
                    {
                        proc.StartInfo = info;
                        proc.Start();
                        result = proc.StandardOutput.ReadToEnd();
                    }
                    ret = Convert.ToDouble(result.Split(' ')[1]);
                }
                else
                {
                    result += "R-Core not found in registry";
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("R failed to compute. Output: " + result, ex);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(file1))
                {
                    TempFileHelper.DeleteTmpFile(file1, false);
                }
                if (!string.IsNullOrWhiteSpace(file2))
                {
                    TempFileHelper.DeleteTmpFile(file2, false);
                }
            }
            
        }
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
