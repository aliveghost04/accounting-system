//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoPropietaria
{
    using System;
    using System.Collections.Generic;
    
    public partial class countables_accounts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public countables_accounts()
        {
            this.countables_accounts1 = new HashSet<countables_accounts>();
            this.wholesale = new HashSet<wholesale>();
        }
    
        public int id { get; set; }
        public string description { get; set; }
        public int account_type { get; set; }
        public bool allow_transaction { get; set; }
        public int level { get; set; }
        public Nullable<int> account_major { get; set; }
        public decimal balance { get; set; }
        public bool state { get; set; }
    
        public virtual account_types account_types { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<countables_accounts> countables_accounts1 { get; set; }
        public virtual countables_accounts countables_accounts2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wholesale> wholesale { get; set; }
    }
}