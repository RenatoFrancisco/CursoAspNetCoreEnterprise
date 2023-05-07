var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration, builder);

builder.Services.AddSwaggerConfig(builder);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.RegisterServices();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerConfig();

app.UseApiConfiguration();