using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.DataLayer.Models
{
    public class Connection
    {
        public virtual int Id { get; set; }
        public virtual string ConnectionId { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual Room Room { get; set; }
    }
}