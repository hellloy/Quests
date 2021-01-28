using Microsoft.AspNetCore.Mvc;
using Quests.Server.Payment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Quests.Server.Models;
using Quests.Server.Models.Rbk;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    public class RbkMoneyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RbkMoneyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("/payments/rbk/result")]

        public async Task<IActionResult> ResultRbk()
        {
            try
            {
                // Достаем сигнатуру из заголовка
                var signatureFromHeader = GetSignatureFromHeader(Request.Headers["Content-Signature"]);

                // Декодируем данные
                byte[] decodedSignature = Convert.FromBase64String(signatureFromHeader.Replace("-","+").Replace("_","/"));

                byte[] content = await GetRawBodyBytesAsync(Request);


                //проверка подписи
                const string key = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArm0Y0ZQUQYQYtQSh9m0ALSayFHMEKXJP0F9ndN/cQcV3gxEAg/1FD744+VNlOkIvNcxaZp5JmysjXpBt0ObfF+0YahADxHCe/Xqo9LDaEoQY2/WxtC54yomElGF2UpQx74M3Eaf6l5Y/UTPlwZbl9qoicOaTCxn3DSuUhPRMvsTzT+HnNmwQn4N4xDSPXmFjvivC8wm3JQFqIR4J99SsTMZXDo14v1xw/OvaQD+kfDj0Z1ZZ8wmiMdgYDHkjeJL5PeCVkUP/k3OiTE9zNnouA2ryIafMiL2RvtE90BwirTTukW9G+bwqDuUgfV10n6RZBeyoGPWwRRSkgRLEretlVQIDAQAB-----END PUBLIC KEY-----";
                
                var rsa = RSA.Create();

                byte[] pubKey = ReadPemPublicKey(key);

                rsa.ImportSubjectPublicKeyInfo(pubKey, out _);

                bool check = rsa.VerifyData(content, decodedSignature, HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                if (!check)
                {
                    return BadRequest();
                }

                //проверка события
                string jsonStr = Encoding.UTF8.GetString(content);
                RbkEventModel eventModel = JsonConvert.DeserializeObject<RbkEventModel>(jsonStr);

                if (eventModel.eventType == "PaymentProcessed")
                {
                    var number = Convert.ToInt32(eventModel.invoice.metadata.order_id);
                    var invoice = await _context.Invoices.FindAsync(number);
                    if (invoice.InvoiceStatus == InvoiceStatus.Payment)
                    {
                        return Ok();
                    }
                    invoice.InvoiceStatus = InvoiceStatus.Payment;

                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == invoice.UserId);

                    

                    if (user != null) user.Points += eventModel.invoice.amount / 100;

                    await _userManager.UpdateAsync(user);

                    _context.Entry(invoice).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }

               

                return Ok();
            }
            catch (Exception e)
            {
               return BadRequest();
            }
            
        }

        private byte[] ReadPemPublicKey(string publicKey)
        {
            string encodedPublicKey = publicKey
                .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
                .Replace("-----END PUBLIC KEY-----", string.Empty)
                .Trim();

            return Convert.FromBase64String(encodedPublicKey);
        }
        private string GetSignatureFromHeader(string contentSignature)
        {
            const string pattern = @"alg=(\S+);\sdigest=";

            var trim = Regex.Match(contentSignature, pattern).Value;
            var signature = contentSignature.Replace(trim, string.Empty);
            
            return signature;

        }

        private async Task<byte[]> GetRawBodyBytesAsync(HttpRequest request)
        {
            using (var ms = new MemoryStream(2048))
            {
                await request.Body.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }

   
}






