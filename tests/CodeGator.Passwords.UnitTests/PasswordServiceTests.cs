using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeGator.Passwords;

/// <summary>
/// This class contains unit tests for <see cref="PasswordService"/>.
/// </summary>
[TestClass]
public sealed class PasswordServiceTests
{
    private static int CountUpper(string s) => s.Count(c => c is >= 'A' and <= 'Z');

    private static int CountLower(string s) => s.Count(c => c is >= 'a' and <= 'z');

    private static int CountDigit(string s) => s.Count(c => c is >= '0' and <= '9');

    [TestCategory("Unit")]
    [TestMethod]
    public async Task GeneratePasswordAsync_null_parameters_throws_ArgumentNullException()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );
        builder.AddCodeGatorPasswordServices();
        await using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => service.GeneratePasswordAsync(null!)
            );
    }

    [TestCategory("Unit")]
    [TestMethod]
    public async Task GeneratePasswordAsync_all_counts_zero_returns_empty_string()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );
        builder.AddCodeGatorPasswordServices();
        await using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var parameters = new PasswordParameters();

        var password = await service.GeneratePasswordAsync(parameters);

        Assert.AreEqual(string.Empty, password);
    }

    [TestCategory("Unit")]
    [TestMethod]
    public async Task GeneratePasswordAsync_negative_counts_are_treated_as_zero()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );
        builder.AddCodeGatorPasswordServices();
        await using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var parameters = new PasswordParameters
        {
            UpperCase = -2,
            LowerCase = -1,
            Symbols = -3,
            Numbers = -4
        };

        var password = await service.GeneratePasswordAsync(parameters);

        Assert.AreEqual(0, parameters.UpperCase);
        Assert.AreEqual(0, parameters.LowerCase);
        Assert.AreEqual(0, parameters.Symbols);
        Assert.AreEqual(0, parameters.Numbers);
        Assert.AreEqual(string.Empty, password);
    }

    [TestCategory("Unit")]
    [TestMethod]
    public async Task GeneratePasswordAsync_mixed_negative_and_positive_counts_only_positive_contribute()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );
        builder.AddCodeGatorPasswordServices();
        await using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var parameters = new PasswordParameters
        {
            UpperCase = -1,
            LowerCase = 4,
            Symbols = 0,
            Numbers = 0
        };

        var password = await service.GeneratePasswordAsync(parameters);

        Assert.AreEqual(4, password.Length);
        Assert.AreEqual(0, CountUpper(password));
        Assert.AreEqual(4, CountLower(password));
        Assert.AreEqual(0, CountDigit(password));
    }

    [TestCategory("Unit")]
    [TestMethod]
    public async Task GeneratePasswordAsync_password_length_matches_sum_of_counts()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );
        builder.AddCodeGatorPasswordServices();
        await using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var parameters = new PasswordParameters
        {
            UpperCase = 2,
            LowerCase = 3,
            Symbols = 1,
            Numbers = 4
        };

        var password = await service.GeneratePasswordAsync(parameters);

        Assert.AreEqual(10, password.Length);
        Assert.AreEqual(2, CountUpper(password));
        Assert.AreEqual(3, CountLower(password));
        Assert.AreEqual(4, CountDigit(password));
        Assert.AreEqual(1, password.Length - CountUpper(password) - CountLower(password) - CountDigit(password));
    }

    [TestCategory("Unit")]
    [TestMethod]
    public async Task GeneratePasswordAsync_invocations_produce_varied_passwords()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );
        builder.AddCodeGatorPasswordServices();
        await using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var parameters = new PasswordParameters
        {
            UpperCase = 5,
            LowerCase = 5,
            Symbols = 2,
            Numbers = 2
        };

        var passwords = new HashSet<string>();
        for (var i = 0; i < 12; i++)
        {
            passwords.Add(await service.GeneratePasswordAsync(parameters));
        }

        Assert.IsTrue(passwords.Count > 1, "Expected multiple distinct passwords from random generation.");
    }
}
