using Microsoft.Extensions.Options;

namespace SSLCommerz.NetCore.OptionsSetup;

public class SSLCommerzOptionsSetup(IConfiguration configuration) 
    : IConfigureOptions<SSLCommerzOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(SSLCommerzOptions options)
    {
        _configuration.GetSection(SSLCommerzOptions.SSLCommerz).Bind(options);
    }
}
