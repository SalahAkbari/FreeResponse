using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using FreeResponse.EntityFrameworkCore.EntityFrameworkCore.Seed.SdlcSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace FreeResponse.EntityFrameworkCore.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<FreeResponseDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(FreeResponseDbContext context)
        {
            // SdlcSystem seed
            new InitialSdlcSystemDbBuilder(context).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
