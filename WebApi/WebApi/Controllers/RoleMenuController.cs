using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Helper;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [RoutePrefix("api/RoleMenu")]
    public class RoleMenuController : ApiController
    {
        [Route("R1_RoleMenuGetByList")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_RoleMenuGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                string A0001_ID = httpRequest["A0001_ID"];
                try
                {
                    if (string.IsNullOrWhiteSpace(A0002_ID))
                    {
                        A0002_ID = null;
                    }
                    else
                    {
                        var check = db.A0005.FirstOrDefault(x => x.A0001_ID == A0001_ID && x.A0002_ID == A0002_ID);
                        if(check == null)
                        {
                            A0002_ID = null;
                        }
                    }
                    string Connection = db.Database.Connection.ConnectionString;
                    var data = db.R1_List_PhanQuyen(A0001_ID,A0002_ID).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(data));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R2_AddRoleMenu")]
        [HttpPost]
        public async Task<HttpResponseMessage> R2_AddRoleMenu()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;
                    string A0001_ID = httpRequest["A0001_ID"];
                    string Istype = httpRequest["type"];
                    if(Istype == "2")
                    {
                        var arrDelete = db.A0005.Where(x => x.A0001_ID == A0001_ID).ToList();
                        db.A0005.RemoveRange(arrDelete);
                        db.SaveChanges();
                    }
                    List<A0005> cts = JsonConvert.DeserializeObject<List<A0005>>(httpRequest["rolemenu"]);
                    foreach (var ct in cts)
                    {
                        var pq = db.A0005.FirstOrDefault(c => c.A0001_ID == A0001_ID && c.A0007_ID == ct.A0007_ID && c.A0002_ID == null);
                        if (pq == null)
                        {
                            ct.A0005_ID = helper.GenKey();
                            ct.A0001_ID = A0001_ID;
                            db.A0005.Add(ct);
                        }
                        else
                        {
                            pq.quyenXem = ct.quyenXem ? true : false;
                            pq.quyenThem = ct.quyenThem ? true : false;
                            pq.quyenCapNhap = ct.quyenCapNhap ? true : false;
                            pq.quyenXoa = ct.quyenXoa ? true : false;
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

        [Route("R2_AddRoleMenuUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> R2_AddRoleMenuUser()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    db.Configuration.LazyLoadingEnabled = false;
                    string A0001_ID = httpRequest["A0001_ID"];
                    string A0002_ID = httpRequest["A0002_ID"];
                    List<A0005> cts = JsonConvert.DeserializeObject<List<A0005>>(httpRequest["rolemenu"]);
                    var check = db.A0005.FirstOrDefault(c => c.A0001_ID == A0001_ID);
                    if(check == null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return response;
                    }
                    if(A0002_ID == "null")
                    {
                        A0002_ID = null;
                    }
                    foreach (var ct in cts)
                    {
                        var pq = db.A0005.FirstOrDefault(c => c.A0001_ID == A0001_ID && c.A0007_ID == ct.A0007_ID && c.A0002_ID == A0002_ID);
                        if (pq == null)
                        {
                            ct.A0005_ID = helper.GenKey();
                            ct.A0001_ID = A0001_ID;
                            ct.A0002_ID = A0002_ID;
                            db.A0005.Add(ct);
                        }
                        else
                        {
                            pq.quyenXem = ct.quyenXem ? true : false;
                            pq.quyenThem = ct.quyenThem ? true : false;
                            pq.quyenCapNhap = ct.quyenCapNhap ? true : false;
                            pq.quyenXoa = ct.quyenXoa ? true : false;
                            pq.A0002_ID = A0002_ID;
                        }
                    }
                    await db.SaveChangesAsync();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new {error = 0 }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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
