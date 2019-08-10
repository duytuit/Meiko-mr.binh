using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebApi.Models;
using WebApi.Helper;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        [HttpPost]
        [Route("R1_RoleGetByList")]
        public async Task<HttpResponseMessage> R1_RoleGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                string userID = httpRequest["userID"];

                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                var Role = db.A0001.Where(x => x.A0001_ID != null).Select(x => new {
                    x.A0001_ID,
                    x.tenRole,
                    x.maRole,
                    x.thuTu,
                    x.tinhTrang,
                    count = x.A0003.Count(),
                    x.kieuDuLieu
                });

                var UserCheck = db.A0002.FirstOrDefault(x => x.A0002_ID == userID);
                if(UserCheck != null && UserCheck.IsPosition == 1)
                {
                    var a00042 = db.A0042.Where(x => x.A0002_ID == userID).Select(x => x.A0001_ID).ToList();
                    Role = Role.Where(x => a00042.Contains(x.A0001_ID) == true);
                }

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
                        Role = Role.Where(x => x.tinhTrang == true);
                    }
                    else
                    {
                        Role = Role.Where(x => x.tinhTrang == false);
                    }
                }

                Role = Role.Where(cd => (s == "" || cd.tenRole.Contains(s)));
                var qrs = Role.OrderBy(x => x.thuTu).Skip(pz * (p - 1)).Take(pz).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = Role.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            } 
        }

        [HttpGet]
        [Route("R1_RoleGetByID/{Id}")]
        public HttpResponseMessage R1_RoleGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Role = db.A0001.Where(x => x.A0001_ID == Id).Select(x => new {
                    x.A0001_ID,
                    x.tenRole,
                    x.maRole,
                    x.thuTu,
                    x.tinhTrang,
                    x.kieuDuLieu
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(Role));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_RoleGetBySelect")]
        public HttpResponseMessage R1_RoleGetBySelect()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var Role = db.A0001.Where(x => x.tinhTrang == true).OrderBy(x => x.thuTu).Select(x => new {
                    x.A0001_ID,
                    x.tenRole,
                    x.tinhTrang
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(Role));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> R1_UserGetByRole()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string ks = httpRequest["s"];
                    string A0001_ID = httpRequest["A0001_ID"];
                    XmlDocument doc = new XmlDocument();
                    doc.Load(HttpContext.Current.Server.MapPath("~/Content/Api.xml"));
                    XmlNode node = doc.DocumentElement.SelectSingleNode("/ApiWeb/Api");
                    string url = node.ChildNodes[0]?.InnerText;
                    if (url == null)
                    {
                        url = "http://192.84.100.207/";
                    }

                    string apiUrl = url + "EC0002";
                    List<PhongBanVModel> ListPB = new List<PhongBanVModel>();
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responsedata = await client.GetAsync(apiUrl);
                    if (responsedata.IsSuccessStatusCode)
                    {
                        var data = await responsedata.Content.ReadAsStringAsync();
                        ListPB = JsonConvert.DeserializeObject<List<PhongBanVModel>>(data);
                    }
                    db.Configuration.LazyLoadingEnabled = false;
                    var tables = db.A0002.Where(x => x.A0003.Count(a => a.A0001_ID == A0001_ID) > 0).OrderBy(x => x.hoVaTen).ToList().Select(a => new
                    {
                         a.A0002_ID,
                         a.hoVaTen,
                         a.anhDaiDien,
                         a.A0004_ID,
                         a.userName,
                        tenPhongBan = ListPB.FirstOrDefault(x => x.id == a.A0004_ID) != null ? ListPB.FirstOrDefault(x => x.id == a.A0004_ID).bophan_ten : ""
                    });

                    tables = tables.Where(cd => (ks == ""
                          || cd.hoVaTen.Contains(ks))
                    );
                    var qr = tables.ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(qr));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [HttpPost]
        [Route("R2_AddUserToRole")]
        public HttpResponseMessage R2_AddUserToRole()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0001_ID = httpRequest["A0001_ID"];
                    List<string> ListUser = JsonConvert.DeserializeObject<List<string>>(httpRequest["ListUser"]);
                    foreach (var item in ListUser)
                    {
                        var check = db.A0003.FirstOrDefault(x => x.A0001_ID == A0001_ID && x.A0002_ID == item);
                       if(check == null)
                       {
                            var obj = new A0003();
                            obj.A0003_ID = helper.GenKey();
                            obj.A0001_ID = A0001_ID;
                            obj.A0002_ID = item;
                            db.A0003.Add(obj);
                       }
                    }
                    db.SaveChanges();
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [HttpPost]
        [Route("R2_AddRole")] 
        public HttpResponseMessage R2_AddRole(A0001 role)
        {
            using (MeikoEntities db = new MeikoEntities())
            { 
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0001();
                obj.A0001_ID = helper.GenKey();
                obj.tenRole = role.tenRole;
                obj.maRole = role.maRole;
                obj.thuTu = role.thuTu;
                obj.tinhTrang = role.tinhTrang;
                obj.kieuDuLieu = role.kieuDuLieu;
                db.A0001.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateRole")]
        public HttpResponseMessage R3_UpdateRole(A0001 role)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0001.Find(role.A0001_ID);
                if(role != null)
                {
                    obj.tenRole = role.tenRole;
                    obj.maRole = role.maRole;
                    obj.thuTu = role.thuTu;
                    obj.tinhTrang = role.tinhTrang;
                    obj.kieuDuLieu = role.kieuDuLieu;
                    db.SaveChanges();
                } 
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteRole")]
        public HttpResponseMessage R4_DeleteRole(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0001.Where(x => Id.Contains(x.A0001_ID)).ToList();
                if (obj.Count > 0)
                {
                    db.A0001.RemoveRange(obj);
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R1_UserGetByRoleBuaAn")]
        public HttpResponseMessage R1_UserGetByRoleBuaAn()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0002_ID = httpRequest["A0002_ID"];
                    string A0001_ID = httpRequest["A0001_ID"];                    
                    db.Configuration.LazyLoadingEnabled = false;
                    var tables = db.A0010.Where(x => x.A0001_ID == A0001_ID && x.A0002_ID == A0002_ID).Select(a => new
                    {
                        a.A0010_ID,
                        a.A0001_ID,
                        a.A0002_ID,
                        a.A0004_ID,
                        a.BuaAnID,
                        a.thoiGianGioiHan,
                        a.quyenThem,
                        a.quyenCapNhap,
                        a.quyenXoa                       
                    });                     
                    var qr = tables.ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(qr));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R2_AddRoleBuaAn")]
        [HttpPost]
        public async Task<HttpResponseMessage> R2_AddRoleBuaAn()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;    
                    List<A0010> cts = JsonConvert.DeserializeObject<List<A0010>>(httpRequest["rolebuaan"]);
                    foreach (var ct in cts)
                    {
                        var pq = db.A0010.FirstOrDefault(c => c.A0001_ID == ct.A0001_ID && c.A0004_ID == ct.A0004_ID && c.A0002_ID == ct.A0002_ID && c.BuaAnID == ct.BuaAnID);
                        if (pq == null)
                        {
                            ct.A0010_ID = helper.GenKey();
                            db.A0010.Add(ct);
                        }
                        else
                        {
                            pq.A0004_ID = ct.A0004_ID;
                            pq.thoiGianGioiHan = ct.thoiGianGioiHan;
                            pq.quyenThem = ct.quyenThem;
                            pq.quyenCapNhap = ct.quyenCapNhap;
                            pq.quyenXoa = ct.quyenXoa;
                        }
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

        [Route("R2_UpdateRoleBuaAn")]
        [HttpPost]
        public async Task<HttpResponseMessage> R2_UpdateRoleBuaAn()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;
                    List<A0010> cts = JsonConvert.DeserializeObject<List<A0010>>(httpRequest["rolebuaan"]);
                    foreach (var ct in cts)
                    {
                        var pq = db.A0010.FirstOrDefault(c => c.A0001_ID == ct.A0001_ID && c.A0010_ID == ct.A0010_ID && c.A0002_ID == ct.A0002_ID && c.BuaAnID == ct.BuaAnID);
                        if (pq == null)
                        {
                            ct.A0010_ID = helper.GenKey();
                            db.A0010.Add(ct);
                        }
                        else
                        {
                            pq.A0004_ID = ct.A0004_ID;
                            pq.thoiGianGioiHan = ct.thoiGianGioiHan;
                            pq.quyenThem = ct.quyenThem;
                            pq.quyenCapNhap = ct.quyenCapNhap;
                            pq.quyenXoa = ct.quyenXoa;
                        }
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

        [Route("R2_AddRoleBuaAnAll")]
        [HttpPost]
        public async Task<HttpResponseMessage> R2_AddRoleBuaAnAll()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;
                    List<A0010> cts = JsonConvert.DeserializeObject<List<A0010>>(httpRequest["rolebuaan"]);
                    foreach (var ct in cts)
                    {
                        var pq = db.A0010.FirstOrDefault(c => c.A0001_ID == ct.A0001_ID && c.A0002_ID == ct.A0002_ID && c.A0004_ID == ct.A0004_ID && c.BuaAnID == ct.BuaAnID);
                        if (pq == null)
                        {
                            ct.A0010_ID = helper.GenKey();
                            db.A0010.Add(ct);
                        }
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

        [Route("R3_SetTimeBARole")]
        [HttpPost]
        public async Task<HttpResponseMessage> R3_SetTimeBARole(A0010 a0010)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var pq = db.A0010.Where(c => c.A0001_ID == a0010.A0001_ID && c.BuaAnID == a0010.BuaAnID).ToList();
                    if(pq.Count > 0)
                    {
                        foreach (var item in pq)
                        {
                            item.thoiGianGioiHan = a0010.thoiGianGioiHan;
                        }
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

        [Route("R4_DeleteRoleBuaAn")]
        [HttpPost]
        public async Task<HttpResponseMessage> R4_DeleteRoleBuaAn(A0010 row)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;
                    var pq = db.A0010.FirstOrDefault(c => c.A0010_ID == row.A0010_ID);
                    if(pq != null)
                    {
                        db.A0010.Remove(pq);
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


        [Route("R4_DeleteUserRole")]
        [HttpPost]
        public async Task<HttpResponseMessage> R4_DeleteUserRole(A0003 row)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;

                    var a0005List = db.A0005.Where(x => x.A0002_ID == row.A0002_ID && x.A0001_ID == row.A0001_ID).ToList();
                    if(a0005List.Count > 0)
                    {
                        db.A0005.RemoveRange(a0005List);
                    }

                    var pq = db.A0003.FirstOrDefault(c => c.A0002_ID == row.A0002_ID && c.A0001_ID == row.A0001_ID);
                    if (pq != null)
                    {
                        db.A0003.Remove(pq);
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
