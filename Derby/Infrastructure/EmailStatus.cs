using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Infrastructure
{
    public enum EmailStatus
    {
        Sent = 0,
        Failed = 1, 
        Resent = 2,
        Pending = 3
    }
}