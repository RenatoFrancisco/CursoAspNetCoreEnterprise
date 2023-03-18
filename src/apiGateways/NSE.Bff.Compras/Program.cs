var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration, builder);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfig(builder);

builder.Services.RegisterServices();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("catalog", policy =>
        policy
            .RequireClaim("Catalog", "Read"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerConfig();

app.UseApiConfiguration();
