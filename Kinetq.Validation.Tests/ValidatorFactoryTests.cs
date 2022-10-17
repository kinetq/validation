using System.Reflection;
using Kinetq.Validation.Exceptions;
using Kinetq.Validation.Factories;
using Kinetq.Validation.Helpers;
using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Tests.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Kinetq.Validation.Tests
{
    public class ValidatorFactoryTests
    {
        private readonly IValidatorFactory _validatorFactory;

        public ValidatorFactoryTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IValidatorFactory, ValidatorFactory>();
            services.AddValidators(Assembly.GetExecutingAssembly());

            var serviceProvider = services.BuildServiceProvider();
            _validatorFactory = serviceProvider.GetService<IValidatorFactory>();
        }

        [Fact]
        public async Task Test_Recursive_Validation()
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

            ValidationsException exception = 
                await Record.ExceptionAsync(() => _validatorFactory.Validate(user)) as ValidationsException;

            Assert.Equal(1, exception.ValidationErrors.ErrorMessages.Count);
        }        
        
        [Fact]
        public async Task Test_CallsValidatorsInOrder()
        {
            var user = new UserDto()
            {
                LastName = "Doe",
                Address = new AddressDto()
                {
                    City = "Bisonica",
                    Street = "3 Bison Ave"
                }
            };

            ValidationsException exception = 
                await Record.ExceptionAsync(() => _validatorFactory.Validate(user)) as ValidationsException;

            Assert.Equal("firstName", exception.ValidationErrors.ErrorMessages.First().Field);
            Assert.Equal("address.zipcode", exception.ValidationErrors.ErrorMessages.Last().Field);
        }

        [Fact]
        public async Task Test_ThrowsException_WhenErrorAdded()
        {
            var user = new UserDto()
            {
                LastName = "Doe",
                Address = new AddressDto()
                {
                    City = "Bisonica",
                    Street = "3 Bison Ave",
                    Zipcode = "89989"
                }
            };

            ValidationsException exception =
                await Record.ExceptionAsync(() => _validatorFactory.Validate(user)) as ValidationsException;

            Assert.NotNull(exception);
        }
    }
}