using Microsoft.AspNetCore.Mvc;
using ClientePolizasAPI.Models;
using System.Text.Json;
using System.Text;

namespace ClientePolizasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/Cliente
    public class ClienteController : ControllerBase
    {
        private readonly ClienteDataStore _dataStore = ClienteDataStore.Current;
        private readonly HttpClient _httpClient;

        // Constructor to initialize HttpClient
        public ClienteController()
        {
            _httpClient = new HttpClient();
        }
        

        [HttpGet]
        public ActionResult<List<Cliente>> GetClientes()
        {
            return Ok(_dataStore.Clientes);
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetClientePorId(int id)
        {
            var cliente = _dataStore.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> AgregarCliente([FromBody] Cliente nuevoCliente)
        {
            if (nuevoCliente == null)
                return BadRequest("El cliente no puede ser nulo.");

            // Validate name and surname with external service
            bool esValido = await ValidarNombreApellido(nuevoCliente.Nombre, nuevoCliente.Apellido);
            if (!esValido)
                return BadRequest("El nombre y apellido no son válidos según la validación externa.");

            // Check if DNI already exists
            var existeCliente = _dataStore.Clientes.Any(c => c.DNI == nuevoCliente.DNI);
            if (existeCliente)
                return BadRequest($"El DNI {nuevoCliente.DNI} ya está registrado en otro cliente.");

            // Generate a new ID automatically
            int nuevoId = _dataStore.Clientes.Count > 0 ? _dataStore.Clientes.Max(c => c.Id) + 1 : 1;
            nuevoCliente.Id = nuevoId;

            _dataStore.Clientes.Add(nuevoCliente);

            return CreatedAtAction(nameof(GetClientePorId), new { id = nuevoCliente.Id }, nuevoCliente);
        }

        private async Task<bool> ValidarNombreApellido(string nombre, string apellido)
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

                // Extraer el valor "result" del JSON correctamente
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

        [HttpPatch("{id}")]
        public ActionResult<Cliente> ActualizarClienteParcial(int id, [FromBody] Dictionary<string, object> camposActualizados)
        {
            var clienteExistente = _dataStore.Clientes.FirstOrDefault(c => c.Id == id);
            if (clienteExistente == null)
                return NotFound($"No se encontró un cliente con ID {id}.");

            // Validar cada campo recibido en la solicitud
            foreach (var campo in camposActualizados)
            {
                switch (campo.Key.ToLower())
                {
                    case "nombre":
                        clienteExistente.Nombre = campo.Value?.ToString() ?? clienteExistente.Nombre;
                        break;
                    case "apellido":
                        clienteExistente.Apellido = campo.Value?.ToString() ?? clienteExistente.Apellido;
                        break;
                    case "dni":
                        var nuevoDNI = campo.Value?.ToString();
                        if (!string.IsNullOrEmpty(nuevoDNI) && _dataStore.Clientes.Any(c => c.DNI == nuevoDNI && c.Id != id))
                            return BadRequest($"El DNI {nuevoDNI} ya está registrado en otro cliente.");
                        clienteExistente.DNI = nuevoDNI ?? clienteExistente.DNI;
                        break;
                    case "fechadenacimiento":
                        clienteExistente.FechaDeNacimiento = campo.Value?.ToString() ?? clienteExistente.FechaDeNacimiento;
                        break;
                }
            }

            return Ok(clienteExistente);
        }


        
        
    }
}
