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
    
    public partial class placements
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public placements()
        {
            this.placements_movements = new HashSet<placements_movements>();
        }
    
        public int id { get; set; }
        public string description { get; set; }
        public int auxiliary_id { get; set; }
        public System.DateTime date { get; set; }
        public int currency_type { get; set; }
        public decimal exchange_rate { get; set; }
        public string state { get; set; }
    
        public virtual currencies_types currencies_types { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<placements_movements> placements_movements { get; set; }
    }
}
