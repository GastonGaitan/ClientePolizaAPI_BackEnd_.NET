using Microsoft.AspNetCore.Mvc;
using ClientePolizasAPI.Models;
using ClientePolizasAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace ClientePolizasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolizaController : ControllerBase
    {
        private readonly PolizaDataStore _polizaDataStore;

        public PolizaController(PolizaDataStore polizaDataStore)
        {
            _polizaDataStore = polizaDataStore;
        }

        [HttpGet]
        public ActionResult<List<Poliza>> GetPolizas()
        {
            return Ok(_polizaDataStore.Polizas);
        }

        [HttpGet("{id}")]
        public ActionResult<Poliza> GetPolizaPorId(int id)
        {
            var poliza = _polizaDataStore.Polizas.FirstOrDefault(p => p.Id == id);
            if (poliza == null)
                return NotFound($"No se encontró una póliza con ID {id}.");

            return Ok(poliza);
        }

        [HttpPost]
        public ActionResult<Poliza> AgregarPoliza([FromBody] Poliza nuevaPoliza)
        {
            if (nuevaPoliza == null)
                return BadRequest("La póliza no puede ser nula.");

            // Generar un nuevo ID automáticamente
            int nuevoId = _polizaDataStore.Polizas.Count > 0 ? _polizaDataStore.Polizas.Max(p => p.Id) + 1 : 1;
            nuevaPoliza.Id = nuevoId;

            _polizaDataStore.Polizas.Add(nuevaPoliza);

            return CreatedAtAction(nameof(GetPolizaPorId), new { id = nuevaPoliza.Id }, nuevaPoliza);
        }

        [HttpPatch("{id}")]
        public ActionResult<Poliza> ActualizarPolizaParcial(int id, [FromBody] Dictionary<string, object> camposActualizados)
        {
            var polizaExistente = _polizaDataStore.Polizas.FirstOrDefault(p => p.Id == id);
            if (polizaExistente == null)
                return NotFound($"No se encontró una póliza con ID {id}.");

            // Validar y actualizar cada campo recibido en la solicitud
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
                            polizaExistente.IdCliente = nuevoIdCliente;
                        break;
                }
            }

            return Ok(polizaExistente);
        }


        [HttpDelete("{id}")]
        public ActionResult EliminarPoliza(int id)
        {
            var poliza = _polizaDataStore.Polizas.FirstOrDefault(p => p.Id == id);
            if (poliza == null)
                return NotFound($"No se encontró una póliza con ID {id}.");

            _polizaDataStore.Polizas.Remove(poliza);

            return NoContent(); // Código HTTP 204 (sin contenido)
        }
    }
}
