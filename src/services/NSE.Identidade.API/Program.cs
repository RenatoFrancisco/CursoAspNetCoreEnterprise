var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration, builder);

builder.Services.AddSwaggerConfig(builder);

builder.Services.AddIdentityConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfig();

app.UseApiConfiguration();
