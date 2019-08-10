using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class PhongBanVModel
    {
        public string id { get; set; }
        public string bophan_diachi { get; set; }
        public string bophan_dienthoai { get; set; }
        public string bophan_ma { get; set; }
        public string bophan_ten { get; set; }
        public string congty_id { get; set; }
        public string idcha { get; set; }
        public string logo { get; set; }
        public string muc { get; set; }
        public int thutu { get; set; }
        public int tinhtrang { get; set; } 
    }
}