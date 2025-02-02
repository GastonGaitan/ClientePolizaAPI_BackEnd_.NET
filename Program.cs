using ClientePolizasAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Habilita MVC y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTRO DE SERVICIOS
builder.Services.AddHttpClient<ClienteValidationService>();  // Registra HttpClient para ClienteValidationService
builder.Services.AddScoped<ClienteValidationService>();  // Inyecta ClienteValidationService

// REGISTRO DE DATASTORES COMO SINGLETON
builder.Services.AddSingleton<ClienteDataStore>();  // Registro de ClienteDataStore
builder.Services.AddSingleton<PolizaDataStore>();   // Registro de PolizaDataStore

var app = builder.Build();

// Habilita Swagger en cualquier entorno
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization(); // Si agregas autenticación en el futuro

app.MapControllers(); // REGISTRA LOS CONTROLADORES

app.Run();
