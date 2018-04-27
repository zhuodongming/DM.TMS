using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DM.Infrastructure
{
    /// <summary>
    /// 程序集加载器
    /// 解决Assembly.Load()不自动加载运行目录下的第三方程序集
    /// </summary>
    public class AssemblyLoader : AssemblyLoadContext
    {
        private string folderPath;

        public AssemblyLoader(string folderPath)
        {
            this.folderPath = folderPath;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string filePath = $"{folderPath}{Path.DirectorySeparatorChar}{assemblyName.Name}.dll";//检查运行目录下是否有该程序集
            if (File.Exists(filePath))
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(filePath);
            }

            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }
    }
}
