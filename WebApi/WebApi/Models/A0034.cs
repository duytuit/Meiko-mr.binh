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
    
    public partial class A0034
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A0034()
        {
            this.A0036 = new HashSet<A0036>();
            this.A0037 = new HashSet<A0037>();
        }
    
        public string A0034_ID { get; set; }
        public string Parent_ID { get; set; }
        public string phongBanMapID { get; set; }
        public string maPhongBan { get; set; }
        public string tenPhongBan { get; set; }
        public Nullable<bool> trangThai { get; set; }
        public int kieuPhongBan { get; set; }
        public Nullable<int> STT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0036> A0036 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0037> A0037 { get; set; }
    }
}