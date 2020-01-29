using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Xunit
{
    public static class XunitExtensions
    {
        public static ILoggingBuilder AddXunitLogger(this ILoggingBuilder builder, ITestOutputHelper output)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new XunitLoggerProvider(output));
            return builder;
        }
    }
}
