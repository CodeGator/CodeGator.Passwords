using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeGator.Passwords;

/// <summary>
/// This class contains tests for host password service registration extensions.
/// </summary>
[TestClass]
public sealed class HostApplicationBuilderExtensionsTests
{
    [TestCategory("Unit")]
    [TestMethod]
    public void AddCodeGatorPasswordServices_null_builder_throws_ArgumentNullException()
    {
        IHostApplicationBuilder? builder = null;

        Assert.ThrowsExactly<ArgumentNullException>(
            () => builder!.AddCodeGatorPasswordServices()
            );
    }

    [TestCategory("Unit")]
    [TestMethod]
    public async Task AddCodeGatorPasswordServices_registers_IPasswordService_and_RandomNumberGenerator()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings { DisableDefaults = true }
            );

        builder.AddCodeGatorPasswordServices();

        await using var provider = builder.Services.BuildServiceProvider();

        using var scope = provider.CreateAsyncScope();
        var passwords = scope.ServiceProvider.GetRequiredService<IPasswordService>();
        var rng = scope.ServiceProvider.GetRequiredService<RandomNumberGenerator>();

        Assert.IsNotNull(passwords);
        Assert.IsNotNull(rng);
    }
}
