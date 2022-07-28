using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;



namespace HoroscopeBot.AHoroscope
{
    class AClient
    {
        public HttpClient client;
        
        public AClient()
        {
            client = new HttpClient();
        }
        public async Task<AModel> GetAHoro(string sign, string period)
        {
            var client = new HttpClient();
            if (period == "month")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:44300/MonthlyW?sign={sign}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AModel>(result);
            }
            else if (period == "week")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:44300/WeeklyW?sign={sign}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AModel>(result);
            }
            else if (period == "today"||period=="yesterday"||period=="tomorroq")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:44300/DailyW?sign={sign}&day={period}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AModel>(result);
            }
            string error = "something has gone wrong, please check if you have written everything correct;";
            return JsonConvert.DeserializeObject<AModel>(error);


        }
    }
}
