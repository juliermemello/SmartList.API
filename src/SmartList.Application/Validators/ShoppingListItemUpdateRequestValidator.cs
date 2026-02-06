using Ardalis.GuardClauses.Net9;
using FluentValidation;
using SmartList.Application.DTOs.ShoppingListItem;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Validators;

public class ShoppingListItemUpdateRequestValidator : AbstractValidator<ShoppingListItemUpdateRequest>
{
    private readonly IUnitOfWork _uow;

    public ShoppingListItemUpdateRequestValidator(IUnitOfWork uow)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.")
            .MustAsync(ProductExists).WithMessage("O produto informado não existe ou não pertence ao seu usuário."); ;

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
    }

    private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
    {
        var products = await _uow.Products.GetAllAsync(u => u.Id == productId);

        return products.Any();
    }
}
