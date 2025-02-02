using ClientePolizasAPI.Models;
using ClientePolizasAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Habilita MVC y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTRO DE SERVICIOS
builder.Services.AddHttpClient<ClienteValidationService>();  // HttpClient para ClienteValidationService

// Configurar SQLite en un solo DbContext que maneja Clientes y Pólizas
builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseSqlite("Data Source=ClientePolizas.db"));

// 🔹 Eliminamos ClienteDataStore y PolizaDataStore ya que ahora usamos SQLite

var app = builder.Build();

// Aplicar migraciones automáticamente al iniciar la API
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClienteDbContext>();
    dbContext.Database.Migrate();
}

// Habilita Swagger en cualquier entorno
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // REGISTRA LOS CONTROLADORES

app.Run();
