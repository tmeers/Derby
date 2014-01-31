using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Infrastructure
{
    public enum OwnershipType
    {
        None = 0,
        Owner,
        Contributor,
        Guest
    }
}