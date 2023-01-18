var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration();

builder.Services.AddMvcConfiguration(builder.Configuration, builder);

builder.Services.RegisterServices();

var app = builder.Build();

app.UseMvcConfiguration();
