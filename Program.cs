var builder = WebApplication.CreateBuilder(args);

// Habilita MVC y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
