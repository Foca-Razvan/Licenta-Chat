//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class History
    {
        public int IdHistory { get; set; }
        public string Conversation { get; set; }
        public int UserIdUser { get; set; }
        public int UserIdUser1 { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
