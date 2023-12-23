using SSLCommerz.NetCore.OptionsSetup;
using SSLCommerz.NetCore.SSLCommerz;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("SSLCommerzOrigins", builder =>
    {
        builder.WithOrigins("https://securepay.sslcommerz.com", "https://sandbox.sslcommerz.com")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<SSLCommerzOptionsSetup>();

builder.Services.AddHttpClient("SSLCommerz")
    .AddTransientHttpErrorPolicy(builder =>
        builder.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(30));

builder.Services.AddScoped<ISSLCommerzService, SSLCommerzService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
