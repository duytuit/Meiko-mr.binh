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
    
    public partial class A0017
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A0017()
        {
            this.A0019 = new HashSet<A0019>();
            this.A0021 = new HashSet<A0021>();
        }
    
        public string A0017_ID { get; set; }
        public string maNhomNguoiKy { get; set; }
        public string tenNhomNguoiKy { get; set; }
        public string moTaNhomNguoiKy { get; set; }
        public int kieuNhomNguoiKy { get; set; }
        public int STT { get; set; }
        public bool trangThai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0019> A0019 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0021> A0021 { get; set; }
    }
}
