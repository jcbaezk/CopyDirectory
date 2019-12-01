using System.Threading.Tasks;

namespace CopyDirectory.Process.Process
{
    public interface IDirectoryCopier
    {
        Task CopyAsync(string sourceDirectory, string targetDirectory);
    }
}