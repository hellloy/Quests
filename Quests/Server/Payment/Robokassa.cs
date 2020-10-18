
using System.Security.Cryptography;
using System.Text;
using Quests.Server.Models;


namespace Quests.Server.Payment
{
    public static class RoboKassa
    {
        public static string GenerateAuthLink(decimal amount, int invoiceId)
        {
            const bool isTest = true;

            const string roboShopName = "testshop25";

            const string roboPassw1 = "iwIh2F2cIzPi8kTo5TT4";

            string
                amountStr = amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                invoiceIdStr = invoiceId.ToString(),
                srcBase = $"{roboShopName}:{amountStr}:{invoiceIdStr}:{roboPassw1}";

            string signatureValue = GenerateMd5Hash(srcBase);
            string authPaymentString;

            if (isTest)
            {
                authPaymentString = "https://auth.robokassa.ru/Merchant/Index.aspx" + "?isTest=1" + "&MrchLogin=" +
                                    roboShopName + "&InvId=" + invoiceIdStr + "&OutSum=" + amountStr +
                                    "&SignatureValue=" + signatureValue + "&Culture=ru";
            }
            else
            {
                authPaymentString = "https://auth.robokassa.ru/Merchant/Index.aspx" + "?MrchLogin=" + roboShopName +
                                    "&InvId=" + invoiceIdStr + "&OutSum=" + amountStr + "&SignatureValue=" +
                                    signatureValue + "&Culture=ru";
            }

            return authPaymentString;
        }

        static string GenerateMd5Hash(string stringToHash)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(stringToHash));
                StringBuilder sbSignature = new StringBuilder();
                foreach (byte b in bSignature)
                    sbSignature.AppendFormat("{0:x2}", b);
                return sbSignature.ToString();
            }
        }

        public static bool CheckResult(RobokassaResult roboResult)
        {
            const string roboPassw2 = "JxxIv93Sr7Ft0U2hylXz";

            if (roboResult == null || roboResult.InvId == 0)
                return false;

            string
                outSummStr = roboResult.OutSum,
                outInvIdStr = roboResult.InvId.ToString(),
                srcBase = $"{outSummStr}:{outInvIdStr}:{roboPassw2}";

            string srcMD5Hash = GenerateMd5Hash(srcBase);

            return roboResult.SignatureValue.ToLower().Equals(srcMD5Hash);
        }
    }

    
}
