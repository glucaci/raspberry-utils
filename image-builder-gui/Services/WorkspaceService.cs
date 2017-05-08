using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageBuilder.Services
{
    public class WorkspaceService
    {
        private readonly DirectoryInfo _workspace;

        public WorkspaceService()
        {
            EnsureInitialized();

            _workspace = Directory.CreateDirectory(Path.Combine(AppEnvironment.WorkspaceFolder, Guid.NewGuid().ToString()));
        }

        private void EnsureInitialized()
        {
            if (!Directory.Exists(AppEnvironment.WorkspaceFolder))
            {
                Directory.CreateDirectory(AppEnvironment.WorkspaceFolder);
            }
        }

        public Task CopyResourcesAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c xcopy {AppEnvironment.ResourceFolder} {_workspace.FullName} /e /i",
                UseShellExecute = true
            };

            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(true);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }

        public ImageConfiguration ReadImageConfiguration()
        {
            var result = new ImageConfiguration();

            var dockerFile = _workspace.GetFiles("Dockerfile").FirstOrDefault();
            if (dockerFile != null)
            {
                result.DockerFile = dockerFile;
            }

            return result;
        }
    }

    public class ImageConfiguration
    {
        public FileInfo DockerFile { get; set; }
    }
}