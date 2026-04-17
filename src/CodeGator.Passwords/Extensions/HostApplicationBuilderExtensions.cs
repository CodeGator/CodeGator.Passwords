
namespace Microsoft.Extensions.Hosting;

/// <summary>
/// This class defines extension methods for <see cref="IHostApplicationBuilder"/>.
/// </summary>
/// <remarks>
/// Callers use these helpers to register CodeGator password services during host
/// construction.
/// </remarks>
public static partial class HostApplicationBuilderExtensions
{

    /// <summary>
    /// This method adds support for CodeGator password services.
    /// </summary>
    /// <typeparam name="TBuilder">The type of associated host application 
    /// builder.</typeparam>
    /// <param name="hostApplicationBuilder">The host application builder
    /// to use for the operation.</param>
    /// <param name="bootstrapLogger">Optional logger for registration debug output.</param>
    /// <returns>The value of the <paramref name="hostApplicationBuilder"/>
    /// parameter, for chaining method calls together, Fluent style.</returns>
    /// <exception cref="ArgumentNullException">This exception is thrown when
    /// <paramref name="hostApplicationBuilder"/> is <see langword="null"/>.</exception>
    public static TBuilder AddCodeGatorPasswordServices<TBuilder>(
        [NotNull] this TBuilder hostApplicationBuilder,
        [AllowNull] ILogger? bootstrapLogger = null
        ) where TBuilder : IHostApplicationBuilder
    {
        Guard.Instance().ThrowIfNull(hostApplicationBuilder, nameof(hostApplicationBuilder));

        bootstrapLogger?.LogDebug(
            "Registering CodeGator password services."
            );

        hostApplicationBuilder.Services.AddScoped<IPasswordService, PasswordService>();

        bootstrapLogger?.LogDebug(
            "Registering a random number generator."
            );

        hostApplicationBuilder.Services.AddSingleton<RandomNumberGenerator>(serviceProvider =>
        {
            return RandomNumberGenerator.Create();
        });

        return hostApplicationBuilder;
    }
}
