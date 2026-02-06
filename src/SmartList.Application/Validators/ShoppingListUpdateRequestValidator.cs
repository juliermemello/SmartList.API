using FluentValidation;
using SmartList.Application.DTOs.ShoppingList;

namespace SmartList.Application.Validators;

public class ShoppingListUpdateRequestValidator : AbstractValidator<ShoppingListUpdateRequest>
{
    public ShoppingListUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");
    }
}
