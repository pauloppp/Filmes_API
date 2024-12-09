using Filmes_API.Context;
using Filmes_API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Banco de dados InMemory
builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Oscar"));

// Dependências
builder.Services.ResolverDependencias();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Filmes Web API - Golden Raspberry Awards",
            Description = "Obter informações sobre filmes.",
            Contact = new OpenApiContact() { Name = "sysXYZ", Email = "sysxyz@xyz.com.zyx" },
        });
    }
);

var app = builder.Build();

// Pega o context e salva dados iniciais no banco.
var scopo = app.Services.CreateScope();
ApiContext context = scopo.ServiceProvider.GetRequiredService<ApiContext>();
CommonServices.AdicionarDadosIniciais(context);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

