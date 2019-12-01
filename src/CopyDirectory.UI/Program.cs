using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using CopyDirectory.Process.Process;
using CopyDirectory.UI.Validators;
using Easy.MessageHub;
using Microsoft.Extensions.DependencyInjection;

namespace CopyDirectory.UI
{
    class Program
    {
        private static IArgumentsValidator _argumentsValidator;
        private static IDirectoryCopier _directoryCopier;
        private static IMessageHub _messageHub;

        static async Task Main(string[] args)
        {
            var serviceProvider = RegisterServices();
            ResolveDependencies(serviceProvider);

            if (_argumentsValidator.IsValid(args))
            {
                var sourceDirectory = args[0];
                var targetDirectory = args[1];

                Console.WriteLine("Directory copy process started");

                _messageHub.Subscribe<string>(x => Console.WriteLine($"Copying file {x}"));
                await _directoryCopier.CopyAsync(sourceDirectory, targetDirectory);

                Console.WriteLine("Directory copy process finished");
            }
            else
            {
                Console.WriteLine("Arguments not valid, verify that source and target directories exist.");
            }
        }

        private static ServiceProvider RegisterServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IFileSystem, FileSystem>()
                .AddSingleton<IMessageHub, MessageHub>()
                .Scan(scan => scan
                    .FromAssemblyOf<IArgumentsValidator>()
                    .AddClasses()
                    .AsImplementedInterfaces()
                    .WithScopedLifetime())
                .Scan(scan => scan
                    .FromAssemblyOf<IDirectoryCopier>()
                    .AddClasses()
                    .AsImplementedInterfaces()
                    .WithScopedLifetime())
                .BuildServiceProvider();

            return serviceProvider;
        }

        private static void ResolveDependencies(ServiceProvider serviceProvider)
        {
            _directoryCopier = serviceProvider.GetService<IDirectoryCopier>();
            _argumentsValidator = serviceProvider.GetService<IArgumentsValidator>();
            _messageHub = serviceProvider.GetService<IMessageHub>();
        }
    }
}
