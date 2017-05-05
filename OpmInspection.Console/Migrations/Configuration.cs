namespace OpmInspection.Console.Migrations
{
    using MySql.Data.Entity;
    using OpmInspection.Console.Seeder;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OpmInspection.Shared.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            // ใช้สำหรับ mysql code generator
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());
            CodeGenerator = new MySqlMigrationCodeGenerator();
        }

        protected override void Seed(OpmInspection.Shared.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            RolesSeeder.Seed(context);
            SkinsSeeder.Seed(context);
            AdministratorsSeeder.Seed(context);
        }
    }
}
