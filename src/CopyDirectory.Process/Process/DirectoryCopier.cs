using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Easy.MessageHub;

namespace CopyDirectory.Process.Process
{
    public class DirectoryCopier : IDirectoryCopier
    {
        private readonly IMessageHub _messageHub;
        private readonly IFileSystem _fileSystem;

        public DirectoryCopier(IMessageHub messageHub, IFileSystem fileSystem)
        {
            _messageHub = messageHub;
            _fileSystem = fileSystem;
        }

        public async Task CopyAsync(string sourceDirectory, string targetDirectory)
        {
            await CopyDirectoryFilesAsync(sourceDirectory, targetDirectory);

            await CopyChildDirectoriesAsync(sourceDirectory, targetDirectory);
        }

        private async Task CopyDirectoryFilesAsync(string sourceDirectory, string targetDirectory)
        {
            if (!_fileSystem.Directory.Exists(targetDirectory))
            {
                _fileSystem.Directory.CreateDirectory(targetDirectory);
            }

            var files = _fileSystem.Directory.GetFiles(sourceDirectory);
            foreach (var file in files)
            {
                var name = _fileSystem.Path.GetFileName(file);
                var targetPath = _fileSystem.Path.Combine(targetDirectory, name);
                _messageHub.Publish(file);
                await CopyFileAsync(file, targetPath);
            }
        }

        private async Task CopyFileAsync(string sourceFile, string targetPath)
        {
            using (var sourceStream = _fileSystem.File.Open(sourceFile, FileMode.Open))
            {
                using (var destinationStream = _fileSystem.File.Create(targetPath))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
        }

        private async Task CopyChildDirectoriesAsync(string sourceDirectory, string targetDirectory)
        {
            var directories = _fileSystem.Directory.GetDirectories(sourceDirectory);
            foreach (var directory in directories)
            {
                var directoryName = _fileSystem.Path.GetFileName(directory);
                var targetPath = _fileSystem.Path.Combine(targetDirectory, directoryName);
                await CopyAsync(directory, targetPath);
            }
        }
    }
}