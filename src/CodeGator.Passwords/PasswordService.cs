
namespace CodeGator.Passwords;

/// <summary>
/// This class is the default <see cref="IPasswordService"/> implementation.
/// </summary>
/// <param name="randomNumberGenerator">The RNG used when sampling password characters.</param>
/// <param name="logger">The logger used when password generation fails.</param>
internal class PasswordService(
    [NotNull] RandomNumberGenerator randomNumberGenerator,
    [NotNull] ILogger<PasswordService> logger
    ) : IPasswordService
{

    /// <inheritdoc/>
    public async Task<string> GeneratePasswordAsync(
        [NotNull] PasswordParameters parameters,
        CancellationToken cancellationToken = default
        )
    {
        Guard.Instance().ThrowIfNull(parameters, nameof(parameters));

        try
        {
            if (parameters.UpperCase < 0)
            {
                parameters.UpperCase = 0;
            }

            if (parameters.LowerCase < 0)
            {
                parameters.LowerCase = 0;
            }

            if (parameters.Symbols < 0)
            {
                parameters.Symbols = 0;
            }

            if (parameters.Numbers < 0)
            {
                parameters.Numbers = 0;
            }

            var password = await GenerateCharactersAsync(
                parameters, 
                cancellationToken
                ).ConfigureAwait(false);  

            return password;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "The '{t1}' service failed to generate a password",
                nameof(PasswordService)
                );

            throw new ServiceException(
                innerException: ex,
                message: "Failed to generate a password."
                );
        }
    }

    // Private methods.

    /// <summary>
    /// This method builds a password string from random character buckets.
    /// </summary>
    /// <param name="parameters">The counts and rules that define the password shape.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for a cancellation request throughout the lifetime of the method.</param>
    /// <returns>A task that completes with the generated password.</returns>
    private Task<string> GenerateCharactersAsync(
        [NotNull] PasswordParameters parameters,
        CancellationToken cancellationToken = default
        )
    {
        if (parameters.Symbols + 
            parameters.Numbers + 
            parameters.UpperCase +
            parameters.LowerCase == 0)
        {
            return Task.FromResult("");
        }

        var sb = new StringBuilder();

        sb.Append(
            parameters.UpperCase > 0 ? 
                randomNumberGenerator.NextUpper(parameters.UpperCase) 
                : ""
                );

        sb.Append(
            parameters.LowerCase > 0 ? 
                randomNumberGenerator.NextLower(parameters.LowerCase) 
                : ""
                );

        sb.Append(
            parameters.Symbols > 0 ?
                randomNumberGenerator.NextSymbols(parameters.Symbols)
                : ""
                );

        sb.Append(
            parameters.Numbers > 0 ?
                randomNumberGenerator.NextDigits(parameters.Numbers)
                : ""
                );

        sb.Shuffle(randomNumberGenerator)
            .Reverse()
            .Shuffle(randomNumberGenerator);

        var password = sb.ToString();
        return Task.FromResult(password);
    }
}
