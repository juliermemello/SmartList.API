using Serilog;
using SmartList.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGeneralConfiguration(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.AddGeneralApp();
app.UseSerilogRequestLogging();
app.AddAPICors();
app.AddSwagger();
app.AddAPIMiddleware();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.AddHealthCheck();
app.Run();
