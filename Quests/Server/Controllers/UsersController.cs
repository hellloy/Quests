using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Server.Models;
using Quests.Shared.VM;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserVm>>> GetUserVm()
        {
            var users = await _context.Users.Select(x => new UserVm
            {
                Id = x.Id,
                Points = x.Points,
                Name = x.FirstName,
                ActiveQuestId = x.ActiveQuest,
                Img = x.Img,
                LatestSign = x.LastLogin,
                Phone = x.PhoneNumber,
                UserName = x.UserName,
                ActiveStateStatus = x.ActiveQuestStatus,
                RoleName = x.RoleName
            }).ToListAsync();
            return users;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserVm(string id, UserVm userVm)
        {
            if (id != userVm.Id)
            {
                return BadRequest();
            }

            var user =  await _userManager.FindByIdAsync(id);
            user.Img = userVm.Img;
            user.FirstName = userVm.Name;
            user.Points = userVm.Points;
            user.PhoneNumber = userVm.Phone;
            await _userManager.UpdateAsync(user);

            await _userManager.RemoveFromRolesAsync(user, new List<string> {"Admin", "Member"});
            await _userManager.AddToRoleAsync(user, userVm.RoleName);
            
            return NoContent();
        }

        

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserVm>> DeleteUserVm(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return NotFound();
            }
            
            await _userManager.DeleteAsync(user);
           return NoContent();
        }

      
    }
}
