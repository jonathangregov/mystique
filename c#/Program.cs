using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http;

namespace ConsoleApp2
{
    class Program
    {

        struct HashResult
        {
            public string hash;
            public long date;
        }

        private static readonly HttpClient client = new HttpClient();
        private static string merchantKey;
        private static string merchantSecret;
        static void Main(string[] args)
        {
            merchantKey = Environment.GetEnvironmentVariable("MERCHANT_KEY");
            merchantSecret = Environment.GetEnvironmentVariable("MERCHANT_SECRET");

            Console.WriteLine("requesting...");
            string response = CreateOrderAsync().GetAwaiter().GetResult();
            Console.WriteLine(response);
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }

        static async Task<string> CreateOrderAsync()
        {
            KeyValuePair<string, string>[] payload = new[] {
                new KeyValuePair<string, string>("currency", "CLP"),
                new KeyValuePair<string, string>("description", "description"),
                new KeyValuePair<string, string>("merchant_order_id", "--order-id--"),
                new KeyValuePair<string, string>("notify_url", "http://notification.merchant.com"),
                new KeyValuePair<string, string>("price", "1500"),
                new KeyValuePair<string, string>("return_url", "http://final.merchant.com"),
                new KeyValuePair<string, string>("timeout", "60")
            };

            var formContent = new FormUrlEncodedContent(payload);

            // GENERATE HASH
            HashResult hashResult = GenerateHash("POST", "/merchant/orders/", payload);

            client.BaseAddress = new Uri("https://sandboxapi.pago46.com/");
            client.DefaultRequestHeaders.Add("merchant-key", merchantKey);
            client.DefaultRequestHeaders.Add("message-hash", hashResult.hash);
            client.DefaultRequestHeaders.Add("message-date", hashResult.date.ToString());

            HttpResponseMessage response = await client.PostAsync("merchant/orders/", formContent);
            string response_contents = await response.Content.ReadAsStringAsync();
            return response_contents;
        }


        static int CompareAlphabetically(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        static HashResult GenerateHash(string requestMethod, string requestPath, KeyValuePair<string, string>[] payload)
        {

            payload.OrderBy(x => x);

            string concatenatedParams = "";
            foreach (KeyValuePair<string, string> data in payload)
            {
                string escapedValue = Uri.EscapeDataString(data.Value);
                concatenatedParams += '&' + data.Key + '=' + escapedValue;
            }

            long date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string encrypt_base = merchantKey + '&' + date + '&' + requestMethod + '&' + Uri.EscapeDataString(requestPath) + concatenatedParams;
            // WORKS: "E76F78A754CD06573B145266A956CFDF&1548881268251&POST&%2Fmerchant%2Forders%2F&currency=CLP&description=description&merchant_order_id=--order-id--&notify_url=http%3A%2F%2Fnotification.merchant.com&price=1500&return_url=http%3A%2F%2Ffinal.merchant.com&timeout=60"
            // GIVEN: "E76F78A754CD06573B145266A956CFDF&1548881580861&POST&%2Fmerchant%2Forders%2F&currency=CLP&description=description&merchant_order_id=--order-id--&notify_url=http%3A%2F%2Fnotification.merchant.com&price=1500&return_url=http%3A%2F%2Ffinal.merchant.com&timeout=60"
            ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] key = encoding.GetBytes(merchantSecret);
            byte[] encrypt_base_bytes = encoding.GetBytes(encrypt_base);
            using (var hmacSHA256 = new HMACSHA256(key))
            {
                byte[] hashmessage = hmacSHA256.ComputeHash(encrypt_base_bytes);

                // GIVEN : 80e3d25dca88b15898cc7fd800ac8d4b2d1cd8843c970fe23614770a00939f87
                // WORKS : 491d24ebae08805d7e1b2655209fd1c02c5c8b3f57d4f3d805a539d418439a88

                string computed_hash = BitConverter.ToString(hashmessage).Replace("-", "").ToLower();

                return new HashResult{ hash = computed_hash,  date = date};
            }
        }
    }
}
