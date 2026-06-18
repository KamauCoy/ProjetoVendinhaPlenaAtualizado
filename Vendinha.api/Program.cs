using Vendinha.Data;
using Vendinha.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


Environment.SetEnvironmentVariable(
"ConnectionStrings__Default",
"Server=localhost;Port=5432;Database=Vendinha;User Id=postgres;Password=1234"
);


builder.Services.AddTransient<ClienteService>();
builder.Services.AddTransient<DividaService>();
builder.Services.AddTransient<VendinhaDbContext>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
