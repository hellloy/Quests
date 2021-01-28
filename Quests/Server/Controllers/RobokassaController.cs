using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Server.Models;
using Quests.Server.Payment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quests.Server.Controllers
{
    [ApiController]
    public class RobokassaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RobokassaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("/payments/result")]

        public async Task<IActionResult> Result([FromForm] RobokassaResult result)
        {
            bool isCallbackRequestValid = RoboKassa.CheckResult(result);
            if (isCallbackRequestValid)
            {
                try
                {
                    var invoice = await _context.Invoices.FindAsync(result.InvId);
                    invoice.InvoiceStatus = InvoiceStatus.Payment;

                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == invoice.UserId);

                    int number = int.Parse(result.OutSum.Contains('.') ? result.OutSum.Substring(0, result.OutSum.IndexOf('.')) : result.OutSum);

                    if (user != null) user.Points += number;

                    await _userManager.UpdateAsync(user);

                    _context.Entry(invoice).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return Content($"OK{result.InvId}");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest();
        }

        
    }

}
