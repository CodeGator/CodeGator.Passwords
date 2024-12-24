
namespace CodeGator.Passwords;

/// <summary>
/// This class is a default implementation of the <see cref="IPasswordService"/>    
/// interface.
/// </summary>
/// <param name="randomNumberGenerator">The random number generator to use with this service.</param>
/// <param name="logger">The logger to use with this service.</param>
internal class PasswordService(
    [NotNull] RandomNumberGenerator randomNumberGenerator,
    [NotNull] ILogger<PasswordService> logger
    ) : IPasswordService
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

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

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method generates a password containing random characters.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for a cancellation request throughout the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a password.</returns>
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

    #endregion
}
