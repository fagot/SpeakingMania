using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.DataLayer.Models
{
    public class Room
    {
        public virtual int Id { get; set; }
        public virtual string RoomIdentity { get; set; }
        public virtual string RoomName { get; set; }
        public virtual IList<User> Users { get; set; }
        public virtual User RoomOwner { get; set; }
    }
}