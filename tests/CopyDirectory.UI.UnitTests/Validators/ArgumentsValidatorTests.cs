using CopyDirectory.Process.Validator;
using CopyDirectory.UI.Validators;
using FluentAssertions;
using Moq;
using Xunit;

namespace CopyDirectory.UI.UnitTests.Validators
{
    public class ArgumentsValidatorTests
    {
        private readonly ArgumentsValidator _validator;
        private readonly Mock<IDirectoryValidator> _directoryValidator;

        public ArgumentsValidatorTests()
        {
            _directoryValidator = new Mock<IDirectoryValidator>();
            _validator = new ArgumentsValidator(_directoryValidator.Object);
        }

        [Fact]
        public void IsValid_ShouldReturnFalseGivenNullArguments()
        {
            string[] nullArguments = null;

            var result = _validator.IsValid(nullArguments);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnFalseGivenEmptyArguments()
        {
            string[] nullArguments = { };

            var result = _validator.IsValid(nullArguments);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnFalseGivenOnlyOneArgument()
        {
            string[] arguments = { "first" };

            var result = _validator.IsValid(arguments);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnFalseGivenMoreThanTwoArguments()
        {
            string[] arguments = { "first", "second", "third" };

            var result = _validator.IsValid(arguments);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnFalseGivenPathsAreNotValid()
        {
            string[] arguments = { "invalidPath1", "invalidPath2" };
            _directoryValidator.Setup(x => x.PathsExist(arguments)).Returns(false);

            var result = _validator.IsValid(arguments);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnTrueGivenThePathsAreValid()
        {
            string[] arguments = { "validPath1", "validPath2" };
            _directoryValidator.Setup(x => x.PathsExist(arguments)).Returns(true);

            var result = _validator.IsValid(arguments);

            result.Should().BeTrue();
        }
    }
}