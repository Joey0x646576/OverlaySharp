namespace OverlaySharp.Runner
{
    internal static partial class Program
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions<RunnerConfiguration>()
                .Bind(configuration.GetSection(nameof(RunnerConfiguration)));
            serviceCollection.AddSingleton<IValidateOptions<RunnerConfiguration>, RunnerConfigurationValidator>();

            return serviceCollection;
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<OverlayHostedService>();
            return serviceCollection;
        }
    }
}
