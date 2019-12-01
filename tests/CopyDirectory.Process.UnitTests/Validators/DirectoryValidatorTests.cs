using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using CopyDirectory.Process.Validator;
using FluentAssertions;
using Xunit;

namespace CopyDirectory.Process.UnitTests.Validators
{
    public class DirectoryValidatorTests
    {
        [Fact]
        public void Exists_ShouldReturnFalseGivenADirectoryDoesNotExist()
        {
            const string nonExistentDirectory = @"c:\idonotexist";
            var paths = new List<string> { nonExistentDirectory };
            var emptyFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryValidator = new DirectoryValidator(emptyFileSystem);

            var result = directoryValidator.PathsExist(paths);

            result.Should().BeFalse();
        }

        [Fact]
        public void Exists_ShouldReturnTrueGivenTheDirectoriesExists()
        {
            const string firstDirectory = @"c:\iexist";
            const string secondDirectory = @"c:\iexisttoo";
            var paths = new List<string> { firstDirectory, secondDirectory };
            var emptyFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {firstDirectory, new MockDirectoryData()},
                {secondDirectory, new MockDirectoryData()}
            });
            var directoryValidator = new DirectoryValidator(emptyFileSystem);

            var result = directoryValidator.PathsExist(paths);

            result.Should().BeTrue();
        }
    }
}