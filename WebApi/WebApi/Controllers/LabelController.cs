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
    [RoutePrefix("api/Label")]
    public class LabelController : ApiController
    {
        [HttpPost]
        [Route("R1_LabelGetByList")]
        public HttpResponseMessage R1_LabelGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string s = httpRequest["s"];
                if(s == null)
                {
                    s = "";
                }else
                {
                    s = s.ToLower();
                }
                var Label = db.List_ThamSoDungChung(s).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = Label }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_LabelGetByID")]
        public HttpResponseMessage R1_LabelGetByID(A0009 obj)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListLabel = db.A0009.Where(x => x.maCode == obj.maCode).Select(x => new {
                    x.A0009_ID,
                    x.tenTieuDe,
                    x.ghiChu,
                    x.maCode,
                    x.Module,
                    x.A0008_ID,
                    x.A0008.tenNgonNgu
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(ListLabel));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddLabel")]
        public HttpResponseMessage R2_AddLabel(A0009 label)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Language = db.A0008.Where(x => x.tinhTrang == true).ToList();
                for (int i = 0; i < Language.Count; i++)
                {
                    A0009 obj = new A0009();
                    obj.A0009_ID = helper.GenKey();
                    obj.tenTieuDe = label.tenTieuDe;
                    obj.ghiChu = label.ghiChu;
                    obj.maCode = label.maCode;
                    obj.Module = label.Module;
                    obj.A0008_ID = Language[i].A0008_ID;
                    db.A0009.Add(obj);
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateLabel")]
        public HttpResponseMessage R3_UpdateLabel(List<A0009> label)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                for (int i = 0; i < label.Count; i++)
                {
                    string labelID = label[i].A0009_ID.ToString();
                    var obj = db.A0009.FirstOrDefault(x => x.A0009_ID == labelID);
                    if(obj != null)
                    {
                        obj.tenTieuDe = label[i].tenTieuDe;
                    }
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteLabel")]
        public HttpResponseMessage R4_DeleteLabel(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                for (int i = 0; i < Id.Count; i++)
                {
                    string maCode = Id[i].ToString();
                    var arrdetete = db.A0009.Where(m => m.maCode == maCode).ToList();
                    db.A0009.RemoveRange(arrdetete);                   
                }
                db.SaveChanges();
                return response;
            }
        }
      
    }
}
