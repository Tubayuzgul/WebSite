//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyWebSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Resimler
    {
        public int ResimId { get; set; }
        public string ResimUrl { get; set; }
        public Nullable<int> ResimMakaleId { get; set; }
    
        public virtual Makaleler Makaleler { get; set; }
    }
}
