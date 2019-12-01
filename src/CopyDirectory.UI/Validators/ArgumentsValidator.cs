using System.Collections.Generic;
using CopyDirectory.Process.Validator;

namespace CopyDirectory.UI.Validators
{
    public class ArgumentsValidator : IArgumentsValidator
    {
        private readonly IDirectoryValidator _directoryValidator;

        public ArgumentsValidator(IDirectoryValidator directoryValidator)
        {
            _directoryValidator = directoryValidator;
        }

        public bool IsValid(string[] arguments)
        {
            return arguments != null &&
                   arguments.Length == 2 &&
                   PathsExist(arguments);
        }

        private bool PathsExist(IEnumerable<string> paths)
        {
            return _directoryValidator.PathsExist(paths);
        }
    }
}