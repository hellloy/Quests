using Newtonsoft.Json;
using Quests.Server.Models;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace Quests.Server.Payment
{
    public static class RbkMoney
    {
        public static string CreateOrder(int sum,int invoiceId)
        {
            const string apiKey =
                "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIzaG14MU95bjJWc2psTEZRNTNLYk5Nek12RGZUcjdzcGxYelJDeDRERVR3In0.eyJqdGkiOiIwMjY3ZDcxOS1jODhkLTQ2OTktYjEzOS1lNTM4ZjI3OGQyNTgiLCJleHAiOjE2NDMwOTYyOTgsIm5iZiI6MCwiaWF0IjoxNjExNTYwMjk4LCJpc3MiOiJodHRwczovL2F1dGgucmJrLm1vbmV5L2F1dGgvcmVhbG1zL2V4dGVybmFsIiwiYXVkIjoiZGFzaGJvYXJkIiwic3ViIjoiYjY0YTliMGUtZTRiOC00Yjg5LWIyY2EtMjJlMDlkZTI4ODJlIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZGFzaGJvYXJkIiwibm9uY2UiOiI3YTI2NDRkMi1kOWI4LTQ0ZmEtYjRiMi0yMzNmOTc5NWRiOTMiLCJhdXRoX3RpbWUiOjE2MTE1NjAyOTgsInNlc3Npb25fc3RhdGUiOiIzMDFmNWI5My0zZTFhLTQwZjAtOGY1ZC1jYzhkNDlkNjMzOTkiLCJhY3IiOiIxIiwiYWxsb3dlZC1vcmlnaW5zIjpbImh0dHBzOi8vYmV0YS5kYXNoYm9hcmQucmJrLm1vbmV5IiwiaHR0cHM6Ly9kYXNoYm9hcmQucmJrLm1vbmV5Il0sInJlc291cmNlX2FjY2VzcyI6eyJjb21tb24tYXBpIjp7InJvbGVzIjpbImludm9pY2VzLioucGF5bWVudHM6d3JpdGUiLCJjdXN0b21lcnMuKi5iaW5kaW5nczp3cml0ZSIsInBhcnR5OnJlYWQiLCJpbnZvaWNlcy4qLnBheW1lbnRzOnJlYWQiLCJjdXN0b21lcnM6d3JpdGUiLCJwYXJ0eTp3cml0ZSIsImN1c3RvbWVycy4qLmJpbmRpbmdzOnJlYWQiLCJjdXN0b21lcnM6cmVhZCIsInBheW1lbnRfcmVzb3VyY2VzOndyaXRlIiwiaW52b2ljZXM6d3JpdGUiLCJpbnZvaWNlczpyZWFkIl19LCJ1cmwtc2hvcnRlbmVyIjp7InJvbGVzIjpbInNob3J0ZW5lZC11cmxzOndyaXRlIiwic2hvcnRlbmVkLXVybHM6cmVhZCJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJuYW1lIjoi0JDQu9C10LrRgdC10Lkg0JrQsNGH0YPQvdC40L0iLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJoZWxsbG95QGdtYWlsLmNvbSIsImdpdmVuX25hbWUiOiLQkNC70LXQutGB0LXQuSIsImZhbWlseV9uYW1lIjoi0JrQsNGH0YPQvdC40L0iLCJlbWFpbCI6ImhlbGxsb3lAZ21haWwuY29tIn0.hkb_joZrlsVFVFKhrQsXrQ8tS4S_Flc_nxgPdPDsLvp6eB3X4G-pAGoIzCKTNBtadPtBSDT20hOcBX6W7-B7NL8aSHpdCmLkp26oWEy9-esbyggnVS9MUhkVm1JRh_pNirUY5BsRDlpxoVv4_eVtAsK_otfEvpVl6erdA_jwGwErQYRGM2CwJ66gdZcbGHHI5RTzc94PLA9ghVB6RMrJZG2MFnSOZtmGmixhuyOl0x3oxNQr5U1yK0FflrJAPSc1CHpLrQsJXO9_SeowNPzAQjLJahF50_Hzz8sXBrT4211_RUaa01PClUog9Ch0D8N6gGX_LBxmQbke4-LiSS6tVr5D6gR5BX6Zb69Ayw2pu65enyV7Ss4NwkXXvwnsChD9zguIpecGzP0PVO4Q7Sh4gpdhUIIfNs4Em50iiMVeYuOLhxShk0-v1QE2JA9iud8Obl-0QxY8ed926BV1alW8VB8ebkzckcboRqdVZrncVGVH_3RqzK4lZ-3-59V-GXjfM0Bgu73ry639F35DZFyYfpIBJZe5SvcWgqw4fUSLuL4JPJOaqdbhcjZ0RE_Tv7PyKpNV4iyYypmMMeLegUQXMYkZOfat4HJ5EqWVxBJFTF-uCZhRjUcPE08WxbrMf9znAkhtxD23CkkQJecpR5MhjPhuaG-p6pght7GZsNsY7Us";

            const string shopId = "c60e515f-a394-42e0-9a46-7af53f18f825";

            var dataLoad = new RbkModel
            {
                shopID = shopId,
                dueDate = DateTime.Now.AddHours(1),
                amount = sum*100,
                currency = "RUB",
                product = "Заказ номер "+invoiceId.ToString(),
                description = "Тестовая услуга",
                cart = new[]
                {
                    new Cart
                    {
                        price = sum*100,
                        product = "",
                        quantity = 1,
                        taxMode = new Taxmode
                        {
                            rate = "0%",
                            type = "InvoiceLineTaxVAT"
                        }
                    }
                },
                metadata = new Metadata
                {
                    order_id = invoiceId.ToString()
                }

            };



            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = "https://api.rbk.money";
                    var url = "/v1/processing/invoices";


                    webClient.Headers.Add("X-Request-ID", GetUniqId());
                    webClient.Headers[HttpRequestHeader.Authorization] = $"Bearer {apiKey}";
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";


                    string data = JsonConvert.SerializeObject(dataLoad);
                    var response = webClient.UploadString(url, data);
                    return response;
                }
            }
            catch (Exception ex)
            {
                return "Bad request";
            }

        }

        private static string GetUniqId()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double t = ts.TotalMilliseconds / 1000;

            int a = (int)Math.Floor(t);
            int b = (int)((t - Math.Floor(t)) * 1000000);

            return a.ToString("x8") + b.ToString("x5");
        }
    }

     internal class RsaSignatureVerifier : IDisposable
    {
        private readonly RSA _rsa;

        /// <summary>
        /// Create a new instance of <see cref="RsaSignatureVerifier"/>.
        /// </summary>
        /// <param name="publicKeyPath">
        /// The path to the public key corresponding to the private key that was used to sign files.
        /// </param>
        public RsaSignatureVerifier(string publicKeyString)
        {
            _rsa = RSA.Create();

            byte[] pubKey = ReadPemPublicKey(publicKeyString);

            _rsa.ImportSubjectPublicKeyInfo(pubKey, out _);
        }

        /// <summary>
        /// Verifies the specified file using the specified <see cref="RSA"/> signature. The digest
        /// used is <see cref="HashAlgorithmName.SHA256"/>.
        /// </summary>
        /// <param name="fileToVerifyPath">The path of the file to verify.</param>
        /// <param name="fileSignaturePath">
        /// The path of the signature used to verify the specified file.
        /// </param>
        /// <returns>Whether the file was verified successfully.</returns>
        public bool Verify(Stream toVerifyStream, byte[] signatureBytes)
        {
           
            return _rsa.VerifyData(toVerifyStream, signatureBytes, HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
        }

        
        /// <summary>
        /// Reads a PEM encoded public key and returns the corresponding binary key.
        /// </summary>
        /// <param name="publicKey">The path to the PEM encoded key.</param>
        /// <returns>The corresponding binary key.</returns>
        private byte[] ReadPemPublicKey(string publicKey)
        {
            string encodedPublicKey = publicKey
                .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
                .Replace("-----END PUBLIC KEY-----", string.Empty)
                .Trim();

            return Convert.FromBase64String(encodedPublicKey);
        }

        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}
