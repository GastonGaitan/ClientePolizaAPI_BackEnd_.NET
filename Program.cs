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
builder.Services.AddScoped<ClienteValidationService>();      // Inyecta ClienteValidationService

// Configurar SQLite para ClienteDbContext
builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseSqlite("Data Source=clientes.db"));

// Elimina ClienteDataStore porque ahora usas una base de datos
// builder.Services.AddSingleton<ClienteDataStore>();  // YA NO ES NECESARIO

// MANTÉN PolizaDataStore si aún no migraste Poliza a SQLite
builder.Services.AddSingleton<PolizaDataStore>();   

var app = builder.Build();

// Asegurar que las migraciones se apliquen al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClienteDbContext>();
    dbContext.Database.Migrate(); // Aplica migraciones automáticamente
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
