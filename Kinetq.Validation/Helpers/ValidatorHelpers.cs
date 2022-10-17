using System.Reflection;
using Kinetq.Validation.Factories;
using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Validators.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Kinetq.Validation.Helpers
{
    public static class ValidatorHelpers
    {
        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly validatorAssembly)
        {
            List<Type> types =
                validatorAssembly.GetTypes()
                    .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.Name == "IValidator`1"))
                    .ToList();

            foreach (Type type in types)
            {
                var dtoType = type.GetInterfaces().First().GenericTypeArguments[0];
                services.Add(new ServiceDescriptor(typeof(IValidator<>).MakeGenericType(dtoType), type, ServiceLifetime.Scoped));
            }

            services.AddScoped<IValidatorFactory, ValidatorFactory>();
            return services;
        }
    }
}