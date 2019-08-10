using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Token
    {
        public string Users_ID { get; set; }
        public string displayName { get; set; }
        public string anhDaiDien { get; set; }
        public string UserCode { get; set; }
        public string Department { get; set; }
    }
}