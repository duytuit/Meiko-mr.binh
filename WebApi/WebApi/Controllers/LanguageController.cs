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
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));
        [HttpPost]
        [Route("R1_LanguageGetByList")]
        public HttpResponseMessage R1_LanguageGetByList()
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
                var tables = db.A0008.Select(x => new {
                    x.A0008_ID,
                    x.tenNgonNgu,
                    Icon = temp + x.Icon,
                    x.maCode,
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
                cd.tenNgonNgu.Contains(s) ||
                cd.maCode.Contains(s)));
                var qrs = tables.OrderBy(x => x.thuTu).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_LanguageGetByID/{Id}")]
        public HttpResponseMessage R1_LanguageGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Role = db.A0008.Where(x => x.A0008_ID == Id).Select(x => new {
                    x.A0008_ID,
                    x.tenNgonNgu,
                    Icon = temp + x.Icon,
                    x.maCode,
                    x.thuTu,
                    x.tinhTrang
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(Role));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddLanguage")]
        public HttpResponseMessage R2_AddLanguage()
        {
            var httpRequest = HttpContext.Current.Request;
            var lang = JsonConvert.DeserializeObject<A0008>(httpRequest["lang"]);
            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    HttpPostedFile file = httpRequest.Files[i];
                    string genkey = helper.GenKey();
                    string ext = helper.GetFileExtension(genkey + file.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    lang.Icon = "/Portals/images/Users/" + genkey + file.FileName;
                }
            }
            else
            {
                lang.Icon = null;
            }
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0008();
                obj.A0008_ID = helper.GenKey();
                obj.tenNgonNgu = lang.tenNgonNgu;
                obj.Icon = lang.Icon == null ? "Content/noimages.gif" : lang.Icon;
                obj.maCode = lang.maCode;
                obj.thuTu = lang.thuTu;
                obj.tinhTrang = lang.tinhTrang;
                db.A0008.Add(obj);

                var checkLang = db.A0008.FirstOrDefault();
                if(checkLang != null)
                {
                    string NgonNguID = checkLang.A0008_ID;
                    var LabelList = db.A0009.Where(x => x.A0008_ID == NgonNguID).ToList();
                    if(LabelList.Count > 0)
                    {
                        foreach (var item in LabelList)
                        {
                            var objlabel = new A0009();
                            objlabel.A0009_ID = helper.GenKey();
                            objlabel.tenTieuDe = item.tenTieuDe;
                            objlabel.ghiChu = item.ghiChu;
                            objlabel.maCode = item.maCode;
                            objlabel.Module = item.Module;
                            objlabel.A0008_ID = obj.A0008_ID;
                            db.A0009.Add(objlabel);
                        }
                    }
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateLanguage")]
        public HttpResponseMessage R3_UpdateLanguage()
        {
            var httpRequest = HttpContext.Current.Request;
            var lang = JsonConvert.DeserializeObject<A0008>(httpRequest["lang"]);
            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    HttpPostedFile file = httpRequest.Files[i];
                    string genkey = helper.GenKey();
                    string ext = helper.GetFileExtension(genkey + file.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    lang.Icon = "/Portals/images/Users/" + genkey + file.FileName;
                }
            }
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0008.Find(lang.A0008_ID);
                if (lang != null)
                {
                    obj.tenNgonNgu = lang.tenNgonNgu;                    
                    if(obj.Icon != lang.Icon)
                    {
                        lang.Icon = lang.Icon.Replace(temp, "");
                        obj.Icon = lang.Icon == null ? "Content/noimages.gif" : lang.Icon;
                    }
                    obj.maCode = lang.maCode;
                    obj.thuTu = lang.thuTu;
                    obj.tinhTrang = lang.tinhTrang;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteLanguage")]
        public HttpResponseMessage R4_DeleteLanguage(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0008.Where(x => Id.Contains(x.A0008_ID)).ToList();
                if (obj.Count > 0)
                {
                    db.A0008.RemoveRange(obj);
                    db.SaveChanges();
                }
                return response;
            }
        }
    }
}
