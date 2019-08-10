using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UserASoftVModel
    {
        public string id { get; set; }
        public string phong_id { get; set; }
        public string ban_id { get; set; }
        public string congdoan_id { get; set; }        
        public string manhansu { get; set; }   
        public string hodem { get; set; }
        public string ten { get; set; } 
        public string dienthoai_didong { get; set; }
        public string diachithuongtru { get; set; }
        public int gioitinh { get; set; }
        public string cmtnd_so { get; set; }
        public DateTime? ngaysinh { get; set; } 
        public string anhdaidien { get; set; }
        public string email { get; set; }
        public int tinhtrangnhansu { get; set; }
        public DateTime? ngayvaocongty { get; set; }        
    }
}