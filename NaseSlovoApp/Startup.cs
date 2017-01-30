using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using NaseSlovoApp.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(NaseSlovoApp.Startup))]
namespace NaseSlovoApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Simpatizer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Simpatizer";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Clan"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Clan";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Autor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Autor";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Urednik"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Urednik";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Lektor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Lektor";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("GrafickiUrednik"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "GrafickiUrednik";
                roleManager.Create(role);
            }
        }
    }
}
