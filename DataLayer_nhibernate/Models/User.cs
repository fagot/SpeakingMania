using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.DataLayer.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string UserIdentity { get; set; }
        public virtual string UserName { get; set; }
        public virtual Room Room { get; set; }
        
    }
}