using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpmInspection.Shared.Libraries
{
    public class ApiService
    {
        /// <summary>
        /// เรียกใช้งาน api จากที่อื่น
        /// </summary>
        /// <typeparam name="T">type ของ object ที่ต้องการแปลงจาก json output</typeparam>
        /// <param name="url">url string</param>
        /// <returns>object ที่แปลงจาก json output</returns>
        public static async Task<ApiResult<T>> Json<T>(string url)
        {
            var result = new ApiResult<T>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    result.Status = true;
                    result.RawJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    result.DataObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.RawJson);
                }
                else
                {
                    result.Status = false;
                    result.RawJson = string.Empty;
                    result.DataObject = default(T);
                }
            }

            return result;
        }

        /// <summary>
        /// ตรวจสอบสถานะ url
        /// </summary>
        /// <param name="url">url string</param>
        /// <returns>http status code</returns>
        public static HttpStatusCode UrlStatus(string url)
        {
            var uriBuilder = new UriBuilder(url);
            var request = HttpWebRequest.Create(uriBuilder.Uri) as HttpWebRequest;
            var response = request.GetResponse() as HttpWebResponse;
            response.Close();

            return response.StatusCode;
        }
    }

    public class ApiResult<T>
    {
        public bool Status { get; set; }
        public string RawJson { get; set; }
        public T DataObject { get; set; }
    }
}
