using System;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace ImageBuilder.Services
{
    public class ImageBuilderService
    {
        private readonly WorkspaceService _workspaceService;

        public ImageBuilderService()
        {
            _workspaceService = new WorkspaceService();
        }

        public async Task BuildImageAsync()
        {
            await _workspaceService.CopyResourcesAsync();

            var dockerClient = new DockerClientConfiguration(new Uri("http://localhost:2375/"))
                .CreateClient();

            var imageConfiguration = _workspaceService.ReadImageConfiguration();
             // base image dockerClient.Images.CreateImageAsync()
             // run configurations
            var images = await dockerClient.Images.ListImagesAsync(new ImagesListParameters());
        }
    }
}
