using Microsoft.AspNetCore.Mvc;
using ClientePolizasAPI.Models;
using ClientePolizasAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientePolizasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteValidationService _validationService;
        private readonly ClienteDbContext _context;

        public ClienteController(ClienteValidationService validationService, ClienteDbContext context)
        {
            _validationService = validationService;
            _context = context;
        }

        // Obtener todos los clientes desde SQLite
        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // Obtener un cliente por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClientePorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();
            return Ok(cliente);
        }

        // Crear un nuevo cliente en la base de datos
        [HttpPost]
        public async Task<ActionResult<Cliente>> AgregarCliente([FromBody] Cliente nuevoCliente)
        {
            if (nuevoCliente == null)
                return BadRequest("El cliente no puede ser nulo.");

            // Validación de nombre y apellido usando el servicio externo
            bool esValido = await _validationService.ValidarNombreApellido(nuevoCliente.Nombre, nuevoCliente.Apellido);
            if (!esValido)
                return BadRequest("El nombre y apellido no son válidos según la validación externa.");

            // Verificar si el DNI ya existe
            if (await _context.Clientes.AnyAsync(c => c.DNI == nuevoCliente.DNI))
                return BadRequest($"El DNI {nuevoCliente.DNI} ya está registrado en otro cliente.");

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientePorId), new { id = nuevoCliente.Id }, nuevoCliente);
        }

        // Actualizar datos parcialmente
        [HttpPatch("{id}")]
        public async Task<ActionResult<Cliente>> ActualizarClienteParcial(int id, [FromBody] Dictionary<string, object> camposActualizados)
        {
            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null)
                return NotFound($"No se encontró un cliente con ID {id}.");

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
                        if (!string.IsNullOrEmpty(nuevoDNI) && await _context.Clientes.AnyAsync(c => c.DNI == nuevoDNI && c.Id != id))
                            return BadRequest($"El DNI {nuevoDNI} ya está registrado en otro cliente.");
                        clienteExistente.DNI = nuevoDNI ?? clienteExistente.DNI;
                        break;
                    case "fechadenacimiento":
                        clienteExistente.FechaDeNacimiento = campo.Value?.ToString() ?? clienteExistente.FechaDeNacimiento;
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(clienteExistente);
        }

        // Eliminar un cliente de la base de datos
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound($"No se encontró un cliente con ID {id}.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
