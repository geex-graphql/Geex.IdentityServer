using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Geex.IdentityServer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Geex.IdentityServer
{
    public class GeexUserManager : UserManager<User>
    {
        private IServiceProvider _services;

        public GeexUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<GeexUserManager> logger) : base(store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
        {
            this._services = services;
        }
        public async Task<User> FindByPhoneNumberAsync(string phone)
        {
            this.ThrowIfDisposed();
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));
            User byEmailAsync = await (this.Users).FirstOrDefaultAsync((Expression<Func<User, bool>>)(u => u.PhoneNumber == phone), CancellationToken.None);
            if ((object)byEmailAsync == null && this.Options.Stores.ProtectPersonalData)
            {
                ILookupProtectorKeyRing service = this._services.GetService<ILookupProtectorKeyRing>();
                ILookupProtector protector = this._services.GetService<ILookupProtector>();
                if (service != null && protector != null)
                {
                    foreach (string allKeyId in service.GetAllKeyIds())
                    {
                        
                        byEmailAsync = await (this.Users).FirstOrDefaultAsync((Expression<Func<User, bool>>)(u => u.PhoneNumber == protector.Protect(allKeyId, phone)), CancellationToken.None);
                        if ((object)byEmailAsync != null)
                            return byEmailAsync;
                    }
                }
                protector = (ILookupProtector)null;
            }
            return byEmailAsync;
        }

        private IUserPhoneNumberStore<User> GetPhoneNumberStore()
        {
            IUserPhoneNumberStore<User> store = this.Store as IUserPhoneNumberStore<User>;
            if (store != null)
                return store;
            throw new NotSupportedException("StoreNotIUserPhoneNumberStore");
        }
    }
}
