using AllInOne.Domains.Infrastructure.SqlServer;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Data
{
    public abstract class BaseDataBuilder
    {
        public readonly AllInOneDbContext Context;
        public readonly ITestOutputHelper Output;

        public BaseDataBuilder(AllInOneDbContext context, ITestOutputHelper output)
        {
            Context = context;
            Output = output;
        }

        public abstract void Seed();
    }
}
