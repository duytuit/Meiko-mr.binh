using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/ChiTieuBai")]
    public class ChiTieuBaiController : ApiController
    {
        [HttpPost]
        [Route("R1_ChiTieuBaiGetByList")]
        public HttpResponseMessage R1_ChiTieuBaiGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                int thang = int.Parse(httpRequest["thang"].ToString());
                int nam = int.Parse(httpRequest["nam"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0044.Where(x => x.thang == thang && x.nam == nam).Select(x => new {
                    x.A0044_ID,
                    x.boPhan_ID,
                    x.Parent_ID,
                    x.tenBoPhan,
                    x.thang,
                    x.nam,
                    x.chiTieuBai
                });

                if (string.IsNullOrWhiteSpace(sort))
                {
                    sort = "STT";
                }
                if (string.IsNullOrWhiteSpace(s))
                {
                    s = "";
                }
                if (string.IsNullOrWhiteSpace(ob))
                {
                    ob = "descending";
                }
                if (ob.ToLower() == "asc")
                {
                    ob = " descending";
                }
                else
                {
                    ob = "";
                }

                tables = tables.Where(cd => (s == "" ||
                cd.tenBoPhan.Contains(s)));
                var qrs = tables.OrderBy(x => x.tenBoPhan).Skip(pz * (p - 1)).Take(pz).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_ChiTieuBaiGetBySelect")]
        public HttpResponseMessage R1_ChiTieuBaiGetBySelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0044.Select(x => new {
                    x.A0044_ID,
                    x.boPhan_ID,
                    x.Parent_ID,
                    x.tenBoPhan,
                    x.thang,
                    x.nam,
                    x.chiTieuBai
                });
                var qrs = tables.ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_ChiTieuBaiGetByID/{Id}")]
        public HttpResponseMessage R1_ChiTieuBaiGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0044 = db.A0044.Where(x => x.A0044_ID == Id).Select(x => new {
                    x.A0044_ID,
                    x.boPhan_ID,
                    x.Parent_ID,
                    x.tenBoPhan,
                    x.thang,
                    x.nam,
                    x.chiTieuBai
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0044));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddChiTieuBai")]
        public HttpResponseMessage R2_AddChiTieuBai(A0044 a0044)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0044();
                obj.A0044_ID = helper.GenKey();
                obj.boPhan_ID = a0044.boPhan_ID;
                obj.Parent_ID = a0044.Parent_ID;
                obj.tenBoPhan = a0044.tenBoPhan;
                obj.thang = a0044.thang;
                obj.nam = a0044.nam;
                obj.chiTieuBai = a0044.chiTieuBai;
                db.A0044.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateChiTieuBai")]
        public HttpResponseMessage R3_UpdateChiTieuBai(A0044 a0044)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0044.Find(a0044.A0044_ID);
                if (obj != null)
                {
                    obj.boPhan_ID = a0044.boPhan_ID;
                    obj.Parent_ID = a0044.Parent_ID;
                    obj.tenBoPhan = a0044.tenBoPhan;
                    obj.thang = a0044.thang;
                    obj.nam = a0044.nam;
                    obj.chiTieuBai = a0044.chiTieuBai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteChiTieuBai")]
        public HttpResponseMessage R4_DeleteChiTieuBai(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0044.Where(x => Id.Contains(x.A0044_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0044.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

    }
}
