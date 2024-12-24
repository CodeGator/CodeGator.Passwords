
namespace CodeGator.Passwords;

/// <summary>
/// This interface represents an object that generates passwords.
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// This method generates a secure password.
    /// </summary>
    /// <param name="parameters">The parameters to use for the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for a cancellation request throughout the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a password.</returns>
    /// <exception cref="ArgumentException">This exception is throw whenever one or
    /// more arguments are missing, or invalid.</exception>
    /// <exception cref="ServiceException">This exception is thrown whenever the 
    /// service fails to complete the operation.</exception>
    Task<string> GeneratePasswordAsync(
        [NotNull] PasswordParameters parameters,
        CancellationToken cancellationToken = default
        );
}
