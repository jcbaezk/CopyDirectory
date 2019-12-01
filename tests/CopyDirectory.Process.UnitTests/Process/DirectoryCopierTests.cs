using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using CopyDirectory.Process.Process;
using Easy.MessageHub;
using FluentAssertions;
using Moq;
using Xunit;

namespace CopyDirectory.Process.UnitTests.Process
{
    public class DirectoryCopierTests
    {
        [Fact]
        public async Task CopyAsync_ShouldCopyTopLevelFilesFromDirectory()
        {
            const string sourceDirectory = "c:\\source\\";
            const string targetDirectory = "c:\\target\\";
            const string firstFile = "thefirstfile.csv";
            const string secondFile = "thesecondfile.pdf";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {$"{sourceDirectory}{firstFile}", new MockFileData("Some,content")},
                {$"{sourceDirectory}{secondFile}", new MockFileData("PDF title")}
            });
            var directoryCopier = new DirectoryCopier(new Mock<IMessageHub>().Object, fileSystem);

            await directoryCopier.CopyAsync(sourceDirectory, targetDirectory);

            fileSystem.Directory.Exists(targetDirectory).Should().BeTrue();
            fileSystem.File.Exists($"{targetDirectory}{firstFile}").Should().BeTrue();
            fileSystem.File.Exists($"{targetDirectory}{secondFile}").Should().BeTrue();
        }

        [Fact]
        public async Task CopyAsync_ShouldCopyChildDirectories()
        {
            const string sourceDirectory = "c:\\source\\";
            const string targetDirectory = "c:\\target\\";
            const string firstDirectory = "folder1";
            const string secondDirectory = "folder2";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {$"{sourceDirectory}{firstDirectory}", new MockDirectoryData()},
                {$"{sourceDirectory}{secondDirectory}", new MockDirectoryData()}
            });
            var directoryCopier = new DirectoryCopier(new Mock<IMessageHub>().Object, fileSystem);

            await directoryCopier.CopyAsync(sourceDirectory, targetDirectory);

            fileSystem.Directory.Exists(targetDirectory).Should().BeTrue();
            fileSystem.Directory.Exists($"{targetDirectory}{firstDirectory}").Should().BeTrue();
            fileSystem.Directory.Exists($"{targetDirectory}{secondDirectory}").Should().BeTrue();
        }

        [Fact]
        public async Task CopyAsync_ShouldCopyFilesInsideChildDirectory()
        {
            const string sourceDirectory = "c:\\source\\";
            const string targetDirectory = "c:\\target\\";
            const string childDirectory = "folder1\\";
            const string childFile = "thefirstfile.csv";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {$"{sourceDirectory}{childDirectory}", new MockDirectoryData()},
                {$"{sourceDirectory}{childDirectory}{childFile}", new MockFileData("Some,content")}
            });
            var directoryCopier = new DirectoryCopier(new Mock<IMessageHub>().Object, fileSystem);

            await directoryCopier.CopyAsync(sourceDirectory, targetDirectory);

            fileSystem.Directory.Exists(targetDirectory).Should().BeTrue();
            fileSystem.Directory.Exists($"{targetDirectory}{childDirectory}").Should().BeTrue();
            fileSystem.File.Exists($"{targetDirectory}{childDirectory}{childFile}").Should().BeTrue();
        }
    }
}