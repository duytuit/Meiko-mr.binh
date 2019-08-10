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
using System.Xml;
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/QuyTrinh")]
    public class QuyTrinhController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        #region Loại Công Việc        
        [HttpPost]
        [Route("R1_LCVGetByList")]
        public HttpResponseMessage R1_LCVGetByList()
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
                var tables = db.A0015.Select(x => new {
                    x.A0015_ID,
                    x.maLoaiCongViec,
                    x.tenLoaiCongViec,
                    x.STT,
                    x.trangThai
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
                cd.tenLoaiCongViec.Contains(s) ||
                cd.maLoaiCongViec.Contains(s)));
                var qrs = tables.OrderBy(x => x.STT).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_LCVGetBySelect")]
        public HttpResponseMessage R1_LCVGetBySelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {               
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0015.Where(x => x.trangThai == true).Select(x => new {
                    x.A0015_ID,
                    x.tenLoaiCongViec,
                    x.STT,
                    x.trangThai
                });
                var qrs = tables.OrderBy(x => x.STT).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs}));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_LCVGetByID/{Id}")]
        public HttpResponseMessage R1_LCVGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0015 = db.A0015.Where(x => x.A0015_ID == Id).Select(x => new {
                    x.A0015_ID,
                    x.maLoaiCongViec,
                    x.tenLoaiCongViec,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0015));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddLCV")]
        public HttpResponseMessage R2_AddLCV(A0015 lcv)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0015();
                obj.A0015_ID = helper.GenKey();
                obj.maLoaiCongViec = lcv.maLoaiCongViec;
                obj.tenLoaiCongViec = lcv.tenLoaiCongViec;
                obj.STT = lcv.STT;
                obj.trangThai = lcv.trangThai;
                db.A0015.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateLCV")]
        public HttpResponseMessage R3_UpdateLCV(A0015 lcv)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0015.Find(lcv.A0015_ID);
                if (obj != null)
                {
                    obj.maLoaiCongViec = lcv.maLoaiCongViec;
                    obj.tenLoaiCongViec = lcv.tenLoaiCongViec;
                    obj.STT = lcv.STT;
                    obj.trangThai = lcv.trangThai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteLCV")]
        public HttpResponseMessage R4_DeleteLCV(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0015.Where(x => Id.Contains(x.A0015_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0015.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
        
        [HttpGet]
        [Route("R1_CVGroupLCVSelect")]
        public HttpResponseMessage R1_CVGroupLCVSelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0016.Where(x => x.trangThai == true).Select(x => new {
                    x.A0016_ID,
                    x.A0015_ID,
                    x.A0015.tenLoaiCongViec,
                    x.maCongViec,
                    x.tenCongViec,
                    x.A0004_ID,
                    x.loaiCongViec,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.tenLoaiCongViec).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }
        #endregion

        #region Công Việc

        [HttpPost]
        [Route("R1_CongViecGetByList")]
        public HttpResponseMessage R1_CongViecGetByList()
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
                var tables = db.A0016.Select(x => new {
                    x.A0016_ID,
                    x.A0015_ID,
                    x.A0032_ID,
                    x.A0032.tenForm,
                    x.maCongViec,
                    x.tenCongViec,
                    x.A0004_ID,
                    x.loaiCongViec,
                    x.STT,
                    x.trangThai
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
                cd.tenCongViec.Contains(s) ||
                cd.maCongViec.Contains(s)));
                var qrs = tables.OrderBy(x => x.STT).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_CongViecGetByID/{Id}")]
        public HttpResponseMessage R1_CongViecGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0016 = db.A0016.Where(x => x.A0016_ID == Id).Select(x => new {
                    x.A0016_ID,
                    x.A0015_ID,
                    x.A0032_ID,
                    x.A0032.tenForm,
                    x.maCongViec,
                    x.tenCongViec,
                    x.A0004_ID,
                    x.loaiCongViec,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0016));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddCongViec")]
        public HttpResponseMessage R2_AddCongViec(A0016 cv)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0016();
                obj.A0016_ID = helper.GenKey();
                obj.A0015_ID = cv.A0015_ID;
                obj.A0032_ID = cv.A0032_ID;
                obj.maCongViec = cv.maCongViec;
                obj.tenCongViec = cv.tenCongViec;
                obj.A0004_ID = cv.A0004_ID != null && cv.A0004_ID != "null" ? cv.A0004_ID : null;
                obj.loaiCongViec = cv.loaiCongViec;
                obj.STT = cv.STT;
                obj.trangThai = cv.trangThai;
                db.A0016.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateCongViec")]
        public HttpResponseMessage R3_UpdateCongViec(A0016 cv)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0016.Find(cv.A0016_ID);
                if (obj != null)
                {
                    obj.A0015_ID = cv.A0015_ID;
                    obj.A0032_ID = cv.A0032_ID;
                    obj.maCongViec = cv.maCongViec;
                    obj.tenCongViec = cv.tenCongViec;
                    obj.A0004_ID = cv.A0004_ID != null && cv.A0004_ID != "null" ? cv.A0004_ID : null;
                    obj.loaiCongViec = cv.loaiCongViec;
                    obj.STT = cv.STT;
                    obj.trangThai = cv.trangThai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteCongViec")]
        public HttpResponseMessage R4_DeleteCongViec(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0016.Where(x => Id.Contains(x.A0016_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0016.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpGet]
        [Route("R1_GetGroupSignByCVID/{Id}")]
        public HttpResponseMessage R1_GetGroupSignByCVID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0016 = db.A0033.Where(x => x.A0016_ID == Id).Select(x => new {
                    x.A0033_ID,
                    x.A0016_ID,
                    x.A0004_ID,
                    x.A0021_ID,
                    x.A0019_ID,
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0016));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddConfigSignGroupCV")]
        public HttpResponseMessage R2_AddConfigSignGroupCV(A0033 gsconfig)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    if(gsconfig.A0016_ID == "null")
                    {
                        gsconfig.A0016_ID = null;
                    }
                    if (gsconfig.A0019_ID == "null")
                    {
                        gsconfig.A0019_ID = null;
                    }
                    var obj = new A0033();
                    obj.A0033_ID = helper.GenKey();
                    obj.A0004_ID = gsconfig.A0004_ID;
                    obj.A0021_ID = gsconfig.A0021_ID;
                    obj.A0019_ID = gsconfig.A0019_ID;
                    obj.A0016_ID = gsconfig.A0016_ID;
                    db.A0033.Add(obj);
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
        [Route("R3_UpdateConfigSignGroupCV")]
        public HttpResponseMessage R3_UpdateConfigSignGroupCV(A0033 gsconfig)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                { 
                    var obj = db.A0033.Find(gsconfig.A0033_ID);
                    if (obj != null)
                    {
                        if (gsconfig.A0016_ID == "null")
                        {
                            gsconfig.A0016_ID = null;
                        }
                        if (gsconfig.A0019_ID == "null")
                        {
                            gsconfig.A0019_ID = null;
                        }

                        obj.A0004_ID = gsconfig.A0004_ID;
                        obj.A0021_ID = gsconfig.A0021_ID;
                        obj.A0019_ID = gsconfig.A0019_ID;
                        obj.A0016_ID = gsconfig.A0016_ID; 
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

        #endregion

        #region Nhóm người ký

        [HttpPost]
        [Route("R1_NhomKyGetByList")]
        public HttpResponseMessage R1_NhomKyGetByList()
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
                var tables = db.A0017.Select(x => new {
                    x.A0017_ID,
                    x.maNhomNguoiKy,
                    x.tenNhomNguoiKy,
                    x.moTaNhomNguoiKy,
                    x.kieuNhomNguoiKy,
                    x.STT,
                    x.trangThai
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
                cd.maNhomNguoiKy.Contains(s) ||
                cd.tenNhomNguoiKy.Contains(s)));
                var qrs = tables.OrderBy(x => x.STT).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_NhomKyGetByID/{Id}")]
        public HttpResponseMessage R1_NhomKyGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0017 = db.A0017.Where(x => x.A0017_ID == Id).Select(x => new {
                    x.A0017_ID,
                    x.maNhomNguoiKy,
                    x.tenNhomNguoiKy,
                    x.moTaNhomNguoiKy,
                    x.kieuNhomNguoiKy,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0017));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddNhomKy")]
        public HttpResponseMessage R2_AddNhomKy(A0017 cv)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0017();
                obj.A0017_ID = helper.GenKey();
                obj.maNhomNguoiKy = cv.maNhomNguoiKy; ;
                obj.tenNhomNguoiKy = cv.tenNhomNguoiKy;
                obj.moTaNhomNguoiKy = cv.moTaNhomNguoiKy;
                obj.kieuNhomNguoiKy = cv.kieuNhomNguoiKy;
                obj.STT = cv.STT;
                obj.trangThai = cv.trangThai;
                db.A0017.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateNhomKy")]
        public HttpResponseMessage R3_UpdateNhomKy(A0017 cv)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0017.Find(cv.A0017_ID);
                if (obj != null)
                {
                    obj.maNhomNguoiKy = cv.maNhomNguoiKy; ;
                    obj.tenNhomNguoiKy = cv.tenNhomNguoiKy;
                    obj.moTaNhomNguoiKy = cv.moTaNhomNguoiKy;
                    obj.kieuNhomNguoiKy = cv.kieuNhomNguoiKy;
                    obj.STT = cv.STT;
                    obj.trangThai = cv.trangThai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteNhomKy")]
        public HttpResponseMessage R4_DeleteNhomKy(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0017.Where(x => Id.Contains(x.A0017_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0017.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpGet]
        [Route("R1_NhomKyGetByGroupSign")]
        public async Task<HttpResponseMessage> R1_NhomKyGetByGroupSign()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {               
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
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
                var tables = ListPB.Where(x => x.muc == "A00" || x.muc == "A01" || x.muc == "A02" || x.muc == "A03" || x.muc == "A04" || x.muc == "A05").Select(x => new
                {
                    x.id,
                    x.idcha,
                    x.bophan_ten,
                    x.thutu,
                    GroupSign = db.A0017.Where(g => g.trangThai == true && g.kieuNhomNguoiKy == 0).Select(g => new {
                        g.maNhomNguoiKy,
                        g.tenNhomNguoiKy,
                        g.moTaNhomNguoiKy,
                        g.STT,
                        g.trangThai,
                        CountUser = db.A0019.Count(c => c.A0017_ID == g.A0017_ID && c.A0004_ID == x.id)
                    })
                }).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_GroupSignGetByPhongBanID")]
        public HttpResponseMessage R1_GroupSignGetByPhongBanID(PhongBanVModel obj)
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);             
                var GroupSign = db.A0017.Where(g => g.trangThai == true && g.kieuNhomNguoiKy == 0).OrderBy(x => x.STT).Select(g => new
                {
                    g.A0017_ID,
                    g.maNhomNguoiKy,
                    g.tenNhomNguoiKy,
                    g.moTaNhomNguoiKy,
                    g.STT,
                    g.trangThai,
                    CountUser = db.A0019.Count(c => c.A0017_ID == g.A0017_ID && c.A0004_ID == obj.id),
                    thanhvien = db.A0019.Where(c => c.A0017_ID == g.A0017_ID && c.A0004_ID == obj.id).OrderBy(x => x.STT).Select(x => x.A0002.maNhanVien + " - " + x.A0002.hoVaTen)
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new {data = GroupSign }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_UserGetByGroupSignIDPhongBanID")]
        public async Task<HttpResponseMessage> R1_UserGetByGroupSignIDPhongBanID(A0019 obj)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath("~/Content/Api.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/ApiWeb/Api");
            string url = node.ChildNodes[0]?.InnerText;
            if (url == null)
            {
                url = "http://192.84.100.207/";
            }

            var httpRequest = HttpContext.Current.Request;
            string apiUrl = url + "AsoftAPI/EC0002";
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
            using (MeikoEntities db = new MeikoEntities())
            {
                //db.Configuration.LazyLoadingEnabled = false;
                //db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var UserGroupSign = db.A0019.Where(g => g.A0004_ID == obj.A0004_ID && g.A0017_ID == obj.A0017_ID).OrderBy(x => x.STT).ToList().Select(g => new
                    {
                        g.A0002_ID,
                        g.A0004_ID,
                        g.A0017_ID,
                        g.A0019_ID,
                        g.A0002.hoVaTen,
                        anhDaiDien = temp + g.A0002.anhDaiDien,
                        tenphongban = ListPB.FirstOrDefault(x => x.id == g.A0002.A0004_ID) != null ? ListPB.FirstOrDefault(x => x.id == g.A0002.A0004_ID).bophan_ten : ""
                    }).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0,data = UserGroupSign }));
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
        [Route("R2_AddUserToGroupSign")]
        public HttpResponseMessage R2_AddUserToGroupSign()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                //db.Configuration.LazyLoadingEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var objcheck = JsonConvert.DeserializeObject<A0019>(httpRequest["A0019"]);
                    List<string> ListUser = JsonConvert.DeserializeObject<List<string>>(httpRequest["UserList"]);
                    int maxOrder = 1;
                    var docmax = db.A0019.Where(x => x.A0017_ID == objcheck.A0017_ID && x.A0004_ID == objcheck.A0004_ID).OrderByDescending(x => x.STT).ToList();
                    if (docmax.Count > 0)
                    {
                        maxOrder = docmax[0].STT + 1;
                    }
                    string usertontai = "";
                    foreach (var item in ListUser)
                    {
                        var check = db.A0019.FirstOrDefault(x => x.A0017_ID == objcheck.A0017_ID && x.A0004_ID == objcheck.A0004_ID && x.A0002_ID == item);
                        if (check == null)
                        {
                            var obj = new A0019();
                            obj.A0019_ID = helper.GenKey();
                            obj.A0004_ID = objcheck.A0004_ID;
                            obj.A0017_ID = objcheck.A0017_ID;
                            obj.A0002_ID = item.ToString();
                            obj.STT = maxOrder;
                            maxOrder = maxOrder + 1;
                            db.A0019.Add(obj);
                        }
                        else
                        {
                            usertontai += check.A0002.hoVaTen + " ,";
                        }
                    }
                    if (usertontai != "")
                    {
                        usertontai = usertontai.Substring(0, usertontai.Length - 1);
                    }
                    db.SaveChanges();
                    if(usertontai != "")
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 2, ms = usertontai }));
                    }
                    else
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    }
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
            }
        }

        [HttpPost]
        [Route("R4_DeleteUserGroupSign")]
        public HttpResponseMessage R4_DeleteUserGroupSign(A0019 a0019)
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                //db.Configuration.LazyLoadingEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = db.A0019.FirstOrDefault(x => x.A0017_ID == a0019.A0017_ID && x.A0004_ID == a0019.A0004_ID && x.A0002_ID == a0019.A0002_ID);
                    if(obj != null)
                    {
                        db.A0019.Remove(obj);
                    }

                    db.SaveChanges();

                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0}));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
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

        #region Quy trình

        [HttpGet]
        [Route("R1_QuytrinhGetByList")]
        public HttpResponseMessage R1_QuytrinhGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {             
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0018.Select(x => new {
                    x.A0018_ID,
                    x.A0015_ID,
                    x.A0004_ID,
                    x.tenQuyTrinh,
                    x.kieuQuyTrinh,
                    x.STT,
                    x.trangThai
                }).ToList(); 
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_QuytrinhGetByID")]
        public HttpResponseMessage R1_QuytrinhGetByID(A0018 a0018)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0018 = db.A0018.Where(x => x.A0018_ID == a0018.A0018_ID).Select(x => new {
                    x.A0018_ID,
                    x.A0015_ID,
                    x.A0004_ID,
                    x.tenQuyTrinh,
                    x.kieuQuyTrinh,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0018));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddQuytrinh")]
        public HttpResponseMessage R2_AddQuytrinh(A0018 qt)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0018();
                obj.A0018_ID = helper.GenKey();
                obj.A0015_ID = qt.A0015_ID;
                obj.A0004_ID = qt.A0004_ID != null && qt.A0004_ID != "null" ? qt.A0004_ID : null;
                obj.tenQuyTrinh = qt.tenQuyTrinh;
                obj.kieuQuyTrinh = qt.kieuQuyTrinh;
                obj.STT = qt.STT;
                obj.trangThai = qt.trangThai;
                db.A0018.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateQuytrinh")]
        public HttpResponseMessage R3_UpdateQuytrinh(A0018 qt)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0018.Find(qt.A0018_ID);
                if (obj != null)
                {
                    obj.A0015_ID = qt.A0015_ID;
                    obj.A0004_ID = qt.A0004_ID != null && qt.A0004_ID != "null" ? qt.A0004_ID : null;
                    obj.tenQuyTrinh = qt.tenQuyTrinh;
                    obj.kieuQuyTrinh = qt.kieuQuyTrinh;
                    obj.STT = qt.STT;
                    obj.trangThai = qt.trangThai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteQuytrinh")]
        public HttpResponseMessage R4_DeleteQuytrinh(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0018.Where(x => Id.Contains(x.A0018_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0018.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
        
        [HttpPost]
        [Route("R1_WorkFollowByLCVID")]
        public async Task<HttpResponseMessage> R1_WorkFollowByLCVID(A0018 obj)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath("~/Content/Api.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/ApiWeb/Api");
            string url = node.ChildNodes[0]?.InnerText;
            if (url == null)
            {
                url = "http://192.84.100.207/";
            }

            var httpRequest = HttpContext.Current.Request;
            string apiUrl = url + "AsoftAPI/EC0002";
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

            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0018.Where(x => x.A0015_ID == obj.A0015_ID && x.trangThai == true).OrderBy(x => x.STT).Select(x => new {
                    x.A0018_ID,
                    x.A0004_ID,
                    x.A0015_ID,
                    x.tenQuyTrinh,
                    x.kieuQuyTrinh,
                    x.STT,
                    x.trangThai,
                    SteepSign = db.A0020.Where(s => s.trangThai == true).OrderBy(s => s.STT).Select(s => new {
                        s.A0020_ID, 
                        s.tenBuocKy,
                        s.AliasBuocKy,
                        s.STT,
                        GroupSignSteep = db.A0021.Where(gs => gs.A0020_ID == s.A0020_ID && gs.A0018_ID == x.A0018_ID).ToList().Select(gs => new {
                            gs.A0021_ID,
                            gs.A0015_ID,
                            gs.A0017_ID,
                            gs.A0018_ID,
                            gs.A0020_ID,
                            gs.A0004_ID,
                            gs.A0017.tenNhomNguoiKy, 
                            thanhvien = db.A0019.Where(c => c.A0017_ID == gs.A0017_ID && c.A0004_ID == gs.A0004_ID).OrderBy(c => c.STT).Select(c => c.A0002.maNhanVien + " - " + c.A0002.hoVaTen)
                        })
                    })
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_WorkFollowByPhongBanID")]
        public HttpResponseMessage R1_WorkFollowByPhongBanID(A0018 obj)
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var GroupSign = db.A0017.Where(g => g.trangThai == true && g.kieuNhomNguoiKy == 0 && g.A0019.Count(x => x.A0004_ID == obj.A0004_ID) > 0).OrderBy(x => x.STT).Select(g => new
                {
                    g.A0017_ID,
                    g.maNhomNguoiKy,
                    g.tenNhomNguoiKy,
                    g.moTaNhomNguoiKy,
                    g.STT,
                    g.trangThai,
                    thanhvien = db.A0019.Where(c => c.A0017_ID == g.A0017_ID && c.A0004_ID == obj.A0004_ID).OrderBy(x => x.STT).Select(x => x.A0002.maNhanVien + " - " + x.A0002.hoVaTen)
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = GroupSign }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddWorkFollowByCVID")]
        public HttpResponseMessage R2_AddWorkFollowByCVID(A0021 a0021)
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                //db.Configuration.LazyLoadingEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var check = db.A0021.FirstOrDefault(x => x.A0015_ID == a0021.A0015_ID && x.A0020_ID == a0021.A0020_ID && x.A0018_ID == a0021.A0018_ID);
                    if(check != null)
                    {
                        check.A0017_ID = a0021.A0017_ID;
                    }
                    else
                    {
                        a0021.A0021_ID = helper.GenKey();
                        db.A0021.Add(a0021);
                    } 
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
            }
        }

        [HttpPost]
        [Route("R4_DeleteWorkFollowByQuyTrinhID")]
        public HttpResponseMessage R4_DeleteWorkFollowByQuyTrinhID(A0021 a0021)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = db.A0021.FirstOrDefault(x => x.A0021_ID == a0021.A0021_ID);
                    if(obj != null)
                    {
                        db.A0021.Remove(obj);
                    }

                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
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

        #region Bước ký

        [HttpGet]
        [Route("R1_SteepKyGetByList")]
        public HttpResponseMessage R1_SteepKyGetByList()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0020.OrderBy(x => x.STT).Select(x => new {
                    x.A0020_ID,
                    x.tenBuocKy,
                    x.AliasBuocKy,
                    x.kieuBuocKy, 
                    x.STT,
                    x.trangThai
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_SteepKyGetByID/{Id}")]
        public HttpResponseMessage R1_SteepKyGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0020 = db.A0020.Where(x => x.A0020_ID == Id).Select(x => new {
                    x.A0020_ID,
                    x.tenBuocKy,
                    x.AliasBuocKy,
                    x.kieuBuocKy,
                    x.STT,
                    x.trangThai
                }).OrderBy(x => x.STT).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0020));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddSteepKy")]
        public HttpResponseMessage R2_AddSteepKy(A0020 stk)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0020();
                obj.A0020_ID = helper.GenKey();
                obj.tenBuocKy = stk.tenBuocKy;
                obj.AliasBuocKy = stk.AliasBuocKy; 
                obj.kieuBuocKy = stk.kieuBuocKy;
                obj.STT = stk.STT;
                obj.trangThai = stk.trangThai;
                db.A0020.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateSteepKy")]
        public HttpResponseMessage R3_UpdateSteepKy(A0020 stk)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0020.Find(stk.A0020_ID);
                if (obj != null)
                {
                    obj.tenBuocKy = stk.tenBuocKy;
                    obj.AliasBuocKy = stk.AliasBuocKy;
                    obj.kieuBuocKy = stk.kieuBuocKy;
                    obj.STT = stk.STT;
                    obj.trangThai = stk.trangThai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteSteepKy")]
        public HttpResponseMessage R4_DeleteSteepKy(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0020.Where(x => Id.Contains(x.A0020_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0020.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
        #endregion

        #region Get Group Sign LCVID

        [HttpPost]
        [Route("R1_GroupSignByCVIDPBID")]
        public HttpResponseMessage R1_GroupSignByCVIDPBID()
        {
            var httpRequest = HttpContext.Current.Request;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            using (MeikoEntities db = new MeikoEntities())
            {
                try
                {
                    string A0016_ID = httpRequest["A0016_ID"];
                    string A0004_ID = httpRequest["A0004_ID"];
                    db.Configuration.LazyLoadingEnabled = false;
                    var obj = db.A0016.FirstOrDefault(x => x.A0016_ID == A0016_ID);
                    var tables = db.A0018.Where(x => x.A0015_ID == obj.A0015_ID && x.A0004_ID == A0004_ID && x.trangThai == true).OrderBy(x => x.STT).Select(x => new {
                        x.A0018_ID,
                        x.A0004_ID,
                        x.A0015_ID,
                        x.tenQuyTrinh,
                        x.kieuQuyTrinh,
                        x.STT,
                        x.trangThai,
                        GroupSign = db.A0021.Where(gs => gs.A0018_ID == x.A0018_ID).ToList().Select(gs => new {
                            gs.A0021_ID,
                            gs.A0015_ID,
                            gs.A0017_ID,
                            gs.A0018_ID,
                            gs.A0020_ID,
                            gs.A0004_ID,
                            gs.A0017.tenNhomNguoiKy,
                            thanhvien = db.A0019.Where(c => c.A0017_ID == gs.A0017_ID && c.A0004_ID == gs.A0004_ID).OrderBy(c => c.STT).Select(c => c.A0002.maNhanVien + " - " + c.A0002.hoVaTen)
                        })
                    }).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0, data = tables }));
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                return response;
            }
        }

        #endregion

        #region getUserbyGroupSign

        [HttpPost]
        [Route("R1_GetUserbyGroupSign")]
        public HttpResponseMessage R1_GetUserbyGroupSign()
        {
            var httpRequest = HttpContext.Current.Request;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            using (MeikoEntities db = new MeikoEntities())
            {
                try
                {
                    string A0004_ID = httpRequest["A0004_ID"];
                    string A0017_ID = httpRequest["A0017_ID"];                    
                    db.Configuration.LazyLoadingEnabled = false;
                    var tables = db.A0019.Where(c => c.A0017_ID == A0017_ID && c.A0004_ID == A0004_ID).OrderBy(c => c.STT).Select(x => new {                    
                        x.A0002_ID,
                        x.A0004_ID,
                        x.A0017_ID,
                        x.A0019_ID,
                        x.A0002.maNhanVien,
                        x.A0002.hoVaTen,
                    }).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0, data = tables }));
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                return response;
            }
        }

        #endregion

        #region Form
        [HttpPost]
        [Route("R1_FormGetByList")]
        public HttpResponseMessage R1_FormGetByList()
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
                var tables = db.A0032.Select(x => new {
                    x.A0032_ID,
                    x.maForm,
                    x.tenForm,
                    x.tenTiengNhat,
                    x.formPhongBan,
                    x.kieuForm,
                    x.images,
                    x.ngayPhatHanh,
                    x.nguoiTao,
                    x.thuTu,
                    x.trangThai
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
                        tables = tables.Where(x => x.trangThai == true);
                    }
                    else
                    {
                        tables = tables.Where(x => x.trangThai == false);
                    }
                }

                tables = tables.Where(cd => (s == "" ||
                cd.tenForm.Contains(s) ||
                cd.maForm.Contains(s)));
                var qrs = tables.OrderBy(x => x.thuTu).Skip(pz * (p - 1)).Take(pz).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_FormGetBySelect")]
        public HttpResponseMessage R1_FormGetBySelect()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var tables = db.A0032.Where(x => x.trangThai == true).Select(x => new {
                        x.A0032_ID,
                        x.maForm,
                        x.tenForm,
                        x.tenTiengNhat,
                        x.formPhongBan,
                        x.kieuForm,
                        x.images,
                        x.ngayPhatHanh,
                        x.nguoiTao,
                        x.thuTu,
                        x.trangThai
                    });
                    var qrs = tables.OrderBy(x => x.thuTu).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, error = 0 }));
                }
                catch (Exception)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_FormGetByID/{Id}")]
        public HttpResponseMessage R1_FormGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var A0032 = db.A0032.Where(x => x.A0032_ID == Id).Select(x => new {
                    x.A0032_ID,
                    x.maForm,
                    x.tenForm,
                    x.tenTiengNhat,
                    x.formPhongBan,
                    x.kieuForm,
                    x.images,
                    x.ngayPhatHanh,
                    x.nguoiTao,
                    x.thuTu,
                    x.trangThai
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(A0032));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddForm")]
        public HttpResponseMessage R2_AddForm(A0032 form)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = new A0032();
                obj.A0032_ID = helper.GenKey();
                obj.maForm = form.maForm;
                obj.tenForm = form.tenForm;
                obj.tenTiengNhat = form.tenTiengNhat;
                obj.formPhongBan = form.formPhongBan;
                obj.kieuForm = form.kieuForm;
                obj.images = form.images;
                obj.ngayPhatHanh = form.ngayPhatHanh;
                obj.nguoiTao = form.nguoiTao;
                obj.thuTu = form.thuTu;
                obj.trangThai = form.trangThai;
                db.A0032.Add(obj);
                db.SaveChanges();
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateForm")]
        public HttpResponseMessage R3_UpdateForm(A0032 form)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var obj = db.A0032.Find(form.A0032_ID);
                if (obj != null)
                {
                    obj.maForm = form.maForm;
                    obj.tenForm = form.tenForm;
                    obj.tenTiengNhat = form.tenTiengNhat;
                    obj.formPhongBan = form.formPhongBan;
                    obj.kieuForm = form.kieuForm;
                    obj.images = form.images;
                    obj.ngayPhatHanh = form.ngayPhatHanh;
                    obj.nguoiTao = form.nguoiTao;
                    obj.thuTu = form.thuTu;
                    obj.trangThai = form.trangThai;
                    db.SaveChanges();
                }
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteForm")]
        public HttpResponseMessage R4_DeleteForm(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var ListCheck = db.A0032.Where(x => Id.Contains(x.A0032_ID)).ToList();
                if (ListCheck.Count > 0)
                {
                    db.A0032.RemoveRange(ListCheck);
                }
                db.SaveChanges();
                return response;
            }
        }
        
        #endregion
    }
}
