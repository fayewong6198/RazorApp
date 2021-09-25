using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace aspnetapp.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string Avatar {get;set;}

    }
}