using Microsoft.AspNet.Identity;
using OpmInspection.Shared;
using OpmInspection.Shared.Models;
using OpmInspection.Shared.ViewManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpmInspection.WebApp
{
    public class ApplicationActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// ส่ง viewbag ไปทุกหน้าของ view
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser;
                List<Skin> skins;

                using (var context = new ApplicationDbContext())
                {
                    currentUser = context.Users.Find(HttpContext.Current.User.Identity.GetUserId<int>());
                    skins = context.Skins.ToList();
                }

                filterContext.Controller.ViewBag.CurrentUser = currentUser;
                filterContext.Controller.ViewBag.Skins = skins;
                filterContext.Controller.ViewBag.Menu = MainMenu.WebApp();
            }
        }
    }
}