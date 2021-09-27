using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace aspnetapp.Models
{
    [Index(nameof(Token))]
    public class ApplicationUserToken
    {
        public int Id {get;set;}
        public string ApplicationUserId {get;set;}
        public string Token {get;set;}
        public ApplicationUser ApplicationUser {get;set;}
        public DateTime CreatedAt {get;set;}

        public ApplicationUserToken() {
            this.CreatedAt = DateTime.Now;
        }
        
    }
}