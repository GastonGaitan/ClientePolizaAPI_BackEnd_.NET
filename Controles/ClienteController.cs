using ClientePolizasAPI.Models;
using ClientePolizasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientePolizasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController: ControllerBase {
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetClientes() {
            return Ok(ClienteDataStore.Current.Clientes);
        }
    }
}