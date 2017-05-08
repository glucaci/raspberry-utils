using System;
using System.IO;

namespace ImageBuilder.Services
{
    public static class AppEnvironment
    {
        private static string _appFolder;
        private static string _resourceFolder;
        private static string _workspaceFolder;

        public static string AppFolder
        {
            get
            {
                if (_appFolder == null)
                {
                    var assemblyUrl = typeof(AppEnvironment).Assembly.GetName().CodeBase;
                    var assemblyPath = Path.GetDirectoryName(assemblyUrl);
                    if (assemblyPath != null && assemblyPath.StartsWith(@"file:\", StringComparison.OrdinalIgnoreCase))
                    {
                        assemblyPath = assemblyPath.Remove(0, 6);
                    }
                    _appFolder = assemblyPath;
                }
                return _appFolder;
            }
        }

        public static string ResourceFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_resourceFolder))
                {
                    _resourceFolder = Path.Combine(AppFolder, "Pi-Gen");
                }
                return _resourceFolder;
            }
        }

        public static string WorkspaceFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_workspaceFolder))
                {
                    _workspaceFolder = Path.Combine(AppFolder, "Workspace");
                }
                return _workspaceFolder;
            }
        }
    }
}