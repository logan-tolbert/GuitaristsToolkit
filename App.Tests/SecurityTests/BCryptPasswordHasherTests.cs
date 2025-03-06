using App.Models;
using App.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitaristsToolkit.Tests.SecurityTests
{
    public class BCryptPasswordHasherTests
    {
        private readonly BCryptPasswordHasher _hasher;
        private readonly User _user;

        public BCryptPasswordHasherTests()
        {
            _hasher = new BCryptPasswordHasher();
            _user = new User { Username = "testuser" };
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange 
            string password = "password123";
            // Act
            string hashedPassword = _hasher.HashPassword(_user, password);

            // Assert 
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            string password = "securepassword";
            string hashedPassword = _hasher.HashPassword(_user, password);

            // Act 
            var result = _hasher.VerifyHashedPassword(_user, hashedPassword, password);

            // Assert
            Assert.Equal(PasswordVerificationResult.Success, result);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnFailedForincorrectPassword()
        {
            // Arrange 
            string hashedPassword = _hasher.HashPassword(_user, "correctpassword");

            // Act 
            var result = _hasher.VerifyHashedPassword(_user, hashedPassword, "wrongpassword");

            // Assert
            Assert.Equal(PasswordVerificationResult.Failed, result);
        }

        [Fact]
        public void HashPassword_ShouldGenerateDifferentHashesForSameInput()
        {
            // Arrange
            string hash1 = _hasher.HashPassword(_user, "password");
            string hash2 = _hasher.HashPassword(_user, "password");

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
