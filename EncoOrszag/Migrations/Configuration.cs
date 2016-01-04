namespace EncoOrszag.Migrations
{
    using EncoOrszag.Models.DataAccess.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EncoOrszag.Models.DataAccess.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EncoOrszag.Models.DataAccess.ApplicationDbContext context)
        {
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

            context.Epuletek.AddOrUpdate(
                  p => p.Name,
                  new Epulet { Name = "Tanya", Ido = 5},
                  new Epulet { Name = "Barakk", Ido = 5}
                );

            context.Egysegek.AddOrUpdate(
                p => p.Name,
                new Egyseg { Name = "Lovas", Tamadas = 6, Vedekezes = 2, Ar = 50, Zsold = 1, Ellatmany = 1 },
                new Egyseg { Name = "Íjász", Tamadas = 2, Vedekezes = 6, Ar = 50, Zsold = 1, Ellatmany = 1 },
                new Egyseg { Name = "Elit", Tamadas = 5, Vedekezes = 5, Ar = 100, Zsold = 3, Ellatmany = 2 }
                );
            context.Fejlesztesek.AddOrUpdate(
                p => p.Name,
                new Fejlesztes { Name = "Traktor", Ido = 15 },
                new Fejlesztes { Name = "Kombájn", Ido = 15 },
                new Fejlesztes { Name = "Városfal", Ido = 15 },
                new Fejlesztes { Name = "Operation Rebirth", Ido = 15 },
                new Fejlesztes { Name = "Taktika", Ido = 15 },
                new Fejlesztes { Name = "Alkímia", Ido = 15 }

                );
        }
    }
}
