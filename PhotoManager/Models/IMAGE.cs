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
    
    public partial class IMAGE
    {
        public int IDIMAGE { get; set; }
        public int IDALBUM { get; set; }
        public string NAMEIMAGE { get; set; }
        public string TITLEIMAGE { get; set; }
        public string DESCRIPTIONIMAGE { get; set; }
    
        public virtual ALBUM ALBUM { get; set; }
    }
}
