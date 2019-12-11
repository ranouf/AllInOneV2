using System.Net.Http;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Controllers
{
    public class BaseTest
    {
        public ITestOutputHelper Output { get; private set; }
        public TestServerFixture TestServerFixture { get; private set; }
        public HttpClient Client { get; private set; }

        public BaseTest(ITestOutputHelper output)
        {
            Output = output;
            TestServerFixture = new TestServerFixture(Output);
            Client = TestServerFixture.Client;
        }
    }
}
