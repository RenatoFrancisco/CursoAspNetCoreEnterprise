var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration, builder);

builder.Services.AddSwaggerConfig(builder);

builder.Services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerConfig();

app.UseApiConfiguration();
