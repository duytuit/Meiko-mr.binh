using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Helper;
using WebApi.Models;
namespace WebApi.Controllers
{
    [RoutePrefix("api/Baophe")]
    public class BaoPheController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        #region Phòng ban báo phế
        [HttpPost]
        [Route("R1_PhongBanMapperGetByList")]
        public HttpResponseMessage R1_PhongBanMapperGetByList()
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
                int kieuPhongBan = int.Parse(httpRequest["kieuPhongBan"]);
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0034.Where(x => x.kieuPhongBan == kieuPhongBan).Select(x => new
                {
                    x.A0034_ID,
                    x.Parent_ID,
                    x.phongBanMapID,
                    x.maPhongBan,
                    x.tenPhongBan,
                    x.kieuPhongBan,
                    x.trangThai,
                    x.STT,
                    count = x.A0036.Count(c => c.IsType == 0)
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
                if (!string.IsNullOrWhiteSpace(sts))
                {
                    if (sts == "on")
                    {
                        tables = tables.Where(x => x.trangThai == true);
                    }
                    else
                    {
                        tables = tables.Where(x => x.trangThai == false);
                    }
                }

                tables = tables.Where(cd => (s == "" ||
                cd.tenPhongBan.Contains(s) ||
                cd.maPhongBan.Contains(s)));
                var qrs = tables.OrderBy(x => x.STT).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_PhongBanGetAll")]
        public HttpResponseMessage R1_PhongBanGetAll()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0034.Where(x => x.kieuPhongBan == 1).Select(x => new
                {
                    x.A0034_ID,
                    x.Parent_ID,
                    x.phongBanMapID,
                    x.maPhongBan,
                    x.tenPhongBan,
                    x.kieuPhongBan,
                    x.trangThai,
                    x.STT
                });
                var qrs = tables.ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_BoPhanGetAll")]
        public HttpResponseMessage R1_BoPhanGetAll()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0034.Where(x => x.kieuPhongBan == 2).Select(x => new
                {
                    x.A0034_ID,
                    x.Parent_ID,
                    x.phongBanMapID,
                    x.maPhongBan,
                    x.tenPhongBan,
                    x.kieuPhongBan,
                    x.trangThai,
                    x.STT
                });
                var qrs = tables.ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_PhongBanMapGetBySelect")]
        public HttpResponseMessage R1_PhongBanMapGetBySelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0034.Where(x => x.trangThai == true).Select(x => new
                {
                    x.A0034_ID,
                    x.Parent_ID,
                    x.phongBanMapID,
                    x.maPhongBan,
                    x.tenPhongBan,
                    x.kieuPhongBan,
                    x.trangThai,
                    x.STT
                });
                var qrs = tables.OrderBy(x => x.STT).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_PhongBanMapperGetByID/{Id}")]
        public HttpResponseMessage R1_PhongBanMapperGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0034 = db.A0034.FirstOrDefault(x => x.trangThai == true && x.A0034_ID == Id);
                response.Content = new StringContent(JsonConvert.SerializeObject(a0034));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddPhongBanMapper")]
        public HttpResponseMessage R2_AddPhongBanMapper(A0034 pb)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    if(pb.Parent_ID == "null")
                    {
                        pb.Parent_ID = null;
                    }

                    if (pb.phongBanMapID == "null")
                    {
                        pb.phongBanMapID = null;
                    }

                    var obj = new A0034();
                    obj.A0034_ID = helper.GenKey();
                    obj.Parent_ID = pb.Parent_ID;
                    obj.phongBanMapID = pb.phongBanMapID;
                    obj.maPhongBan = pb.maPhongBan;
                    obj.tenPhongBan = pb.tenPhongBan;
                    obj.kieuPhongBan = pb.kieuPhongBan;
                    obj.trangThai = pb.trangThai;
                    obj.STT = pb.STT;
                    db.A0034.Add(obj);
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdatePhongBanMapper")]
        public HttpResponseMessage R3_UpdatePhongBanMapper(A0034 pb)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    if (pb.Parent_ID == "null")
                    {
                        pb.Parent_ID = null;
                    }

                    if (pb.phongBanMapID == "null")
                    {
                        pb.phongBanMapID = null;
                    }

                    var obj = db.A0034.Find(pb.A0034_ID);
                    if (obj != null)
                    {
                        obj.Parent_ID = pb.Parent_ID;
                        obj.phongBanMapID = pb.phongBanMapID;
                        obj.maPhongBan = pb.maPhongBan;
                        obj.tenPhongBan = pb.tenPhongBan;
                        obj.kieuPhongBan = pb.kieuPhongBan;
                        obj.trangThai = pb.trangThai;
                        obj.STT = pb.STT;
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
        }

        [HttpPost]
        [Route("R4_DeletePhongBanMapper")]
        public HttpResponseMessage R4_DeletePhongBanMapper(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0034.Where(x => Id.Contains(x.A0034_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0034.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpGet]
        [Route("R1_GetDMBaoPheByPhongBanID/{Id}")]
        public HttpResponseMessage R1_GetDMBaoPheByPhongBanID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0035 = db.A0035.Where(x => x.trangThai == true && x.A0036.Count(c => c.A0034_ID == Id && c.IsType == 0) > 0).Select(x => new
                {
                    x.A0035_ID,
                    x.maNoiDungBaoPhe,
                    x.tenTiengViet,
                    x.tenTiengNhat,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(a0035));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteDMBPPhongBan")]
        public HttpResponseMessage R4_DeleteDMBPPhongBan()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var httpRequest = HttpContext.Current.Request;
                var A0034_ID = httpRequest["A0034_ID"];
                var isType = int.Parse(httpRequest["IsType"]);
                var ListA0035 = JsonConvert.DeserializeObject<List<string>>(httpRequest["ListA0035"]);

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0036.Where(x => ListA0035.Contains(x.A0035_ID) && x.A0034_ID == A0034_ID && x.IsType == isType).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0036.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

        #endregion

        #region Nội dung báo phế    
        [HttpPost]
        [Route("R1_NoiDungBPGetByList")]
        public HttpResponseMessage R1_NoiDungBPGetByList()
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
                var tables = db.A0035.Select(x => new
                {
                    x.A0035_ID,
                    x.maNoiDungBaoPhe,
                    x.tenTiengViet,
                    x.tenTiengNhat,
                    x.STT,
                    x.trangThai,
                    count = x.A0036.Count(c => c.IsType == 1)
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
                if (!string.IsNullOrWhiteSpace(sts))
                {
                    if (sts == "on")
                    {
                        tables = tables.Where(x => x.trangThai == true);
                    }
                    else
                    {
                        tables = tables.Where(x => x.trangThai == false);
                    }
                }

                tables = tables.Where(cd => (s == "" ||
                cd.tenTiengNhat.Contains(s) ||
                cd.tenTiengViet.Contains(s)));
                var qrs = tables.OrderBy(x => x.STT).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_NoiDungBPGetBySelect")]
        public HttpResponseMessage R1_NoiDungBPGetBySelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0035.Where(x => x.trangThai == true).Select(x => new
                {
                    x.A0035_ID,
                    x.maNoiDungBaoPhe,
                    x.tenTiengViet,
                    x.tenTiengNhat,
                    x.STT,
                    x.trangThai
                });
                var qrs = tables.OrderBy(x => x.STT).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_NoiDungBPGetByID/{Id}")]
        public HttpResponseMessage R1_NoiDungBPGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0035 = db.A0035.Where(x => x.A0035_ID == Id).Select(x => new
                {
                    x.A0035_ID,
                    x.maNoiDungBaoPhe,
                    x.tenTiengViet,
                    x.tenTiengNhat,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(a0035));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_PhongBanGetBaoPhe")]
        public HttpResponseMessage R1_PhongBanGetBaoPhe()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0034.Where(x => x.kieuPhongBan == 1).Select(x => new
                {
                    x.A0034_ID,
                    x.Parent_ID,
                    x.phongBanMapID,
                    x.maPhongBan,
                    x.tenPhongBan,
                    x.kieuPhongBan,
                    x.trangThai,
                    x.STT,
                    ListA0035 = db.A0035.Where(a => a.A0036.Count(c => c.A0034_ID == x.A0034_ID) > 0 && x.trangThai == true).Select(a => new
                    {
                        a.A0035_ID,
                        a.maNoiDungBaoPhe,
                        a.tenTiengViet,
                        a.tenTiengNhat,
                        a.STT,
                        a.trangThai,
                        ListBoPhan = a.A0036.Where(b => b.A0035_ID == a.A0035_ID && b.A0034.kieuPhongBan == 2).Select(c => new
                        {
                            c.A0034.A0034_ID,
                            c.A0034.tenPhongBan,
                            c.A0034.STT
                        }).ToList()
                    }).OrderBy(c => c.STT).ToList()
                });
                var qrs = tables.ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_NoiDungBPGetByPhongBan/{Id}")]
        public HttpResponseMessage R1_NoiDungBPGetByPhongBan(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0035 = db.A0035.Where(x => x.A0036.Count(c => c.A0034_ID == Id) > 0 && x.trangThai == true).Select(x => new
                {
                    x.A0035_ID,
                    x.maNoiDungBaoPhe,
                    x.tenTiengViet,
                    x.tenTiengNhat,
                    x.STT,
                    x.trangThai,
                    ListBoPhan = x.A0036.Where(a => a.A0035_ID == x.A0035_ID && a.A0034.kieuPhongBan == 2).Select(c => new
                    {
                        c.A0034.A0034_ID,
                        c.A0034.Parent_ID,
                        c.A0034.phongBanMapID,
                        c.A0034.maPhongBan,
                        c.A0034.tenPhongBan,
                        c.A0034.kieuPhongBan,
                        c.A0034.trangThai,
                        c.A0034.STT
                    }).ToList()
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(a0035));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("getPhongBanByDMBaoPheID/{Id}")]
        public HttpResponseMessage getPhongBanByDMBaoPheID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0034 = db.A0034.Where(x => x.A0036.Count(c => c.A0035_ID == Id && c.IsType == 1) > 0 && x.trangThai == true).Select(x => new
                {
                    x.A0034_ID,
                    x.Parent_ID,
                    x.phongBanMapID,
                    x.maPhongBan,
                    x.tenPhongBan,
                    x.kieuPhongBan,
                    x.trangThai,
                    x.STT
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(a0034));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeletePBDMBP")]
        public HttpResponseMessage R4_DeletePBDMBP()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var httpRequest = HttpContext.Current.Request;
                var A0035_ID = httpRequest["A0035_ID"];
                var isType = int.Parse(httpRequest["IsType"]);
                var ListA0034 = JsonConvert.DeserializeObject<List<string>>(httpRequest["ListA0034"]);

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0036.Where(x => ListA0034.Contains(x.A0034_ID) && x.A0035_ID == A0035_ID && x.IsType == isType).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0036.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddNoiDungBP")]
        public HttpResponseMessage R2_AddNoiDungBP(A0035 pb)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = new A0035();
                    obj.A0035_ID = helper.GenKey();
                    obj.maNoiDungBaoPhe = pb.maNoiDungBaoPhe;
                    obj.tenTiengViet = pb.tenTiengViet;
                    obj.tenTiengNhat = pb.tenTiengNhat;
                    obj.STT = pb.STT;
                    obj.trangThai = pb.trangThai;
                    db.A0035.Add(obj);
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                }
                catch (Exception)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateNoiDungBP")]
        public HttpResponseMessage R3_UpdatePhongBanMap(A0035 pb)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = db.A0035.Find(pb.A0035_ID);
                    if (obj != null)
                    {
                        obj.maNoiDungBaoPhe = pb.maNoiDungBaoPhe;
                        obj.tenTiengViet = pb.tenTiengViet;
                        obj.tenTiengNhat = pb.tenTiengNhat;
                        obj.STT = pb.STT;
                        obj.trangThai = pb.trangThai;
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
        }

        [HttpPost]
        [Route("R4_DeleteNoiDungBP")]
        public HttpResponseMessage R4_DeleteNoiDungBP(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0035.Where(x => Id.Contains(x.A0035_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0035.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddPBToDMBaoPhe")]
        public HttpResponseMessage R2_AddPBToDMBaoPhe()
        {
            var httpRequest = HttpContext.Current.Request;

            var A0035_ID = httpRequest["A0035_ID"];
            var ListA0034 = JsonConvert.DeserializeObject<List<A0034>>(httpRequest["ListA0034"]);

            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    foreach (var item in ListA0034)
                    {
                        var check = db.A0036.FirstOrDefault(x => x.A0034_ID == item.A0034_ID && x.A0035_ID == A0035_ID);
                        if (check == null)
                        {
                            var obj = new A0036();
                            obj.A0036_ID = helper.GenKey();
                            obj.A0034_ID = item.A0034_ID;
                            obj.A0035_ID = A0035_ID;
                            obj.IsType = 1;
                            db.A0036.Add(obj);
                        }
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddDMBaoPheToPhongBan")]
        public HttpResponseMessage R2_AddDMBaoPheToPhongBan()
        {
            var httpRequest = HttpContext.Current.Request;

            var A0034_ID = httpRequest["A0034_ID"];
            var ListA0035 = JsonConvert.DeserializeObject<List<A0035>>(httpRequest["ListA0035"]);

            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    foreach (var item in ListA0035)
                    {
                        var check = db.A0036.FirstOrDefault(x => x.A0035_ID == item.A0035_ID && x.A0034_ID == A0034_ID);
                        if (check == null)
                        {
                            var obj = new A0036();
                            obj.A0036_ID = helper.GenKey();
                            obj.A0034_ID = A0034_ID;
                            obj.A0035_ID = item.A0035_ID;
                            db.A0036.Add(obj);
                        }
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
        }

        #endregion

        #region Quy trình báo phế

        [Route("R1_MyWorkDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_MyWorkDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var tables = db.A0028.Where(x => x.daXoa == false && x.viTriHienTaiMenuID == "M04.02" && x.viTriHienTaiUserID == A0002_ID && x.maForm == "SF006").Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.maForm,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_WaitingForSignDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_WaitingForSignDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var tables = db.A0028.Where(x => x.trangThai == 1 && x.daXoa == false && x.viTriHienTaiMenuID == "M04.04" && x.viTriHienTaiUserID == A0002_ID).Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.maForm,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_WaitingProcessDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_WaitingProcessDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var UserDocSign = db.A0029.Where(x => x.A0002_ID == A0002_ID).Select(x => x.A0028_ID).Distinct().ToList();
                    var tables = db.A0028.Where(x => x.trangThai == 1 && x.daXoa == false && x.viTriHienTaiMenuID == "M04.08" && UserDocSign.Contains(x.A0028_ID)).Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.maForm,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_SenddingDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_SenddingDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var UserDocTran = db.A0030.Where(x => x.A0002_ID == A0002_ID && x.trangThai == 1).Select(x => x.A0028_ID).Distinct().ToList();
                    var tables = db.A0028.Where(x => x.daXoa == false && UserDocTran.Contains(x.A0028_ID) && (x.viTriHienTaiMenuID != "M04.08" && x.viTriHienTaiMenuID != "M04.05")).Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.maForm,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_CompletedDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_CompletedDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var UserDocTran = db.A0030.Where(x => x.A0002_ID == A0002_ID).Select(x => x.A0028_ID).Distinct().ToList();
                    var tables = db.A0028.Where(x => x.daXoa == false && UserDocTran.Contains(x.A0028_ID) && x.viTriHienTaiMenuID == "M04.05").Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.maForm,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_PassDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_PassDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var UserDocTran = db.A0030.Where(x => x.A0002_ID == A0002_ID && x.trangThai == 3).Select(x => x.A0028_ID).Distinct().ToList();
                    var tables = db.A0028.Where(x => x.daXoa == false && UserDocTran.Contains(x.A0028_ID)).Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.maForm,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_TrashDocumentCam")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_TrashDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var tables = db.A0028.Where(x => x.daXoa == true && x.A0002_ID == A0002_ID).Select(x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.maForm,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.daXoa,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        IsRead = x.A0029.Where(a => x.trangThai == 4).Count(a => a.A0002_ID == A0002_ID && a.daXem == false) > 0 ? false : true,
                        CountSign = x.A0029.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0029.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
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
        [Route("R1_MyWorkDocumentDetail")]
        public HttpResponseMessage R1_MyWorkDocumentDetail()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string Id = httpRequest["A0028_ID"];
                    string A0034_ID = httpRequest["A0034_ID"];
                    string A0037_ID = httpRequest["A0037_ID"];

                    if (A0034_ID == "null" || A0034_ID == "undefined")
                    {
                        A0034_ID = null;
                    }
                    if (A0037_ID == "null" || A0034_ID == "undefined")
                    {
                        A0037_ID = null;
                    }

                    var A0036List = db.A0036.Where(x => x.A0034_ID == A0034_ID).Select(x => x.A0035_ID).ToList();
                    var tables = db.A0028.Where(x => x.A0028_ID == Id).Select(
                    x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.maForm,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        x.T026C,
                        x.T027C,
                        x.T028C,
                        x.T029C,
                        x.T030C,
                        x.T031C,
                        x.T032C,
                        x.T033C,
                        x.T034C,
                        x.T035C,
                        x.T036C,
                        x.T037C,
                        x.T038C,
                        x.T039C,
                        x.T040C,
                        x.T041C,
                        x.T042C,
                        x.T043C,
                        x.T044C,
                        x.T045C,
                        x.T046C,
                        x.T047C,
                        x.T048C,
                        x.T049C,
                        x.T050C,
                        x.T051C,
                        x.T052C,
                        x.T053C,
                        x.T054C,
                        x.T055C,
                        x.T056C,
                        x.T057C,
                        x.T058C,
                        x.T059C,
                        x.T060C,
                        x.T061C,
                        x.T062C,
                        x.T063C,
                        x.T064C,
                        x.T065C,
                        x.T066C,
                        x.T067C,
                        x.T068C,
                        x.T069C,
                        x.T070C,
                        x.T071C,
                        x.T072C,
                        x.T073C,
                        x.T074C,
                        x.T075C,
                        x.T076C,
                        x.T077C,
                        x.T078C,
                        x.T079C,
                        x.T080C,
                        x.T081C,
                        x.T082C,
                        x.T083C,
                        x.T084C,
                        x.T085C,
                        x.T086C,
                        x.T087C,
                        x.T088C,
                        x.T089C,
                        x.T090C,
                        x.T091C,
                        x.T092C,
                        x.T093C,
                        x.T094C,
                        x.T095C,
                        x.T096C,
                        x.T097C,
                        x.T098C,
                        x.T099C,
                        x.T100C,
                    }).ToList();

                    var tables2 = db.A0028D.Where(x => x.A0028_ID == Id).Select(x => new
                    {
                        x.A0028D_ID,
                        x.A0028_ID,
                        x.IsPostion,
                        x.C001C,
                        x.C002C,
                        x.C003C,
                        x.C004C,
                        x.C005C,
                        x.C006C,
                        x.C007C,
                        x.C008C,
                        x.C009C,
                        x.C010C,
                        x.C011C,
                        x.C012C,
                        x.C013C,
                        x.C014C,
                        x.C015C,
                        x.C016C,
                        x.C017C,
                        x.C018C,
                        x.C019C,
                        x.C020C,
                        x.C021C,
                        x.C022C,
                        x.C023C,
                        x.C024C,
                        x.C025C,
                        x.C026C,
                        x.C027C,
                        x.C028C,
                        x.C029C,
                        x.C030C,
                        x.C031C,
                        x.C032C,
                        x.C033C,
                        x.C034C,
                        x.C035C,
                        x.C036C,
                        x.C037C,
                        x.C038C,
                        x.C039C,
                        x.C040C
                    });


                    if (!string.IsNullOrWhiteSpace(A0034_ID))
                    {
                        tables2 = tables2.Where(x => A0036List.Contains(x.C008C) == true);
                    }

                    var tables3 = db.A0037.Where(x => x.A0028_ID == Id).Select(x => new
                    {
                        x.A0037_ID,
                        x.A0002_ID,
                        x.A0028_ID,
                        x.A0034_ID,
                        tenPhongBan = x.tenBoPhan
                    }).OrderBy(x => x.tenPhongBan).ToList();

                    var tables4 = db.A0039.Where(x => x.A0028_ID == Id && x.A0037_ID == A0037_ID).Select(x => new
                    {
                        x.A0039_ID,
                        x.A0028_ID,
                        x.A0028D_ID,
                        x.A0037_ID,
                        x.ngayXuLy,
                        x.nguoiXuLy,
                        x.noiDungXuLy,
                        x.trangThai
                    }).ToList();

                    var WorkFile = db.A0031.Where(x => x.A0028_ID == Id).Select(x => new
                    {
                        x.A0031_ID,
                        x.A0028_ID,
                        x.tenFile,
                        x.dungLuong,
                        x.duongDan,
                        x.ngayTao,
                        x.loaiFile,
                        x.thuTu
                    }).OrderBy(x => x.thuTu).ToList();

                    var qrtables2 = tables2.OrderBy(x => x.C009C).GroupBy(x => x.C001C).Select(x => new { C001C = x.Key, C009C = x.Select(c => c.C009C).FirstOrDefault(), ListDMBaoPhe = x.OrderBy(c => c.C010C) }).OrderBy(x => x.C009C).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables, data2 = qrtables2, data3 = tables3, data4 = WorkFile, data5 = tables4 }));
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
        [Route("R2_AddWorkDocumentCam")]
        public HttpResponseMessage R2_AddWorkDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            var tk = JsonConvert.DeserializeObject<Token>(httpRequest["tk"]);
            var a0028 = JsonConvert.DeserializeObject<A0028>(httpRequest["a0028"]);
            var a0028D = JsonConvert.DeserializeObject<List<A0028D>>(httpRequest["a0028D"]);
            var a0034 = JsonConvert.DeserializeObject<List<A0034>>(httpRequest["a0034"]);

            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0015 = db.A0016.FirstOrDefault(x => x.maCongViec == "CV006");
                try
                {
                    #region ADD A0028
                    var MaxSteepSign = 1;
                    var obj = new A0028();
                    obj.A0028_ID = helper.GenKey();
                    obj.A0002_ID = tk.Users_ID;
                    obj.A0016_ID = a0015.A0016_ID;
                    obj.A0032_ID = a0028.A0032_ID;
                    obj.trangThaiNguoiTao = a0028.trangThaiNguoiTao;
                    obj.trangThai = 1;
                    obj.ngayTao = DateTime.Now;
                    obj.viTriHienTaiMenuID = "M04.02";
                    obj.viTriHienTaiNhomKyID = "";
                    obj.viTriHienTaiUserID = tk.Users_ID;
                    obj.daXoa = false;
                    obj.ngayXuLyMongMuon = a0028.ngayXuLyMongMuon;
                    obj.maForm = a0028.maForm;
                    obj.T001C = a0028.T001C;
                    obj.T002C = a0028.T002C;
                    obj.T003C = a0028.T003C;
                    obj.T004C = a0028.T004C;
                    obj.T005C = a0028.T005C;
                    obj.T006C = a0028.T006C;
                    obj.T007C = a0028.T007C;
                    obj.T008C = a0028.T008C;
                    obj.T009C = a0028.T009C;
                    obj.T010C = a0028.T010C;
                    obj.T011C = a0028.T011C;
                    obj.T012C = a0028.T012C;
                    obj.T013C = a0028.T013C;
                    obj.T014C = a0028.T014C;
                    obj.T015C = a0028.T015C;
                    obj.T016C = a0028.T016C;
                    obj.T017C = a0028.T017C;
                    obj.T018C = a0028.T018C;
                    obj.T019C = a0028.T019C;
                    obj.T020C = a0028.T020C;
                    obj.T021C = a0028.T021C;
                    obj.T022C = a0028.T022C;
                    obj.T023C = a0028.T023C;
                    obj.T024C = a0028.T024C;
                    obj.T025C = a0028.T025C;
                    obj.T026C = a0028.T026C;
                    obj.T027C = a0028.T027C;
                    obj.T028C = a0028.T028C;
                    obj.T029C = a0028.T029C;
                    obj.T030C = a0028.T030C;
                    obj.T031C = a0028.T031C;
                    obj.T032C = a0028.T032C;
                    obj.T033C = a0028.T033C;
                    obj.T034C = a0028.T034C;
                    obj.T035C = a0028.T035C;
                    obj.T036C = a0028.T036C;
                    obj.T037C = a0028.T037C;
                    obj.T038C = a0028.T038C;
                    obj.T039C = a0028.T039C;
                    obj.T040C = a0028.T040C;
                    obj.T041C = a0028.T041C;
                    obj.T042C = a0028.T042C;
                    obj.T043C = a0028.T043C;
                    obj.T044C = a0028.T044C;
                    obj.T045C = a0028.T045C;
                    obj.T046C = a0028.T046C;
                    obj.T047C = a0028.T047C;
                    obj.T048C = a0028.T048C;
                    obj.T049C = a0028.T049C;
                    obj.T050C = a0028.T050C;
                    obj.T051C = a0028.T051C;
                    obj.T052C = a0028.T052C;
                    obj.T053C = a0028.T053C;
                    obj.T054C = a0028.T054C;
                    obj.T055C = a0028.T055C;
                    obj.T056C = a0028.T056C;
                    obj.T057C = a0028.T057C;
                    obj.T058C = a0028.T058C;
                    obj.T059C = a0028.T059C;
                    obj.T060C = a0028.T060C;
                    obj.T061C = a0028.T061C;
                    obj.T062C = a0028.T062C;
                    obj.T063C = a0028.T063C;
                    obj.T064C = a0028.T064C;
                    obj.T065C = a0028.T065C;
                    obj.T066C = a0028.T066C;
                    obj.T067C = a0028.T067C;
                    obj.T068C = a0028.T068C;
                    obj.T069C = a0028.T069C;
                    obj.T070C = a0028.T070C;
                    obj.T071C = a0028.T071C;
                    obj.T072C = a0028.T072C;
                    obj.T073C = a0028.T073C;
                    obj.T074C = a0028.T074C;
                    obj.T075C = a0028.T075C;
                    obj.T076C = a0028.T076C;
                    obj.T077C = a0028.T077C;
                    obj.T078C = a0028.T078C;
                    obj.T079C = a0028.T079C;
                    obj.T080C = a0028.T080C;
                    obj.T081C = a0028.T081C;
                    obj.T082C = a0028.T082C;
                    obj.T083C = a0028.T083C;
                    obj.T084C = a0028.T084C;
                    obj.T085C = a0028.T085C;
                    obj.T086C = a0028.T086C;
                    obj.T087C = a0028.T087C;
                    obj.T088C = a0028.T088C;
                    obj.T089C = a0028.T089C;
                    obj.T090C = a0028.T090C;
                    obj.T091C = a0028.T091C;
                    obj.T092C = a0028.T092C;
                    obj.T093C = a0028.T093C;
                    obj.T094C = a0028.T094C;
                    obj.T095C = a0028.T095C;
                    obj.T096C = a0028.T096C;
                    obj.T097C = a0028.T097C;
                    obj.T098C = a0028.T098C;
                    obj.T099C = a0028.T099C;
                    obj.T100C = a0028.T100C;
                    db.A0028.Add(obj);
                    #endregion

                    if (a0028D != null)
                    {
                        foreach (var item in a0028D)
                        {
                            #region ADD A0028D
                            var objD = new A0028D();
                            objD.A0028_ID = obj.A0028_ID;
                            objD.A0028D_ID = helper.GenKey();
                            objD.IsPostion = item.IsPostion;
                            objD.C001C = item.C001C;
                            objD.C002C = item.C002C;
                            objD.C003C = item.C003C;
                            objD.C004C = item.C004C;
                            objD.C005C = item.C005C;
                            objD.C006C = item.C006C;
                            objD.C007C = item.C007C;
                            objD.C008C = item.C008C;
                            objD.C009C = item.C009C;
                            objD.C010C = item.C010C;
                            objD.C011C = item.C011C;
                            objD.C012C = item.C012C;
                            objD.C013C = item.C013C;
                            objD.C014C = item.C014C;
                            objD.C015C = item.C015C;
                            objD.C016C = item.C016C;
                            objD.C017C = item.C017C;
                            objD.C018C = item.C018C;
                            objD.C019C = item.C019C;
                            objD.C020C = item.C020C;
                            objD.C021C = item.C021C;
                            objD.C022C = item.C022C;
                            objD.C023C = item.C023C;
                            objD.C024C = item.C024C;
                            objD.C025C = item.C025C;
                            objD.C026C = item.C026C;
                            objD.C027C = item.C027C;
                            objD.C028C = item.C028C;
                            objD.C029C = item.C029C;
                            objD.C030C = item.C030C;
                            objD.C031C = item.C031C;
                            objD.C032C = item.C032C;
                            objD.C033C = item.C033C;
                            objD.C034C = item.C034C;
                            objD.C035C = item.C035C;
                            objD.C036C = item.C036C;
                            objD.C037C = item.C037C;
                            objD.C038C = item.C038C;
                            objD.C039C = item.C039C;
                            objD.C040C = item.C040C;
                            db.A0028D.Add(objD);
                            #endregion
                        }
                    }

                    foreach (var item in a0034)
                    {
                        var obj037 = new A0037();
                        obj037.A0037_ID = helper.GenKey();
                        obj037.A0002_ID = tk.Users_ID;
                        obj037.A0028_ID = obj.A0028_ID;
                        obj037.A0034_ID = item.A0034_ID;
                        obj037.tenBoPhan = item.tenPhongBan;
                        obj037.trangThai = 1;
                        obj037.ngayTao = DateTime.Now;
                        obj037.viTriHienTaiMenuID = "";
                        obj037.viTriHienTaiUserID = tk.Users_ID;
                        obj037.IsEdit = true;
                        db.A0037.Add(obj037);

                        var a0036 = db.A0036.Where(x => x.A0034_ID == item.A0034_ID).Select(x => x.A0035_ID).ToList();
                        var a0039List = a0028D.Where(x => a0036.Contains(x.C001C)).ToList();
                        foreach (var item35 in a0039List)
                        {
                            var a0039 = new A0039();
                            a0039.A0039_ID = helper.GenKey();
                            a0039.A0028_ID = item35.A0028_ID;
                            a0039.A0028D_ID = item35.A0028D_ID;
                            a0039.A0037_ID = obj037.A0037_ID;
                            a0039.trangThai = 0;
                            db.A0039.Add(a0039);
                        }
                    }

                    var user = db.A0002.Find(obj.A0002_ID);
                    var A0015_ID = a0015.A0015_ID;
                    var A0004_ID = user.A0004_ID;

                    var Process = db.A0018.OrderBy(x => x.STT).FirstOrDefault(x => x.A0015_ID == A0015_ID && x.trangThai == true && x.A0004_ID == A0004_ID);
                    if (Process == null)
                    {
                        Process = db.A0018.OrderBy(x => x.STT).FirstOrDefault(x => x.A0015_ID == A0015_ID && x.trangThai == true && x.A0004_ID == null);
                    }

                    var SteepSign = db.A0020.Where(s => s.trangThai == true).OrderBy(s => s.STT).Select(s => new
                    {
                        s.A0020_ID,
                        s.tenBuocKy,
                        s.AliasBuocKy,
                        s.STT
                    });

                    var a0030 = new A0030();
                    a0030.A0030_ID = helper.GenKey();
                    a0030.A0028_ID = obj.A0028_ID;
                    a0030.A0002_ID = obj.A0002_ID;
                    a0030.tieuDe = "Tạo hồ sơ công việc";
                    a0030.noiDung = "Tạo hồ sơ công việc";
                    a0030.thuThu = 1;
                    a0030.trangThai = 2;
                    a0030.thoiGianGui = DateTime.Now;
                    db.A0030.Add(a0030);

                    if (Process != null)
                    {
                        string A0018_ID = Process.A0018_ID;
                        var StepCheckPass = SteepSign.Select(x => x.A0020_ID).ToList();
                        var GroupSignCheckPass004 = db.A0021.Where(gs => StepCheckPass.Contains(gs.A0020_ID) == true && gs.A0018_ID == A0018_ID).Select(x => x.A0004_ID).ToList();
                        var GroupSignCheckPass017 = db.A0021.Where(gs => StepCheckPass.Contains(gs.A0020_ID) == true && gs.A0018_ID == A0018_ID).Select(x => x.A0017_ID).ToList();
                        var UserPass = db.A0019.Where(c => GroupSignCheckPass004.Contains(c.A0004_ID) == true && GroupSignCheckPass017.Contains(c.A0017_ID) == true && c.A0002_ID == obj.A0002_ID).OrderBy(c => c.STT).FirstOrDefault();
                        if (UserPass != null)
                        {
                            var A0017_ID = UserPass.A0017_ID;
                            var PassA0021 = db.A0021.FirstOrDefault(x => x.A0017_ID == A0017_ID && x.A0018_ID == A0018_ID);
                            var SteepPass = db.A0020.FirstOrDefault(x => x.A0020_ID == PassA0021.A0020_ID);
                            if (SteepPass != null)
                            {
                                SteepSign = SteepSign.Where(x => x.STT >= SteepPass.STT);
                            }
                        }

                        var STSign = SteepSign.ToList();
                        for (int i = 0; i < STSign.Count; i++)
                        {
                            if (i == 0)
                            {
                                var a0029 = new A0029();
                                a0029.A0029_ID = helper.GenKey();
                                a0029.A0028_ID = obj.A0028_ID;
                                a0029.A0002_ID = user.A0002_ID;
                                a0029.A0017_ID = null;
                                a0029.A0020_ID = STSign[i].A0020_ID;
                                a0029.tenNhomKy = "Sign by";
                                a0029.tenViTri = STSign[i].tenBuocKy;
                                a0029.daKy = false;
                                a0029.thuThu = 0;
                                a0029.thoiGianGui = null;
                                a0029.viTriHienTai = false;
                                a0029.daXem = false;
                                db.A0029.Add(a0029);
                            }
                            else
                            {
                                string A0020_ID = STSign[i].A0020_ID;
                                var GroupSign = db.A0021.FirstOrDefault(gs => gs.A0020_ID == A0020_ID && gs.A0018_ID == A0018_ID);
                                if (GroupSign != null)
                                {
                                    var usergs = db.A0019.Where(c => c.A0017_ID == GroupSign.A0017_ID && c.A0004_ID == GroupSign.A0004_ID).OrderBy(c => c.STT).FirstOrDefault();
                                    if (usergs != null)
                                    {
                                        var a0029 = new A0029();
                                        a0029.A0029_ID = helper.GenKey();
                                        a0029.A0028_ID = obj.A0028_ID;
                                        a0029.A0002_ID = usergs.A0002_ID;
                                        a0029.A0017_ID = usergs.A0017_ID;
                                        a0029.A0020_ID = STSign[i].A0020_ID;
                                        a0029.tenNhomKy = GroupSign.A0017.tenNhomNguoiKy;
                                        a0029.tenViTri = STSign[i].tenBuocKy;
                                        a0029.daKy = false;
                                        a0029.thuThu = STSign[i].STT;
                                        a0029.thoiGianGui = null;
                                        a0029.viTriHienTai = false;
                                        a0029.daXem = false;
                                        db.A0029.Add(a0029);
                                        if (STSign[i].STT > MaxSteepSign)
                                        {
                                            MaxSteepSign = STSign[i].STT;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (httpRequest.Files.Count > 0)
                    {
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            var a0031 = new A0031();
                            HttpPostedFile file = httpRequest.Files[i];
                            string genkey = helper.GenKey();
                            a0031.A0031_ID = helper.GenKey();
                            a0031.A0028_ID = obj.A0028_ID;
                            a0031.tenFile = file.FileName;
                            a0031.dungLuong = file.ContentLength.ToString();
                            a0031.ngayTao = DateTime.Now;
                            a0031.loaiFile = System.IO.Path.GetExtension(helper.NameToTag(file.FileName));
                            a0031.thuTu = i + 1;
                            a0031.duongDan = "/Portals/images/Users/" + genkey + file.FileName;
                            file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                            db.A0031.Add(a0031);
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateWorkDocumentCam")]
        public HttpResponseMessage R3_UpdateWorkDocumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            var tk = JsonConvert.DeserializeObject<Token>(httpRequest["tk"]);
            var a0028 = JsonConvert.DeserializeObject<A0028>(httpRequest["a0028"]);
            var a0028D = JsonConvert.DeserializeObject<List<A0028D>>(httpRequest["a0028D"]);
            var a0034 = JsonConvert.DeserializeObject<List<A0034>>(httpRequest["a0034"]);

            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var a0015 = db.A0016.FirstOrDefault(x => x.maCongViec == "CV006");
                try
                {
                    #region ADD A0028
                    var MaxSteepSign = 1;
                    var obj = db.A0028.FirstOrDefault(x => x.A0028_ID == a0028.A0028_ID);
                    obj.A0002_ID = tk.Users_ID;
                    obj.A0016_ID = a0015.A0016_ID;
                    obj.A0032_ID = a0028.A0032_ID;
                    obj.trangThaiNguoiTao = a0028.trangThaiNguoiTao;
                    obj.trangThai = 1;
                    obj.viTriHienTaiUserID = tk.Users_ID;
                    obj.daXoa = false;
                    obj.maForm = a0028.maForm;
                    obj.T001C = a0028.T001C;
                    obj.T002C = a0028.T002C;
                    obj.T003C = a0028.T003C;
                    obj.T004C = a0028.T004C;
                    obj.T005C = a0028.T005C;
                    obj.T006C = a0028.T006C;
                    obj.T007C = a0028.T007C;
                    obj.T008C = a0028.T008C;
                    obj.T009C = a0028.T009C;
                    obj.T010C = a0028.T010C;
                    obj.T011C = a0028.T011C;
                    obj.T012C = a0028.T012C;
                    obj.T013C = a0028.T013C;
                    obj.T014C = a0028.T014C;
                    obj.T015C = a0028.T015C;
                    obj.T016C = a0028.T016C;
                    obj.T017C = a0028.T017C;
                    obj.T018C = a0028.T018C;
                    obj.T019C = a0028.T019C;
                    obj.T020C = a0028.T020C;
                    obj.T021C = a0028.T021C;
                    obj.T022C = a0028.T022C;
                    obj.T023C = a0028.T023C;
                    obj.T024C = a0028.T024C;
                    obj.T025C = a0028.T025C;
                    obj.T026C = a0028.T026C;
                    obj.T027C = a0028.T027C;
                    obj.T028C = a0028.T028C;
                    obj.T029C = a0028.T029C;
                    obj.T030C = a0028.T030C;
                    obj.T031C = a0028.T031C;
                    obj.T032C = a0028.T032C;
                    obj.T033C = a0028.T033C;
                    obj.T034C = a0028.T034C;
                    obj.T035C = a0028.T035C;
                    obj.T036C = a0028.T036C;
                    obj.T037C = a0028.T037C;
                    obj.T038C = a0028.T038C;
                    obj.T039C = a0028.T039C;
                    obj.T040C = a0028.T040C;
                    obj.T041C = a0028.T041C;
                    obj.T042C = a0028.T042C;
                    obj.T043C = a0028.T043C;
                    obj.T044C = a0028.T044C;
                    obj.T045C = a0028.T045C;
                    obj.T046C = a0028.T046C;
                    obj.T047C = a0028.T047C;
                    obj.T048C = a0028.T048C;
                    obj.T049C = a0028.T049C;
                    obj.T050C = a0028.T050C;
                    obj.T051C = a0028.T051C;
                    obj.T052C = a0028.T052C;
                    obj.T053C = a0028.T053C;
                    obj.T054C = a0028.T054C;
                    obj.T055C = a0028.T055C;
                    obj.T056C = a0028.T056C;
                    obj.T057C = a0028.T057C;
                    obj.T058C = a0028.T058C;
                    obj.T059C = a0028.T059C;
                    obj.T060C = a0028.T060C;
                    obj.T061C = a0028.T061C;
                    obj.T062C = a0028.T062C;
                    obj.T063C = a0028.T063C;
                    obj.T064C = a0028.T064C;
                    obj.T065C = a0028.T065C;
                    obj.T066C = a0028.T066C;
                    obj.T067C = a0028.T067C;
                    obj.T068C = a0028.T068C;
                    obj.T069C = a0028.T069C;
                    obj.T070C = a0028.T070C;
                    obj.T071C = a0028.T071C;
                    obj.T072C = a0028.T072C;
                    obj.T073C = a0028.T073C;
                    obj.T074C = a0028.T074C;
                    obj.T075C = a0028.T075C;
                    obj.T076C = a0028.T076C;
                    obj.T077C = a0028.T077C;
                    obj.T078C = a0028.T078C;
                    obj.T079C = a0028.T079C;
                    obj.T080C = a0028.T080C;
                    obj.T081C = a0028.T081C;
                    obj.T082C = a0028.T082C;
                    obj.T083C = a0028.T083C;
                    obj.T084C = a0028.T084C;
                    obj.T085C = a0028.T085C;
                    obj.T086C = a0028.T086C;
                    obj.T087C = a0028.T087C;
                    obj.T088C = a0028.T088C;
                    obj.T089C = a0028.T089C;
                    obj.T090C = a0028.T090C;
                    obj.T091C = a0028.T091C;
                    obj.T092C = a0028.T092C;
                    obj.T093C = a0028.T093C;
                    obj.T094C = a0028.T094C;
                    obj.T095C = a0028.T095C;
                    obj.T096C = a0028.T096C;
                    obj.T097C = a0028.T097C;
                    obj.T098C = a0028.T098C;
                    obj.T099C = a0028.T099C;
                    obj.T100C = a0028.T100C;
                    #endregion

                    var a0028DList = db.A0028D.Where(x => x.A0028_ID == a0028.A0028_ID).ToList();
                    if (a0028DList.Count() > 0)
                    {
                        db.A0028D.RemoveRange(a0028DList);
                    }

                    var ListA0029 = db.A0029.Where(x => x.A0028_ID == obj.A0028_ID).ToList();
                    if (ListA0029.Count() > 0)
                    {
                        db.A0029.RemoveRange(ListA0029);
                    }

                    var ListA0037 = db.A0037.Where(x => x.A0028_ID == obj.A0028_ID).ToList();
                    if (ListA0037.Count() > 0)
                    {
                        db.A0037.RemoveRange(ListA0037);
                    }

                    var ListA0039 = db.A0039.Where(x => x.A0028_ID == obj.A0028_ID).ToList();
                    if (ListA0039.Count() > 0)
                    {
                        db.A0039.RemoveRange(ListA0039);
                    }

                    if (a0028D != null)
                    {
                        foreach (var item in a0028D)
                        {
                            #region ADD A0028D
                            var objD = new A0028D();
                            objD.A0028_ID = obj.A0028_ID;
                            objD.A0028D_ID = helper.GenKey();
                            objD.IsPostion = item.IsPostion;
                            objD.C001C = item.C001C;
                            objD.C002C = item.C002C;
                            objD.C003C = item.C003C;
                            objD.C004C = item.C004C;
                            objD.C005C = item.C005C;
                            objD.C006C = item.C006C;
                            objD.C007C = item.C007C;
                            objD.C008C = item.C008C;
                            objD.C009C = item.C009C;
                            objD.C010C = item.C010C;
                            objD.C011C = item.C011C;
                            objD.C012C = item.C012C;
                            objD.C013C = item.C013C;
                            objD.C014C = item.C014C;
                            objD.C015C = item.C015C;
                            objD.C016C = item.C016C;
                            objD.C017C = item.C017C;
                            objD.C018C = item.C018C;
                            objD.C019C = item.C019C;
                            objD.C020C = item.C020C;
                            objD.C021C = item.C021C;
                            objD.C022C = item.C022C;
                            objD.C023C = item.C023C;
                            objD.C024C = item.C024C;
                            objD.C025C = item.C025C;
                            objD.C026C = item.C026C;
                            objD.C027C = item.C027C;
                            objD.C028C = item.C028C;
                            objD.C029C = item.C029C;
                            objD.C030C = item.C030C;
                            objD.C031C = item.C031C;
                            objD.C032C = item.C032C;
                            objD.C033C = item.C033C;
                            objD.C034C = item.C034C;
                            objD.C035C = item.C035C;
                            objD.C036C = item.C036C;
                            objD.C037C = item.C037C;
                            objD.C038C = item.C038C;
                            objD.C039C = item.C039C;
                            objD.C040C = item.C040C;
                            db.A0028D.Add(objD);
                            #endregion
                        }
                    }

                    foreach (var item in a0034)
                    {
                        var obj037 = new A0037();
                        obj037.A0037_ID = helper.GenKey();
                        obj037.A0002_ID = tk.Users_ID;
                        obj037.A0028_ID = obj.A0028_ID;
                        obj037.A0034_ID = item.A0034_ID;
                        obj037.tenBoPhan = item.tenPhongBan;
                        obj037.trangThai = 1;
                        obj037.ngayTao = DateTime.Now;
                        obj037.viTriHienTaiMenuID = "";
                        obj037.viTriHienTaiUserID = tk.Users_ID;
                        obj037.IsEdit = true;
                        db.A0037.Add(obj037);

                        var a0036 = db.A0036.Where(x => x.A0034_ID == item.A0034_ID).Select(x => x.A0035_ID).ToList();
                        var a0039List = a0028D.Where(x => a0036.Contains(x.C001C)).ToList();
                        foreach (var item35 in a0039List)
                        {
                            var a0039 = new A0039();
                            a0039.A0039_ID = helper.GenKey();
                            a0039.A0028_ID = item35.A0028_ID;
                            a0039.A0028D_ID = item35.A0028D_ID;
                            a0039.A0037_ID = obj037.A0037_ID;
                            a0039.trangThai = 0;
                            db.A0039.Add(a0039);
                        }
                    }

                    var user = db.A0002.Find(obj.A0002_ID);
                    var A0015_ID = a0015.A0015_ID;
                    var A0004_ID = user.A0004_ID;

                    var Process = db.A0018.OrderBy(x => x.STT).FirstOrDefault(x => x.A0015_ID == A0015_ID && x.trangThai == true && x.A0004_ID == A0004_ID);
                    if (Process == null)
                    {
                        Process = db.A0018.OrderBy(x => x.STT).FirstOrDefault(x => x.A0015_ID == A0015_ID && x.trangThai == true && x.A0004_ID == null);
                    }

                    var SteepSign = db.A0020.Where(s => s.trangThai == true).OrderBy(s => s.STT).Select(s => new
                    {
                        s.A0020_ID,
                        s.tenBuocKy,
                        s.AliasBuocKy,
                        s.STT
                    });

                    if (Process != null)
                    {
                        string A0018_ID = Process.A0018_ID;
                        var StepCheckPass = SteepSign.Select(x => x.A0020_ID).ToList();
                        var GroupSignCheckPass004 = db.A0021.Where(gs => StepCheckPass.Contains(gs.A0020_ID) == true && gs.A0018_ID == A0018_ID).Select(x => x.A0004_ID).ToList();
                        var GroupSignCheckPass017 = db.A0021.Where(gs => StepCheckPass.Contains(gs.A0020_ID) == true && gs.A0018_ID == A0018_ID).Select(x => x.A0017_ID).ToList();
                        var UserPass = db.A0019.Where(c => GroupSignCheckPass004.Contains(c.A0004_ID) == true && GroupSignCheckPass017.Contains(c.A0017_ID) == true && c.A0002_ID == a0028.A0002_ID).OrderBy(c => c.STT).FirstOrDefault();
                        if (UserPass != null)
                        {
                            var A0017_ID = UserPass.A0017_ID;
                            var PassA0021 = db.A0021.FirstOrDefault(x => x.A0017_ID == A0017_ID && x.A0018_ID == A0018_ID);
                            var SteepPass = db.A0020.FirstOrDefault(x => x.A0020_ID == PassA0021.A0020_ID);
                            if (SteepPass != null)
                            {
                                SteepSign = SteepSign.Where(x => x.STT >= SteepPass.STT);
                            }
                        }

                        var STSign = SteepSign.ToList();
                        for (int i = 0; i < STSign.Count; i++)
                        {
                            if (i == 0)
                            {
                                var a0029 = new A0029();
                                a0029.A0029_ID = helper.GenKey();
                                a0029.A0028_ID = obj.A0028_ID;
                                a0029.A0002_ID = user.A0002_ID;
                                a0029.A0017_ID = null;
                                a0029.A0020_ID = STSign[i].A0020_ID;
                                a0029.tenNhomKy = "Sign by";
                                a0029.tenViTri = STSign[i].tenBuocKy;
                                a0029.daKy = false;
                                a0029.thuThu = 0;
                                a0029.thoiGianGui = null;
                                a0029.viTriHienTai = false;
                                a0029.daXem = false;
                                db.A0029.Add(a0029);
                            }
                            else
                            {
                                string A0020_ID = STSign[i].A0020_ID;
                                var GroupSign = db.A0021.FirstOrDefault(gs => gs.A0020_ID == A0020_ID && gs.A0018_ID == A0018_ID);
                                if (GroupSign != null)
                                {
                                    var usergs = db.A0019.Where(c => c.A0017_ID == GroupSign.A0017_ID && c.A0004_ID == GroupSign.A0004_ID).OrderBy(c => c.STT).FirstOrDefault();
                                    if (usergs != null)
                                    {
                                        var a0029 = new A0029();
                                        a0029.A0029_ID = helper.GenKey();
                                        a0029.A0028_ID = obj.A0028_ID;
                                        a0029.A0002_ID = usergs.A0002_ID;
                                        a0029.A0017_ID = usergs.A0017_ID;
                                        a0029.A0020_ID = STSign[i].A0020_ID;
                                        a0029.tenNhomKy = GroupSign.A0017.tenNhomNguoiKy;
                                        a0029.tenViTri = STSign[i].tenBuocKy;
                                        a0029.daKy = false;
                                        a0029.thuThu = STSign[i].STT;
                                        a0029.thoiGianGui = null;
                                        a0029.viTriHienTai = false;
                                        a0029.daXem = false;
                                        db.A0029.Add(a0029);
                                        if (STSign[i].STT > MaxSteepSign)
                                        {
                                            MaxSteepSign = STSign[i].STT;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (httpRequest.Files.Count > 0)
                    {
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            var a0031 = new A0031();
                            HttpPostedFile file = httpRequest.Files[i];
                            string genkey = helper.GenKey();
                            a0031.A0031_ID = helper.GenKey();
                            a0031.A0028_ID = obj.A0028_ID;
                            a0031.tenFile = file.FileName;
                            a0031.dungLuong = file.ContentLength.ToString();
                            a0031.ngayTao = DateTime.Now;
                            a0031.loaiFile = System.IO.Path.GetExtension(helper.NameToTag(file.FileName));
                            a0031.thuTu = i + 1;
                            a0031.duongDan = "/Portals/images/Users/" + genkey + file.FileName;
                            file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                            db.A0031.Add(a0031);
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteDoccumentCam")]
        public async Task<HttpResponseMessage> R4_DeleteDoccumentCam()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0028_ID = httpRequest["A0028_ID"];
                    db.Configuration.LazyLoadingEnabled = false;
                    var obj = db.A0028.FirstOrDefault(x => x.A0028_ID == A0028_ID);
                    if (obj != null)
                    {
                        obj.daXoa = true;
                    }
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_SignAndSend")]
        public HttpResponseMessage R2_SignAndSend()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string trangthaigui = "";
                    string nguoinhan = "";
                    string nguoigui = "";
                    string emailnguoinhan = "";
                    string A0028_ID = httpRequest["A0028_ID"];
                    string A0002_ID = httpRequest["A0002_ID"];
                    string Note = httpRequest["Note"];
                    var ListEmail = new List<string>();
                    var doc = db.A0028.FirstOrDefault(x => x.A0028_ID == A0028_ID);
                    if (doc == null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    }

                    var nsform = db.A0002.Find(A0002_ID);
                    if (nsform != null)
                    {
                        nguoigui = nsform.hoVaTen;
                    }
                    var Docsign = db.A0029.Where(x => x.A0028_ID == A0028_ID && x.daKy == false).OrderBy(x => x.thuThu).ToList();
                    var DocSignCheck = db.A0029.Where(x => x.A0028_ID == A0028_ID && x.daKy == true).OrderBy(x => x.thuThu).ToList();
                    if (Docsign.Count > 0)
                    {
                        var CurrentSign = Docsign[0];
                        var obj = db.A0029.FirstOrDefault(x => x.A0029_ID == CurrentSign.A0029_ID && x.daKy == false);
                        if (obj != null)
                        {
                            obj.daKy = true;
                            obj.thoiGianGui = DateTime.Now;

                            int maxOrder = 1;
                            var docmax = db.A0030.Where(x => x.A0028_ID == doc.A0028_ID).OrderByDescending(x => x.thuThu).ToList();
                            if (docmax.Count > 0)
                            {
                                maxOrder = docmax[0].thuThu + 1;
                            }
                            var objlog = new A0030();
                            objlog.A0030_ID = helper.GenKey();
                            objlog.A0028_ID = doc.A0028_ID;
                            objlog.A0002_ID = Docsign[0].A0002_ID;
                            objlog.tieuDe = "Ký Duyệt Gửi Báo Phế";
                            objlog.noiDung = Note;
                            objlog.thuThu = maxOrder;
                            objlog.trangThai = 1;
                            objlog.thoiGianGui = DateTime.Now;
                            maxOrder = maxOrder + 1;
                            db.A0030.Add(objlog);

                            Docsign.Remove(CurrentSign);
                            if (Docsign.Count > 0)
                            {
                                doc.viTriHienTaiUserID = Docsign[0].A0002_ID;
                                doc.viTriHienTaiMenuID = "M04.04";
                                doc.trangThai = 1;

                                var nsto = db.A0002.Find(Docsign[0].A0002_ID);
                                if (nsto != null)
                                {
                                    nguoinhan = nsto.hoVaTen;
                                    emailnguoinhan = nsto.Email;
                                }
                                trangthaigui = "Đã gửi Form tới ";
                            }
                            else
                            {
                                nguoinhan = "";
                                trangthaigui = "Đã gửi Form tới các bộ phận xử lý";

                                doc.viTriHienTaiUserID = A0002_ID;
                                doc.viTriHienTaiMenuID = "M04.08";
                                doc.trangThai = 1;
                                var objcompleted = new A0030();
                                objcompleted.A0030_ID = helper.GenKey();
                                objcompleted.A0028_ID = doc.A0028_ID;
                                objcompleted.A0002_ID = A0002_ID;
                                objcompleted.tieuDe = "Duyệt Báo Phế Chuyển Đến Các Bộ Phận";
                                objcompleted.noiDung = "Duyệt Báo Phế Chuyển Đến Các Bộ Phận";
                                objcompleted.thuThu = maxOrder;
                                objcompleted.trangThai = 2;
                                objcompleted.thoiGianGui = DateTime.Now;
                                db.A0030.Add(objcompleted);

                                var DocBPBoPhan = db.A0037.Where(x => x.A0028_ID == doc.A0028_ID).ToList();
                                foreach (var item in DocBPBoPhan)
                                {
                                    var GroupSign = db.A0040.FirstOrDefault(x => x.A0034_ID == item.A0034_ID);
                                    #region Add nhóm ký bộ phận
                                    if (GroupSign != null && GroupSign.nguoiKy1 != null && GroupSign.nguoiKy1 != "")
                                    {
                                        var docBP = db.A0037.Find(item.A0037_ID);
                                        docBP.viTriHienTaiUserID = GroupSign.nguoiKy1;
                                        docBP.viTriHienTaiMenuID = "M04.06";
                                        docBP.A0002_ID = A0002_ID;

                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy1;
                                        a0038.viTri = 1;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy1;
                                        a0038.viTriHienTai = true;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 1;
                                        db.A0038.Add(a0038);
                                        ListEmail.Add(GroupSign.MailNguoiKy1);
                                    }

                                    if (GroupSign != null && GroupSign.nguoiKy2 != null && GroupSign.nguoiKy2 != "")
                                    {
                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy2;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy2;
                                        a0038.viTri = 2;
                                        a0038.viTriHienTai = false;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 2;                                        
                                        db.A0038.Add(a0038);
                                        if(ListEmail.Count() == 0)
                                        {
                                            ListEmail.Add(GroupSign.MailNguoiKy2);
                                        }
                                    }

                                    if (GroupSign != null && GroupSign.nguoiKy3 != null && GroupSign.nguoiKy3 != "")
                                    {
                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy3;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy3;
                                        a0038.viTri = 3;
                                        a0038.viTriHienTai = false;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 3;
                                        db.A0038.Add(a0038);
                                        if (ListEmail.Count() == 0)
                                        {
                                            ListEmail.Add(GroupSign.MailNguoiKy3);
                                        }
                                    }

                                    if (GroupSign != null && GroupSign.nguoiKy4 != null && GroupSign.nguoiKy4 != "")
                                    {
                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy4;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy4;
                                        a0038.viTri = 4;
                                        a0038.viTriHienTai = false;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 4;
                                        db.A0038.Add(a0038);
                                        if (ListEmail.Count() == 0)
                                        {
                                            ListEmail.Add(GroupSign.MailNguoiKy4);
                                        }
                                    }

                                    if (GroupSign != null && GroupSign.nguoiKy5 != null && GroupSign.nguoiKy5 != "")
                                    {
                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy5;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy5;
                                        a0038.viTri = 5;
                                        a0038.viTriHienTai = false;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 5;
                                        db.A0038.Add(a0038);
                                        if (ListEmail.Count() == 0)
                                        {
                                            ListEmail.Add(GroupSign.MailNguoiKy5);
                                        }
                                    }

                                    if (GroupSign != null && GroupSign.nguoiKy6 != null && GroupSign.nguoiKy6 != "")
                                    {
                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy6;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy6;
                                        a0038.viTri = 6;
                                        a0038.viTriHienTai = false;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 6;
                                        db.A0038.Add(a0038);
                                        if (ListEmail.Count() == 0)
                                        {
                                            ListEmail.Add(GroupSign.MailNguoiKy6);
                                        }
                                    }

                                    if (GroupSign != null && GroupSign.nguoiKy7 != null && GroupSign.nguoiKy7 != "")
                                    {
                                        var a0038 = new A0038();
                                        a0038.A0038_ID = helper.GenKey();
                                        a0038.A0028D_ID = doc.A0028_ID;
                                        a0038.A0037_ID = item.A0037_ID;
                                        a0038.A0002_ID = GroupSign.nguoiKy7;
                                        a0038.mailNguoiKy = GroupSign.MailNguoiKy7;
                                        a0038.viTri = 7;
                                        a0038.viTriHienTai = false;
                                        a0038.daKy = false;
                                        a0038.thoiGianKy = null;
                                        a0038.thuTu = 7;
                                        db.A0038.Add(a0038);
                                        if (ListEmail.Count() == 0)
                                        {
                                            ListEmail.Add(GroupSign.MailNguoiKy7);
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    db.SaveChanges();

                    Thread email = new Thread(delegate ()
                    {
                        if (nguoinhan != "" && emailnguoinhan != "")
                        {
                            Sendmail_CurrentSign(emailnguoinhan, A0028_ID, trangthaigui + nguoinhan, nguoigui, "BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX", Note);
                        }
                        Sendmail_Gmail(A0028_ID, trangthaigui + nguoinhan, nguoigui, "BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX", Note);
                        if(ListEmail.Count() > 0)
                        {
                            Sendmail_GmailBP(ListEmail, A0028_ID, nguoigui.ToUpper() + " Đã gửi tới bạn yêu cầu báo phế", nguoigui, "BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX", Note);
                        }
                    });
                    email.IsBackground = true;
                    email.Start();
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [Route("R1_WaitingConfirmDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_WaitingConfirmDocument()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var tables = db.A0037.Where(x => x.trangThai == 1 && x.viTriHienTaiMenuID == "M04.06" && x.viTriHienTaiUserID == A0002_ID).Select(x => new
                    {
                        x.A0037_ID,
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0028.A0016.tenCongViec,
                        x.A0034_ID,
                        x.tenBoPhan,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiUserID,
                        x.ngayHoanThanh,
                        x.IsEdit,
                        x.A0028.T001C,
                        x.A0028.T002C,
                        x.A0028.T003C,
                        x.A0028.T004C,
                        x.A0028.T005C,
                        x.A0028.T006C,
                        x.A0028.T007C,
                        x.A0028.T008C,
                        x.A0028.T009C,
                        x.A0028.T010C,
                        x.A0028.T011C,
                        x.A0028.T012C,
                        x.A0028.T013C,
                        x.A0028.T014C,
                        x.A0028.T015C,
                        x.A0028.T016C,
                        x.A0028.T017C,
                        x.A0028.T018C,
                        x.A0028.T019C,
                        x.A0028.T020C,
                        x.A0028.T021C,
                        x.A0028.T022C,
                        x.A0028.T023C,
                        x.A0028.T024C,
                        x.A0028.T025C,
                        CountSign = x.A0038.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0038.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_CompletedDocumentConfirm")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_CompletedDocumentConfirm()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var UserDocSign = db.A0038.Where(x => x.A0002_ID == A0002_ID).Select(x => x.A0037_ID).Distinct().ToList();
                    var tables = db.A0037.Where(x => x.trangThai == 2 && UserDocSign.Contains(x.A0037_ID) == true && x.viTriHienTaiMenuID == "M04.07").Select(x => new
                    {
                        x.A0037_ID,
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0028.A0016.tenCongViec,
                        x.A0034_ID,
                        x.tenBoPhan,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiUserID,
                        x.ngayHoanThanh,
                        x.A0028.T001C,
                        x.A0028.T002C,
                        x.A0028.T003C,
                        x.A0028.T004C,
                        x.A0028.T005C,
                        x.A0028.T006C,
                        x.A0028.T007C,
                        x.A0028.T008C,
                        x.A0028.T009C,
                        x.A0028.T010C,
                        x.A0028.T011C,
                        x.A0028.T012C,
                        x.A0028.T013C,
                        x.A0028.T014C,
                        x.A0028.T015C,
                        x.A0028.T016C,
                        x.A0028.T017C,
                        x.A0028.T018C,
                        x.A0028.T019C,
                        x.A0028.T020C,
                        x.A0028.T021C,
                        x.A0028.T022C,
                        x.A0028.T023C,
                        x.A0028.T024C,
                        x.A0028.T025C,
                        CountSign = x.A0038.Where(a => a.daKy == true).Count(),
                        Countprocess = x.A0038.Count()
                    });

                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        sort = "ngayTao";
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
                    var qr = tables.OrderBy(x => x.ngayTao).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, count = tables.Count() }));
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
        [Route("R1_MyWorkDocumentDetailConfirm")]
        public HttpResponseMessage R1_MyWorkDocumentDetailConfirm()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0028_ID = httpRequest["A0028_ID"];
                    string A0037_ID = httpRequest["A0037_ID"];
                    string A0034_ID = httpRequest["A0034_ID"];

                    var A0036List = db.A0036.Where(x => x.A0034_ID == A0034_ID).Select(x => x.A0035_ID).ToList();
                    var tables = db.A0028.Where(x => x.A0028_ID == A0028_ID).Select(
                    x => new
                    {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        x.A0016_ID,
                        x.A0032_ID,
                        x.maForm,
                        x.A0016.maCongViec,
                        x.A0016.tenCongViec,
                        x.trangThaiNguoiTao,
                        x.trangThai,
                        x.ngayTao,
                        x.ngayXuLyMongMuon,
                        x.viTriHienTaiMenuID,
                        x.viTriHienTaiNhomKyID,
                        x.viTriHienTaiUserID,
                        x.T001C,
                        x.T002C,
                        x.T003C,
                        x.T004C,
                        x.T005C,
                        x.T006C,
                        x.T007C,
                        x.T008C,
                        x.T009C,
                        x.T010C,
                        x.T011C,
                        x.T012C,
                        x.T013C,
                        x.T014C,
                        x.T015C,
                        x.T016C,
                        x.T017C,
                        x.T018C,
                        x.T019C,
                        x.T020C,
                        x.T021C,
                        x.T022C,
                        x.T023C,
                        x.T024C,
                        x.T025C,
                        x.T026C,
                        x.T027C,
                        x.T028C,
                        x.T029C,
                        x.T030C,
                        x.T031C,
                        x.T032C,
                        x.T033C,
                        x.T034C,
                        x.T035C,
                        x.T036C,
                        x.T037C,
                        x.T038C,
                        x.T039C,
                        x.T040C,
                        x.T041C,
                        x.T042C,
                        x.T043C,
                        x.T044C,
                        x.T045C,
                        x.T046C,
                        x.T047C,
                        x.T048C,
                        x.T049C,
                        x.T050C,
                        x.T051C,
                        x.T052C,
                        x.T053C,
                        x.T054C,
                        x.T055C,
                        x.T056C,
                        x.T057C,
                        x.T058C,
                        x.T059C,
                        x.T060C,
                        x.T061C,
                        x.T062C,
                        x.T063C,
                        x.T064C,
                        x.T065C,
                        x.T066C,
                        x.T067C,
                        x.T068C,
                        x.T069C,
                        x.T070C,
                        x.T071C,
                        x.T072C,
                        x.T073C,
                        x.T074C,
                        x.T075C,
                        x.T076C,
                        x.T077C,
                        x.T078C,
                        x.T079C,
                        x.T080C,
                        x.T081C,
                        x.T082C,
                        x.T083C,
                        x.T084C,
                        x.T085C,
                        x.T086C,
                        x.T087C,
                        x.T088C,
                        x.T089C,
                        x.T090C,
                        x.T091C,
                        x.T092C,
                        x.T093C,
                        x.T094C,
                        x.T095C,
                        x.T096C,
                        x.T097C,
                        x.T098C,
                        x.T099C,
                        x.T100C
                    }).ToList();

                    var tables2 = db.A0028D.Where(x => x.A0028_ID == A0028_ID && A0036List.Contains(x.C008C) == true).Select(x => new
                    {
                        x.A0028D_ID,
                        x.A0028_ID,
                        x.IsPostion,
                        x.C001C,
                        x.C002C,
                        x.C003C,
                        x.C004C,
                        x.C005C,
                        x.C006C,
                        x.C007C,
                        x.C008C,
                        x.C009C,
                        x.C010C,
                        x.C011C,
                        x.C012C,
                        x.C013C,
                        x.C014C,
                        x.C015C,
                        x.C016C,
                        x.C017C,
                        x.C018C,
                        x.C019C,
                        x.C020C,
                        x.C021C,
                        x.C022C,
                        x.C023C,
                        x.C024C,
                        x.C025C,
                        x.C026C,
                        x.C027C,
                        x.C028C,
                        x.C029C,
                        x.C030C,
                        x.C031C,
                        x.C032C,
                        x.C033C,
                        x.C034C,
                        x.C035C,
                        x.C036C,
                        x.C037C,
                        x.C038C,
                        x.C039C,
                        x.C040C
                    }).OrderBy(x => x.C009C).GroupBy(x => x.C001C).Select(x => new { C001C = x.Key, C009C = x.Select(c => c.C009C).FirstOrDefault(), ListDMBaoPhe = x.OrderBy(c => c.C010C) }).OrderBy(x => x.C009C).ToList();

                    var tables3 = db.A0034.Where(x => x.A0034_ID == A0034_ID).Select(x => new
                    {
                        x.A0034_ID,
                        x.Parent_ID,
                        x.phongBanMapID,
                        x.maPhongBan,
                        x.tenPhongBan,
                        x.kieuPhongBan,
                        x.trangThai,
                        x.STT
                    }).ToList();

                    var tables4 = db.A0039.Where(x => x.A0028_ID == A0028_ID && x.A0037_ID == A0037_ID).Select(x => new
                    {
                        x.A0039_ID,
                        x.A0028_ID,
                        x.A0028D_ID,
                        x.A0037_ID,
                        x.ngayXuLy,
                        x.nguoiXuLy,
                        x.noiDungXuLy,
                        x.trangThai
                    }).ToList();

                    var WorkFile = db.A0031.Where(x => x.A0028_ID == A0028_ID).Select(x => new
                    {
                        x.A0031_ID,
                        x.A0028_ID,
                        x.tenFile,
                        x.dungLuong,
                        x.duongDan,
                        x.ngayTao,
                        x.loaiFile,
                        x.thuTu
                    }).OrderBy(x => x.thuTu).ToList();

                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables, data2 = tables2, data3 = tables3, data4 = tables4, data5 = WorkFile }));
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
        [Route("R2_SignAndSendBPConfirm")]
        public HttpResponseMessage R2_SignAndSendBPConfirm()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0028_ID = httpRequest["A0028_ID"];
                    string A0037_ID = httpRequest["A0037_ID"];
                    string A0002_ID = httpRequest["A0002_ID"];
                    string Note = httpRequest["Note"];
                    string trangthaigui = "";
                    string nguoinhan = "";
                    string nguoigui = "";
                    string emailnguoinhan = "";

                    var doc = db.A0037.FirstOrDefault(x => x.A0037_ID == A0037_ID);
                    if (doc == null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    }

                    var Docsign = db.A0038.Where(x => x.A0037_ID == A0037_ID && x.daKy == false).OrderBy(x => x.thuTu).ToList();
                    var DocSignCheck = db.A0038.Where(x => x.A0037_ID == A0037_ID && x.daKy == true).OrderBy(x => x.thuTu).ToList();
                    if (Docsign.Count > 0)
                    {
                        var CurrentSign = Docsign[0];
                        var obj = db.A0038.FirstOrDefault(x => x.A0038_ID == CurrentSign.A0038_ID && x.daKy == false);
                        if (obj != null)
                        {
                            obj.daKy = true;
                            obj.thoiGianKy = DateTime.Now;
                            //int maxOrder = 1;
                            //var docmax = db.A0030.Where(x => x.A0028_ID == doc.A0028_ID).OrderByDescending(x => x.thuThu).ToList();
                            //if (docmax.Count > 0)
                            //{
                            //    maxOrder = docmax[0].thuThu + 1;
                            //}
                            //var objlog = new A0030();
                            //objlog.A0030_ID = helper.GenKey();
                            //objlog.A0028_ID = doc.A0028_ID;
                            //objlog.A0002_ID = Docsign[0].A0002_ID;
                            //objlog.tieuDe = "Ký Duyệt Gửi Báo Phế";
                            //objlog.noiDung = Note;
                            //objlog.thuThu = maxOrder;
                            //objlog.trangThai = 1;
                            //objlog.thoiGianGui = DateTime.Now;
                            //maxOrder = maxOrder + 1;
                            //db.A0030.Add(objlog);

                            var nsform = db.A0002.Find(A0002_ID);
                            if (nsform != null)
                            {
                                nguoigui = nsform.hoVaTen;
                            }

                            Docsign.Remove(CurrentSign);
                            if (Docsign.Count > 0)
                            {
                                doc.viTriHienTaiUserID = Docsign[0].A0002_ID;
                                doc.viTriHienTaiMenuID = "M04.06";
                                doc.trangThai = 1;
                                doc.IsEdit = false;

                                var nsto = db.A0002.Find(Docsign[0].A0002_ID);
                                if (nsto != null)
                                {
                                    nguoinhan = nsto.hoVaTen;
                                    emailnguoinhan = Docsign[0].mailNguoiKy;
                                }
                                trangthaigui = "Đã gửi yêu cầu báo phế tới ";
                            }
                            else
                            {
                                doc.viTriHienTaiUserID = A0002_ID;
                                doc.viTriHienTaiMenuID = "M04.07";
                                doc.trangThai = 2;
                                doc.IsEdit = false;

                                nguoinhan = "";
                                trangthaigui = "Hoàn thành báo phế bộ phận " + doc.tenBoPhan;

                                int maxOrder = 1;
                                var docmax = db.A0030.Where(x => x.A0028_ID == doc.A0028_ID).OrderByDescending(x => x.thuThu).ToList();
                                if (docmax.Count > 0)
                                {
                                    maxOrder = docmax[0].thuThu + 1;
                                }

                                var objnote = new A0030();
                                objnote.A0030_ID = helper.GenKey();
                                objnote.A0028_ID = doc.A0028_ID;
                                objnote.A0002_ID = A0002_ID;
                                objnote.tieuDe = "Hoàn thành báo phế bộ phận " + doc.tenBoPhan;
                                objnote.noiDung = "Hoàn thành báo phế bộ phận " + doc.tenBoPhan;
                                objnote.thuThu = maxOrder;
                                objnote.trangThai = 2;
                                objnote.thoiGianGui = DateTime.Now;
                                db.A0030.Add(objnote);
                                maxOrder = maxOrder + 1;

                                var checkDocA037 = db.A0037.Count(x => x.A0028_ID == doc.A0028_ID);
                                var checkDocA037Sucess = db.A0037.Count(x => (x.A0028_ID == doc.A0028_ID && x.trangThai == 2) || (x.A0037_ID == A0037_ID));
                                if (checkDocA037 == checkDocA037Sucess)
                                {
                                    var docmaster = db.A0028.FirstOrDefault(x => x.A0028_ID == doc.A0028_ID);
                                    docmaster.viTriHienTaiUserID = A0002_ID;
                                    docmaster.viTriHienTaiMenuID = "M04.05";
                                    docmaster.trangThai = 1;

                                    var objcompleted = new A0030();
                                    objcompleted.A0030_ID = helper.GenKey();
                                    objcompleted.A0028_ID = doc.A0028_ID;
                                    objcompleted.A0002_ID = A0002_ID;
                                    objcompleted.tieuDe = "Hoàn thành Form báo phế";
                                    objcompleted.noiDung = "Hoàn thành Form báo phế";
                                    objcompleted.thuThu = maxOrder;
                                    objcompleted.trangThai = 2;
                                    objcompleted.thoiGianGui = DateTime.Now;
                                    db.A0030.Add(objcompleted);
                                }

                                //var objcompleted = new A0030();
                                //objcompleted.A0030_ID = helper.GenKey();
                                //objcompleted.A0028_ID = doc.A0028_ID;
                                //objcompleted.A0002_ID = A0002_ID;
                                //objcompleted.tieuDe = "Hoàn thành xử lý báo phế";
                                //objcompleted.noiDung = "Hoàn thành xử lý báo phế";
                                //objcompleted.thuThu = maxOrder;
                                //objcompleted.trangThai = 2;
                                //objcompleted.thoiGianGui = DateTime.Now;
                                //db.A0030.Add(objcompleted);
                            }
                        }
                    }
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    db.SaveChanges();

                    Thread email = new Thread(delegate ()
                    {
                        if (nguoinhan != "" && emailnguoinhan != "")
                        {
                            Sendmail_CurrentSign(emailnguoinhan, A0028_ID, trangthaigui + nguoinhan, nguoigui, "BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX", Note);
                        }
                        Sendmail_GmailBPBP(A0037_ID, trangthaigui + nguoinhan, nguoigui, "BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX", Note);
                    });
                    email.IsBackground = true;
                    email.Start();
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_EventNoteCam/{id}")]
        public HttpResponseMessage R1_EventNoteCam(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var tables = db.A0038.Where(x => x.A0037_ID == Id).Select(x => new
                    {
                        x.A0038_ID,
                        x.A0028D_ID,
                        x.A0037_ID,
                        x.A0002_ID,
                        x.viTri,
                        x.viTriHienTai,
                        x.daKy,
                        x.thoiGianKy,
                        x.thuTu,
                        hoVaTen = db.A0002.FirstOrDefault(a => a.A0002_ID == x.A0002_ID).hoVaTen,
                        anhDaiDien = temp + db.A0002.FirstOrDefault(a => a.A0002_ID == x.A0002_ID).anhDaiDien
                    }).OrderBy(x => x.thuTu).ToList();

                    //var table2 = db.A0030.Where(x => x.A0028_ID == Id).Select(x => new
                    //{
                    //    x.A0030_ID,
                    //    x.A0002.hoVaTen,
                    //    anhDaiDien = temp + x.A0002.anhDaiDien,
                    //    x.tieuDe,
                    //    x.noiDung,
                    //    x.thoiGianGui,
                    //    x.trangThai,
                    //    x.thuThu
                    //}).OrderBy(x => x.thuThu).ToList();

                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables }));
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
        [Route("R3_UpdateWorkdocumentCamConfirm")]
        public HttpResponseMessage R3_UpdateWorkdocumentCamConfirm()
        {
            var httpRequest = HttpContext.Current.Request;
            var tk = JsonConvert.DeserializeObject<Token>(httpRequest["tk"]);
            var a0037 = JsonConvert.DeserializeObject<A0037>(httpRequest["a0037"]);
            var a0028D = JsonConvert.DeserializeObject<List<A0028D>>(httpRequest["a0028D"]);
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    if (a0028D != null)
                    {
                        foreach (var item in a0028D)
                        {
                            var obj = new A0039();
                            obj.A0039_ID = helper.GenKey();
                            obj.A0028_ID = item.A0028_ID;
                            obj.A0028D_ID = item.A0028D_ID;
                            obj.A0037_ID = a0037.A0037_ID;
                            if(item.C005C != null && item.C005C != "")
                            {
                                try
                                {
                                    obj.ngayXuLy = DateTime.Parse(item.C005C);
                                }
                                catch (Exception)
                                {
                                    
                                }                                
                            }
                            obj.nguoiXuLy = item.C006C;
                            obj.noiDungXuLy = item.C007C;
                            obj.trangThai = 1;
                            db.A0039.Add(obj);
                        }
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
        }

        #endregion

        #region Send Mail

        private string MailBody(string A0028_ID, string mdh, string UserSendCurrent, string cmtForm)
        {
            string strHTML = "";
            using (MeikoEntities db = new MeikoEntities())
            {
                var doc = db.A0028.Find(A0028_ID);
                strHTML += "<div style='display:block;position:relative;width:100%'>";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX</span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>Tạo bởi: " + doc.A0002.hoVaTen + "</span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>Ngày gửi: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>Comment From: " + UserSendCurrent + "</span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'> -------------------------------------------------------------------- </span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>" + cmtForm + "</span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>Bạn hãy vào đường link: <a href='http://192.84.100.207/smartoffice' target='_blank'>http://192.84.100.207/Smartoffice</a></span><br />";
                strHTML += "<span style='width:100%;font-size:13px;font-weight:bold;font-family:Arial;line-height: 20px;'>Đăng nhập vào hệ thống, vào Module báo phế để xem hoặc xử lý</span><br />";
                strHTML += "<div style='width:100%;font-size:13px;font-family:Arial;line-height: 16px;margin-top: 20px;display: block;position: relative;'>";
                strHTML += "***Đây là Email được gửi tự động bởi hệ thống Smart Office. Vui lòng không trả lời email này.***<br />";
                strHTML += "Nội dung email và tệp đính kèm này được gửi từ Meiko Electronics Vietnam Co., ltd cho (các) cá nhân / người được nêu tên ở trên.<br />";
                strHTML += "Nếu bạn không biết gì về nội dung này vui lòng bỏ qua email, và liên hệ thống báo với chúng tôi thông qua điện thoại<br />";
                strHTML += "Vui lòng xóa bỏ email này để không ai có thể đọc, không được chuyển tiếp hoặc sao chép, chúng tôi rất cảm ơn vì sự hợp tác của bạn. <br />";
                strHTML += "**************************************************************************************************************************************************************";
                strHTML += "</div>";
                strHTML += "</div>";
            }
            return strHTML;
        }

        public bool Sendmail_Gmail(string A0028_ID, string mdh, string UserSendCurrent, string Subject, string cmtForm)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var email = db.A0041.FirstOrDefault();
                bool checkreturn = false;
                if (Subject != null && Subject != "")
                {
                    mdh = mdh + " - " + Subject;
                }
                MailMessage mail = new MailMessage();

                var usersign = db.A0029.Where(x => x.A0028_ID == A0028_ID && x.daKy == true).OrderBy(x => x.thuThu).ToList();
                if (usersign.Count > 0)
                {
                    if (usersign.Count > 0)
                    {
                        foreach (var docsign in usersign)
                        {
                            var ns = db.A0002.Find(docsign.A0002_ID);
                            if (ns.Email != "" && ns.Email != null)
                            {
                                try
                                {
                                    mail.To.Add(ns.Email);
                                    mail.Subject = mdh;
                                    mail.IsBodyHtml = true;
                                    mail.Body = MailBody(A0028_ID, mdh, UserSendCurrent, cmtForm);
                                    mail.From = new MailAddress(email.Email);

                                    SmtpClient client = new SmtpClient();
                                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    client.EnableSsl = true;
                                    client.Host = email.SMTPIn;
                                    client.Port = email.Port;
                                    NetworkCredential credentials = new NetworkCredential(email.Email, email.PassWord);
                                    client.UseDefaultCredentials = false;
                                    client.Credentials = credentials;
                                    client.Send(mail);
                                    checkreturn = true;
                                    mail.To.Clear();
                                }
                                catch (Exception ex)
                                {
                                    checkreturn = false;
                                }
                            }
                        }
                    }
                }
                return checkreturn;
            }
        }

        public bool Sendmail_CurrentSign(string to, string A0028_ID, string mdh, string UserSendCurrent, string Subject, string cmtForm)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                bool checkreturn = false;
                try
                {
                    var email = db.A0041.FirstOrDefault();
                    if (Subject != null && Subject != "")
                    {
                        mdh = mdh + " - " + Subject;
                    }
                    MailMessage mail = new MailMessage();
                    mail.To.Add(to);
                    mail.Subject = mdh;
                    mail.IsBodyHtml = true;
                    mail.Body = MailBody(A0028_ID, mdh, UserSendCurrent, cmtForm);
                    mail.From = new MailAddress(email.Email);

                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    client.Host = email.SMTPIn;
                    client.Port = email.Port;
                    NetworkCredential credentials = new NetworkCredential(email.Email, email.PassWord);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credentials;
                    client.Send(mail);
                    checkreturn = true;
                    mail.To.Clear();
                }
                catch (Exception ex)
                {
                    checkreturn = false;
                }
                return checkreturn;
            }
        }

        public bool Sendmail_GmailBP(List<string> EmailTo,string A0028_ID, string mdh, string UserSendCurrent, string Subject, string cmtForm)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var email = db.A0041.FirstOrDefault();
                bool checkreturn = false;
                if (Subject != null && Subject != "")
                {
                    mdh = mdh + " - " + Subject;
                }
                MailMessage mail = new MailMessage();

                var usersign = db.A0029.Where(x => x.A0028_ID == A0028_ID && x.daKy == true).OrderBy(x => x.thuThu).ToList();
                if (usersign.Count > 0)
                {
                    if (usersign.Count > 0)
                    {
                        foreach (var eto in EmailTo)
                        {                            
                            if (eto != "" && eto != null)
                            {
                                try
                                {
                                    mail.To.Add(eto);
                                    mail.Subject = mdh;
                                    mail.IsBodyHtml = true;
                                    mail.Body = MailBody(A0028_ID, mdh, UserSendCurrent, cmtForm);
                                    mail.From = new MailAddress(email.Email);

                                    SmtpClient client = new SmtpClient();
                                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    client.EnableSsl = true;
                                    client.Host = email.SMTPIn;
                                    client.Port = email.Port;
                                    NetworkCredential credentials = new NetworkCredential(email.Email, email.PassWord);
                                    client.UseDefaultCredentials = false;
                                    client.Credentials = credentials;
                                    client.Send(mail);
                                    checkreturn = true;
                                    mail.To.Clear();
                                }
                                catch (Exception ex)
                                {
                                    checkreturn = false;
                                }
                            }
                        }
                    }
                }
                return checkreturn;
            }
        }

        public bool Sendmail_GmailBPBP(string A0037_ID, string mdh, string UserSendCurrent, string Subject, string cmtForm)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var email = db.A0041.FirstOrDefault();
                bool checkreturn = false;
                if (Subject != null && Subject != "")
                {
                    mdh = mdh + " - " + Subject;
                }
                MailMessage mail = new MailMessage();

                var usersign = db.A0038.Where(x => x.A0037_ID == A0037_ID && x.daKy == true).OrderBy(x => x.thuTu).ToList();
                if (usersign.Count > 0)
                {
                    if (usersign.Count > 0)
                    {
                        foreach (var docsign in usersign)
                        { 
                            if (docsign.mailNguoiKy != "" && docsign.mailNguoiKy != null)
                            {
                                try
                                {
                                    mail.To.Add(docsign.mailNguoiKy);
                                    mail.Subject = mdh;
                                    mail.IsBodyHtml = true;
                                    mail.Body = MailBody(docsign.A0037.A0028_ID, mdh, UserSendCurrent, cmtForm);
                                    mail.From = new MailAddress(email.Email);
                                    SmtpClient client = new SmtpClient();
                                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    client.EnableSsl = true;
                                    client.Host = email.SMTPIn;
                                    client.Port = email.Port;
                                    NetworkCredential credentials = new NetworkCredential(email.Email, email.PassWord);
                                    client.UseDefaultCredentials = false;
                                    client.Credentials = credentials;
                                    client.Send(mail);
                                    checkreturn = true;
                                    mail.To.Clear();
                                }
                                catch (Exception ex)
                                {
                                    checkreturn = false;
                                }
                            }
                        }
                    }
                }
                return checkreturn;
            }
        }

        #endregion

        #region Cấu hình quy trình báo phế

        [HttpGet]
        [Route("R1_GetAllQuyTrinh")]
        public async Task<HttpResponseMessage> R1_GetAllQuyTrinh()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var task = Task.Run(() => SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, "QuyTrinhBaoPhe").Tables[0]);
                    var tables = await task;
                    string JSONresult;
                    JSONresult = JsonConvert.SerializeObject(tables);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0, data = JSONresult }));
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_UpdateQuyTrinhBaoPhe")]
        public HttpResponseMessage R2_UpdateQuyTrinhBaoPhe()
        {
            var httpRequest = HttpContext.Current.Request;
            var a0040 = JsonConvert.DeserializeObject<A0040>(httpRequest["A0040"]);
            var istype = httpRequest["IsTypeSign"];
            string nguoiky = "";
            if(istype == "1")
            {
                nguoiky = a0040.nguoiKy1;
            }
            else if (istype == "2")
            {
                nguoiky = a0040.nguoiKy2;
            }
            else if (istype == "3")
            {
                nguoiky = a0040.nguoiKy3;
            }
            else if (istype == "4")
            {
                nguoiky = a0040.nguoiKy4;
            }
            else if (istype == "5")
            {
                nguoiky = a0040.nguoiKy5;
            }
            else if (istype == "6")
            {
                nguoiky = a0040.nguoiKy6;
            }
            else if (istype == "7")
            {
                nguoiky = a0040.nguoiKy7;
            }

            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var A0001_ID = "";
                    var role = db.A0001.FirstOrDefault(x => x.maRole == "R006");
                    if(role != null)
                    {
                        A0001_ID = role.A0001_ID;
                    }
                    if (a0040.A0040_ID != null && a0040.A0040_ID != "")
                    {
                        var obj = db.A0040.FirstOrDefault(x => x.A0040_ID == a0040.A0040_ID);
                        obj.nguoiKy1 = a0040.nguoiKy1;
                        obj.nguoiKy2 = a0040.nguoiKy2;
                        obj.nguoiKy3 = a0040.nguoiKy3;
                        obj.nguoiKy4 = a0040.nguoiKy4;
                        obj.nguoiKy5 = a0040.nguoiKy5;
                        obj.nguoiKy6 = a0040.nguoiKy6;
                        obj.nguoiKy7 = a0040.nguoiKy7;
                        obj.MailNguoiKy1 = a0040.MailNguoiKy1;
                        obj.MailNguoiKy2 = a0040.MailNguoiKy2;
                        obj.MailNguoiKy3 = a0040.MailNguoiKy3;
                        obj.MailNguoiKy4 = a0040.MailNguoiKy4;
                        obj.MailNguoiKy5 = a0040.MailNguoiKy5;
                        obj.MailNguoiKy6 = a0040.MailNguoiKy6;
                        obj.MailNguoiKy7 = a0040.MailNguoiKy7;
                        if(A0001_ID != "" && nguoiky != null)
                        {                
                            var checkrole = db.A0003.FirstOrDefault(x => x.A0002_ID == nguoiky && x.A0001_ID == A0001_ID);
                            if(checkrole == null)
                            {
                                var objrole = new A0003();
                                objrole.A0003_ID = helper.GenKey();
                                objrole.A0002_ID = nguoiky;
                                objrole.A0001_ID = A0001_ID;
                                db.A0003.Add(objrole);
                            }
                        }                        
                    }
                    else
                    {
                        var obj = new A0040();
                        obj.A0040_ID = helper.GenKey();
                        obj.A0034_ID = a0040.A0034_ID;
                        obj.nguoiKy1 = a0040.nguoiKy1 != "null" && a0040.nguoiKy1 != null ? a0040.nguoiKy1 : null;
                        obj.nguoiKy2 = a0040.nguoiKy2 != "null" && a0040.nguoiKy2 != null ? a0040.nguoiKy2 : null;
                        obj.nguoiKy3 = a0040.nguoiKy3 != "null" && a0040.nguoiKy3 != null ? a0040.nguoiKy3 : null;
                        obj.nguoiKy4 = a0040.nguoiKy4 != "null" && a0040.nguoiKy4 != null ? a0040.nguoiKy4 : null;
                        obj.nguoiKy5 = a0040.nguoiKy5 != "null" && a0040.nguoiKy5 != null ? a0040.nguoiKy5 : null;
                        obj.nguoiKy6 = a0040.nguoiKy6 != "null" && a0040.nguoiKy6 != null ? a0040.nguoiKy6 : null;
                        obj.nguoiKy7 = a0040.nguoiKy7 != "null" && a0040.nguoiKy7 != null ? a0040.nguoiKy7 : null;
                        obj.MailNguoiKy1 = a0040.MailNguoiKy1 != "null" && a0040.MailNguoiKy1 != null ? a0040.MailNguoiKy1 : null;
                        obj.MailNguoiKy2 = a0040.MailNguoiKy2 != "null" && a0040.MailNguoiKy2 != null ? a0040.MailNguoiKy2 : null;
                        obj.MailNguoiKy3 = a0040.MailNguoiKy3 != "null" && a0040.MailNguoiKy3 != null ? a0040.MailNguoiKy3 : null;
                        obj.MailNguoiKy4 = a0040.MailNguoiKy4 != "null" && a0040.MailNguoiKy4 != null ? a0040.MailNguoiKy4 : null;
                        obj.MailNguoiKy5 = a0040.MailNguoiKy5 != "null" && a0040.MailNguoiKy5 != null ? a0040.MailNguoiKy5 : null;
                        obj.MailNguoiKy6 = a0040.MailNguoiKy6 != "null" && a0040.MailNguoiKy6 != null ? a0040.MailNguoiKy6 : null;
                        obj.MailNguoiKy7 = a0040.MailNguoiKy7 != "null" && a0040.MailNguoiKy7 != null ? a0040.MailNguoiKy7 : null;
                        db.A0040.Add(obj);

                        if (A0001_ID != "" && nguoiky != null)
                        {
                            var checkrole = db.A0003.FirstOrDefault(x => x.A0002_ID == nguoiky && x.A0001_ID == A0001_ID);
                            if (checkrole == null)
                            {
                                var objrole = new A0003();
                                objrole.A0003_ID = helper.GenKey();
                                objrole.A0002_ID = nguoiky;
                                objrole.A0001_ID = A0001_ID;
                                db.A0003.Add(objrole);
                            }
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
        }

        #endregion

        #region Export Excel

        public HttpResponseMessage ExportKetQua()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string NameForm = "";
            NameForm = "Bieu yeu cau bao phe, thu hoi ccsx";
            var httpRequest = HttpContext.Current.Request;
            string Id = httpRequest["A0028_ID"];
            string A0037_ID = httpRequest["A0037_ID"];
            string A0034_ID = httpRequest["A0034_ID"];            

            using (MeikoEntities db = new MeikoEntities())
            {
                try
                {
                    FileInfo tempExcel = new FileInfo(HttpContext.Current.Server.MapPath("~/Template/Temp.xlsx"));
                    using (ExcelPackage pck = new ExcelPackage(tempExcel))
                    { 
                        if (A0034_ID == "null" || A0034_ID == "undefined")
                        {
                            A0034_ID = null;
                        }
                        if (A0037_ID == "null" || A0034_ID == "undefined")
                        {
                            A0037_ID = null;
                        }

                        var A0036List = db.A0036.Where(x => x.A0034_ID == A0034_ID).Select(x => x.A0035_ID).ToList();
                        var tables = db.A0028.Where(x => x.A0028_ID == Id).Select(
                        x => new
                        {
                            x.A0028_ID,
                            x.A0002_ID,
                            x.A0002.hoVaTen,
                            x.A0016_ID,
                            x.A0032_ID,
                            x.maForm,
                            x.A0016.maCongViec,
                            x.A0016.tenCongViec,
                            x.trangThaiNguoiTao,
                            x.trangThai,
                            x.ngayTao,
                            x.ngayXuLyMongMuon,
                            x.viTriHienTaiMenuID,
                            x.viTriHienTaiNhomKyID,
                            x.viTriHienTaiUserID,
                            x.T001C,
                            x.T002C,
                            x.T003C,
                            x.T004C,
                            x.T005C,
                            x.T006C,
                            x.T007C,
                            x.T008C,
                            x.T009C,
                            x.T010C,
                            x.T011C,
                            x.T012C,
                            x.T013C,
                            x.T014C,
                            x.T015C,
                            x.T016C,
                            x.T017C,
                            x.T018C,
                            x.T019C,
                            x.T020C,
                            x.T021C,
                            x.T022C,
                            x.T023C,
                            x.T024C,
                            x.T025C,
                            x.T026C,
                            x.T027C,
                            x.T028C,
                            x.T029C,
                            x.T030C,
                            x.T031C,
                            x.T032C,
                            x.T033C,
                            x.T034C,
                            x.T035C,
                            x.T036C,
                            x.T037C,
                            x.T038C,
                            x.T039C,
                            x.T040C,
                            x.T041C,
                            x.T042C,
                            x.T043C,
                            x.T044C,
                            x.T045C,
                            x.T046C,
                            x.T047C,
                            x.T048C,
                            x.T049C,
                            x.T050C
                        }).ToList();

                        var tables2 = db.A0028D.Where(x => x.A0028_ID == Id).Select(x => new
                        {
                            x.A0028D_ID,
                            x.A0028_ID,
                            x.IsPostion,
                            x.C001C,
                            x.C002C,
                            x.C003C,
                            x.C004C,
                            x.C005C,
                            x.C006C,
                            x.C007C,
                            x.C008C,
                            x.C009C,
                            x.C010C,
                            x.C011C,
                            x.C012C,
                            x.C013C,
                            x.C014C,
                            x.C015C,
                            x.C016C,
                            x.C017C,
                            x.C018C,
                            x.C019C,
                            x.C020C,
                            x.C021C,
                            x.C022C,
                            x.C023C,
                            x.C024C,
                            x.C025C,
                            x.C026C,
                            x.C027C,
                            x.C028C,
                            x.C029C,
                            x.C030C,
                            x.C031C,
                            x.C032C,
                            x.C033C,
                            x.C034C,
                            x.C035C,
                            x.C036C,
                            x.C037C,
                            x.C038C,
                            x.C039C,
                            x.C040C
                        });


                        if (!string.IsNullOrWhiteSpace(A0034_ID))
                        {
                            tables2 = tables2.Where(x => A0036List.Contains(x.C008C) == true);
                        }

                        var tables3 = db.A0037.Where(x => x.A0028_ID == Id).Select(x => new
                        {
                            x.A0037_ID,
                            x.A0002_ID,
                            x.A0028_ID,
                            x.A0034_ID,
                            tenPhongBan = x.tenBoPhan
                        }).OrderBy(x => x.tenPhongBan).ToList();

                        var tables4 = db.A0039.Where(x => x.A0028_ID == Id && x.A0037_ID == A0037_ID).Select(x => new
                        {
                            x.A0039_ID,
                            x.A0028_ID,
                            x.A0028D_ID,
                            x.A0037_ID,
                            x.ngayXuLy,
                            x.nguoiXuLy,
                            x.noiDungXuLy,
                            x.trangThai
                        }).ToList();

                        var WorkFile = db.A0031.Where(x => x.A0028_ID == Id).Select(x => new
                        {
                            x.A0031_ID,
                            x.A0028_ID,
                            x.tenFile,
                            x.dungLuong,
                            x.duongDan,
                            x.ngayTao,
                            x.loaiFile,
                            x.thuTu
                        }).OrderBy(x => x.thuTu).ToList();

                        var qrtables2 = tables2.OrderBy(x => x.C009C).GroupBy(x => x.C001C).Select(x => new { C001C = x.Key, C009C = x.Select(c => c.C009C).FirstOrDefault(), ListDMBaoPhe = x.OrderBy(c => c.C010C) }).OrderBy(x => x.C009C).ToList();

                        OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault();
                        ws.Cells[8, 7, 8, 11].Merge = true;
                        ws.Cells[8, 7].Value = tables[0].T001C; // Người báo phế 

                        ws.Cells[8, 15, 8, 17].Merge = true;
                        ws.Cells[8, 15].Value = tables[0].T002C; //Người báo phế

                        ws.Cells[8, 18].Value = "Sđt内線:" + tables[0].T003C; //Số điện thoại

                        ws.Cells[9, 7, 9, 11].Merge = true;
                        ws.Cells[9, 7].Value = tables[0].T004C; // Mã sản phẩm 

                        ws.Cells[9, 15, 9, 19].Merge = true;
                        ws.Cells[9, 15].Value = tables[0].T005C; //Mã số báo phế 
                        

                        ws.Cells[15, 15, 23, 18].Merge = true;
                        ws.Cells[15, 15].Value = tables[0].T008C; //Nội dung thay đổi
                        ws.Cells[15, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[15, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws.Cells[25, 16, 33, 18].Merge = true;
                        ws.Cells[25, 16].Value = tables[0].T009C; //Dữ liệu báo phế
                        ws.Cells[25, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[25, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws.Cells[36, 16, 44, 18].Merge = true;
                        ws.Cells[36, 16].Value = tables[0].T010C; //Mã số công cụ
                        ws.Cells[36, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[36, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws.Cells[46, 16, 50, 18].Merge = true;
                        ws.Cells[46, 16].Value = tables[0].T011C; //Ghi chú
                        ws.Cells[46, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[46, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws.Cells[83, 16, 85, 18].Merge = true;
                        ws.Cells[83, 16].Value = tables[0].T013C; //Phương pháp xử lý tồn kho
                        ws.Cells[83, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[83, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        ws.Cells[92, 15, 93, 16].Merge = true;
                        ws.Cells[92, 15].Value = tables[0].T014C; //Số lớp
                        ws.Cells[92, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[92, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        ws.Cells[92, 17, 93, 17].Merge = true;
                        ws.Cells[92, 17].Value = tables[0].T015C; //Số sheet
                        ws.Cells[92, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[92, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        ws.Cells[92, 18, 93, 18].Merge = true;
                        ws.Cells[92, 18].Value = tables[0].T016C; //Số m3 Sheet
                        ws.Cells[92, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[92, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        ws.Cells[12, 18, 13, 19].Merge = true;
                        ws.Cells[12, 18].Value = tables[0].T006C + " / " + tables[0].T007C; //Độ ưu tiên

                        var stt = 0;
                        var mcell = 12;
                        if (tables3.Count > 0)
                        {
                            for (int i = 12; i < (tables3.Count + 12); i++)
                            {
                                stt = stt + 1;
                                int j = i - 12;
                                ws.Cells[mcell, 2, mcell + 1, 2].Merge = true;
                                ws.Cells[mcell, 2].Value = stt.ToString();
                                ws.Cells[mcell, 2].Style.Font.Bold = true;
                                ws.Cells[mcell, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //ws.Cells[mcell, 2].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                                ws.Cells[mcell, 3, mcell + 1, 3].Merge = true;
                                ws.Cells[mcell, 3].Style.Font.Bold = true;
                                ws.Cells[mcell, 3].Value = tables3[j].tenPhongBan;
                                ws.Cells[mcell, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                //ws.Cells[mcell, 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                mcell = mcell + 2;
                            }
                        }

                        var cellmercell = 12;
                        var cellchild = 12;
                        if (tables2.Count() > 0)
                        {
                            for (int i = 12; i < (qrtables2.Count + 12); i++)
                            { 
                                int j = i - 12;
                                var cellcount = qrtables2[j].ListDMBaoPhe.Count() * 2;
                                cellcount = cellcount- 1;
                                var A034ID = qrtables2[j].C001C.ToString();
                                var PhongBan = db.A0034.FirstOrDefault(x => x.A0034_ID == A034ID);
                                ws.Cells[cellmercell, 5, cellmercell + cellcount, 5].Merge = true;
                                ws.Cells[cellmercell, 5].Value = PhongBan.tenPhongBan;
                                ws.Cells[cellmercell, 5].Style.Font.Bold = true;
                                ws.Cells[cellmercell, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //ws.Cells[cellmercell, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                //ws.Cells[cellmercell, 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                //ws.Cells[cellmercell, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                //ws.Cells[cellmercell, 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                cellmercell = cellmercell + cellcount + 1;

                                foreach (var item in qrtables2[j].ListDMBaoPhe)
                                {
                                    ws.Cells[cellchild, 6, cellchild, 7].Merge = true;
                                    ws.Cells[cellchild, 6].Value = item.C002C;
                                    ws.Cells[cellchild, 6, cellchild, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    ws.Cells[cellchild + 1, 6, cellchild + 1, 7].Merge = true;
                                    ws.Cells[cellchild + 1, 6].Value = item.C003C;
                                    ws.Cells[cellchild + 1, 6, cellchild + 1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    ws.Cells[cellchild, 8, cellchild + 1, 8].Merge = true;
                                    ws.Cells[cellchild, 8].Value = item.C004C == "1" ? "O" : "";
                                    ws.Cells[cellchild, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    ws.Cells[cellchild, 9, cellchild + 1, 9].Merge = true;
                                    ws.Cells[cellchild, 9].Value = item.C004C == "2" ? "O" : "";
                                    ws.Cells[cellchild, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    ws.Cells[cellchild, 10, cellchild + 1, 10].Merge = true;
                                    ws.Cells[cellchild, 10].Value = item.C004C == "3" ? "O" : "";
                                    ws.Cells[cellchild, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    ws.Cells[cellchild, 11, cellchild + 1, 11].Merge = true;
                                    ws.Cells[cellchild, 11].Value = item.C004C == "4" ? "O" : "";
                                    ws.Cells[cellchild, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                    var a0039 = tables4.FirstOrDefault(x => x.A0028D_ID == item.A0028D_ID);
                                    ws.Cells[cellchild, 12, cellchild + 1, 12].Merge = true;
                                    if(a0039 != null && a0039.ngayXuLy != null)
                                    {
                                        ws.Cells[cellchild, 12].Value = a0039.ngayXuLy.Value.ToString("dd/MM/yyyy");
                                        ws.Cells[cellchild, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }

                                    ws.Cells[cellchild, 13, cellchild + 1, 13].Merge = true;
                                    if (a0039 != null)
                                    {
                                        ws.Cells[cellchild, 13].Value = a0039.nguoiXuLy;
                                        ws.Cells[cellchild, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }

                                    ws.Cells[cellchild, 14, cellchild + 1, 14].Merge = true;
                                    if (a0039 != null)
                                    {
                                        ws.Cells[cellchild, 14].Value = a0039.noiDungXuLy;
                                        ws.Cells[cellchild, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }

                                    cellchild = cellchild + 2;
                                }
                                //mcell = mcell + 2;
                            }
                        }

                        byte[] data = pck.GetAsByteArray();
                        string path = HttpContext.Current.Server.MapPath("~/Template/" + NameForm + " " +tables[0].T005C + ".xlsx");
                        System.IO.File.WriteAllBytes(path, data);
                        var UrlFile = "/Template/" + NameForm + " " + tables[0].T005C + ".xlsx";
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { data = UrlFile, error = 0 }));
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
            }
        }

        #endregion
    }
}
