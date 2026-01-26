using SmartList.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGeneralConfiguration(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddGeneralApp();
app.MapControllers();
app.Run();
