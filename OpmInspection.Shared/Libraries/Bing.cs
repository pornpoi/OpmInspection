using Newtonsoft.Json;
using OpmInspection.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace OpmInspection.Shared.Libraries
{
    public class Bing
    {
        // Constants
        private const string FORMAT = "js";
        private const int LIMIT_N = 1; // Bing"s API returns at most 8 images
        private const string LOCALE = "en-US";
        private const string RESOLUTION_LOW = "1366x768";
        private const string RESOLUTION_HIGH = "1920x1080";

        // API
        private const string BASE_URL = "http://www.bing.com";
        private const string JSON_URL = "HPImageArchive.aspx";

        private string _resolution;
        private string _path;

        public Bing()
        {
            this._resolution = Bing.RESOLUTION_HIGH;
        }

        /// <summary>
        /// เรียกรูปภาพของ bing
        /// </summary>
        /// <param name="d">ลำดับวันที่ที่ต้องการเลือก 0-4</param>
        /// <returns>object Background</returns>
        public static Background GetPhoto(int d = 0)
        {
            int date = (d < 4) ? d : 4;

            using (var context = new ApplicationDbContext())
            {
                DateTime selected = context.Backgrounds.Max(x => x.StartedAt).AddDays(-1 * date).Date;

                return context.Backgrounds.Where(x => DbFunctions.TruncateTime(x.StartedAt) == selected).SingleOrDefault();
            }
        }

        /// <summary>
        /// ดาวน์โหลดรูปจาก bing api
        /// </summary>
        /// <param name="path">path ที่ต้องการนำรูปที่วาง</param>
        /// <returns>จำนวนรูปที่ดาวน์โหลดมา</returns>
        public static int Download(string path)
        {
            var bing = new Bing();

            return bing.Prepare(path);
        }

        /// <summary>
        /// ดาวน์โหลดรูปจาก bing api
        /// </summary>
        /// <param name="path">path ที่ต้องการนำรูปที่วาง</param>
        /// <returns>จำนวนรูปที่ดาวน์โหลดมา</returns>
        public int Prepare(string path)
        {
            this._path = path;
            this.Restore();

            var photo = this.FetchPhotos(0);
            var result = 0;

            if (photo != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var date = (context.Backgrounds.Count() > 0) ? context.Backgrounds.Max(x => x.CreatedAt).Date : DateTime.MinValue.Date;
                    var newest = DateTime.ParseExact(photo.StartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    var expired = newest.AddDays(-5);

                    if (newest.Subtract(date).Days > 0)
                    {
                        this.Delete(expired);
                        result = this.Save(newest.Subtract(date).Days);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// ในกรณีที่มีข้อมูลแต่ไม่มีรูปภาพให้ทำการโหลดรูปมาใหม่
        /// </summary>
        protected void Restore()
        {
            if (!Directory.Exists(this._path.TrimEnd(System.IO.Path.DirectorySeparatorChar)))
            {
                Directory.CreateDirectory(this._path.TrimEnd(System.IO.Path.DirectorySeparatorChar));
            }

            using (var context = new ApplicationDbContext())
            {
                if (context.Backgrounds.Count() > 0)
                {
                    context.Backgrounds.ForEachAsync(photo =>
                    {
                        var filename = string.Format("{0}.jpg", photo.StartedAt.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));

                        if (!File.Exists(string.Format("{0}/{1}", this._path.TrimEnd(System.IO.Path.DirectorySeparatorChar), filename)))
                        {
                            using (var client = new WebClient())
                            {
                                if (ApiService.UrlStatus(photo.Url) == HttpStatusCode.OK)
                                {
                                    client.DownloadFile(photo.Url, string.Format(@"{0}\{1}", this._path.TrimEnd(System.IO.Path.DirectorySeparatorChar), filename));
                                }
                            }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// ลบรูปที่หมดอายุ
        /// </summary>
        /// <param name="expired">วันที่หมดอายุ</param>
        protected void Delete(DateTime expired)
        {
            using (var context = new ApplicationDbContext())
            {
                var photos = context.Backgrounds.Where(x => DbFunctions.TruncateTime(x.StartedAt) <= expired);

                foreach (var p in photos)
                {
                    var filename = string.Format("{0}.jpg", p.StartedAt.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                    File.Delete(string.Format(@"{0}\{1}", this._path.TrimEnd(System.IO.Path.DirectorySeparatorChar), filename));
                }

                context.Backgrounds.RemoveRange(photos);
            }
        }

        /// <summary>
        /// ดาวน์โหลดและจับเก็บรูปลงฐานข้อมูล
        /// </summary>
        /// <param name="n">จำนวนรูป</param>
        /// <returns>จำนวนรูปที่ดาวน์โหลด</returns>
        protected int Save(int n)
        {
            int loop = (n < 4) ? n : 5;
            int count = 0;

            for (int i = 0; i < loop; i++)
            {
                var photo = this.FetchPhotos(i);

                if (photo != null)
                {
                    using (var context = new ApplicationDbContext())
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var url = this.SetQuality(photo.Url);
                            var filename = string.Format("{0}.jpg", photo.StartDate);

                            using (var client = new WebClient())
                            {
                                if (ApiService.UrlStatus(url) == HttpStatusCode.OK)
                                {
                                    client.DownloadFile(url, string.Format(@"{0}\{1}", this._path.TrimEnd(System.IO.Path.DirectorySeparatorChar), filename));
                                }
                            }

                            var background = new Background()
                            {
                                Url = url,
                                Copyright = photo.Copyright,
                                CopyrightLink = photo.CopyrightLink,
                                StartedAt = DateTime.ParseExact(photo.StartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
                            };

                            context.Backgrounds.Add(background);
                            context.SaveChanges();
                            transaction.Commit();

                            count++;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// เรียกข้อมูลจาก bing api
        /// </summary>
        /// <param name="d">วันที่ของรูป 0-4</param>
        /// <returns>รูปภาพ</returns>
        private Photo FetchPhotos(int d)
        {
            string url = string.Format("{0}/{1}?format={2}&idx={3}&n={4}&mkt={5}",
                Bing.BASE_URL, Bing.JSON_URL, Bing.FORMAT, d, Bing.LIMIT_N, Bing.LOCALE);

            var photos = this.FetchJSON(url);

            return (photos != null) ? photos.Photos[0] : null;
        }

        /// <summary>
        /// เรียกข้อมูล json จาก bing api ด้วย recusive function
        /// </summary>
        /// <param name="url">bing api url</param>
        /// <param name="count">จำนวนรอบที่ทำการติดต่อ สูงสุดได้ 3 รอบ เกินจากนี้ยกเลิกการดาวน์โหลด</param>
        /// <returns>รูปภาพ</returns>
        private BingPhoto FetchJSON(string url, int count = 0)
        {
            if (count < 3)
            {
                var result = ApiService.Json<BingPhoto>(url).Result;

                if (result.Status)
                {
                    return result.DataObject;
                }
                else
                {
                    // หยุดรอ 3 วินาทีแล้วทำการโหลดใหม่
                    System.Threading.Thread.Sleep(3000);

                    return this.FetchJSON(url, count + 1);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// แต่ค่าคุณภาพรูปที่จะดาวน์โหลด
        /// </summary>
        /// <param name="url">bing api url</param>
        /// <returns>utl ที่ตั้งค่าแล้ว</returns>
        private string SetQuality(string url)
        {
            return string.Format("{0}{1}", Bing.BASE_URL, url.Replace(Bing.RESOLUTION_HIGH, this._resolution));
        }
    }

    internal class BingPhoto
    {
        [JsonProperty(PropertyName = "images")]
        public List<Photo> Photos { get; set; }
    }

    internal class Photo
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }

        [JsonProperty(PropertyName = "copyrightlink")]
        public string CopyrightLink { get; set; }

        [JsonProperty(PropertyName = "startdate")]
        public string StartDate { get; set; }
    }
}
