using Ardalis.GuardClauses.Net9;
using FluentValidation;
using SmartList.Application.DTOs.ShoppingList;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Validators;

public class ShoppingListCreateRequestValidator : AbstractValidator<ShoppingListCreateRequest>
{
    private readonly IUnitOfWork _uow;

    public ShoppingListCreateRequestValidator(IUnitOfWork uow)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.")
            .MustAsync(BeUniqueName).WithMessage("Já existe uma categoria com este nome.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var shoppings = await _uow.ShoppingList.GetAllAsync(u => u.Name.ToLower() == name.ToLower());

        return !shoppings.Any();
    }
}
