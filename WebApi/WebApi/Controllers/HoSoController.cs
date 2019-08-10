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
    public class HoSoController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        [Route("R1_HoSoGetByList")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_HoSoGetByList()
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
                    var tables = db.A0022.Where(x => x.nguoiTao == A0002_ID && x.trangThai == 1).Select(x => new
                    {
                         x.A0022_ID,
                         x.A0016_ID,
                         x.A0028_ID,
                         x.soHoSo,
                         x.tenHoSoKhongDau,
                         x.tenHoSo,
                         x.noiDungHoSo,
                         x.noiDungHoSoKhongDau,
                         x.nguoiTao,
                         x.nguoiDong,
                         x.ngayMo,
                         x.ngayDuKienHoanThanh,
                         x.ngayHoanThanh,
                         x.trangThai
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
                    var qr = tables.OrderBy(x => x.ngayMo).Skip(pz * (p - 1)).Take(pz).ToList();
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

        [Route("R1_HoSoGetByList")]
        [HttpPost]
        public async Task<HttpResponseMessage> R1_HoSoGetByID()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string A0002_ID = httpRequest["A0002_ID"];
                string A0022_ID = httpRequest["A0022_ID"];
                int pz = int.Parse(httpRequest["pz"].ToString());
                int p = int.Parse(httpRequest["p"].ToString());
                string sort = httpRequest["sort"];
                string ob = httpRequest["ob"];
                string s = httpRequest["s"];
                string sts = httpRequest["sts"];
                try
                {
                    var tables = db.A0022.Where(x => x.A0022_ID == A0022_ID).Select(x => new
                    {
                        x.A0022_ID,
                        x.A0016_ID,
                        x.A0028_ID,
                        x.soHoSo,
                        x.tenHoSoKhongDau,
                        x.tenHoSo,
                        x.noiDungHoSo,
                        x.noiDungHoSoKhongDau,
                        x.nguoiTao,
                        x.nguoiDong,
                        x.ngayMo,
                        x.ngayDuKienHoanThanh,
                        x.ngayHoanThanh,
                        x.trangThai
                    });

                    var tables2 = db.A0025.Where(x => x.A0022_ID == A0022_ID).Select(
                    x => new
                    {
                        x.A0025_ID,
                        x.A0022_ID,
                        x.A0002_ID,
                        x.kieuNguoiXuLy,
                        x.ngayThem,
                        x.thuTu,
                        x.A0002.hoVaTen,
                        anhDaiDien = x.A0002.anhDaiDien != null ? temp + x.A0002.anhDaiDien : null
                    }).ToList();

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
                    var qr = tables.OrderBy(x => x.ngayMo).Skip(pz * (p - 1)).Take(pz).ToList();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qr, data2 = tables2, count = tables.Count() }));
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
        [Route("R2_AddHoSo")]
        public HttpResponseMessage R2_AddHoSo()
        {
            var httpRequest = HttpContext.Current.Request;
            var a0022 = JsonConvert.DeserializeObject<A0022>(httpRequest["a0022"]);
            var a0025 = JsonConvert.DeserializeObject<List<A0025>>(httpRequest["a0022"]);
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = new A0022();
                    obj.A0022_ID = helper.GenKey();
                    obj.A0016_ID = a0022.A0016_ID;
                    obj.A0028_ID = a0022.A0028_ID;
                    obj.soHoSo = a0022.soHoSo;
                    obj.tenHoSo = a0022.tenHoSo;
                    obj.tenHoSoKhongDau = helper.convertToUnSign3(obj.tenHoSo);
                    obj.noiDungHoSo = a0022.noiDungHoSo;
                    obj.noiDungHoSoKhongDau = helper.convertToUnSign3(obj.noiDungHoSo);
                    obj.nguoiTao = a0022.nguoiTao;
                    obj.nguoiDong = a0022.nguoiDong;
                    obj.ngayMo = a0022.ngayMo;
                    obj.ngayDuKienHoanThanh = a0022.ngayHoanThanh;
                    obj.ngayHoanThanh = a0022.ngayHoanThanh;
                    obj.trangThai = a0022.trangThai;
                    db.A0022.Add(obj);

                    var stt = 1;
                    foreach (var item in a0025)
                    {
                        var objxl = new A0025();
                        objxl.A0025_ID = helper.GenKey();
                        objxl.A0022_ID = obj.A0022_ID;
                        objxl.A0002_ID = item.A0002_ID;
                        objxl.kieuNguoiXuLy = item.kieuNguoiXuLy;
                        objxl.ngayThem = DateTime.Now;
                        objxl.thuTu = stt;
                        db.A0025.Add(objxl);
                        stt = stt + 1;
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
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));                    
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 2 }));
                    throw;
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R3_UpdateHoSo")]
        public HttpResponseMessage R3_UpdateHoSo()
        {
            var httpRequest = HttpContext.Current.Request;
            var a0022 = JsonConvert.DeserializeObject<A0022>(httpRequest["a0022"]);
            var a0025 = JsonConvert.DeserializeObject<List<A0025>>(httpRequest["a0025"]);            

            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = db.A0022.FirstOrDefault(x => x.A0022_ID == a0022.A0022_ID);
                    if(obj != null)
                    {
                        obj.A0016_ID = a0022.A0016_ID;
                        obj.A0028_ID = a0022.A0028_ID;
                        obj.soHoSo = a0022.soHoSo;
                        obj.tenHoSo = a0022.tenHoSo;
                        obj.tenHoSoKhongDau = helper.convertToUnSign3(obj.tenHoSo);
                        obj.noiDungHoSo = a0022.noiDungHoSo;
                        obj.noiDungHoSoKhongDau = helper.convertToUnSign3(obj.noiDungHoSo);
                        obj.nguoiTao = a0022.nguoiTao;
                        obj.nguoiDong = a0022.nguoiDong;
                        obj.ngayMo = a0022.ngayMo;
                        obj.ngayDuKienHoanThanh = a0022.ngayHoanThanh;
                        obj.ngayHoanThanh = a0022.ngayHoanThanh;
                        obj.trangThai = a0022.trangThai;
                    }

                    var MaxOrder = 1;
                    var MaxOrderCheck = db.A0025.Where(x => x.A0022_ID == obj.A0022_ID).OrderBy(x => x.thuTu).FirstOrDefault();
                    if(MaxOrderCheck != null)
                    {
                        MaxOrder = MaxOrderCheck.thuTu + 1;
                    }
                    
                    // Check User XLC
                    foreach (var item in a0025.Where(x => x.kieuNguoiXuLy == 1))
                    {
                        var objcheck = db.A0025.FirstOrDefault(x => x.A0022_ID == obj.A0022_ID && x.kieuNguoiXuLy == 1 && x.A0002_ID == item.A0002_ID);
                        if(objcheck == null)
                        {
                            var objxlc = new A0025();
                            objxlc.A0025_ID = helper.GenKey();
                            objxlc.A0022_ID = obj.A0022_ID;
                            objxlc.A0002_ID = item.A0002_ID;
                            objxlc.kieuNguoiXuLy = item.kieuNguoiXuLy;
                            objxlc.ngayThem = DateTime.Now;
                            objxlc.thuTu = MaxOrder;
                            db.A0025.Add(objxlc);
                            MaxOrder = MaxOrder + 1;
                        }
                    }

                    // Check User DXL
                    foreach (var item in a0025.Where(x => x.kieuNguoiXuLy == 2))
                    {
                        var objcheck = db.A0025.FirstOrDefault(x => x.A0022_ID == obj.A0022_ID && x.kieuNguoiXuLy == 2 && x.A0002_ID == item.A0002_ID);
                        if (objcheck == null)
                        {
                            var objdxl = new A0025();
                            objdxl.A0025_ID = helper.GenKey();
                            objdxl.A0022_ID = obj.A0022_ID;
                            objdxl.A0002_ID = item.A0002_ID;
                            objdxl.kieuNguoiXuLy = item.kieuNguoiXuLy;
                            objdxl.ngayThem = DateTime.Now;
                            objdxl.thuTu = MaxOrder;
                            db.A0025.Add(objdxl);
                            MaxOrder = MaxOrder + 1;
                        }
                    }

                    // Check User DTG
                    foreach (var item in a0025.Where(x => x.kieuNguoiXuLy == 3))
                    {
                        var objcheck = db.A0025.FirstOrDefault(x => x.A0022_ID == obj.A0022_ID && x.kieuNguoiXuLy == 3 && x.A0002_ID == item.A0002_ID);
                        if (objcheck == null)
                        {
                            var objdtg = new A0025();
                            objdtg.A0025_ID = helper.GenKey();
                            objdtg.A0022_ID = obj.A0022_ID;
                            objdtg.A0002_ID = item.A0002_ID;
                            objdtg.kieuNguoiXuLy = item.kieuNguoiXuLy;
                            objdtg.ngayThem = DateTime.Now;
                            objdtg.thuTu = MaxOrder;
                            db.A0025.Add(objdtg);
                            MaxOrder = MaxOrder + 1;
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
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 2 }));
                    throw;
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R4_DeleteHoSo")]
        public HttpResponseMessage R4_DeleteHoSo()
        {
            var httpRequest = HttpContext.Current.Request;
            var a0022 = JsonConvert.DeserializeObject<A0022>(httpRequest["a0022"]); 

            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    var obj = db.A0022.FirstOrDefault(x => x.A0022_ID == a0022.A0022_ID);
                    if (obj != null)
                    {
                        db.A0022.Remove(obj);
                    }  
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 2 }));
                    throw;
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

    }

}
