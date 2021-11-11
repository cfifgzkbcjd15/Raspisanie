using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raspisanie.Models
{
    public class User : IdentityUser
    {
        public string FIO { get; set; }
        public string EducationalInstitutions { get; set; }
        public string Faculty{ get; set; }
        //public string Faculty { get; set; }
    }
}
