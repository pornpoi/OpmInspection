using OpmInspection.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OpmInspection.Shared.Seeder
{
    public class SkinsSeeder
    {
        public static bool Seed(ApplicationDbContext context)
        {
            DbContextTransaction transaction = null;
            bool succeeded = false;

            var skins = new List<Skin>()
            {
                new Skin() { Name = "ฟ้า", ClassName = "skin-blue" },
                new Skin() { Name = "ดำ", ClassName = "skin-black" },
                new Skin() { Name = "เขียว", ClassName = "skin-green" },
                new Skin() { Name = "ม่วง", ClassName = "skin-purple" },
                new Skin() { Name = "แดง", ClassName = "skin-red" },
                new Skin() { Name = "เหลือง", ClassName = "skin-yellow" },
                new Skin() { Name = "ฟ้าขาว", ClassName = "skin-blue-light" },
                new Skin() { Name = "ดำขาว", ClassName = "skin-black-light" },
                new Skin() { Name = "เขียวขาว", ClassName = "skin-green-light" },
                new Skin() { Name = "ม่วงขาว", ClassName = "skin-purple-light" },
                new Skin() { Name = "แดงขาว", ClassName = "skin-red-light" },
                new Skin() { Name = "เหลืองขาว", ClassName = "skin-yellow-light" }
            };

            try
            {
                transaction = context.Database.BeginTransaction();

                foreach (var skin in skins)
                {
                    CreateSkin(context, skin);
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


        private static void CreateSkin(ApplicationDbContext context, Skin skin)
        {
            if (!context.Skins.Any(s => s.Name == skin.Name))
            {
                context.Skins.Add(skin);
                context.SaveChanges();
            }
        }
    }
}
