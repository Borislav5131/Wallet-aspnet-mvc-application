using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Data.Models;

using static Wallet.Core.Constants.UserConstants.Roles;

namespace Wallet.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            SeedAdministrator(services);

            return app;
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(Administrator))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = Administrator };

                    await roleManager.CreateAsync(role);

                    const string adminUserName = "admin";
                    const string adminEmail = "admin@gmail.com";
                    const string adminPassword = "123456";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminUserName,
                        Image = new byte[]{}
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
