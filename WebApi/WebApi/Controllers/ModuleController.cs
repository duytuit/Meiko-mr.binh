using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Helper;
using WebApi.Models;
namespace WebApi.Controllers
{
    [RoutePrefix("api/Module")]
    public class ModuleController : ApiController
    {
        [HttpPost]
        [Route("R1_ModuleGetByList")]
        public HttpResponseMessage R1_ModuleGetByList()
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
                var tables = db.A0006.Select(x => new {
                    x.A0006_ID,
                    x.maModule,
                    x.tenModule,
                    x.thuTu,
                    x.tinhTrang
                });

                if (string.IsNullOrWhiteSpace(sort))
                {
                    sort = "thuTu";
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
                if (!string.IsNullOrWhiteSpace(sts))
                {
                    if (sts == "on")
                    {
                        tables = tables.Where(x => x.tinhTrang == true);
                    }
                    else
                    {
                        tables = tables.Where(x => x.tinhTrang == false);
                    }
                }

                tables = tables.Where(cd => (s == "" ||
                cd.maModule.Contains(s) ||
                cd.tenModule.Contains(s)));
                var qrs = tables.OrderBy(x => x.thuTu).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_ModuleGetByID/{Id}")]
        public HttpResponseMessage R1_ModuleGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Module = db.A0006.Where(x => x.A0006_ID == Id).Select(x => new {
                    x.A0006_ID,
                    x.maModule,
                    x.tenModule,
                    x.thuTu,
                    x.tinhTrang
                }).OrderBy(x => x.thuTu).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(Module));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddModule")]
        public HttpResponseMessage R2_AddModule(A0006 mo)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0006();
                obj.A0006_ID = helper.GenKey();
                obj.maModule = mo.maModule;
                obj.tenModule = mo.tenModule;
                obj.thuTu = mo.thuTu;
                obj.tinhTrang = mo.tinhTrang;
                db.A0006.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateModule")]
        public HttpResponseMessage R3_UpdateModule(A0006 mo)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0006.Find(mo.A0006_ID);
                if(obj != null)
                {
                    obj.maModule = mo.maModule;
                    obj.tenModule = mo.tenModule;
                    obj.thuTu = mo.thuTu;
                    obj.tinhTrang = mo.tinhTrang;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteModule")]
        public HttpResponseMessage R4_DeleteModule(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0006.Where(x => Id.Contains(x.A0006_ID)).ToList();
                if(ListCheck.Count > 0)
                {
                    db.A0006.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
    }
}
