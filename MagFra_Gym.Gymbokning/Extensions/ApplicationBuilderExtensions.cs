using Microsoft.EntityFrameworkCore;
using MagFra_Gym.Gymbokning.Data;

namespace MagFra_Gym.Gymbokning.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.Migrate();

                try
                {
                    await SeedData.InitAsync(db,serviceProvider);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
