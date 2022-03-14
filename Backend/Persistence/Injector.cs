using Backend.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Management.Persistence
{
    public static class Injector
    {
        public static void UseAutoMigration(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var ctx = scope.ServiceProvider.GetRequiredService<DaoContext>();

            ctx.Database.Migrate();

        }
    }
}
