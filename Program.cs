var builder = WebApplication.CreateBuilder(args);

// Agrega Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Habilita Swagger en cualquier entorno (no solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige a HTTPS (puedes comentarlo si da problemas)
app.UseHttpsRedirection();

app.Run();
