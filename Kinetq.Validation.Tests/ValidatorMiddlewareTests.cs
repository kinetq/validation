using System.Net;
using Kinetq.Validation.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Kinetq.Validation.Middleware;
using Kinetq.Validation.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Kinetq.Validation.Tests.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Kinetq.Validation.Tests;

public class ValidatorMiddlewareTests
{
    private readonly TestServer _testServer;

    public ValidatorMiddlewareTests()
    {
        var host = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddValidators(Assembly.GetExecutingAssembly());
                services.AddRouting();
                services.AddControllers().AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

                services.AddSingleton(provider =>
                    provider.GetService<IOptions<JsonOptions>>().Value.SerializerOptions);
            })
            .Configure(app =>
            {
                app.UseMiddleware<ValidatorMiddleware>();

                app.UseRouting();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            });

        _testServer = new TestServer(host);
    }

    [Fact]
    public async Task UserApi_Post_ThrowsValidationError()
    {
        var user = new UserDto()
        {
            LastName = "Doe",
            FirstName = "John",
            Address = new AddressDto()
            {
                City = "Bisonica",
                Street = "3 Bison Ave"
            }
        };

        var payload = JsonSerializer.Serialize(user);
        var testClient = _testServer.CreateClient();
        var response =
            await testClient.PostAsync("/user", new StringContent(payload, Encoding.UTF8, "application/json"));
        var responseObj = await response.Content.ReadAsStringAsync();
        var validations = JsonSerializer.Deserialize<ValidationResponse>(responseObj, _testServer.Services.GetService<JsonSerializerOptions>());

        Assert.Equal((HttpStatusCode)StatusCodes.Status400BadRequest, response.StatusCode);
        Assert.Equal("address.zipcode", validations.Errors.First().Field);
    }
}