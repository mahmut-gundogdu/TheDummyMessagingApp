using System;
using System.Threading;
using MessagingService.Core;
using MessagingService.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MessagingService.Tests
{
    public class JwtManagerTests
    {
        private JwtManager _jwtManager;

        public JwtManagerTests()
        {
            var appSettings = new AppSettings() {Jwt = new JwtSettings() {Secret = "6p5OcwgJDjNm52o5XmAu0zHQ"}};
            var option = Substitute.For<IOptions<AppSettings>>();

            option.Value
                .ReturnsForAnyArgs(appSettings);
            _jwtManager = new JwtManager(option);
        }

        [Fact]
        public void GenerateJwtToken_UserName_WithoutException()
        {
            //Act
            var token = _jwtManager.GenerateJwtToken("a.b", DateTime.Now.AddMinutes(5));
            var user = _jwtManager.ValidateAndGetUserName(token);
            
            //Assert
            token.ShouldNotBeNullOrEmpty();
            user.ShouldNotBeEmpty();
        }
        
        [Fact]
        public void ValidateAndGetUserName_ValidToken_UserNotNull()
        {
            //Assert
            var token = _jwtManager.GenerateJwtToken("a.b", DateTime.Now.AddMinutes(5));
            
            //Act
            var user = _jwtManager.ValidateAndGetUserName(token);
            
            //Assert
            user.ShouldNotBeEmpty();
        }
        [Fact]
        public void ValidateAndGetUserName_InvalidToken_ThrowException()
        {
            //Assert
            var token = _jwtManager.GenerateJwtToken("a.b", DateTime.Now.AddSeconds(1));
            
            Thread.Sleep(2000);
            //Act;
            string userName = null;
            try
            {

                 userName = _jwtManager.ValidateAndGetUserName(token);
            }
            catch (Exception exception)
            {
                
                //Assert
                exception.ShouldBeOfType<SecurityTokenExpiredException>();
            }
            
            //Assert 
            userName.ShouldBeNullOrEmpty();
        }
    }
}