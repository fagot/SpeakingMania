using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.Models
{
    public class RoomCreatingException:Exception
    {
        public RoomCreatingException(string message)
            :base(message)
        {
        }
    }
}