using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerClientApp
{
    public class AWSComms
    {
        public async Task SendRequest(string json, string url)
        {
            // Instantiate once, should reuse HttpClient rather than dispose
            var client = new HttpClient();

            if (json != string.Empty)
            {
                try
                {
                    var response = await client.PostAsync(
                        "https://pqfny1q7g8.execute-api.ap-southeast-2.amazonaws.com/Test/erm",
                        new StringContent(json, Encoding.UTF8, "application/json"));
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }
    }
}
