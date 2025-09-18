using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Web.Extensions
{
    public static class RolesExtensions
    {
        public static async Task<IApplicationBuilder> ConfigRoles(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            return app;
        }
    }
}
