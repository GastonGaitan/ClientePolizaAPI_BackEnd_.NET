using ClientePolizasAPI.Models;

public class ClienteDataStore
{
    public List<Cliente> Clientes { get; set; }

    public static ClienteDataStore Current { get; } = new ClienteDataStore();

    public ClienteDataStore()
    {
        Clientes = new List<Cliente>
        {
            new Cliente { Id = 1, Nombre = "Juan", Apellido = "Pérez", DNI = "12345678", FechaDeNacimiento = "1990-01-01" },
            new Cliente { Id = 2, Nombre = "Ana", Apellido = "Gómez", DNI = "87654321", FechaDeNacimiento = "1985-05-05" }
        };
    }
}
