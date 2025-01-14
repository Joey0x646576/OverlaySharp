try
{
    var host = GetHostBuilder(args).Build();
    await host.RunAsync();
}
catch (OptionsValidationException ex)
{
    Console.WriteLine("Configuration validation failed:");
    foreach (var error in ex.Failures)
    {
        Console.WriteLine($"- {error}");
    }
}
return;

static IHostBuilder GetHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services
                .AddConfiguration(context.Configuration)
                .AddServices()
                .AddHostedServices();
        });