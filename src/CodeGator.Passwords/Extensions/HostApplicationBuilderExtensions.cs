
namespace Microsoft.Extensions.Hosting;

/// <summary>
/// This class utility contains extension methods related to the <see cref="IHostApplicationBuilder"/>
/// type.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds support for CodeGator password services.
    /// </summary>
    /// <typeparam name="TBuilder">The type of associated host application 
    /// builder.</typeparam>
    /// <param name="hostApplicationBuilder">The host application builder
    /// to use for the operation.</param>
    /// <returns>The value of the <paramref name="hostApplicationBuilder"/>
    /// parameter, for chaining method calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
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

    #endregion
}
