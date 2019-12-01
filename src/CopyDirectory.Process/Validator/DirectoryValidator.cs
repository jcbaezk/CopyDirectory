using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace CopyDirectory.Process.Validator
{
    public class DirectoryValidator : IDirectoryValidator
    {
        private readonly IFileSystem _fileSystem;

        public DirectoryValidator(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public bool PathsExist(IEnumerable<string> paths)
        {
            return paths.All(x => _fileSystem.Directory.Exists(x));
        }
    }
}