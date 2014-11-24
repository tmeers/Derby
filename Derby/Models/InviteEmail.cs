using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Postal;

namespace Derby.Models
{
    public class InviteEmail : Email
    {
        public string To { get; set; }
        public string UniqueCode { get; set; }
    }
}