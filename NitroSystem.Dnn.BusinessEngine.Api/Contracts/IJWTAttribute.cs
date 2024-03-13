using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace NitroSystem.Dnn.BusinessEngine.Api.Contracts
{
    public abstract class IJWTAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            await JWTAuthenticateAsync(context, cancellationToken);
        }

        protected abstract Task JWTAuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken);

        protected abstract bool ValidateToken(string token, out string username);

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        protected abstract void Challenge(HttpAuthenticationChallengeContext context);
    }
}
