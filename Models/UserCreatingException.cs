using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.Models
{
    public class UserCreatingException:Exception
    {
        public UserCreatingException(string message) : base(message)
        {
            
        }
    }
}