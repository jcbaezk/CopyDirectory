using System.Collections.Generic;

namespace CopyDirectory.Process.Validator
{
    public interface IDirectoryValidator
    {
        bool PathsExist(IEnumerable<string> paths);
    }
}