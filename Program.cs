using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        const bool webRequest = true;

        if (webRequest)
        {
            using (var client = new HttpClient())
            {
                // 1. open() - Abrir uma requisição HTTP (equivalente ao XMLHttpRequest.open)
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    "https://fakeecommerce.com/produtos"
                );

                // 2. setRequestHeader() - Definir cabeçalhos
                request.Headers.Add("User-Agent", "EcommerceApp/1.0");

                // 3. send() - Enviar a requisição
                var cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    var response = await client.SendAsync(request, cancellationTokenSource.Token);

                    // 4. onload / onreadystatechange - Receber resposta
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        Console.WriteLine("Lista de produtos recebida com sucesso:");
                        Console.WriteLine(content);
                    }
                    else
                    {
                        Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Requisição cancelada.");
                }
            }
        }
        else
        {
            try
            {
                // 1. open() - Criar requisição
                WebRequest request = WebRequest.Create("https://fakeecommerce.com/produtos");

                // 2. Definir método HTTP
                request.Method = "GET";

                // 3. setRequestHeader()
                request.Headers.Add("User-Agent", "EcommerceApp/1.0");

                // 4. send() - Enviar requisição
                using (WebResponse response = request.GetResponse())
                {
                    // Verificar status
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine($"Status: {httpResponse.StatusCode}");

                    // 5. Ler resposta (tratamento de dados)
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string resposta = reader.ReadToEnd();

                        Console.WriteLine("Dados dos produtos:");
                        Console.WriteLine(resposta);
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Status}");

                using (var stream = e.Response?.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }
    }
}