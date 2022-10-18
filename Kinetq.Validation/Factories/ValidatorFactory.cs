using Kinetq.Validation.Exceptions;
using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Helpers;
using Kinetq.Validation.Models;

namespace Kinetq.Validation.Factories
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Validate<T>(T dto, string name = null) where T : class
        {
            IEnumerable<IValidator<T>> validations =
                (IEnumerable<IValidator<T>>)_serviceProvider
                    .GetService(typeof(IEnumerable<>)
                        .MakeGenericType(typeof(IValidator<>)
                            .MakeGenericType(typeof(T))));

            var result = new ValidationErrors();
            foreach (IValidator<T> validator in validations.OrderBy(x => x.Order))
            {
                validator.ValidatorFactory = this;
                validator.GetName = SetupGetNameFunc(name);
                validator.GetNameWithIndex = SetupGetNameWithIndexFunc(name);

                await validator.Execute(dto, result);
            }

            if (result.ErrorMessages.Any()) throw new ValidationsException(result);
        }

        public async Task ValidateNested<T>(T dto, string? name = null, ValidationErrors? validationErrors = null) where T : class
        {
            IEnumerable<IValidator<T>> validations =
                (IEnumerable<IValidator<T>>)_serviceProvider
                    .GetService(typeof(IEnumerable<>)
                        .MakeGenericType(typeof(IValidator<>)
                            .MakeGenericType(typeof(T))));

            var result = validationErrors ?? new ValidationErrors();
            foreach (IValidator<T> validator in validations.OrderBy(x => x.Order))
            {
                validator.ValidatorFactory = this;
                validator.GetName = SetupGetNameFunc(name);
                validator.GetNameWithIndex = SetupGetNameWithIndexFunc(name);

                await validator.Execute(dto, result);
            }
        }


        private Func<string, string> SetupGetNameFunc(string? name = null)
        {
            return (string childName) =>
            {
                var fullName =
                    name == null
                        ? childName.FirstCharToLowerCase()
                        : $"{name}.{childName.FirstCharToLowerCase()}";

                return fullName;
            };
        }

        private Func<string, int, string> SetupGetNameWithIndexFunc(string? name = null)
        {
            return (string childName, int index) =>
            {
                var fullName =
                    name == null
                        ? childName.FirstCharToLowerCase()
                        : $"{name}.{childName.FirstCharToLowerCase()}";

                fullName = $"{fullName}[{index}]";

                return fullName;
            };
        }

    }
}