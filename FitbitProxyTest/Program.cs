using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FitbitProxyTest
{
    class Program
    {
        public static HttpClient httpClient { get; private set; }

        static void Main(string[] args)
        {
            Task.Run(() => MainAsync(args)).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            httpClient = new HttpClient();

            // The line below was tested and does not resolve the issue.
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            while(true)
            { 
                try
                {
                    //the line below will run fine the first time and all subsequent times until the hour rolls over
                    var result = await httpClient.GetStringAsync("https://www.fitbit.com"); //fitbit.com behind CloudFlare Proxy
                    Console.WriteLine(DateTime.UtcNow.ToString() + " Fitbit.com Content-Length:" + result.Length);
                }
                catch (Exception ex)
                {
                    //at xx:00 when the hour rolls over the TLS exception will occur here. Like clockwork.
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(60000); //one minute
            }
        }

    }
}
