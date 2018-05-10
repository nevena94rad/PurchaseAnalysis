using Baza.DTO;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Baza.DTO.TempFile;

namespace Baza.R
{
    public static class RConsoleHelper
    {

        private static ILog log = LogManager.GetLogger(typeof(RConsoleHelper));

        public static double ExecuteRScript(string rCodeFilePath, string p1, string p2, string p3, string p4, string p5, out string model)
        {

            string[] args_r = { p1, p2, p3, p4, p5 };
            double result = RunFromCmd(rCodeFilePath, out model, args_r);
            return result;
        }

        private static double RunFromCmd(string filePath,out string model, params string[] args)
        {
            double ret = 0;
            model = "";
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
                    string strCmdLine = @"/c """"" + binPath + @""" " + filePath;
                    if (args.Any())
                    {
                        strCmdLine += " " + file1 + @""" " + args[1] + " " + args[2] + " " + args[3] + " " + args[4];
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
                    result = result.Substring(result.IndexOf("[1]"));
                    var split = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    ret = Convert.ToDouble(split[0].Remove(0,4));
                    model = split[1].Remove(0, 4);
                }
                else
                {
                    result += "R-Core not found in registry";
                }
                return ret;
            }
            catch (Exception ex)
            {
                log.Warn(ex.Message + " Arguments: " + args);
                return -1;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(file1))
                {
                    TempFileHelper.DeleteTmpFile(file1, false);
                }
            }
        }
    }
}
