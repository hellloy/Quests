using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Server.Models;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quests.Server.Controllers
{
    [ApiController]
    public class CloudPaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CloudPaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        [Route("/webHooks/cloudPayments/pay")]
        [HttpPost]
        public async Task<IActionResult> Result()
        {
            CloudPaymentsModel result = new CloudPaymentsModel();
            using (var reader = new StreamReader(HttpContext.Request.Body)) {
                var body = await reader.ReadToEndAsync();
                var queryString = HttpUtility.ParseQueryString(body);
                string json = JsonConvert.SerializeObject(queryString.Cast<string>().ToDictionary(k => k, v => queryString[v]));
                result = JsonConvert.DeserializeObject<CloudPaymentsModel>(json);
                //result = JsonConvert.DeserializeObject<CloudPaymentsModel>(body);
            }

            int invoiceId = Convert.ToInt32(result.InvoiceId);
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null)
            {
                return Ok("{'code':10}");
            }

            Double temp;
            Boolean isOk = Double.TryParse(result.Amount.Replace(".",","), out temp);
            Int32 value = isOk ? (Int32) temp : 0;

            if (invoice.Amount != value)
            {
                return Ok("{'code':12}");
            }

            if (invoice.UserId != result.AccountId)
            {
                return Ok("{'code':11}");
            }

            invoice.InvoiceStatus = InvoiceStatus.Payment;
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == invoice.UserId);
            if (user != null) user.Points += Convert.ToInt32(value);
            await _userManager.UpdateAsync(user);
            _context.Entry(invoice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("{'code':0}");

        }


    }

}
