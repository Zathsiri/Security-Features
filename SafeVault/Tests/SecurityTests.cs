using NUnit.Framework;

namespace SafeVault.Tests
{
    [TestFixture]
    public class SecurityTests
    {
        [Test]
        public void TestForSQLInjection()
        {
            string maliciousInput = "'; DROP TABLE Users; --";
            bool isValid = InputValidator.IsValidUsername(maliciousInput);
            Assert.IsFalse(isValid, "SQL Injection input should be rejected.");
        }

        [Test]
        public void TestForXSS()
        {
            string maliciousInput = "<script>alert('hacked')</script>";
            string sanitized = InputValidator.SanitizeInput(maliciousInput);
            Assert.IsFalse(sanitized.Contains("<script>"), "XSS payload should be sanitized.");
        }

        [Test]
        public void TestAuthenticationFailure()
        {
            bool loginSuccess = AuthTester.AttemptLogin("fakeUser", "wrongPassword");
            Assert.IsFalse(loginSuccess, "Invalid login should fail.");
        }

        [Test]
        public void TestAuthorizationAdminAccess()
        {
            string token = AuthTester.GetToken("adminUser", "adminPass");
            bool canAccess = AuthTester.CanAccessAdmin(token);
            Assert.IsTrue(canAccess, "Admin should access Admin Dashboard.");
        }
    }
}