using Microsoft.AspNet.Identity;
using OpmInspection.Shared;
using OpmInspection.Shared.Libraries;
using OpmInspection.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OpmInspection.WebApp.Controllers
{
    [AllowAnonymous]
    public class UtilController : Controller
    {
        // POST: Util/Background
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Background(string d)
        {
            return Json(Bing.GetPhoto(int.Parse(d)), JsonRequestBehavior.AllowGet);
        }

        // POST: Util/Skin
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Skin(string className)
        {
            using (var context = new ApplicationDbContext())
            {
                var skin = context.Skins.Where(x => x.ClassName == className).FirstOrDefault();

                var currentUser = context.Users.Find(System.Web.HttpContext.Current.User.Identity.GetUserId());
                currentUser.Skin = skin;
                context.SaveChanges();

                return Json(context.Skins.Select(x => new { x.ClassName }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}