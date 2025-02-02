using Microsoft.AspNetCore.Mvc;
using ClientePolizasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientePolizasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolizaController : ControllerBase
    {
        private readonly ClienteDbContext _context; // Reemplazar PolizaDbContext por ClienteDbContext

        public PolizaController(ClienteDbContext context)
        {
            _context = context;
        }

        // Obtener todas las pólizas
        [HttpGet]
        public async Task<ActionResult<List<Poliza>>> GetPolizas()
        {
            return await _context.Polizas.ToListAsync();
        }

        // Obtener póliza por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Poliza>> GetPolizaPorId(int id)
        {
            var poliza = await _context.Polizas.FindAsync(id);
            if (poliza == null)
                return NotFound($"No se encontró una póliza con ID {id}.");
            
            return Ok(poliza);
        }

        // Crear póliza
        [HttpPost]
        public async Task<ActionResult<Poliza>> AgregarPoliza([FromBody] Poliza nuevaPoliza)
        {
            if (nuevaPoliza == null)
                return BadRequest("La póliza no puede ser nula.");

            // Verificar si el cliente existe antes de asociarlo
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == nuevaPoliza.IdCliente);
            if (!clienteExiste)
                return BadRequest($"El Cliente con ID {nuevaPoliza.IdCliente} no existe.");

            _context.Polizas.Add(nuevaPoliza);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPolizaPorId), new { id = nuevaPoliza.Id }, nuevaPoliza);
        }

        // Actualizar póliza parcialmente
        [HttpPatch("{id}")]
        public async Task<ActionResult<Poliza>> ActualizarPolizaParcial(int id, [FromBody] Dictionary<string, object> camposActualizados)
        {
            var polizaExistente = await _context.Polizas.FindAsync(id);
            if (polizaExistente == null)
                return NotFound($"No se encontró una póliza con ID {id}.");

            foreach (var campo in camposActualizados)
            {
                switch (campo.Key.ToLower())
                {
                    case "auto":
                        polizaExistente.Auto = campo.Value?.ToString() ?? polizaExistente.Auto;
                        break;
                    case "costo":
                        if (int.TryParse(campo.Value?.ToString(), out int nuevoCosto))
                            polizaExistente.Costo = nuevoCosto;
                        break;
                    case "fechavigencia":
                        polizaExistente.FechaVigencia = campo.Value?.ToString() ?? polizaExistente.FechaVigencia;
                        break;
                    case "idcliente":
                        if (int.TryParse(campo.Value?.ToString(), out int nuevoIdCliente))
                        {
                            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == nuevoIdCliente);
                            if (!clienteExiste)
                                return BadRequest($"El Cliente con ID {nuevoIdCliente} no existe.");
                            
                            polizaExistente.IdCliente = nuevoIdCliente;
                        }
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(polizaExistente);
        }

        // Eliminar póliza
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarPoliza(int id)
        {
            var poliza = await _context.Polizas.FindAsync(id);
            if (poliza == null)
                return NotFound($"No se encontró una póliza con ID {id}.");

            _context.Polizas.Remove(poliza);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
