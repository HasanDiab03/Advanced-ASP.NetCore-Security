using API;
using Application.Features.Identity.Queries;
using Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentitySettings();
builder.Services.AddJwtAuth(builder.Services.AddMyOptions(builder.Configuration));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetTokenQueryHandler))));
builder.Services.AddJWTSwagger();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.SeedDatabase();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();