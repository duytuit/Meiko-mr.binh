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
    [RoutePrefix("api/KieuBaiViet")]
    public class KieuBaiVietController : ApiController
    {        
        [HttpPost]
        [Route("R1_KieuBaiVietGetByList")]
        public HttpResponseMessage R1_KieuBaiVietGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0043.Select(x => new {
                    x.A0043_ID,
                    x.tenKieuBaiViet,
                    x.kieuBaiViet,
                    x.giaTri,
                    x.STT
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
                cd.tenKieuBaiViet.Contains(s)));
                var qrs = tables.OrderBy(x => x.STT).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_KieuBaiVietGetBySelect")]
        public HttpResponseMessage R1_KieuBaiVietGetBySelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0043.Select(x => new {
                    x.A0043_ID,
                    x.tenKieuBaiViet,
                    x.kieuBaiViet,
                    x.giaTri,
                    x.STT
                });
                var qrs = tables.OrderBy(x => x.STT).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_KieuBaiVietGetByID/{Id}")]
        public HttpResponseMessage R1_KieuBaiVietGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0043 = db.A0043.Where(x => x.A0043_ID == Id).Select(x => new {
                    x.A0043_ID,
                    x.tenKieuBaiViet,
                    x.kieuBaiViet,
                    x.giaTri,
                    x.STT
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0043));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddKieuBaiViet")]
        public HttpResponseMessage R2_AddKieuBaiViet(A0043 a0043)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0043();
                obj.A0043_ID = helper.GenKey();
                obj.tenKieuBaiViet = a0043.tenKieuBaiViet;
                obj.kieuBaiViet = a0043.kieuBaiViet;
                obj.giaTri = a0043.giaTri;
                obj.STT = a0043.STT;
                db.A0043.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateKieuBaiViet")]
        public HttpResponseMessage R3_UpdateKieuBaiViet(A0043 a0043)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0043.Find(a0043.A0043_ID);
                if (obj != null)
                {
                    obj.tenKieuBaiViet = a0043.tenKieuBaiViet;
                    obj.kieuBaiViet = a0043.kieuBaiViet;
                    obj.giaTri = a0043.giaTri;
                    obj.STT = a0043.STT;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteKieuBaiViet")]
        public HttpResponseMessage R4_DeleteKieuBaiViet(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0043.Where(x => Id.Contains(x.A0043_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0043.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
         
    }
}
