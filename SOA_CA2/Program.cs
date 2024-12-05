using Microsoft.EntityFrameworkCore;
using SOA_CA2.Models;
using Microsoft.OpenApi.Models;
using SOA_CA2.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SingerContext>(opt => opt.UseInMemoryDatabase("SingersList"));

//builder.Services.AddDbContext<SingerContext>(opt => opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//        new MySqlServerVersion(new Version(8, 0, 39))));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Singers' Albums&Songs", Version = "v1" });
});
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<SOA_CA2.Middlewares.ApiKeyMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
