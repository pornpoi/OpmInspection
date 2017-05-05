using System;
using System.Collections.Generic;
using System.Text;

namespace OpmInspection.Shared.ViewManager
{
    public class Breadcrumb
    {
        private readonly List<BreadcrumbItem> _items;

        public List<BreadcrumbItem> Items { get { return _items; } }

        public Breadcrumb()
        {
            this._items = new List<BreadcrumbItem>();
        }
    }

    public class BreadcrumbItem
    {
        public string Text { get; set; }

        public string Link { get; set; }
    }

}
