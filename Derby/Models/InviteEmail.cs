using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class InviteEmail
    {
        public string To { get; set; }
        public string UniqueCode { get; set; }
        public string Message { get; set; }
    }
}