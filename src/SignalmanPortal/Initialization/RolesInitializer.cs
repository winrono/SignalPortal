using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Initialization
{
    public static class RolesInitializer
    {
        private static IEnumerable<string> roles = new List<string>() {
            "Administrator",
            "User"
        };
        public static async void Init(IApplicationBuilder appBuilder)
        {
            var roleManager = appBuilder.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
            foreach(var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
