﻿using Baza.DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.R
{
    public static class RConsoleHelper
    {
        public static double ExecuteRScript(string rCodeFilePath, string p1, string p2, string p3, string p4, string p5)
        {

            string[] args_r = { p1, p2, p3, p4, p5 };
            double result = RunFromCmd(rCodeFilePath, args_r);
            return result;
        }

        private static double RunFromCmd(string filePath, params string[] args)
        {
            double ret = 0;
            // Not required. But our R scripts use allmost all CPU resources if run multiple instances

            string result = string.Empty;
            string file1 = string.Empty;
            //string file2 = string.Empty;

            try
            {
                file1 = TempFile.TempFileHelper.CreateTmpFile();
                using (var streamWriter = new StreamWriter(new FileStream(file1, FileMode.Open, FileAccess.Write)))
                {
                    streamWriter.Write(args[0]);
                }
                //file2 = TempFileHelper.CreateTmpFile();
                //using (var streamWriter = new StreamWriter(new FileStream(file2, FileMode.Open, FileAccess.Write)))
                //{
                //    streamWriter.Write(args[1]);
                //}
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
                   string strCmdLine = @"/c """"" + binPath + @""" """ + filePath + @""" """ + file1 + @"""""";
                    if (args.Any())
                    {
                        strCmdLine += " " + args[1] + " " + args[2] + " " + args[3] + " " + args[4];
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
                    ret = Convert.ToDouble(result.Split(' ')[0]);
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
                    TempFile.TempFileHelper.DeleteTmpFile(file1, false);
                }
                //if (!string.IsNullOrWhiteSpace(file2))
                //{
                //    TempFileHelper.DeleteTmpFile(file2, false);
                //}
            }
        }
    }
}
