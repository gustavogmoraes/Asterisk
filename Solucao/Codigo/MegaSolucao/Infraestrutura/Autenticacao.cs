using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MegaSolucao.Infraestrutura
{
    public static class Autenticacao
    {
        public static async Task<bool> ValidateToken(Guid idToken)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Sessao.Configuracao.HostApiAutenticacao)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var response = await client.PostAsync("api/Auth/Login", new StringContent(JsonConvert.SerializeObject(new { Nome = "admin", Senha = "admin" }), Encoding.UTF8, "application/json"));
            var response = await client.GetAsync($"ValidateToken/{idToken}");

            return Convert.ToBoolean(await response.Content.ReadAsStringAsync());
        }
    }
}
