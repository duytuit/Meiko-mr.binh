using ClosedXML.Excel;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
    [RoutePrefix("api/User")]

    public class UserController : ApiController
    {
        string temp = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/urlconfig.txt"));

        [HttpPost]
        [Route("R1_UserGetByList")]
        public async Task<HttpResponseMessage> R1_UserGetByList()
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
                string RoleID = httpRequest["RoleID"];
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0002.Select(x => new
                {
                    x.A0002_ID,
                    x.APKID,
                    x.A0004_ID,
                    x.maNhanVien,
                    x.hoVaTen,
                    x.Email,
                    x.soDienThoai,
                    x.userName,
                    x.passWord,
                    anhDaiDien = temp + x.anhDaiDien,
                    x.diaChi,
                    x.ngaySinh,
                    x.tinhTrang,
                    x.ngayVao,
                    x.CMTND,
                    x.kieuUser,
                    x.IsPosition,
                    x.A0003,
                    countRole = x.A0042.Count()
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
                        tables = tables.Where(x => x.tinhTrang != 3 && x.tinhTrang != 9);
                    }
                    else
                    {
                        tables = tables.Where(x => x.tinhTrang == 3 || x.tinhTrang == 9);
                    }
                }
                if (RoleID == "null")
                {
                    RoleID = null;
                }
                if (!string.IsNullOrWhiteSpace(RoleID))
                {
                    tables = tables.Where(x => x.A0003.Count(a => a.A0001_ID == RoleID) > 0);
                }
                tables = tables.Where(cd => (s == "" ||
                cd.hoVaTen.Contains(s) ||
                cd.userName.Contains(s)));
                var qrs = tables.OrderBy(x => x.hoVaTen).Skip(pz * (p - 1)).Take(pz).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        //[HttpGet]
        //[Route("ExportUser")]
        //public async Task<HttpResponseMessage> ExportUser()
        //{
        //    var response = new HttpResponseMessage(HttpStatusCode.OK);
        //    using (MeikoEntities db = new MeikoEntities())
        //    {
        //        var model = db.A0002.Select(x => new {
        //            x.A0002_ID,
        //            x.APKID,
        //            x.A0004_ID,
        //            x.maNhanVien,
        //            x.hoVaTen,
        //            x.Email,
        //            x.soDienThoai,
        //            x.userName,
        //            x.passWord,
        //            x.anhDaiDien,
        //            x.diaChi,
        //            x.ngaySinh,
        //            x.tinhTrang,
        //            x.ngayVao,
        //            x.CMTND,
        //            x.kieuUser
        //        }).OrderBy(x => x.hoVaTen).ToList();

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            var ws = wb.Worksheets.Add(helper.convertToUnSign3("Danh sach tai khoan"));
        //            ws.Columns().Width = 10;
        //            ws.Style.Font.FontName = "Times New Roman";
        //            ws.Style.Font.FontSize = 12;

        //            ws.Range("A2:G2").Merge().Value = "DANH SÁCH TÀI KHOẢN".ToUpper();
        //            ws.Cells["A2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["A2").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Justify;
        //            ws.Cells["A2").Style.Font.Bold = true;                    

        //            ws.Range("A4:A4").Merge().Value = "".ToUpper();

        //            ws.Column("A").Width = 8;
        //            ws.Column("B").Width = 25;
        //            ws.Column("C").Width = 30;
        //            ws.Column("D").Width = 10;
        //            ws.Column("E").Width = 8;
        //            ws.Column("F").Width = 10;
        //            ws.Column("G").Width = 10;
        //            ws.Column("H").Width = 10;
        //            ws.Column("I").Width = 35;

        //            // design table
        //            ws.Range("A5:G5").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);

        //            ws.Cells["A5").Value = "STT";
        //            ws.Cells["A5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //            ws.Cells["A5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["A5").Style.Alignment.WrapText = true;
        //            ws.Cells["A5").Style.Font.Bold = true;

        //            ws.Cells["B5").Value = "Mã nhân viên";
        //            ws.Cells["B5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["B5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["B5").Style.Alignment.WrapText = false;
        //            ws.Cells["B5").Style.Font.Bold = true;

        //            ws.Cells["C5").Value = "Họ và  tên";
        //            ws.Cells["C5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["C5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["C5").Style.Alignment.WrapText = true;
        //            ws.Cells["C5").Style.Font.Bold = true;

        //            ws.Cells["D5").Value = "Mã đăng nhập";
        //            ws.Cells["D5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["D5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["D5").Style.Alignment.WrapText = true;
        //            ws.Cells["D5").Style.Font.Bold = true;

        //            ws.Cells["E5").Value = "Email";
        //            ws.Cells["E5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //            ws.Cells["E5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["E5").Style.Alignment.WrapText = true;
        //            ws.Cells["E5").Style.Font.Bold = true;

        //            ws.Cells["F5").Value = "Số điện thoại";
        //            ws.Cells["F5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //            ws.Cells["F5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["F5").Style.Alignment.WrapText = true;
        //            ws.Cells["F5").Style.Font.Bold = true;

        //            ws.Cells["G5").Value = "Ngày sinh";
        //            ws.Cells["G5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["G5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["G5").Style.Alignment.WrapText = true;
        //            ws.Cells["G5").Style.Font.Bold = true;

        //            ws.Cells["H5").Value = "Số CMTND";
        //            ws.Cells["H5").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["H5").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["H5").Style.Alignment.WrapText = true;
        //            ws.Cells["H5").Style.Font.Bold = true;

        //            ws.Cells["I6").Value = "Địa chỉ";
        //            ws.Cells["I6").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //            ws.Cells["I6").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //            ws.Cells["I6").Style.Alignment.WrapText = true;
        //            ws.Cells["I6").Style.Font.Bold = true;
        //            var stt = 0;
        //            for (int i = 7; i < (model.Count + 7); i++)
        //            {
        //                stt = stt + 1;
        //                int j = i - 7;
        //                ws.Cells[i, 1).Value = stt;
        //                ws.Cells[i, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //                ws.Cells[i, 1).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 1).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 2).Value = model[j].maNhanVien;
        //                ws.Cells[i, 2).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //                ws.Cells[i, 2).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 2).Style.Alignment.WrapText = false;

        //                ws.Cells[i, 3).Value = model[j].hoVaTen;
        //                ws.Cells[i, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //                ws.Cells[i, 3).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 3).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 4).Value = model[j].userName;
        //                ws.Cells[i, 4).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //                ws.Cells[i, 4).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 4).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 5).Value = model[j].Email;
        //                ws.Cells[i, 5).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //                ws.Cells[i, 5).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 5).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 6).Value = model[j].soDienThoai;
        //                ws.Cells[i, 6).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //                ws.Cells[i, 6).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 6).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 7).Value = model[j].ngaySinh;
        //                ws.Cells[i, 7).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //                ws.Cells[i, 7).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 7).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 8).Value = model[j].CMTND;
        //                ws.Cells[i, 8).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //                ws.Cells[i, 8).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 8).Style.Alignment.WrapText = true;

        //                ws.Cells[i, 9).Value = model[j].diaChi;
        //                ws.Cells[i, 9).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        //                ws.Cells[i, 9).Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        //                ws.Cells[i, 9).Style.Alignment.WrapText = true;
        //            }
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);                          
        //                var result = new HttpResponseMessage(HttpStatusCode.OK)
        //                {
        //                    Content = new ByteArrayContent(stream.ToArray())
        //                };
        //                result.Content.Headers.ContentDisposition =
        //                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //                {
        //                    FileName = "Danh sach tai khoan" + ".xlsx"
        //                };
        //                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //                return result;
        //            }
        //        } 
        //    }
        //}

        [HttpGet]
        [Route("ExportUser")]
        public HttpResponseMessage ExportUser()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string NameForm = "";
            NameForm = "Danh sach nguoi dung ";
            using (MeikoEntities db = new MeikoEntities())
            {
                try
                {
                    FileInfo tempExcel = new FileInfo(HttpContext.Current.Server.MapPath("~/Template/UserTemp.xlsx"));
                    using (ExcelPackage pck = new ExcelPackage(tempExcel))
                    {
                        OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault();

                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = "DANH SÁCH TÀI KHOẢN".ToUpper();
                        ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A1"].Style.Font.Bold = true;
                        ws.Cells["A2:I2"].Merge = true;
                        ws.Cells["A2:I2"].Value = "";

                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 15;
                        ws.Column(3).Width = 25;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 20;
                        ws.Column(6).Width = 15;
                        ws.Column(7).Width = 13;
                        ws.Column(8).Width = 13;
                        ws.Column(9).Width = 35;

                        ws.Cells["A3"].Value = "STT";
                        ws.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A3"].Style.WrapText = true;
                        ws.Cells["A3"].Style.Font.Bold = true;

                        ws.Cells["B3"].Value = "Mã nhân viên";
                        ws.Cells["B3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["B3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["B3"].Style.WrapText = false;
                        ws.Cells["B3"].Style.Font.Bold = true;

                        ws.Cells["C3"].Value = "Họ và  tên";
                        ws.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["C3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["C3"].Style.WrapText = true;
                        ws.Cells["C3"].Style.Font.Bold = true;

                        ws.Cells["D3"].Value = "Mã đăng nhập";
                        ws.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["D3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["D3"].Style.WrapText = true;
                        ws.Cells["D3"].Style.Font.Bold = true;

                        ws.Cells["E3"].Value = "Email";
                        ws.Cells["E3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["E3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["E3"].Style.WrapText = true;
                        ws.Cells["E3"].Style.Font.Bold = true;

                        ws.Cells["F3"].Value = "Số điện thoại";
                        ws.Cells["F3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["F3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["F3"].Style.WrapText = true;
                        ws.Cells["F3"].Style.Font.Bold = true;

                        ws.Cells["G3"].Value = "Ngày sinh";
                        ws.Cells["G3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["G3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["G3"].Style.WrapText = true;
                        ws.Cells["G3"].Style.Font.Bold = true;

                        ws.Cells["H3"].Value = "Số CMTND";
                        ws.Cells["H3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["H3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["H3"].Style.WrapText = true;
                        ws.Cells["H3"].Style.Font.Bold = true;

                        ws.Cells["I3"].Value = "Địa chỉ";
                        ws.Cells["I3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["I3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["I3"].Style.WrapText = true;
                        ws.Cells["I3"].Style.Font.Bold = true;
                        var model = db.A0002.Select(x => new
                        {
                            x.A0002_ID,
                            x.APKID,
                            x.A0004_ID,
                            x.maNhanVien,
                            x.hoVaTen,
                            x.Email,
                            x.soDienThoai,
                            x.userName,
                            x.passWord,
                            x.anhDaiDien,
                            x.diaChi,
                            x.ngaySinh,
                            x.tinhTrang,
                            x.ngayVao,
                            x.CMTND,
                            x.kieuUser
                        }).OrderBy(x => x.hoVaTen).ToList();
                        var stt = 0;
                        if (model.Count > 0)
                        {
                            for (int i = 4; i < (model.Count + 4); i++)
                            {
                                stt = stt + 1;
                                int j = i - 4;
                                ws.Cells[i, 1].Value = stt.ToString();
                                ws.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[i, 2].Value = model[j].maNhanVien;
                                ws.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 3].Value = model[j].hoVaTen;
                                ws.Cells[i, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 4].Value = model[j].userName;
                                ws.Cells[i, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 5].Value = model[j].Email;
                                ws.Cells[i, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 6].Value = model[j].soDienThoai;
                                ws.Cells[i, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 7].Value = model[j].ngaySinh != null ? model[j].ngaySinh.Value.ToString("dd/MM/yyyy") : "";
                                ws.Cells[i, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 8].Value = model[j].CMTND;
                                ws.Cells[i, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 9].Value = model[j].diaChi;
                                ws.Cells[i, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }
                        }

                        byte[] data = pck.GetAsByteArray();
                        string path = HttpContext.Current.Server.MapPath("~/Template/" + NameForm + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx");
                        System.IO.File.WriteAllBytes(path, data);
                        var UrlFile = "/Template/" + NameForm + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx";
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

        [HttpGet]
        [Route("ExportUserResetPass")]
        public HttpResponseMessage ExportUserResetPass()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string NameForm = "";
            NameForm = "Danh sach yeu cau cap lai mat khau";
            using (MeikoEntities db = new MeikoEntities())
            {
                try
                {
                    FileInfo tempExcel = new FileInfo(HttpContext.Current.Server.MapPath("~/Template/UserTemp.xlsx"));
                    using (ExcelPackage pck = new ExcelPackage(tempExcel))
                    {
                        OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault();

                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = "DANH SÁCH TÀI KHOẢN YÊU CẦU CẤP LẠI MẬT KHẨU".ToUpper();
                        ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A1"].Style.Font.Bold = true;
                        ws.Cells["A2:I2"].Merge = true;
                        ws.Cells["A2:I2"].Value = "";

                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 15;
                        ws.Column(3).Width = 25;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 15;
                        ws.Column(6).Width = 13;
                        ws.Column(7).Width = 13;
                        ws.Column(8).Width = 16;

                        ws.Cells["A3"].Value = "STT";
                        ws.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A3"].Style.WrapText = true;
                        ws.Cells["A3"].Style.Font.Bold = true;

                        ws.Cells["B3"].Value = "Mã nhân viên";
                        ws.Cells["B3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["B3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["B3"].Style.WrapText = false;
                        ws.Cells["B3"].Style.Font.Bold = true;

                        ws.Cells["C3"].Value = "Họ và  tên";
                        ws.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["C3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["C3"].Style.WrapText = true;
                        ws.Cells["C3"].Style.Font.Bold = true;

                        ws.Cells["D3"].Value = "Mã đăng nhập";
                        ws.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["D3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["D3"].Style.WrapText = true;
                        ws.Cells["D3"].Style.Font.Bold = true;

                        ws.Cells["E3"].Value = "Số điện thoại";
                        ws.Cells["E3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["E3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["E3"].Style.WrapText = true;
                        ws.Cells["E3"].Style.Font.Bold = true;

                        ws.Cells["F3"].Value = "Ngày sinh";
                        ws.Cells["F3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["F3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["F3"].Style.WrapText = true;
                        ws.Cells["F3"].Style.Font.Bold = true;

                        ws.Cells["G3"].Value = "Số CMTND";
                        ws.Cells["G3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["G3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["G3"].Style.WrapText = true;
                        ws.Cells["G3"].Style.Font.Bold = true;

                        ws.Cells["H3"].Value = "Ngày gửi yêu cầu";
                        ws.Cells["H3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["H3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["H3"].Style.WrapText = true;
                        ws.Cells["H3"].Style.Font.Bold = true;

                        var model = db.A0014.Where(x => x.tinhTrang == true).Select(x => new
                        {
                            x.A0014_ID,
                            x.A0002_ID,
                            x.A0002.hoVaTen,
                            x.A0002.maNhanVien,
                            x.A0002.anhDaiDien,
                            x.tinhTrang,
                            x.ngayGui,
                            x.A0002.ngaySinh,
                            x.A0002.soDienThoai,
                            x.A0002.CMTND,
                            x.A0002.userName
                        }).ToList();

                        var stt = 0;
                        if (model.Count > 0)
                        {
                            for (int i = 4; i < (model.Count + 4); i++)
                            {
                                stt = stt + 1;
                                int j = i - 4;
                                ws.Cells[i, 1].Value = stt.ToString();
                                ws.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[i, 2].Value = model[j].maNhanVien;
                                ws.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 3].Value = model[j].hoVaTen;
                                ws.Cells[i, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 4].Value = model[j].userName;
                                ws.Cells[i, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 5].Value = model[j].soDienThoai;
                                ws.Cells[i, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 6].Value = model[j].ngaySinh != null ? model[j].ngaySinh.Value.ToString("dd/MM/yyyy") : "";
                                ws.Cells[i, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 7].Value = model[j].CMTND;
                                ws.Cells[i, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 8].Value = model[j].ngayGui != null ? model[j].ngayGui.ToString("dd/MM/yyyy") : "";
                                ws.Cells[i, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }
                        }

                        byte[] data = pck.GetAsByteArray();
                        string path = HttpContext.Current.Server.MapPath("~/Template/" + NameForm + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx");
                        System.IO.File.WriteAllBytes(path, data);
                        var UrlFile = "/Template/" + NameForm + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx";
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

        [HttpGet]
        [Route("ExportUserNoSign")]
        public async Task<HttpResponseMessage> ExportUserNoSign()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string NameForm = "";
            NameForm = "Danh sach nguoi dung chua co tai khoan";
            using (MeikoEntities db = new MeikoEntities())
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(HttpContext.Current.Server.MapPath("~/Content/Api.xml"));
                    XmlNode node = doc.DocumentElement.SelectSingleNode("/ApiWeb/Api");
                    string url = node.ChildNodes[0]?.InnerText;

                    string apiUrl = url + "E00003/GetByStatus/1/500000/1";
                    List<UserASoftVModel> listUser = new List<UserASoftVModel>();
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responsedata = await client.GetAsync(apiUrl);
                    if (responsedata.IsSuccessStatusCode)
                    {
                        var data = await responsedata.Content.ReadAsStringAsync();
                        listUser = JsonConvert.DeserializeObject<List<UserASoftVModel>>(data);
                    }

                    var tables = db.A0002.Select(x => new
                    {
                        x.A0002_ID,
                        x.APKID
                    });
                    var ListUID = tables.Select(x => x.APKID).ToList();
                    var model = listUser.Where(x => ListUID.Contains(x.id) == false).OrderBy(x => x.manhansu).ToList();

                    FileInfo tempExcel = new FileInfo(HttpContext.Current.Server.MapPath("~/Template/UserTemp.xlsx"));
                    using (ExcelPackage pck = new ExcelPackage(tempExcel))
                    {
                        OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault();

                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = "DANH SÁCH NGƯỜI DÙNG CHƯA CÓ TÀI KHOẢN".ToUpper();
                        ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A1"].Style.Font.Bold = true;
                        ws.Cells["A2:I2"].Merge = true;
                        ws.Cells["A2:I2"].Value = "";

                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 15;
                        ws.Column(3).Width = 25;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 15;
                        ws.Column(6).Width = 13;
                        ws.Column(7).Width = 13;

                        ws.Cells["A3"].Value = "STT";
                        ws.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["A3"].Style.WrapText = true;
                        ws.Cells["A3"].Style.Font.Bold = true;

                        ws.Cells["B3"].Value = "Mã nhân viên";
                        ws.Cells["B3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["B3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["B3"].Style.WrapText = false;
                        ws.Cells["B3"].Style.Font.Bold = true;

                        ws.Cells["C3"].Value = "Họ và  tên";
                        ws.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["C3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["C3"].Style.WrapText = true;
                        ws.Cells["C3"].Style.Font.Bold = true;

                        ws.Cells["D3"].Value = "Mã đăng nhập";
                        ws.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["D3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["D3"].Style.WrapText = true;
                        ws.Cells["D3"].Style.Font.Bold = true;

                        ws.Cells["E3"].Value = "Số điện thoại";
                        ws.Cells["E3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["E3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["E3"].Style.WrapText = true;
                        ws.Cells["E3"].Style.Font.Bold = true;

                        ws.Cells["F3"].Value = "Ngày sinh";
                        ws.Cells["F3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["F3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["F3"].Style.WrapText = true;
                        ws.Cells["F3"].Style.Font.Bold = true;

                        ws.Cells["G3"].Value = "Số CMTND";
                        ws.Cells["G3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["G3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells["G3"].Style.WrapText = true;
                        ws.Cells["G3"].Style.Font.Bold = true;

                        var stt = 0;
                        if (model.Count > 0)
                        {
                            for (int i = 4; i < (model.Count + 4); i++)
                            {
                                stt = stt + 1;
                                int j = i - 4;
                                ws.Cells[i, 1].Value = stt.ToString();
                                ws.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                ws.Cells[i, 2].Value = model[j].manhansu;
                                ws.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 3].Value = model[j].hodem + " " + model[j].ten;
                                ws.Cells[i, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 4].Value = model[j].manhansu;
                                ws.Cells[i, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 5].Value = model[j].dienthoai_didong;
                                ws.Cells[i, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 6].Value = model[j].ngaysinh != null ? model[j].ngaysinh.Value.ToString("dd/MM/yyyy") : "";
                                ws.Cells[i, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                ws.Cells[i, 7].Value = model[j].cmtnd_so;
                                ws.Cells[i, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }
                        }

                        byte[] data = pck.GetAsByteArray();
                        string path = HttpContext.Current.Server.MapPath("~/Template/" + NameForm + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx");
                        System.IO.File.WriteAllBytes(path, data);
                        var UrlFile = "/Template/" + NameForm + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx";
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

        [HttpPost]
        [Route("R1_UserGetByListNoSign")]
        public async Task<HttpResponseMessage> R1_UserGetByListNoSign()
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

                XmlDocument doc = new XmlDocument();
                doc.Load(HttpContext.Current.Server.MapPath("~/Content/Api.xml"));
                XmlNode node = doc.DocumentElement.SelectSingleNode("/ApiWeb/Api");
                string url = node.ChildNodes[0]?.InnerText;

                string apiUrl = url + "E00003/GetByStatus/1/500000/1";
                List<UserASoftVModel> listUser = new List<UserASoftVModel>();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responsedata = await client.GetAsync(apiUrl);
                if (responsedata.IsSuccessStatusCode)
                {
                    var data = await responsedata.Content.ReadAsStringAsync();
                    listUser = JsonConvert.DeserializeObject<List<UserASoftVModel>>(data);
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var tables = db.A0002.Select(x => new
                {
                    x.A0002_ID,
                    x.APKID
                });
                var ListUID = tables.Select(x => x.APKID).ToList();
                var qrs = listUser.Where(x => ListUID.Contains(x.id) == false).OrderBy(x => x.manhansu).Skip(pz * (p - 1)).Take(pz).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R1_UserGetByListResetPass")]
        public async Task<HttpResponseMessage> R1_UserGetByListResetPass()
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
                var tables = db.A0014.Where(x => x.tinhTrang == true).Select(x => new
                {
                    x.A0014_ID,
                    x.A0002_ID,
                    x.A0002.hoVaTen,
                    x.A0002.maNhanVien,
                    x.A0002.anhDaiDien,
                    x.tinhTrang,
                    x.ngayGui
                });
                var qrs = tables.OrderBy(x => x.ngayGui).Skip(pz * (p - 1)).Take(pz).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = qrs, total = tables.Count() }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("BindListUser")]
        public HttpResponseMessage BindListUser()
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var cds = db.A0001.Where(a => a.A0001_ID != null).Select(a => new { a.A0001_ID, a.tenRole, a.thuTu }).OrderBy(a => a.thuTu).ToList();
                var users = db.A0002.Where(u => u.tinhTrang != 3 || u.tinhTrang != 9).OrderBy(u => u.hoVaTen).Select(u => new
                {
                    u.A0002_ID,
                    u.soDienThoai,
                    anhDaiDien = temp + u.anhDaiDien,
                    u.hoVaTen,
                    u.A0004_ID,
                    tenPhongBan = db.A0004.FirstOrDefault(x => x.A0004_ID == u.A0004_ID) != null ? db.A0004.FirstOrDefault(x => x.A0004_ID == u.A0004_ID).tenPhongBan : ""
                    ,
                    RoleID = u.A0003.Select(x => x.A0001_ID).ToList()
                }).ToList();
                var models = db.A0004.Where(u => u.tinhTrang == true).Select(u => new { u.A0004_ID, u.tenPhongBan, u.thuTu, u.IDCha }).OrderBy(u => u.thuTu).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(new { users = users, models = models, cds = cds }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpGet]
        [Route("R1_UserGetByID/{id}")]
        public HttpResponseMessage R1_UserGetByID(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var User = db.A0002.Where(x => x.A0002_ID == Id).Select(x => new
                {
                    x.A0002_ID,
                    x.A0004_ID,
                    x.hoVaTen,
                    x.Email,
                    x.soDienThoai,
                    x.userName,
                    x.passWord,
                    x.passWordRandom,
                    anhDaiDien = temp + x.anhDaiDien,
                    x.diaChi,
                    x.ngaySinh,
                    x.tinhTrang,
                    x.ngayVao,
                    x.CMTND,
                    x.kieuUser,
                    x.IsPosition,
                    A0001_ID = db.A0003.FirstOrDefault(r => r.A0002_ID == x.A0002_ID) != null ? db.A0003.FirstOrDefault(r => r.A0002_ID == x.A0002_ID).A0001_ID : null
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(User));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_AddUser")]
        public HttpResponseMessage R2_AddUser()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var user = JsonConvert.DeserializeObject<A0002>(httpRequest["user"]);
                if (httpRequest.Files.Count > 0)
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = httpRequest.Files[i];
                        string genkey = helper.GenKey();
                        string ext = helper.GetFileExtension(genkey + file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                        user.anhDaiDien = "/Portals/images/Users/" + genkey + file.FileName;
                    }
                }
                else
                {
                    user.anhDaiDien = null;
                }

                using (MeikoEntities db = new MeikoEntities())
                {
                    var check = db.A0002.FirstOrDefault(x => x.userName == user.userName);
                    if (check != null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 2 }));
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return response;
                    }
                    string depass = helper.Encrypt("os", user.passWord);
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    var obj = new A0002();
                    obj.A0002_ID = helper.GenKey();
                    obj.hoVaTen = user.hoVaTen;
                    obj.A0004_ID = user.A0004_ID;
                    obj.Email = user.Email != null ? user.Email : "";
                    obj.soDienThoai = user.soDienThoai != null ? user.soDienThoai : "";
                    obj.userName = user.userName;
                    obj.passWord = depass;
                    obj.passWordRandom = user.passWordRandom;
                    obj.anhDaiDien = user.anhDaiDien != null ? user.anhDaiDien : "Content/noimages.gif";
                    obj.diaChi = user.diaChi != null ? user.diaChi : "";
                    obj.ngaySinh = user.ngaySinh;
                    obj.tinhTrang = user.tinhTrang;
                    obj.ngayVao = user.ngayVao;
                    obj.CMTND = user.CMTND;
                    obj.kieuUser = user.kieuUser;
                    obj.IsPosition = user.IsPosition;
                    db.A0002.Add(obj);

                    if (user.APKID != null && user.APKID != "null")
                    {
                        var roleuser = new A0003();
                        roleuser.A0003_ID = helper.GenKey();
                        roleuser.A0001_ID = user.APKID;
                        roleuser.A0002_ID = obj.A0002_ID;
                        db.A0003.Add(roleuser);
                    }

                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                }
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        [HttpPost]
        [Route("R3_UpdateUser")]
        public HttpResponseMessage R3_UpdateUser()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var user = JsonConvert.DeserializeObject<A0002>(httpRequest["user"]);
                if (httpRequest.Files.Count > 0)
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = httpRequest.Files[i];
                        string genkey = helper.GenKey();
                        string ext = helper.GetFileExtension(genkey + file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath("~/Portals/images/Users/" + genkey + file.FileName));
                        user.anhDaiDien = "/Portals/images/Users/" + genkey + file.FileName;
                    }
                }
                using (MeikoEntities db = new MeikoEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    var obj = db.A0002.Find(user.A0002_ID);
                    if (obj != null)
                    {
                        obj.hoVaTen = user.hoVaTen;
                        obj.A0004_ID = user.A0004_ID;
                        obj.Email = user.Email != null ? user.Email : "";
                        obj.soDienThoai = user.soDienThoai != null ? user.soDienThoai : "";
                        if (obj.passWord != user.passWord)
                        {
                            string depass = helper.Encrypt("os", user.passWord);
                            obj.passWord = depass;
                        }
                        if (obj.anhDaiDien != user.anhDaiDien)
                        {
                            user.anhDaiDien = user.anhDaiDien.Replace(temp, "");
                            obj.anhDaiDien = user.anhDaiDien != null ? user.anhDaiDien : "Content/noimages.gif";
                        }
                        obj.diaChi = user.diaChi != null ? user.diaChi : "";
                        obj.passWordRandom = user.passWordRandom;
                        obj.ngaySinh = user.ngaySinh;
                        obj.tinhTrang = user.tinhTrang;
                        obj.ngayVao = user.ngayVao;
                        obj.CMTND = user.CMTND;
                        obj.kieuUser = user.kieuUser;
                        obj.IsPosition = user.IsPosition;
                        db.SaveChanges();
                    }
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
            }
            catch (Exception)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        [HttpPost]
        [Route("R4_DeleteUser")]
        public HttpResponseMessage R4_DeleteUser(List<string> Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var user = db.A0002.Where(x => Id.Contains(x.A0002_ID) == true).ToList();
                if (user.Count > 0)
                {
                    db.A0002.RemoveRange(user);
                    db.SaveChanges();
                }
                return response;
            }
        }

        public HttpResponseMessage ChangePassword(A0002 user)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string uid = user.A0002_ID;
                string password = helper.Encrypt("os", user.passWordRandom); // lưu mật khẩu cũ
                string pwnew = helper.Encrypt("os", user.passWord); // lưu mật khẩu mới
                if (db.A0002.Count(a => a.A0002_ID == uid && a.passWord == password) == 0)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1, ms = "* Mật khẩu cũ không chính xác bạn vui lòng nhập lại !" }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                try
                {
                    var obj = db.A0002.FirstOrDefault(x => x.A0002_ID == uid && x.passWord == password);
                    obj.passWord = pwnew;
                    db.SaveChanges();
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0, ms = "* Đổi mật khẩu thành công !" }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1, ms = "* Đổi mật khẩu không thành công !" }));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                return response;
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public HttpResponseMessage ResetPassword(A0002 user)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var usercheck = db.A0002.FirstOrDefault(x => x.userName == user.userName && x.CMTND == user.CMTND);
                if (usercheck != null)
                {
                    var checkresetpass = db.A0014.FirstOrDefault(x => x.A0002_ID == usercheck.A0002_ID && x.tinhTrang == true);
                    if (checkresetpass != null)
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 1 }));
                    }
                    else
                    {
                        var obj = new A0014();
                        obj.A0014_ID = helper.GenKey();
                        obj.A0002_ID = usercheck.A0002_ID;
                        obj.ngayGui = DateTime.Now;
                        obj.tinhTrang = true;
                        db.A0014.Add(obj);
                        db.SaveChanges();
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 0 }));
                    }
                }
                else
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = 2 }));
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R2_ImportUser")]
        public async Task<HttpResponseMessage> R2_ImportUser(List<UserASoftVModel> user)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(HttpContext.Current.Server.MapPath("~/Content/Api.xml"));
                XmlNode node = doc.DocumentElement.SelectSingleNode("/ApiWeb/Api");
                string url = node.ChildNodes[0]?.InnerText;
                if (url == null)
                {
                    url = "http://192.84.100.207/";
                }

                string apiUrl = url + "E00003/GetByStatus/1/100000/1";
                List<UserASoftVModel> listUser = new List<UserASoftVModel>();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responsedata = await client.GetAsync(apiUrl);
                if (responsedata.IsSuccessStatusCode)
                {
                    var data = await responsedata.Content.ReadAsStringAsync();
                    listUser = JsonConvert.DeserializeObject<List<UserASoftVModel>>(data);
                }
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    var User = db.A0002.Select(x => x.APKID).ToList();
                    var ListUserAdd = listUser.Where(x => User.Contains(x.id) == false).ToList();
                    foreach (var item in ListUserAdd)
                    {
                        string password = helper.Encrypt("os", "123456");
                        var obj = new A0002();
                        obj.A0002_ID = helper.GenKey();
                        obj.APKID = item.id;
                        obj.A0004_ID = item.phong_id != null ? item.phong_id : "";
                        if (item.congdoan_id != null)
                        {
                            obj.A0004_ID = item.congdoan_id;
                        }
                        else if (item.ban_id != null)
                        {
                            obj.A0004_ID = item.ban_id;
                        }
                        else if (item.phong_id != null)
                        {
                            obj.A0004_ID = item.phong_id;
                        }
                        else
                        {
                            obj.A0004_ID = "";
                        }
                        obj.Ana03Name = "";
                        obj.hoVaTen = item.hodem + " " + item.ten;
                        obj.Email = "";
                        obj.soDienThoai = item.dienthoai_didong != null ? item.dienthoai_didong : "";
                        obj.userName = item.manhansu;
                        obj.passWord = password;
                        obj.anhDaiDien = "Content/noimages.gif";
                        obj.diaChi = item.diachithuongtru != null ? item.diachithuongtru : "";
                        obj.ngaySinh = item.ngaysinh;
                        obj.tinhTrang = item.tinhtrangnhansu;
                        obj.CMTND = item.cmtnd_so != null ? item.cmtnd_so : "";
                        obj.passWordRandom = "123456";
                        obj.maNhanVien = item.manhansu;
                        db.A0002.Add(obj);
                    }

                    var ListUserUpdate = listUser.Where(x => User.Contains(x.id) == true).ToList();
                    foreach (var item in ListUserUpdate)
                    {
                        if (item.dienthoai_didong == null)
                        {
                            item.dienthoai_didong = "";
                        }
                        if (item.diachithuongtru == null)
                        {
                            item.diachithuongtru = "";
                        }
                        if (item.cmtnd_so == null)
                        {
                            item.cmtnd_so = "";
                        }
                        if (item.phong_id == null)
                        {
                            item.phong_id = "";
                        }

                        var obj = db.A0002.FirstOrDefault(x => x.APKID == item.id && ((x.diaChi != item.diachithuongtru) || (x.soDienThoai != item.dienthoai_didong) || (x.CMTND != item.cmtnd_so) || (x.ngaySinh != item.ngaysinh) || (x.maNhanVien != item.manhansu) || (x.A0004_ID != item.phong_id && x.A0004_ID != item.ban_id && x.A0004_ID != item.congdoan_id)));
                        if (obj != null)
                        {
                            obj.APKID = item.id;
                            obj.Ana03Name = "";
                            obj.hoVaTen = item.hodem + " " + item.ten;
                            obj.soDienThoai = item.diachithuongtru != null ? item.diachithuongtru : "";
                            obj.diaChi = item.diachithuongtru != null ? item.diachithuongtru : "";
                            obj.CMTND = item.cmtnd_so != null ? item.cmtnd_so : "";
                            obj.ngaySinh = item.ngaysinh;
                            obj.tinhTrang = item.tinhtrangnhansu;
                            if (item.congdoan_id != null)
                            {
                                obj.A0004_ID = item.congdoan_id;
                            }
                            else if (item.ban_id != null)
                            {
                                obj.A0004_ID = item.ban_id;
                            }
                            else if (item.phong_id != null)
                            {
                                obj.A0004_ID = item.phong_id;
                            }
                            else
                            {
                                obj.A0004_ID = "";
                            }
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
        [Route("ResetPasswordRamdom")]
        public HttpResponseMessage ResetPasswordRamdom(List<A0014> user)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                foreach (var u in user)
                {
                    var checku = db.A0014.FirstOrDefault(x => x.A0014_ID == u.A0014_ID && x.tinhTrang == true);
                    if (checku != null)
                    {
                        var obj = db.A0002.FirstOrDefault(x => x.A0002_ID == checku.A0002_ID);
                        if (obj != null)
                        {
                            var password = helper.Encrypt("os", helper.GenPassword(6));
                            var passwordRamdom = helper.Encrypt("os", helper.GenPassword(6));
                            obj.passWord = password;
                            obj.passWordRandom = passwordRamdom;
                            checku.tinhTrang = false;
                            checku.ngayResetMatKhau = DateTime.Now;
                        }
                    }
                }
                db.SaveChanges();
                return response;
            }
        }

        [HttpGet]
        [Route("R1_UserGetRolePermisstion/{id}")]
        public HttpResponseMessage R1_UserGetRolePermisstion(string Id)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var roleuser = db.A0042.Where(x => x.A0002_ID == Id).Select(x => new
                {
                    x.A0001_ID,
                    x.A0042_ID,
                    x.A0002_ID,
                    x.A0001.maRole,
                    x.A0001.tenRole
                }).ToList();

                var Role = db.A0001.Where(x => x.tinhTrang == true).Select(x => new
                {
                    x.A0001_ID,
                    x.maRole,
                    x.tenRole
                }).ToList();

                response.Content = new StringContent(JsonConvert.SerializeObject(new { data = roleuser, data2 = Role }));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [HttpPost]
        [Route("R3_AddRoleUserPermisstion")]
        public HttpResponseMessage R3_AddRoleUserPermisstion()
        {
            var httpRequest = HttpContext.Current.Request;
            using (MeikoEntities db = new MeikoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                try
                {
                    string A0002_ID = httpRequest["UserID"];
                    List<string> Role = JsonConvert.DeserializeObject<List<string>>(httpRequest["RoleList"]);
                    foreach (var item in Role)
                    {
                        var a0042 = new A0042();
                        a0042.A0042_ID = helper.GenKey();
                        a0042.A0001_ID = item;
                        a0042.A0002_ID = A0002_ID;
                        db.A0042.Add(a0042);
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
        [Route("R4_DeleteRoleUser")]
        public HttpResponseMessage R4_DeleteRoleUser(List<A0042> ListUser)
        {
            using (MeikoEntities db = new MeikoEntities())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                foreach (var u in ListUser)
                {
                    var objcheck = db.A0042.FirstOrDefault(x => x.A0042_ID == u.A0042_ID);
                    if(objcheck != null)
                    {
                        db.A0042.Remove(objcheck);
                    }
                }
                db.SaveChanges();
                return response;
            }
        }

        
    }
}
