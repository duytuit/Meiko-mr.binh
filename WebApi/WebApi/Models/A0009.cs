//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class A0009
    {
        public string A0009_ID { get; set; }
        public string A0008_ID { get; set; }
        public string tenTieuDe { get; set; }
        public string ghiChu { get; set; }
        public string maCode { get; set; }
        public string Module { get; set; }
    
        public virtual A0008 A0008 { get; set; }
    }
}
