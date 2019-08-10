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
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Work")]
    public class WorkController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        [Route("R1_MyWorkDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_MyWorkDocument()
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
                    var tables = db.A0028.Where(x => x.daXoa == false && x.viTriHienTaiMenuID == "DM06.01" && x.viTriHienTaiUserID == A0002_ID).Select(x => new
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
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr,count = tables.Count()}));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

        [Route("R1_WaitingForSignDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_WaitingForSignDocument()
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
                    var tables = db.A0028.Where(x => x.trangThai == 1 && x.daXoa == false && x.viTriHienTaiMenuID == "DM06.02" && x.viTriHienTaiUserID == A0002_ID).Select(x => new
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

        [Route("R1_SenddingDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_SenddingDocument()
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
                    var tables = db.A0028.Where(x => x.daXoa == false && UserDocTran.Contains(x.A0028_ID) && x.viTriHienTaiMenuID != "DM06.04").Select(x => new
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

        [Route("R1_CompletedDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_CompletedDocument()
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
                    var tables = db.A0028.Where(x => x.daXoa == false && UserDocTran.Contains(x.A0028_ID) && x.viTriHienTaiMenuID == "DM06.04").Select(x => new
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

        [Route("R1_PassDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_PassDocument()
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

        [Route("R1_TrashDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_TrashDocument()
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

        [Route("R1_WorkFollowForm")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_WorkFollowForm()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0032_ID = httpRequest["A0032_ID"];                
                try
                {
                    var tables = db.A0016.Where(x => x.A0032_ID == A0032_ID).Select(x => new {
                        x.A0016_ID,
                        x.A0015_ID,
                        x.A0015.maLoaiCongViec,
                        x.A0015.tenLoaiCongViec,
                        x.maCongViec,
                        x.tenCongViec
                    }).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables}));
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
        [Route("R2_AddWorkDocument")]
        public HttpResponseMessage R2_AddWorkDocument()
        {
            var httpRequest = HttpContext.Current.Request;
            var tk = JsonConvert.DeserializeObject<Token>(httpRequest["tk"]);
            var a0028 = JsonConvert.DeserializeObject<A0028>(httpRequest["a0028"]);
            var a0028D = JsonConvert.DeserializeObject<List<A0028D>>(httpRequest["a0028D"]);
            //var a0019_ID = httpRequest["A0019_ID"]; // Người ký
            //var a0021_ID = httpRequest["A0021_ID"]; // Nhóm ký 
            string a0019_ID = "";
            string a0021_ID = "";
            if (a0019_ID == "null")
            {
                a0019_ID = null;
            }
            if (a0021_ID == "null")
            {
                a0021_ID = null;
            }
           
            using (MeikoEntities db = new MeikoEntities())
            { 
                //db.Configuration.LazyLoadingEnabled = false;
                //db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var a0016 = db.A0016.FirstOrDefault(x => x.A0016_ID == a0028.A0016_ID);
                    if(a0016.loaiCongViec == 1)
                    {
                        var a0033 = db.A0033.FirstOrDefault(x => x.A0016_ID == a0016.A0016_ID);
                        if(a0033 != null)
                        {
                            a0019_ID = a0033.A0019_ID;
                            a0021_ID = a0033.A0021_ID;
                        }
                    }
                    else if(a0016.loaiCongViec == 2)
                    {

                    }
                    else
                    {
                        a0019_ID = null;
                        a0021_ID = null;
                    }

                    var MaxSteepSign = 1;
                    var obj = new A0028();
                    obj.A0028_ID = helper.GenKey();
                    obj.A0002_ID = tk.Users_ID;
                    obj.A0016_ID = a0028.A0016_ID;
                    obj.A0032_ID = a0028.A0032_ID;
                    obj.trangThaiNguoiTao = a0028.trangThaiNguoiTao;
                    obj.trangThai = 1;
                    obj.ngayTao = DateTime.Now; 
                    obj.viTriHienTaiMenuID = "DM06.01";
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

                    if(a0028D != null)
                    {
                        foreach (var item in a0028D)
                        {
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
                        }
                    }                    

                    var a0015 = db.A0016.Find(a0028.A0016_ID);
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

                    #region Quy trình
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

                        if (a0021_ID != null && obj.maForm != "SF002")
                        {
                            var GroupSign = db.A0021.FirstOrDefault(gs => gs.A0021_ID == a0021_ID);

                            if (GroupSign != null)
                            {
                                var usergs = db.A0019.Where(c => c.A0017_ID == GroupSign.A0017_ID && c.A0004_ID == GroupSign.A0004_ID).OrderBy(c => c.STT).FirstOrDefault();
                                if (a0019_ID != null)
                                {
                                    usergs = db.A0019.Where(c => c.A0019_ID == a0019_ID).OrderBy(c => c.STT).FirstOrDefault();
                                }
                                if (usergs != null)
                                {
                                    var a0029 = new A0029();
                                    a0029.A0029_ID = helper.GenKey();
                                    a0029.A0028_ID = obj.A0028_ID;
                                    a0029.A0002_ID = usergs.A0002_ID;
                                    a0029.A0017_ID = usergs.A0017_ID;
                                    a0029.A0020_ID = GroupSign.A0020_ID;
                                    a0029.tenNhomKy = GroupSign.A0017.tenNhomNguoiKy;
                                    a0029.tenViTri = GroupSign.A0020.tenBuocKy;
                                    a0029.daKy = false;
                                    a0029.thuThu = MaxSteepSign + 1;
                                    a0029.thoiGianGui = null;
                                    a0029.viTriHienTai = false;
                                    a0029.daXem = false;
                                    db.A0029.Add(a0029);
                                }
                            }
                        }
                        else if (obj.maForm == "SF002" && a0028D.Count > 0) // Config nhiều người ký cho Form SF002 Thư liên lạc nội bộ
                        {
                            foreach (var item in a0028D)
                            {
                                var GroupSign = db.A0021.FirstOrDefault(gs => gs.A0021_ID == item.C002C);
                                if (GroupSign != null)
                                {
                                    var usergs = db.A0019.Where(c => c.A0017_ID == GroupSign.A0017_ID && c.A0004_ID == GroupSign.A0004_ID).OrderBy(c => c.STT).FirstOrDefault();
                                    if (item.C003C != null && item.C003C != "")
                                    {
                                        usergs = db.A0019.Where(c => c.A0019_ID == item.C003C).OrderBy(c => c.STT).FirstOrDefault();
                                    }
                                    if (usergs != null)
                                    {
                                        var a0029 = new A0029();
                                        a0029.A0029_ID = helper.GenKey();
                                        a0029.A0028_ID = obj.A0028_ID;
                                        a0029.A0002_ID = usergs.A0002_ID;
                                        a0029.A0017_ID = usergs.A0017_ID;
                                        a0029.A0020_ID = GroupSign.A0020_ID;
                                        a0029.tenNhomKy = GroupSign.A0017.tenNhomNguoiKy;
                                        a0029.tenViTri = GroupSign.A0020.tenBuocKy;
                                        a0029.daKy = false;
                                        a0029.thuThu = MaxSteepSign + 1;
                                        a0029.thoiGianGui = null;
                                        a0029.viTriHienTai = false;
                                        a0029.daXem = false;
                                        db.A0029.Add(a0029);
                                        MaxSteepSign = MaxSteepSign + 1;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region File Attach
                    if (httpRequest.Files.Count > 0)
                    {
                        int i = 0;
                        var stt1 = 1;
                        var stt2 = 1;
                        var stt3 = 1;
                        foreach (var filename in httpRequest.Files)
                        {
                            string genkey = helper.GenKey();
                            if (filename.ToString() == "FileAttach1")
                            {
                                HttpPostedFile file = httpRequest.Files[i];
                                var a0031 = new A0031();
                                a0031.A0031_ID = helper.GenKey();
                                a0031.A0028_ID = obj.A0028_ID;
                                a0031.tenFile = file.FileName;
                                a0031.dungLuong = file.ContentLength.ToString();
                                a0031.ngayTao = DateTime.Now;
                                a0031.loaiFile = System.IO.Path.GetExtension(helper.NameToTag(file.FileName));
                                a0031.thuTu = stt1;
                                a0031.kieuFile = 1;
                                a0031.duongDan = "/Portals/images/Users/" + genkey + file.FileName;
                                file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                                db.A0031.Add(a0031);
                                stt1 = stt1 + 1;
                            }
                            else if (filename.ToString() == "FileAttach2")
                            {
                                HttpPostedFile file = httpRequest.Files[i];
                                var a0031 = new A0031();
                                a0031.A0031_ID = helper.GenKey();
                                a0031.A0028_ID = obj.A0028_ID;
                                a0031.tenFile = file.FileName;
                                a0031.dungLuong = file.ContentLength.ToString();
                                a0031.ngayTao = DateTime.Now;
                                a0031.loaiFile = System.IO.Path.GetExtension(helper.NameToTag(file.FileName));
                                a0031.thuTu = stt2;
                                a0031.kieuFile = 2;
                                a0031.duongDan = "/Portals/images/Users/" + genkey + file.FileName;
                                file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                                db.A0031.Add(a0031);
                                stt2 = stt2 + 1;
                            }
                            else if (filename.ToString() == "FileAttach3")
                            {
                                HttpPostedFile file = httpRequest.Files[i];
                                var a0031 = new A0031();
                                a0031.A0031_ID = helper.GenKey();
                                a0031.A0028_ID = obj.A0028_ID;
                                a0031.tenFile = file.FileName;
                                a0031.dungLuong = file.ContentLength.ToString();
                                a0031.ngayTao = DateTime.Now;
                                a0031.loaiFile = System.IO.Path.GetExtension(helper.NameToTag(file.FileName));
                                a0031.thuTu = stt3;
                                a0031.kieuFile = 3;
                                a0031.duongDan = "/Portals/images/Users/" + genkey + file.FileName;
                                file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                                db.A0031.Add(a0031);
                                stt3 = stt3 + 1;
                            }
                            i = i + 1;
                        }
                    }
                    #endregion

                    //if (httpRequest.Files.Count > 0)
                    //{
                    //    for (int i = 0; i < httpRequest.Files.Count; i++)
                    //    {
                    //        var a0031 = new A0031();
                    //        HttpPostedFile file = httpRequest.Files[i];
                    //        string genkey = helper.GenKey();
                    //        a0031.A0031_ID = helper.GenKey();
                    //        a0031.A0028_ID = obj.A0028_ID;
                    //        a0031.tenFile = file.FileName;
                    //        a0031.dungLuong = file.ContentLength.ToString();
                    //        a0031.ngayTao = DateTime.Now;
                    //        a0031.loaiFile = System.IO.Path.GetExtension(helper.NameToTag(file.FileName));
                    //        a0031.thuTu = i + 1;
                    //        a0031.duongDan = "/Portals/images/Users/" + genkey + file.FileName;
                    //        file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                    //        db.A0031.Add(a0031);
                    //    }
                    //}

                    db.SaveChanges(); 
                }
                catch (Exception ex)
                { 
                    throw;
                }
                return response;
            }
        }
       
        [HttpGet]
        [Route("R1_MyWorkDocumentDetail/{id}")]
        public HttpResponseMessage R1_MyWorkDocumentDetail(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
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
                    }).ToList();

                    var WorkFile = db.A0031.Where(x => x.A0028_ID == Id).Select(x => new
                    {
                        x.A0031_ID,
                        x.A0028_ID,
                        x.tenFile,
                        x.dungLuong,
                        duongDan = temp + x.duongDan,
                        x.ngayTao,
                        x.loaiFile,
                        x.kieuFile,
                        x.thuTu
                    }).OrderBy(x => x.thuTu).ToList();
                     
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables,data2 = tables2,data3 = WorkFile }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    return response;
                }               
            }
        }

        [HttpGet]
        [Route("R1_EventNote/{id}")]
        public HttpResponseMessage R1_EventNote(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var tables = db.A0029.Where(x => x.A0028_ID == Id).Select(x => new {
                        x.A0028_ID,
                        x.A0002_ID,
                        x.A0002.hoVaTen,
                        anhDaiDien = temp + x.A0002.anhDaiDien,
                        x.tenNhomKy,
                        x.tenViTri,
                        x.daKy,
                        x.thoiGianGui,
                        x.viTriHienTai,
                        x.thuThu
                    }).OrderBy(x => x.thuThu).ToList();

                    var table2 = db.A0030.Where(x => x.A0028_ID == Id).Select(x => new
                    {
                        x.A0030_ID,
                        x.A0002.hoVaTen,
                        anhDaiDien = temp + x.A0002.anhDaiDien,
                        x.tieuDe,
                        x.noiDung,
                        x.thoiGianGui,
                        x.trangThai,
                        x.thuThu
                    }).OrderBy(x => x.thuThu).ToList(); 

                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = tables,data2 = table2 }));
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
                    string A0028_ID = httpRequest["A0028_ID"];
                    string A0002_ID = httpRequest["A0002_ID"];
                    string Note = httpRequest["Note"];
                    var doc = db.A0028.FirstOrDefault(x => x.A0028_ID == A0028_ID);
                    if (doc == null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
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
                            objlog.tieuDe = "Ký Duyệt Gửi Hồ Sơ";
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
                                doc.viTriHienTaiMenuID = "DM06.02";
                                doc.trangThai = 1;
                            }
                            else
                            {
                                doc.viTriHienTaiUserID = A0002_ID;
                                doc.viTriHienTaiMenuID = "DM06.04";
                                doc.trangThai = 1;
                                var objcompleted = new A0030();
                                objcompleted.A0030_ID = helper.GenKey();
                                objcompleted.A0028_ID = doc.A0028_ID;
                                objcompleted.A0002_ID = A0002_ID;
                                objcompleted.tieuDe = "Hoàn Thành Hồ Sơ";
                                objcompleted.noiDung = "Hoàn Thành Hồ Sơ";
                                objcompleted.thuThu = maxOrder;
                                objcompleted.trangThai = 2;
                                objcompleted.thoiGianGui = DateTime.Now;
                                db.A0030.Add(objcompleted); 
                            }
                        }
                    }
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    db.SaveChanges();
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
        [Route("R2_RejectDocument")]
        public HttpResponseMessage R2_RejectDocument()
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
                    string A0002_ID = httpRequest["A0002_ID"];
                    string Note = httpRequest["Note"];
                    var doc = db.A0028.FirstOrDefault(x => x.A0028_ID == A0028_ID);
                    if (doc == null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    }  

                    int maxOrder = 1;
                    var docmax = db.A0030.Where(x => x.A0028_ID == doc.A0028_ID).OrderByDescending(x => x.thuThu).ToList();
                    if (docmax.Count > 0)
                    {
                        maxOrder = docmax[0].thuThu + 1;
                    }

                    var user = db.A0002.FirstOrDefault(x => x.A0002_ID == A0002_ID);
                    var objlog = new A0030();
                    objlog.A0030_ID = helper.GenKey();
                    objlog.A0028_ID = doc.A0028_ID;
                    objlog.A0002_ID = doc.A0002_ID;
                    objlog.tieuDe = user.hoVaTen + " Trả lại tài liệu công việc";
                    objlog.noiDung = Note;
                    objlog.thuThu = maxOrder;
                    objlog.trangThai = 1;
                    objlog.thoiGianGui = DateTime.Now;
                    maxOrder = maxOrder + 1;
                    db.A0030.Add(objlog);
                        
                    doc.viTriHienTaiUserID = doc.A0002_ID;
                    doc.viTriHienTaiMenuID = "DM06.01";
                    doc.trangThai = 2;

                    var Docsign = db.A0029.Where(x => x.A0028_ID == doc.A0028_ID).ToList();
                    foreach (var item in Docsign)
                    {
                        item.daKy = false;
                        item.thoiGianGui = null;
                    }
                     
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    db.SaveChanges();
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
        [Route("R4_DeleteFileAttach")]
        public HttpResponseMessage R4_DeleteFileAttach()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0028_ID = httpRequest["A0028_ID"];
                    string A0031_ID = httpRequest["A0031_ID"];
                    db.Configuration.LazyLoadingEnabled = false;
                    var obj = db.A0031.FirstOrDefault(x => x.A0028_ID == A0028_ID && x.A0031_ID == A0031_ID);
                    if(obj != null)
                    {
                        db.A0031.Remove(obj);
                    }
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0}));                    
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
        [Route("R4_DeleteDoccument")]
        public HttpResponseMessage R4_DeleteDoccument()
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
        [Route("R4_DeleteDoccumentTrash")]
        public HttpResponseMessage R4_DeleteDoccumentTrash()
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
                        db.A0028.Remove(obj);
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
    }
}
