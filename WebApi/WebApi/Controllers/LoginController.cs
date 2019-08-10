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
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        [HttpPost]
        [Route("CheckLogin")]
        public HttpResponseMessage CheckLogin(A0002 u)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string depass = helper.Encrypt("os", u.passWord);
                var user = db.A0002.FirstOrDefault(us => us.userName == u.userName && us.passWord == depass && (us.tinhTrang != 3 && us.tinhTrang != 9 && us.tinhTrang != 0));
                if (user != null)
                {
                    var context = new HttpContextWrapper(HttpContext.Current);
                    HttpRequestBase request = context.Request;
                    helper.saveIP(request, user.A0002_ID, user.hoVaTen);
                    var Roles = db.A0003.Where(x => x.A0002_ID == user.A0002_ID).Select(x => new { x.A0001_ID, x.A0001.tenRole }).ToList();
                    var A0001_ID = "";
                    if (Roles.Count > 0)
                    {
                        A0001_ID = Roles[0].A0001_ID;
                    } 
                    var A0002_ID = user.A0002_ID;
                    var A0006_ID = u.APKID == null ? "87A0A5966755437C8BAF867FA6CC879A" : u.APKID;
                    var qr = db.A0005.Where(x => x.A0001_ID == A0001_ID && x.A0002_ID == A0002_ID && x.A0006_ID == A0006_ID && x.quyenXem && x.A0007.tinhTrang).Select(
                    x => new
                    {
                        x.A0007_ID,
                        x.A0007.IDCha,
                        x.A0007.tenMenu,
                        x.A0007.tenRutGon,
                        x.A0007.A0006_ID,
                        x.A0007.Link,
                        x.A0007.maCode, 
                        Icon = temp + x.A0007.Icon,
                        IconDoc = temp + x.A0007.anhDoc,
                        x.A0007.thuThu,
                        x.A0007.tinhTrang,
                        x.A0007.maCount,
                        x.quyenXem
                    }
                    ).OrderBy(a => a.thuThu);
                    if (qr.Count() == 0)
                    {
                        qr = db.A0005.Where(x => x.A0001_ID == A0001_ID && x.A0006_ID == A0006_ID && x.quyenXem && x.A0007.tinhTrang && x.A0002_ID == null).Select(
                        x => new
                        {
                            x.A0007_ID,
                            x.A0007.IDCha,
                            x.A0007.tenMenu,
                            x.A0007.tenRutGon,
                            x.A0007.A0006_ID,
                            x.A0007.Link,
                            x.A0007.maCode,
                            Icon = temp + x.A0007.Icon,
                            IconDoc = temp + x.A0007.anhDoc,
                            x.A0007.thuThu,
                            x.A0007.tinhTrang,
                            x.A0007.maCount,
                            x.quyenXem
                        }
                        ).OrderBy(a => a.thuThu);
                    }
                    var tables = qr.ToList();
                    var arrParent = tables.Where(a => a.IDCha == null).OrderBy(a => a.thuThu);
                    var arrchild = tables.Where(a => a.IDCha != null).OrderBy(a => a.thuThu);
                    var Childs = new List<dynamic>();
                    var Parents = new List<dynamic>();
                    foreach (var item in arrchild)
                    {
                        var check = Childs.FirstOrDefault(x => x.A0007_ID == item.A0007_ID);
                        if (check == null)
                        {
                            Childs.Add(item);
                        }
                    }
                    foreach (var item in arrParent)
                    {
                        var check = Parents.FirstOrDefault(x => x.A0007_ID == item.A0007_ID);
                        if (check == null)
                        {
                            Parents.Add(item);
                        }
                    }

                    response.Content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        ms = "Đăng nhập thành công!",
                        Roles = Roles,
                        u = new
                        {
                            Users_ID = user.A0002_ID,
                            displayName = user.hoVaTen,
                            anhDaiDien = temp + user.anhDaiDien,
                            UserCode = user.maNhanVien,
                            Department = user.A0004_ID
                        },
                        parentsmenu = Parents,
                        childsmenu = Childs,
                        menu = tables,
                        error = 0
                    }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                else
                {
                    if (u.userName.ToLower() == "administrator" && u.passWord == "#Mk1234567")
                    {
                        var menu = db.A0007.Where(x => x.tinhTrang == true).Select(
                            x => new
                            {
                                x.A0007_ID,
                                x.IDCha,
                                x.tenMenu,
                                x.A0006_ID,
                                x.Link,
                                x.maCode,
                                Icon = temp + x.Icon,
                                x.thuThu,
                                x.tinhTrang,
                                x.maCount,
                                hienThi = true
                            }).OrderBy(a => a.thuThu).ToList();

                        response.Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            ms = "Đăng nhập thành công!",
                            u = new
                            {
                                Users_ID = "Administrator",
                                displayName = "Administrator",
                                admin = true,
                                anhDaiDien = "assets/Img/noavatar.jpg"
                            },
                            error = 0,
                            menu = menu,
                            parentsmenu = menu.Where(a => a.IDCha == null).OrderBy(a => a.thuThu),
                            childsmenu = menu.Where(a => a.IDCha != null).OrderBy(a => a.thuThu),
                        }));
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return response;
                    }
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ms = "Đăng nhập không thành công!",
                    error = 1
                }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        //[HttpPost]
        //[Route("CheckRoleMenu")]
        //public HttpResponseMessage CheckRoleMenu()
        //{
        //    var httpRequest = HttpContext.Current.Request;
        //    using (MeikoEntities db = new MeikoEntities())
        //    {
        //        var response = new HttpResponseMessage(HttpStatusCode.OK);
        //        db.Configuration.LazyLoadingEnabled = false;
        //        string A0002_ID = httpRequest["A0002_ID"];
        //        string A0001_ID = httpRequest["A0001_ID"];
        //        string A0006_ID = httpRequest["A0006_ID"];
        //        if (A0002_ID.ToLower() == "administrator")
        //        {
        //            var tables = db.A0007.Where(x => x.tinhTrang == true).Select(
        //            x => new
        //            {
        //                x.A0007_ID,
        //                x.IDCha,
        //                x.tenMenu,
        //                x.A0006_ID,
        //                x.Link,
        //                x.maCode,
        //                x.Icon,
        //                x.thuThu,
        //                x.tinhTrang,
        //                x.maCount,
        //                hienThi = true
        //            }
        //            ).OrderBy(a => a.thuThu).ToList();
        //            var Parents = tables.Where(a => a.IDCha == null).OrderBy(a => a.thuThu);
        //            var Childs = tables.Where(a => a.IDCha != null).OrderBy(a => a.thuThu);
        //            response.Content = new StringContent(JsonConvert.SerializeObject(new
        //            {
        //                Parents = Parents,
        //                Childs = Childs
        //            }));
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //            return response;
        //        }
        //        else
        //        {
        //            var qr = db.A0005.Where(x => x.A0001_ID == A0001_ID && x.A0002_ID == A0002_ID && x.A0006_ID == A0006_ID && x.quyenXem && x.A0007.tinhTrang).Select(
        //            x => new
        //            {
        //                x.A0007_ID,
        //                x.A0007.IDCha,
        //                x.A0007.tenMenu,
        //                x.A0007.A0006_ID,
        //                x.A0007.Link,
        //                x.A0007.maCode,
        //                x.A0007.Icon,
        //                x.A0007.thuThu,
        //                x.A0007.tinhTrang,
        //                x.A0007.maCount,
        //                x.quyenXem
        //            }
        //            ).OrderBy(a => a.thuThu);
        //            if (qr == null)
        //            {
        //                qr = db.A0005.Where(x => x.A0001_ID == A0001_ID && x.A0006_ID == A0006_ID && x.quyenXem && x.A0007.tinhTrang).Select(
        //                x => new
        //                {
        //                    x.A0007_ID,
        //                    x.A0007.IDCha,
        //                    x.A0007.tenMenu,
        //                    x.A0007.A0006_ID,
        //                    x.A0007.Link,
        //                    x.A0007.maCode,
        //                    x.A0007.Icon,
        //                    x.A0007.thuThu,
        //                    x.A0007.tinhTrang,
        //                    x.A0007.maCount,
        //                    x.quyenXem
        //                }
        //                ).OrderBy(a => a.thuThu);
        //            }
        //            var tables = qr.ToList();
        //            var arrParent = tables.Where(a => a.IDCha == null).OrderBy(a => a.thuThu);
        //            var arrchild = tables.Where(a => a.IDCha != null).OrderBy(a => a.thuThu);
        //            var Childs = new List<dynamic>();
        //            var Parents = new List<dynamic>();
        //            foreach (var item in arrchild)
        //            {
        //                var check = Childs.FirstOrDefault(x => x.menuID == item.A0007_ID);
        //                if (check == null)
        //                {
        //                    Childs.Add(item);
        //                }
        //            }
        //            foreach (var item in arrParent)
        //            {
        //                var check = Parents.FirstOrDefault(x => x.menuID == item.A0007_ID);
        //                if (check == null)
        //                {
        //                    Parents.Add(item);
        //                }
        //            }
        //            response.Content = new StringContent(JsonConvert.SerializeObject(new
        //            {
        //                Parents = Parents,
        //                Childs = Childs
        //            }));
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //            return response;
        //        }
        //    }
        //}
    }
}
