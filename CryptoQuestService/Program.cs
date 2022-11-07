using CryptoQuestService.Models.Settings;
using CryptoQuestService.Services;
using CryptoQuestService.Services.HostedServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetRequiredSection(nameof(ApiSettings)));

// Custom services
builder.Services.AddTransient<ContractDeployer>();

// Hosted services
builder.Services.AddHostedService<DeployerHostedService>();

var app = builder.Build();

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