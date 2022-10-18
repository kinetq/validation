# Kinetq.Validation

This Validation library is very unopinionated. There are no built in validations. Instead it simply sets up a pattern where validation is called based on the object posted to a controller by matching it up using reflection. By doing this one can more easily setup custom validation based on backend logic (which a lot of the time needs to go to the database). 



## Setup:

```c#
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
```



You must make sure to register the validator services by call AddValidators as well as setting up the middleware as shown above. It is also important to inject the JsonSerializerOptions:



```c#
services.AddSingleton(provider =>
                    provider.GetService<IOptions<JsonOptions>>().Value.SerializerOptions);
```

## Usage:

In the controller you need to inject the validator factory and call Validate:

```c#
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IValidatorFactory _validatorFactory;

    public UserController(IValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    [HttpPost]
    public async Task<UserDto> Post(UserDto dto)
    {
        await _validatorFactory.Validate(dto);
        return dto;
    }
}
```

It uses type inference to identify which validator is called:

```c#
public class UserValidator : IValidator<UserDto>
{
    public async Task Execute(UserDto dto, ValidationErrors validationErrors)
    {
        if (string.IsNullOrEmpty(dto.FirstName))
        {
            validationErrors.Add(GetName(nameof(UserDto.FirstName)), "First name needs to be supplied");
        }

        if (string.IsNullOrEmpty(dto.LastName))
        {
            validationErrors.Add(GetName(nameof(UserDto.LastName)), "Last name needs to be supplied");
        }

        for (var i = 0; i < dto.Roles.Count; i++)
        {
            if (dto.Roles[i] == null)
            {
                validationErrors.Add(GetNameWithIndex(nameof(UserDto.Roles), i), "Role cannot be null");
            }
        }
    }

    public int Order { get; }   
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int, string> GetNameWithIndex { get; set; }
    public Func<string, string> GetName { get; set; }
}
```

In order to validate Nested objects you have to use the ValidatorFactory inside the validator itself:

```c#
public class SecondUserValidator : IValidator<UserDto>
{
    public async Task Execute(UserDto dto, ValidationErrors validationErrors)
    {
        await ValidatorFactory.ValidateNested(dto.Address, GetName(nameof(UserDto.Address)), validationErrors);
    }

    public int Order => 1;
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int, string> GetNameWithIndex { get; set; }
    public Func<string, string> GetName { get; set; }
}
```

Because validation can be nested it is always important to use either the GetName function or GetNameWithIndex function to set the field name. This will properly nest the names using dot notation like this: "address.street."



The resulting JSON object will look like this:

```json
{
	"errors": [
		{
			"field": "address.zipcode",
			"messages": [
				"Zipcode needs to be supplied"
			],
			"errorCode": null
		}
	]
}
```

