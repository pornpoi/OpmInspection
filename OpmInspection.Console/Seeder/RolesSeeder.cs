using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpmInspection.Shared;
using OpmInspection.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OpmInspection.Console.Seeder
{
    public class RolesSeeder
    {
        public static bool Seed(ApplicationDbContext context)
        {
            DbContextTransaction transaction = null;
            bool succeeded = false;

            var roles = new List<ApplicationRole>()
            {
                new ApplicationRole() { Name = "Administrator" },
                new ApplicationRole() { Name = "Ministor" },
                new ApplicationRole() { Name = "Region" },
                new ApplicationRole() { Name = "Province" },
            };

            try
            {
                transaction = context.Database.BeginTransaction();

                foreach (var role in roles)
                {
                    CreateRole(context, role);
                }

                context.SaveChanges();
                transaction.Commit();
                succeeded = true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }

                System.Diagnostics.Debug.WriteLine(ex.Message);
                succeeded = false;
            }

            return succeeded;
        }

        private static void CreateRole(ApplicationDbContext context, ApplicationRole role)
        {
            if (!context.Roles.Any(r => r.Name == role.Name))
            {
                // Another approach
                var manager = new RoleManager<ApplicationRole, int>(new RoleStore<ApplicationRole, int, UserRole>(context));

                //Create role if it does not exist
                if (!manager.RoleExists(role.Name))
                {
                    var roleResult = manager.Create(role);
                }
            }
        }
    }
}