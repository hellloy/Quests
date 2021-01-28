using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quests.Server.Data;
using Quests.Server.Models;
using Quests.Server.Payment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quests.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET api/<PaymentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }

            var invoice = new Invoice
            {
                UserId = userId,
                Amount = id,
                InvoiceStatus = InvoiceStatus.NotPayment
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            var result = RbkMoney.CreateOrder(id, invoice.Id);
            if (result == "Bad request")
            {
                return BadRequest();
            }

            return result;
            //return RoboKassa.GenerateAuthLink(id, invoice.Id);
        }

       
    }
}
