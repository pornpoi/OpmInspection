using System;
using System.Collections.Generic;
using System.Text;

namespace OpmInspection.Shared.Libraries
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// วันที่เริ่มต้นของสัปดาห์
        /// </summary>
        /// <param name="dt">วันที่</param>
        /// <param name="startOfWeek">ชื่อวันที่ใช้เป็นวันเริ่มต้นสัปดาห์</param>
        /// <returns>วันที่</returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// วันที่สิ้นสุดของสัปดาห์
        /// </summary>
        /// <param name="dt">วันที่</param>
        /// <param name="startOfWeek">ชื่อวันที่ใช้เป็นวันเริ่มต้นสัปดาห์</param>
        /// <returns>วันที่</returns>
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(6 - diff).Date;
        }

        /// <summary>
        /// วันที่เริ่มต้นของเดือน
        /// </summary>
        /// <param name="dt">วันที่</param>
        /// <returns>วันที่</returns>
        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// วันที่สิ้นสุดของเดือน
        /// </summary>
        /// <param name="dt">วันที่</param>
        /// <returns>วันที่</returns>
        public static DateTime EndOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }
    }
}
