//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhotoManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ALBUM
    {
        public ALBUM()
        {
            this.IMAGEs = new HashSet<IMAGE>();
        }
    
        public int IDALBUM { get; set; }
        public string IDUSER { get; set; }
        public string NAMEALBUM { get; set; }
        public string DESCRIPTION_ { get; set; }
    
        public virtual USER USER { get; set; }
        public virtual ICollection<IMAGE> IMAGEs { get; set; }
    }
}
