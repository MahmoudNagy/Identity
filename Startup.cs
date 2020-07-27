using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
            CreateRoles(db);
            CreateUsers(db);
        }

        public async void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db), null, null, null, null);
            IdentityRole role = new IdentityRole();
            bool roleExist = roleManager.RoleExistsAsync("Admin").Result;
            if (!roleExist)
            {
                role.Name = "Admin";
                await roleManager.CreateAsync(role);
            }
            roleExist = roleManager.RoleExistsAsync("Author").Result;
            if (!roleExist)
            {
                role.Name = "Author";
                await roleManager.CreateAsync(role);
            }
            roleExist = roleManager.RoleExistsAsync("Customer").Result;
            if (!roleExist)
            {
                role.Name = "Customer";
                await roleManager.CreateAsync(role);
            }

        }

        public void CreateUsers(ApplicationDbContext db)
        {
            var user = new ApplicationUser();
            user.Email = "ss5@gmail.com";
            user.UserName = "ss5";
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db), null, new PasswordHasher<ApplicationUser>(), null, null, null, null, null, null);

            var exist = userManager.FindByEmailAsync("ss5@gmail.com");
            Task.WaitAll(exist);
            if (exist.Result == null)
            {
                var check = userManager.CreateAsync(user, "P@ssw0rd");
                Task.WaitAll(check);
                if (check.IsCompletedSuccessfully)
                {
                    var c = userManager.AddToRoleAsync(user, "Admin");
                    Task.WaitAll(c);
                }
            }

        }
    }
}
