using API;
using API.Middlewares;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
	var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
	options.Filters.Add(new AuthorizeFilter(policy)); 
});
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentitySettings()
	.AddIdentityServices();
builder.Services.AddJwtAuth(builder.Services.AddMyOptions(builder.Configuration));
builder.Services.AddAppServices();
builder.Services.AddApplicationLayerServices();
builder.Services.AddJWTSwagger();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.SeedDatabase();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();