using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// Add Custom properties to the ASP_NET_USERs table here
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }


    }
}
