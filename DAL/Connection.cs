//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpeakingMania.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Connection
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public Nullable<int> UserId { get; set; }
        public int RoomId { get; set; }
        public Nullable<bool> Connected { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
