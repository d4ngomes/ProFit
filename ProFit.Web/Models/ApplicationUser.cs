using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
    }
}
