using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OpmInspection.Shared.ViewManager
{
    public class MainMenu
    {
        public static Menu WebApp()
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var menu = new Menu();

            menu.Items.Add(new MenuItem() { Title = "หน้าหลัก", Icon = "fa-home", Url = url.Action("Index", "Home") });

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.IsInRole("Administrator"))
                {
                    var website = new MenuItem() { Title = "เว็บไซต์", Icon = "fa-tasks" };
                    website.Items.Add(new MenuItem() { Title = "สไลด์โชว์", Icon = "fa-image", Url = url.Action("Index", "Carousel") });
                    website.Items.Add(new MenuItem() { Title = "ข่าวประชาสัมพันธ์", Icon = "fa-rss", Url = url.Action("Index", "News") });
                    menu.Items.Add(website);

                    var admin = new MenuItem() { Title = "ผู้ดูแลระบบ", Icon = "fa-gears" };
                    admin.Items.Add(new MenuItem() { Title = "จัดการบัญชีผู้ใช้งานระบบฯ", Icon = "fa-user-circle-o", Url = url.Action("Index", "Account") });
                    admin.Items.Add(new MenuItem() { Title = "สถิติการเข้าใช้งาน", Icon = "fa-bar-chart", Url = url.Action("Index", "Statistic") });
                    menu.Items.Add(admin);
                }
                else if (HttpContext.Current.User.IsInRole("Ministor"))
                {

                }
                else if (HttpContext.Current.User.IsInRole("Region"))
                {

                }
                else
                {

                }
            }

            return menu;
        }

        public static Menu WebSite()
        {
            var menu = new Menu();

            return menu;
        }
    }

    public class Menu
    {
        private readonly List<MenuItem> _items;

        public List<MenuItem> Items { get { return _items; } }

        public Menu()
        {
            this._items = new List<MenuItem>();
        }
    }

    public class MenuItem
    {
        private readonly List<MenuItem> _items;

        public List<MenuItem> Items { get { return _items; } }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public MenuPush Push { get; set; }

        public MenuItem()
        {
            this._items = new List<MenuItem>();
            this.Title = "Item";
            this.Icon = "fa-circle-o";
            this.Url = string.Empty;
            this.Push = null;
        }
    }

    public class MenuPush
    {
        public string Background { get; set; }

        public int Value { get; set; }

        public MenuPush()
        {
            this.Background = "bg-red";
            this.Value = 0;
        }
    }
}
