using OnlineBookstore.Api.Security;
using Xunit;

namespace OnlineBookstore.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_And_Verify_Should_Work()
    {
        var hash = PasswordHasher.Hash("admin123");

        Assert.True(PasswordHasher.Verify("admin123", hash));
        Assert.False(PasswordHasher.Verify("wrong", hash));
    }
}
