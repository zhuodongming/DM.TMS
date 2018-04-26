using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// RAR Helper
    /// </summary>
    public sealed class RAR
    {
        public static bool ExistsWinRar()
        {
            string result = string.Empty;

            string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            if (registryKey != null)
            {
                result = registryKey.GetValue("").ToString();
            }
            registryKey.Close();

            return result == string.Empty ? false : true;
        }

        /// <summary>
        /// 将格式为rar的压缩文件解压到指定的目录
        /// </summary>
        /// <param name="rarFileName">要解压rar文件的路径</param>
        /// <param name="saveDir">解压后要保存到的目录</param>
        /// <param name="pwd">空无密码</param>
        public static void DeCompressRar(string rarFileName, string saveDir, string pwd)
        {
            string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regKey);
            string winrarPath = registryKey.GetValue("").ToString();
            registryKey.Close();
            string winrarDir = System.IO.Path.GetDirectoryName(winrarPath);
            String commandOptions = pwd == string.Empty ? string.Format(" x {0} {1} -y", rarFileName, saveDir) : string.Format("-p{0} x {1} {2} -y", pwd, rarFileName, saveDir);

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = System.IO.Path.Combine(winrarDir, "rar.exe");
            processStartInfo.Arguments = commandOptions;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        /// <summary>
        /// 将目录和文件压缩为rar格式并保存到指定的目录
        /// </summary>
        /// <param name="soruceDir">要压缩的文件夹目录</param>
        /// <param name="rarFileName">压缩后的rar保存路径</param>
        /// <param name="pwd">空无密码</param>
        public static void CompressRar(string soruceDir, string rarFileName, string pwd)
        {
            string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regKey);
            string winrarPath = registryKey.GetValue("").ToString();
            registryKey.Close();
            string winrarDir = System.IO.Path.GetDirectoryName(winrarPath);
            String commandOptions = pwd == string.Empty ? string.Format("a -EP -k -r -s -m1 {0} {1}", rarFileName, soruceDir) : string.Format("-p{0} a -EP -k -r -s -m1 {1} {2}", pwd, rarFileName, soruceDir);

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = System.IO.Path.Combine(winrarDir, "rar.exe");
            processStartInfo.Arguments = commandOptions;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
