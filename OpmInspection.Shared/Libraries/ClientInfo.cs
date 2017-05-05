using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpmInspection.Shared.Libraries
{
    /// <summary>
    /// แสดงข้อมูลเครื่อง client เพื่อเก็บสถิติ
    /// </summary>
    public class ClientInfo
    {
        private string _browser;
        private string _platform;
        private string _ipAddress;
        private string _sessionID;

        public ClientInfo()
        {
            string url = string.Format("http://www.useragentstring.com/?uas={0}&getJSON=all", HttpUtility.UrlEncode(HttpContext.Current.Request.UserAgent));
            var api = ApiService.Json<UserAgent>(url).Result;

            if (api.Status)
            {
                // กรณ๊ที่สามารถเรียกใช้ api ได้
                var bc = api.DataObject;
                this._browser = bc.BrowserName;
                this._platform = bc.OsName;
            }
            else
            {
                // กรณ๊ที่ไม่สามารถเรียกใช้ api ได้
                var bc = HttpContext.Current.Request.Browser;
                this._browser = bc.Browser;
                this._platform = bc.Platform;
            }

            this._ipAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            this._sessionID = HttpContext.Current.Session.SessionID;
        }

        /// <summary>
        /// ชื่อ browser ที่ client ใช้
        /// </summary>
        public string Browser
        {
            get { return this._browser; }
        }

        /// <summary>
        /// ชื่อ platform ที่ client ใช้
        /// </summary>
        public string Platform
        {
            get { return this._platform; }
        }

        /// <summary>
        /// ip address ของเครื่อง client
        /// </summary>
        public string IpAddress
        {
            get { return this._ipAddress; }
        }

        /// <summary>
        /// session id ของ connection นี้
        /// </summary>
        public string SessionID
        {
            get { return this._sessionID; }
        }
    }

    internal class UserAgent
    {
        [JsonProperty(PropertyName = "agent_type")]
        public string BrowserType { get; set; }

        [JsonProperty(PropertyName = "agent_name")]
        public string BrowserName { get; set; }

        [JsonProperty(PropertyName = "os_type")]
        public string OsType { get; set; }

        [JsonProperty(PropertyName = "os_name")]
        public string OsName { get; set; }
    }
}