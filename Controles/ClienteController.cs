using Microsoft.AspNetCore.Mvc;
using ClientePolizasAPI.Models;

namespace ClientePolizasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/Cliente
    public class ClienteController : ControllerBase
    {
        private readonly ClienteDataStore _dataStore = ClienteDataStore.Current;

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
        public ActionResult<Cliente> AgregarCliente([FromBody] Cliente nuevoCliente)
        {
            if (nuevoCliente == null)
                return BadRequest("El cliente no puede ser nulo.");

            // Verificar si el DNI ya existe en la lista
            var existeCliente = _dataStore.Clientes.Any(c => c.DNI == nuevoCliente.DNI);
            if (existeCliente)
                return BadRequest($"El DNI {nuevoCliente.DNI} ya esta registrado en otro cliente.");

            // Generar un nuevo ID automaticamente
            int nuevoId = _dataStore.Clientes.Count > 0 ? _dataStore.Clientes.Max(c => c.Id) + 1 : 1;
            nuevoCliente.Id = nuevoId;

            _dataStore.Clientes.Add(nuevoCliente);

            return CreatedAtAction(nameof(GetClientePorId), new { id = nuevoCliente.Id }, nuevoCliente);
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
