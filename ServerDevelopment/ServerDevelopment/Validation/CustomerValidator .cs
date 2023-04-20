using FluentValidation;
using ServerDevelopment.Data;
using ServerDevelopment.Interfaces;

public class CustomerValidator : AbstractValidator<CustomerDTO>
{
    private readonly ICustomerService _customerService;
    public CustomerValidator(ICustomerService customerService, string oldName = "")
    {
        _customerService = customerService;

        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Length(4, 30).WithMessage("Name must be between 4 and 30 characters.")
        .MustAsync(async (name, cancellation) =>
        {
            if (oldName == name && oldName != "")
            {
                return true;
            }
            else
            {
                var customer = await _customerService.GetByNameAsync(name);
                if (customer == null)
                {
                    return true;
                }
                return false;
            }
        })
        .WithMessage("Name must be unique.");

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Company is required.")
            .Length(4, 30).WithMessage("Company must be between 4 and 30 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\d{9}$").WithMessage("Phone must be a 9-digit number.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Length(4, 30).WithMessage("Email must be between 4 and 30 characters.")
            .EmailAddress().WithMessage("Invalid email address.");
    }
}
