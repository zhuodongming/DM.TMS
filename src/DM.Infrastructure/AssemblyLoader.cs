using Microsoft.Extensions.DependencyModel;
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
            var library = DependencyContext.Default.CompileLibraries.Where(d => d.Name.Equals(assemblyName.Name)).FirstOrDefault();//检查依赖上下文中是否有该程序集
            if (library != null)
            {
                return Assembly.Load(new AssemblyName(library.Name));
            }

            var apiApplicationFileInfo = new FileInfo($"{folderPath}{Path.DirectorySeparatorChar}{assemblyName.Name}.dll");//检查运行目录下是否有该程序集
            if (File.Exists(apiApplicationFileInfo.FullName))
            {
                var asl = new AssemblyLoader(apiApplicationFileInfo.DirectoryName);
                return asl.LoadFromAssemblyPath(apiApplicationFileInfo.FullName);
            }

            return Assembly.Load(assemblyName);
        }
    }
}
