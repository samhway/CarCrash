using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Password123456!";
        private const string userBasic = "yeeter";
        private const string userPassword = "YEETismypassord4!";
        private const string adminNoMfa = "cooleradmin";
        private const string adminNoMfaPass = "123456Admin!";

        public static async void EnsurePopulated (IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppIdentityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<IdentityUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByIdAsync(adminUser); 
            IdentityUser user2 = await userManager.FindByIdAsync(userBasic);
            IdentityUser user3 = await userManager.FindByIdAsync(adminNoMfa);

            if (user == null && user2 == null)
            {
                user = new IdentityUser(adminUser);
                user2 = new IdentityUser(userBasic);
                user3 = new IdentityUser(adminNoMfa);

                user.Email = "goten3917@gmail.com";
                user.PhoneNumber = "555-1234";

                user2.Email = "yeeter@yeeter.com";
                user2.PhoneNumber = "420-1234";

                user3.Email = "yeet@yeet.monster";
                user3.PhoneNumber = "234-1234";

                await userManager.CreateAsync(user2, userPassword);
                await userManager.CreateAsync(user, adminPassword);
                await userManager.CreateAsync(user3, adminNoMfaPass);
            }
        }
    }
}
