using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientePolizasAPI.Services
{
    public class ClienteValidationService
    {
        private readonly HttpClient _httpClient;

        public ClienteValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidarNombreApellido(string nombre, string apellido)
        {
            string concatenado = $"{nombre}{apellido}";
            string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(concatenado));

            Console.WriteLine($"Nombre: {nombre}");
            Console.WriteLine($"Apellido: {apellido}");
            Console.WriteLine($"Concatenado: {concatenado}");
            Console.WriteLine($"Base64 Generado: {authHeader}");

            var payload = new { nombre, apellido };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://qa.segurarse.com.ar/pruebas/testencrypt");
            request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            request.Content = content;

            try
            {
                using var response = await _httpClient.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Respuesta del servidor: {jsonResponse}");

                using var jsonDoc = JsonDocument.Parse(jsonResponse);
                if (jsonDoc.RootElement.TryGetProperty("result", out JsonElement resultElement))
                {
                    string? result = resultElement.GetString()?.Trim();
                    return result != null && result.Equals("OK", StringComparison.OrdinalIgnoreCase);
                }

                return false; // Si no encontramos "result", asumimos que la validación falló.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la validación externa: {ex.Message}");
                return false;
            }
        }
    }
}
