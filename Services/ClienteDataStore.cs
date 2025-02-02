using ClientePolizasAPI.Models;

namespace ClientePolizasAPI.Services;

public class ClienteDataStore
{
    public List<Cliente> Clientes { get; set; }

    public static ClienteDataStore Current { get; } = new ClienteDataStore();

    public ClienteDataStore()
    {
        Clientes = new List<Cliente>()
        {
            new Cliente()
            {
                Id = 2,
                Nombre = "Maria",
                Apellido = "Gomez",
                DNI = "12345678",
                FechaDeNacimiento = "1985-05-15"
            },
            new Cliente()
            {
                Id = 3,
                Nombre = "Carlos",
                Apellido = "Lopez",
                DNI = "87654321",
                FechaDeNacimiento = "1990-10-20"
            },
            new Cliente()
            {
                Id = 4,
                Nombre = "Ana",
                Apellido = "Martinez",
                DNI = "11223344",
                FechaDeNacimiento = "1978-03-30"
            },
            new Cliente()
            {
                Id = 5,
                Nombre = "Luis",
                Apellido = "Garcia",
                DNI = "55667788",
                FechaDeNacimiento = "1982-07-25"
            },
            new Cliente()
            {
                Id = 6,
                Nombre = "Sofia",
                Apellido = "Fernandez",
                DNI = "99887766",
                FechaDeNacimiento = "1995-12-10"
            }

        };
    }
}
