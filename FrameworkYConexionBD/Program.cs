using System.Text.Json.Serialization;
using FrameworkYConexionBD;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
 * Aqui logramos traer el conection string para pasarselo abajo y que le permita
 */
var ConnectionString = builder.Configuration.GetConnectionString("SqlServer");

//Esta es la instacion de la conexion a la base de datos



builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });


//Aqui juntamos todo el contexto y el connection string para ahcer la conexion a la base 
//de datos ya levantada con nuestro contenedor y poder pasarle cosas.

builder.Services.AddDbContext<StoreDbCOntext>(o => o
    .UseSqlServer(ConnectionString));

builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI();
app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

