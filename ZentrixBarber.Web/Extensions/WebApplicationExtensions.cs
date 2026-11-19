using Microsoft.AspNetCore.Identity;
using static ZentrixBarber.Common.ApplicationConstants;

namespace ZentrixBarber.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedRolesAndAdministratorAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { AdministratorRoleName, UserRoleName };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string? email = app.Configuration[$"{AdminSettingsSection}:Email"];
            string? password = app.Configuration[$"{AdminSettingsSection}:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            IdentityUser? admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    string errors = string.Join(" ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create the administrator account: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(admin, AdministratorRoleName))
            {
                await userManager.AddToRoleAsync(admin, AdministratorRoleName);
            }
        }
    }
}