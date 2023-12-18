using SSLCommerz.NetCore.OptionsSetup;
using SSLCommerz.NetCore.SSLCommerz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("SSLCommerzOrigins", builder =>
    {
        builder.WithOrigins("https://securepay.sslcommerz5.com", "https://sandbox.sslcommerz5.com")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<SSLCommerzOptionsSetup>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ISSLCommerzService, SSLCommerzService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
