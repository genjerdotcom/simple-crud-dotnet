using FluentValidation;
using Features.Products.DTO;

namespace Features.Products.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .When(x => x.Name != null)
            .WithMessage("Product name must be at least 3 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price.HasValue)
            .WithMessage("Price must be greater than 0");
    }
}