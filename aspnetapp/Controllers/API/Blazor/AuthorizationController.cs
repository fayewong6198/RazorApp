using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetapp.Data;
using aspnetapp.Models;

namespace aspnetapp.Controllers.API.Blazor
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorizationController(ApplicationDbContext context)
        {
            _context = context;
        }

    // GET: api/FeedItems
        [HttpGet("getMe")]
        public async Task<ActionResult<ApplicationUser>> GetFeedItem()
        {
            return await _context.ApplicationUser.FirstAsync();
        }

        
    }
}