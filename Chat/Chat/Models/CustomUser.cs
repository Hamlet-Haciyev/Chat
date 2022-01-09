using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class CustomUser :IdentityUser
    {
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(40)]
        public string Surname { get; set; }
    }
}
