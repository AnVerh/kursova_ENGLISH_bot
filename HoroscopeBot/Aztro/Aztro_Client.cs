using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopeBot.Aztro
{
    class Aztro_Client
    {
        public async Task<Aztro_Model> GetHoro(string sign, string day)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://kursova-api.herokuapp.com/Aztro?sign={sign}&day={day}"),
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Aztro_Model>(result);
        }
    }
}
