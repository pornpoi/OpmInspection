using OpmInspection.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OpmInspection.Shared.Libraries
{
    public class Statistic
    {
        /// <summary>
        /// แสดงข้อมูลเครื่อง client
        /// </summary>
        public ClientInfo Client
        {
            get { return new ClientInfo(); }
        }

        /// <summary>
        /// แสดงสถิตการใช้งานของบุคคลทั่วไปที่เข้าชมเว็บไซต์
        /// </summary>
        public VisitorStatisticInfo Visitor
        {
            get { return new VisitorStatisticInfo(); }
        }

        /// <summary>
        /// แสดงสถิตการใช้งานของเจ้าหน้าที่ที่เข้าใช้งานระบบ
        /// </summary>
        public OfficerStatisticInfo Officer
        {
            get { return new OfficerStatisticInfo(); }
        }

        /// <summary>
        /// แปลงจำนวนผู้เข้าชมเว็บไซต์เป็น html counter code
        /// </summary>
        /// <param name="visitors">จำนวนผู้เข้าชมเว็บไซต์ทั้งหมด</param>
        /// <returns>html code</returns>
        public static string Counter(int visitors)
        {
            var counter = visitors.ToString("D9");
            var result = string.Empty;

            foreach (var chr in counter.ToCharArray())
            {
                result += "<li><img class=\"" + Statistic.ToCss(chr) + "\" src=\"/public/images/blank.png\" /></li>";
            }

            return result;
        }

        /// <summary>
        /// แปลงตัวเลขเป็น css class เพื่อใช้สำหรับ counter
        /// </summary>
        /// <param name="chr">ตัวอักษรของตัวเลข</param>
        /// <returns>css class</returns>
        private static string ToCss(char chr)
        {
            switch (chr)
            {
                default: return "zero";
                case '1': return "one";
                case '2': return "two";
                case '3': return "three";
                case '4': return "four";
                case '5': return "five";
                case '6': return "six";
                case '7': return "seven";
                case '8': return "eight";
                case '9': return "nine";
            }
        }
    }

    public class VisitorStatisticInfo : StatisticInfo
    {
        public VisitorStatisticInfo()
        {
            // เนื่องจาก entity to sql ไม่รู้จัก .adddays() จึงจำเป็นต้องทำให้เสร็จเรียบร้อยก่อนนำไปใช้
            var db = new ApplicationDbContext();
            var yesterday = DateTime.Today.AddDays(-1);
            var startOfWeek = DateTime.Today.StartOfWeek(DayOfWeek.Sunday);
            var startOfLastWeek = DateTime.Today.AddDays(-7).StartOfWeek(DayOfWeek.Sunday);
            var endOfLastWeek = DateTime.Today.AddDays(-7).EndOfWeek(DayOfWeek.Sunday);
            var startOfMonth = DateTime.Today.StartOfMonth();
            var startOfLastMonth = DateTime.Today.AddMonths(-1).StartOfMonth();
            var endOfLastMonth = DateTime.Today.AddMonths(-1).EndOfMonth();

            // เปรียบเทียบ datetime without time
            // เนื่องจาก entity to sql ไม่มี datetime.date จึงต้องใช้ DbFunctions.TruncateTime()
            // สำหรับ mysql ก่อนใช้ DbFunctions.TruncateTime() ให้ไปเพิ่ม function ใน mysql ก่อน (mysql ไม่มี TruncateTime)
            this.Total = db.VisitorStatistics.Count();
            this.Today = db.VisitorStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) == DateTime.Today).Count();
            this.Yesterday = db.VisitorStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) == yesterday).Count();
            this.ThisWeek = db.VisitorStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfWeek && DbFunctions.TruncateTime(x.CreatedAt) <= DateTime.Today).Count();
            this.LastWeek = db.VisitorStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfLastWeek && DbFunctions.TruncateTime(x.CreatedAt) <= endOfLastWeek).Count();
            this.ThisMonth = db.VisitorStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfMonth && DbFunctions.TruncateTime(x.CreatedAt) <= DateTime.Today).Count();
            this.LastMonth = db.VisitorStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfLastMonth && DbFunctions.TruncateTime(x.CreatedAt) <= endOfLastMonth).Count();
            this.StartCountAt = db.VisitorStatistics.Min(x => x.CreatedAt);
        }
    }

    public class OfficerStatisticInfo : StatisticInfo
    {
        public OfficerStatisticInfo()
        {
            // เนื่องจาก entity to sql ไม่รู้จัก .adddays() จึงจำเป็นต้องทำให้เสร็จเรียบร้อยก่อนนำไปใช้
            var db = new ApplicationDbContext();
            var yesterday = DateTime.Today.AddDays(-1);
            var startOfWeek = DateTime.Today.StartOfWeek(DayOfWeek.Sunday);
            var startOfLastWeek = DateTime.Today.AddDays(-7).StartOfWeek(DayOfWeek.Sunday);
            var endOfLastWeek = DateTime.Today.AddDays(-7).EndOfWeek(DayOfWeek.Sunday);
            var startOfMonth = DateTime.Today.StartOfMonth();
            var startOfLastMonth = DateTime.Today.AddMonths(-1).StartOfMonth();
            var endOfLastMonth = DateTime.Today.AddMonths(-1).EndOfMonth();

            // เปรียบเทียบ datetime without time
            // เนื่องจาก entity to sql ไม่มี datetime.date จึงต้องใช้ DbFunctions.TruncateTime()
            // สำหรับ mysql ก่อนใช้ DbFunctions.TruncateTime() ให้ไปเพิ่ม function ใน mysql ก่อน (mysql ไม่มี TruncateTime)
            this.Total = db.OfficerStatistics.Count();
            this.Today = db.OfficerStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) == DateTime.Today).Count();
            this.Yesterday = db.OfficerStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) == yesterday).Count();
            this.ThisWeek = db.OfficerStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfWeek && DbFunctions.TruncateTime(x.CreatedAt) <= DateTime.Today).Count();
            this.LastWeek = db.OfficerStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfLastWeek && DbFunctions.TruncateTime(x.CreatedAt) <= endOfLastWeek).Count();
            this.ThisMonth = db.OfficerStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfMonth && DbFunctions.TruncateTime(x.CreatedAt) <= DateTime.Today).Count();
            this.LastMonth = db.OfficerStatistics.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= startOfLastMonth && DbFunctions.TruncateTime(x.CreatedAt) <= endOfLastMonth).Count();
            this.StartCountAt = db.OfficerStatistics.Min(x => x.CreatedAt);
        }
    }

    public abstract class StatisticInfo
    {
        public DateTime StartCountAt { get; protected set; }

        public int Total { get; protected set; }

        public int Today { get; protected set; }

        public int Yesterday { get; protected set; }

        public int ThisWeek { get; protected set; }

        public int LastWeek { get; protected set; }

        public int ThisMonth { get; protected set; }

        public int LastMonth { get; protected set; }
    }
}