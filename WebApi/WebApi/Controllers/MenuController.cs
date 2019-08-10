using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Helper;
using WebApi.Models;
namespace WebApi.Controllers
{
    [RoutePrefix("api/Menu")]
    public class MenuController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        [HttpGet]
        [Route("R1_MenuGetByList")]
        public HttpResponseMessage R1_MenuGetByList()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Menu = db.A0007.Select(x => new {
                  x.A0007_ID,
                  x.IDCha,
                  x.tenMenu, 
                  x.tenRutGon,
                  x.A0006_ID,
                  x.Link,
                  x.maCode,
                  Icon = temp + x.Icon,
                  anhDoc = temp + x.anhDoc,
                  x.thuThu,
                  x.tinhTrang,
                  x.maCount
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = Menu }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_MenuGetByID")]
        public HttpResponseMessage R1_MenuGetByID(A0007 obj)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Menu = db.A0007.Where(x => x.A0007_ID == obj.A0007_ID).Select(x => new {
                    x.A0007_ID,
                    x.IDCha,
                    x.tenMenu,
                    x.tenRutGon,
                    x.A0006_ID,
                    x.Link,
                    x.maCode,
                    Icon = temp + x.Icon,
                    anhDoc = temp + x.anhDoc,
                    x.thuThu,
                    x.tinhTrang,
                    x.maCount
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(Menu));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddMenu")]
        public HttpResponseMessage R2_AddMenu()
        {
            var httpRequest = HttpContext.Current.Request;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var menu = JsonConvert.DeserializeObject<A0007>(httpRequest["menu"]);
                if (httpRequest.Files.Count > 0)
                {
                    var IconMenu = HttpContext.Current.Request.Files["FileIcon"];
                    if(IconMenu != null)
                    {
                        HttpPostedFile file = IconMenu;
                        string genkey = helper.GenKey();
                        string ext = helper.GetFileExtension(genkey + file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                        menu.Icon = "/Portals/images/Users/" + genkey + file.FileName;
                    }
                    else
                    {
                        menu.Icon = null;
                    }

                    var IconMenuDoc = HttpContext.Current.Request.Files["FileIconDoc"];
                    if (IconMenuDoc != null)
                    {
                        HttpPostedFile file = IconMenuDoc;
                        string genkey = helper.GenKey();
                        string ext = helper.GetFileExtension(genkey + file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                        menu.anhDoc = "/Portals/images/Users/" + genkey + file.FileName;
                    }
                    else
                    {
                        menu.anhDoc = null;
                    }
                }
                 
                using (MeikoEntities db = new MeikoEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;                    
                    var obj = new A0007();
                    obj.A0007_ID = menu.A0007_ID;
                    obj.IDCha = menu.IDCha == "null" ? null : menu.IDCha;
                    obj.tenMenu = menu.tenMenu;
                    obj.tenRutGon = menu.tenRutGon;
                    obj.A0006_ID = menu.A0006_ID;
                    obj.Link = menu.Link != null ? menu.Link : "";
                    obj.maCode = menu.maCode != null ? menu.maCode : "";
                    obj.Icon = menu.Icon != null ? menu.Icon : "Content/noimages.gif";
                    obj.anhDoc = menu.anhDoc != null ? menu.anhDoc : "Content/noimages.gif";
                    obj.thuThu = menu.thuThu;
                    obj.tinhTrang = menu.tinhTrang;
                    obj.maCount = menu.maCount != null ? menu.maCount : null;
                    db.A0007.Add(obj);
                    db.SaveChanges();                   
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
            } 
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        [HttpPost]
        [Route("R3_UpdateMenu")]
        public HttpResponseMessage R3_UpdateMenu()
        {
            var httpRequest = HttpContext.Current.Request;
            var menu = JsonConvert.DeserializeObject<A0007>(httpRequest["menu"]);
            if (httpRequest.Files.Count > 0)
            {
                var IconMenu = HttpContext.Current.Request.Files["FileIcon"];
                if (IconMenu != null)
                {
                    HttpPostedFile file = IconMenu;
                    string genkey = helper.GenKey();
                    string ext = helper.GetFileExtension(genkey + file.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    menu.Icon = "/Portals/images/Users/" + genkey + file.FileName;
                }
                else
                {
                    menu.Icon = null;
                }

                var IconMenuDoc = HttpContext.Current.Request.Files["FileIconDoc"];
                if (IconMenuDoc != null)
                {
                    HttpPostedFile file = IconMenuDoc;
                    string genkey = helper.GenKey();
                    string ext = helper.GetFileExtension(genkey + file.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    menu.anhDoc = "/Portals/images/Users/" + genkey + file.FileName;
                }
                else
                {
                    menu.anhDoc = null;
                }
            }

            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0007.FirstOrDefault(x => x.A0007_ID == menu.A0007_ID);
                if(obj != null)
                {
                    obj.IDCha = menu.IDCha == "null" ? null : menu.IDCha;
                    obj.tenMenu = menu.tenMenu;
                    obj.A0006_ID = menu.A0006_ID;
                    if(obj.Icon != menu.Icon && menu.Icon != null)
                    {
                        menu.Icon = menu.Icon.Replace(temp, ""); 
                        obj.Icon = menu.Icon != null ? menu.Icon : "Content/noimages.gif";
                    }
                    if (obj.anhDoc != menu.anhDoc && menu.anhDoc != null)
                    {
                        menu.anhDoc = menu.anhDoc.Replace(temp, "");
                        obj.anhDoc = menu.anhDoc != null ? menu.anhDoc : "Content/noimages.gif";
                    }
                    obj.tenRutGon = menu.tenRutGon;
                    obj.Link = menu.Link != null ? menu.Link : "";
                    obj.maCode = menu.maCode != null ? menu.maCode : "";                    
                    obj.thuThu = menu.thuThu;
                    obj.tinhTrang = menu.tinhTrang;
                    obj.maCount = menu.maCount;
                    db.SaveChanges();
                } 
                return response;
            }
        }

        [Route("R4_DeleteMenu")]
        [HttpPost]
        public async Task<HttpResponseMessage> R4_DeleteMenu(A0007 obj)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var Menu = db.A0007.FirstOrDefault(x => x.A0007_ID == obj.A0007_ID);
                    if (Menu != null)
                    {
                        var menuC1 = db.A0007.Where(x => x.IDCha == Menu.A0007_ID).ToList();
                        if (menuC1.Count > 0)
                        {
                            foreach (var item in menuC1)
                            {
                                var A0007_IDC1 = item.A0007_ID;
                                var menuC2 = db.A0007.Where(a => a.IDCha == A0007_IDC1).ToList();
                                if (menuC2.Count > 0)
                                {
                                    db.A0007.RemoveRange(menuC2);
                                }
                            }
                            db.A0007.RemoveRange(menuC1);
                        }
                        db.A0007.Remove(Menu);
                    }
                    await db.SaveChangesAsync();
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }
    }
}
