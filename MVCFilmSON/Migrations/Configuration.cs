namespace MVCFilmSON.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MVCFilmSON.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using MVCFilmSON;
    internal sealed class Configuration : DbMigrationsConfiguration<MVCFilmSON.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MVCFilmSON.Models.ApplicationDbContext context)
        {
            context.Kategoriler.AddOrUpdate(x => x.KategoriID,
                new Kategori() { KategoriAdi = "Korku" },
                new Kategori() { KategoriAdi = "Gerilim" },
                new Kategori() { KategoriAdi = "Aksiyon" });

            //Admin rolü
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var rstore = new RoleStore<IdentityRole>(context);
                var rmanager = new RoleManager<IdentityRole>(rstore);
                var role = new IdentityRole { Name = "Admin" };
                rmanager.Create(role);
            }
            if (!context.Users.Any(x => x.Email == "d@d.com"))
            {
                //Kullanýcý
                var kstore = new UserStore<ApplicationUser>(context);
                var kmanager = new UserManager<ApplicationUser>(kstore);
                var user = new ApplicationUser { UserName = "d@d.com", Email="d@d.com" };
                kmanager.Create(user, "123456");
                kmanager.AddToRole(user.Id, "Admin");
            }


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
