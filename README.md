# SquareWidget.HMAC.Sample

Code sample to exercise SquareWidget.HMAC.Server.Core and SquareWidget.HMAC.Client.Core NuGet Packages 

### Prerequisites

.NET Core 6.0
[SquareWidget.HMAC.Server.Core](https://www.nuget.org/packages/SquareWidget.HMAC.Server.Core)
[SquareWidget.HMAC.Client.Core](https://www.nuget.org/packages/SquareWidget.HMAC.Client.Core)

### Getting Started

See the [documentation](https://squarewidget.com/squarewidget-hmac-middleware) for usage. Download the NuGet package in your .NET Core 6.0 API. SampleApi implements a WidgetsController with an AuthorizeAttribute. The HMAC middleware is configured in Startup.cs and uses the TestSharedSecretStoreService.cs class:

```
using SquareWidget.HMAC.Server.Core;
using System.Threading.Tasks;

namespace SampleApi
{
    public class TestSharedSecretStoreService : SharedSecretStoreService
    {
        public override Task<string> GetSharedSecretAsync(string clientId)
        {
            // TODO: Use clientId to get the shared secret from 
            // Azure Key Vault, IdentityServer4, or a database
            return Task.Run(() => "P@ssw0rd");
        }
    }
}
```

In your application you need to implement this to get the shared secret for the clientId that is passed into the async method.

The SampleUI is a routine CRUD app that references the SquareWidget.HMAC.Client.Core NuGet package. This client package has an implementation of HmacHttpClient that can be used out of the box in the controller:


```
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleUI.Models;
using SquareWidget.HMAC.Client.Core;

namespace SampleUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _baseAddress;
        private readonly ClientCredentials _credentials;
        private readonly HmacHttpClient _client;

        // use DI in a real app
        public HomeController()5001
        {
            _baseAddress = "https://localhost:";
            _credentials = new ClientCredentials
            {
                ClientId = "TestClient",
                ClientSecret = "P@ssw0rd"
            };

            _client = new HmacHttpClient(_baseAddress, _credentials);
        }

        public IActionResult Index()
        {
            var requestUri = "api/widgets";
            var widgets = _client.Get<List<Widget>>(requestUri).Result;
            return View(widgets);
        }

        // Removed for brevity

    }
}

```

## Authors

[James Still](http://www.squarewidget.com)