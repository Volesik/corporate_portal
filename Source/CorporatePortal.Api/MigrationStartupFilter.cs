using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.Api;

public class MigrationStartupFilter<TContext> : IStartupFilter
    where TContext : DbContext
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                foreach (var context in scope.ServiceProvider.GetServices<TContext>())
                {
                    if (context.Database.IsRelational())
                    {
                        context.Database.SetCommandTimeout(180);
                        context.Database.Migrate();
                    }
                }
            }

            next(app);
        };
    }

}