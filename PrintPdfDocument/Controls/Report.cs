using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PrintPdfDocument.Controls
{
    class Report
    {
        public int id { get; set; }
        public string name { get; set; }
        public string fileUrl { get; set; }
    }

    class ReportHttp
    {
        static HttpClient client = client = new HttpClient();
        public ReportHttp()
        {
            InitHttp();
        }

        public async Task<List<Report>> GetReportAsync()
        {
            List<Report> report = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/getProducts");
                response.EnsureSuccessStatusCode();
                report = await response.Content.ReadAsAsync<List<Report>>();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught! Message :{0} ", e.Message);
            }
            
            return report;
        }

        /// <summary>
        /// 初始化请求
        /// </summary>
        protected void InitHttp()
        {
            try
            {
                client.BaseAddress = new Uri("https://nei.netease.com/api/apimock/1d150f1ef158dbd6bb3b95fe12d5400a/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught! Message :{0} ", e.Message);
            }
        }
    }
}
