using OpmInspection.Shared.ViewManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpmInspection.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/Index
        public ActionResult Index()
        {
            ViewBag.Title = "หน้าหลัก";
            ViewBag.Header = "หน้าหลัก";
            ViewBag.HeaderDescription = "แสดงสรุปข้อมูลการดำเนินงานโครงการทั้งหมด";
            ViewBag.Breadcrumb = new Breadcrumb();

            return View();
        }
    }
}