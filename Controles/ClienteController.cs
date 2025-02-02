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
    }
}
