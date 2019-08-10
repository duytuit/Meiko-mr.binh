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
    [RoutePrefix("api/CompanyDiagram")]
    public class CompanyDiagramController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        [HttpGet]
        [Route("R1_PhongBanGetByList")]
        public HttpResponseMessage R1_PhongBanGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0004.Select(x => new {
                    x.A0004_ID,
                    x.maPhongBan,
                    x.IDCha,
                    x.tenPhongBan,
                    x.diaChi,
                    x.dienThoai,
                    x.Email,
                    logo = temp + x.logo,
                    x.CongTyID,
                    x.thuTu,
                    count = db.A0002.Count(c => c.A0004_ID == x.A0004_ID),
                    x.tinhTrang
                });                               
                var qrs = tables.ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_PhongBanGetByID/{Id}")]
        public HttpResponseMessage R1_PhongBanGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var PhongBan = db.A0004.Where(x => x.A0004_ID == Id).Select(x => new {
                    x.A0004_ID,
                    x.maPhongBan,
                    x.IDCha,
                    x.tenPhongBan,
                    x.diaChi,
                    x.dienThoai,
                    x.Email,
                    logo = temp + x.logo,
                    x.CongTyID,
                    x.thuTu,
                    x.tinhTrang
                }).OrderBy(x => x.thuTu).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(PhongBan));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddPhongBan")]
        public HttpResponseMessage R2_AddPhongBan()
        {
            var httpRequest = HttpContext.Current.Request;
            var pb = JsonConvert.DeserializeObject<A0004>(httpRequest["pb"]);
            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    HttpPostedFile file = httpRequest.Files[i];
                    string genkey = helper.GenKey();
                    string ext = helper.GetFileExtension(genkey + file.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    pb.logo = "/Portals/images/Users/" + genkey + file.FileName;
                }
            }
            else
            {
                pb.logo = null;
            }

            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0004();
                obj.A0004_ID = helper.GenKey();
                obj.IDCha = (pb.IDCha == "null" || pb.IDCha == null) ? null : pb.IDCha;
                obj.maPhongBan = pb.maPhongBan;
                obj.tenPhongBan = pb.tenPhongBan;
                obj.diaChi = pb.diaChi;
                obj.dienThoai = pb.dienThoai;
                obj.Email = pb.Email;
                obj.logo = pb.logo == null ? "Content/noimages.gif" : pb.logo;
                obj.CongTyID = pb.CongTyID;
                obj.thuTu = pb.thuTu;
                obj.tinhTrang = pb.tinhTrang;
                db.A0004.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdatePhongBan")]
        public HttpResponseMessage R3_UpdatePhongBan()
        {
            var httpRequest = HttpContext.Current.Request;
            var pb = JsonConvert.DeserializeObject<A0004>(httpRequest["pb"]);
            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    HttpPostedFile file = httpRequest.Files[i];
                    string genkey = helper.GenKey();
                    string ext = helper.GetFileExtension(genkey + file.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    pb.logo = "/Portals/images/Users/" + genkey + file.FileName;
                }
            }
            else
            {
                pb.logo = null;
            }
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0004.Find(pb.A0004_ID);
                if (obj != null)
                {
                    obj.IDCha = (pb.IDCha == "null" || pb.IDCha == null) ? null : pb.IDCha;
                    obj.maPhongBan = pb.maPhongBan;
                    obj.tenPhongBan = pb.tenPhongBan;
                    obj.diaChi = pb.diaChi;
                    obj.dienThoai = pb.dienThoai;
                    obj.Email = pb.Email;
                    obj.logo = pb.logo == null ? "Content/noimages.gif" : pb.logo;
                    obj.CongTyID = pb.CongTyID;
                    obj.thuTu = pb.thuTu;
                    obj.tinhTrang = pb.tinhTrang;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeletePhongBan")]
        public HttpResponseMessage R4_DeletePhongBan(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0004.Where(x => Id.Contains(x.A0004_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0004.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
    }
}
