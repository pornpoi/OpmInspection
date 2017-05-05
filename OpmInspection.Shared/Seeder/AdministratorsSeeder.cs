using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OpmInspection.Shared.Models;

namespace OpmInspection.Shared.Seeder
{
    public class AdministratorsSeeder
    {
        public static bool Seed(ApplicationDbContext context)
        {
            DbContextTransaction transaction = null;
            bool succeeded = false;

            try
            {
                transaction = context.Database.BeginTransaction();

                CreateAdmin(context, new ApplicationUser()
                {
                    Email = "admin@opminspection.go.th",
                    UserName = "Administrator",
                    EmailConfirmed = true,
                    PasswordChanged = true,
                    LockoutEnabled = false,
                    TwoFactorEnabled = false
                });

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

        private static void CreateAdmin(ApplicationDbContext context, ApplicationUser user)
        {
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                // Another approach
                var manager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, ApplicationRole, int, UserLogin, UserRole, UserClaim>(context));
                var password = "oPm1n$pecti0N";
                var admin = manager.Create(user, password);

                //Add User Admin to Role Administrator
                if (admin.Succeeded)
                {
                    var adminResult = manager.AddToRole(user.Id, "Administrator");
                }
            }
        }
    }
}