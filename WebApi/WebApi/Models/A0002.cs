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
    
    public partial class A0002
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A0002()
        {
            this.A0014 = new HashSet<A0014>();
            this.A0019 = new HashSet<A0019>();
            this.A0025 = new HashSet<A0025>();
            this.A0028 = new HashSet<A0028>();
            this.A0029 = new HashSet<A0029>();
            this.A0030 = new HashSet<A0030>();
            this.A0037 = new HashSet<A0037>();
            this.A0042 = new HashSet<A0042>();
            this.A0003 = new HashSet<A0003>();
            this.A0005 = new HashSet<A0005>();
            this.A0010 = new HashSet<A0010>();
        }
    
        public string A0002_ID { get; set; }
        public string A0004_ID { get; set; }
        public string APKID { get; set; }
        public string maNhanVien { get; set; }
        public string Ana03Name { get; set; }
        public string hoVaTen { get; set; }
        public string Email { get; set; }
        public string soDienThoai { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public string anhDaiDien { get; set; }
        public string diaChi { get; set; }
        public Nullable<System.DateTime> ngaySinh { get; set; }
        public int tinhTrang { get; set; }
        public string CMTND { get; set; }
        public string passWordRandom { get; set; }
        public Nullable<System.DateTime> ngayVao { get; set; }
        public Nullable<System.DateTime> ngayCapNhap { get; set; }
        public int kieuUser { get; set; }
        public int IsPosition { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0014> A0014 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0019> A0019 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0025> A0025 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0028> A0028 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0029> A0029 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0030> A0030 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0037> A0037 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0042> A0042 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0003> A0003 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0005> A0005 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0010> A0010 { get; set; }
    }
}
