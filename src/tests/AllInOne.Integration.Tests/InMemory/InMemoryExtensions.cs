using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AllInOne.Integration.Tests.InMemory
{
    public static class InMemoryExtensions
    {
        public static ILoggingBuilder AddInMemoryLogger(this ILoggingBuilder builder, [NotNull]List<string> output)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new InMemoryLoggerProvider(output));
            return builder;
        }
    }
}
