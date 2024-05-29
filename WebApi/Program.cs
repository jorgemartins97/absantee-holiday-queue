using Application.Services;
using DataModel.Mapper;
using DataModel.Repository;
using Domain.Factory;
using Domain.IRepository;
using Gateway;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using WebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);


var config = builder.Configuration;
string replicaNameArg = Array.Find(args, arg => arg.Contains("--replicaName"));
string replicaName;
if (replicaNameArg != null)
    replicaName = replicaNameArg.Split('=')[1];
else
    replicaName = config.GetConnectionString("replicaName");

replicaName = "Repl1";


var queueName = config["Queues:" + replicaName];
 
var port = config["Ports:" + replicaName];

Console.WriteLine("Environment Development: " + config["ASPNETCORE_ENVIRONMENT"]);
Console.WriteLine("DB_CONNECTION: " + config["DB_CONNECTION"]);
 
string dbConnectionString = config.DefineDbConnection();
Console.WriteLine("DBConnectionString: " + dbConnectionString);
 
RabbitMqConfiguration rabbitMqConfig = config.DefineRabbitMqConfiguration();
Console.WriteLine("RabbitMqConfig: " + rabbitMqConfig.Hostname);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(option =>
{
    option.UseNpgsql(dbConnectionString);
}, optionsLifetime: ServiceLifetime.Scoped);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
    opt.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
    })
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    return new ConnectionFactory()
    {
        HostName = rabbitMqConfig.Hostname,
        UserName = rabbitMqConfig.Username,
        Password = rabbitMqConfig.Password,
        Port = int.Parse(rabbitMqConfig.Port.ToString())
    };
});

builder.Services.AddTransient<IHolidayRepository, HolidayRepository>();
builder.Services.AddTransient<IHolidayFactory, HolidayFactory>();
builder.Services.AddTransient<HolidayMapper>();
builder.Services.AddTransient<HolidayService>();
builder.Services.AddTransient<HolidayAmpqGateway>();

builder.Services.AddSingleton<IHolidayPeriodFactory, HolidayPeriodFactory>();

builder.Services.AddTransient<IColaboratorsIdRepository, ColaboratorsIdRepository>();
builder.Services.AddTransient<IColaboratorIdFactory, ColaboratorIdFactory>();
builder.Services.AddTransient<ColaboratorsIdMapper>();
builder.Services.AddTransient<ColaboratorIdService>();

builder.Services.AddSingleton<IRabbitMQConsumerController, RabbitMQConsumerController>();
builder.Services.AddSingleton<IRabbitMQConsumerController, RabbitMQColabConsumerController>();

var app = builder.Build();

var rabbitMQConsumerServices = app.Services.GetServices<IRabbitMQConsumerController>();
foreach (var service in rabbitMQConsumerServices)
{
    service.ConfigQueue(queueName);
    service.StartConsuming();
};

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program{ }