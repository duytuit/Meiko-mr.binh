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
    
    public partial class A0007
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A0007()
        {
            this.A0005 = new HashSet<A0005>();
        }
    
        public string A0007_ID { get; set; }
        public string A0006_ID { get; set; }
        public string IDCha { get; set; }
        public string tenMenu { get; set; }
        public string tenRutGon { get; set; }
        public string anhDoc { get; set; }
        public string Link { get; set; }
        public string maCode { get; set; }
        public string Icon { get; set; }
        public int thuThu { get; set; }
        public bool tinhTrang { get; set; }
        public Nullable<int> maCount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<A0005> A0005 { get; set; }
        public virtual A0006 A0006 { get; set; }
    }
}