using System.Text.Json.Serialization;
using ParkingLot.Api.Models;
using ParkingLot.Api.Services;

var builder = WebApplication.CreateBuilder(args);

const string AngularDevClient = "AngularDevClient";

builder.Services.Configure<ParkingLotOptions>(builder.Configuration.GetSection(ParkingLotOptions.SectionName));
builder.Services.AddSingleton<IParkingLotService, ParkingLotService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevClient, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AngularDevClient);
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
