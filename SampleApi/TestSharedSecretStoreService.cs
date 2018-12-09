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
