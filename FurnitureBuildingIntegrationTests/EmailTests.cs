using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Services;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace FurnitureBuildingIntegrationTests
{
    public class EmailTests
    {
        [Fact]
        public void Send()
        {
            var email = new Email(){
                HtmlBody = "<h1>Hello world!</h1>",
                TextBody = "Hello world!",
                Subject = "Shelfer email integration test",
                Receiver = "alexander@copalex.com"
            };
            var testDir = AppContext.BaseDirectory;
            var settings = AppSettings.GetSettings(testDir, "Test");
            var options = Options.Create(settings);
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<EmailSender>();

            var emailSender = new EmailSender(options, logger);

            var response = emailSender.Send(email);

            Assert.True(response);
        }
    }
}
