using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Controllers
{
    public class BaseTest : IDisposable
    {
        public ITestOutputHelper Output { get; private set; }
        public TestServerFixture TestServerFixture { get; private set; }
        public HttpClient Client { get; private set; }
        public IFormFile Logo
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly
                    .GetManifestResourceNames()
                    .First(str => str.EndsWith("Assets.Logo.png")); 

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    var ms = new MemoryStream();
                    try
                    {
                        stream.CopyTo(ms);
                        return new FormFile(ms, 0, ms.Length, resourceName, resourceName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/png"
                        };
                    }
                    finally
                    {
                        //ms.Dispose();
                    }
                }
            }
        }

        public BaseTest(ITestOutputHelper output)
        {
            Output = output;
            TestServerFixture = new TestServerFixture(Output);
            Client = TestServerFixture.Client;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TestServerFixture.Dispose();
            }
        }
    }
}
