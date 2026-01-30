using Ardalis.GuardClauses.Net9;
using FluentValidation;
using SmartList.Application.DTOs.Product;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    private readonly IUnitOfWork _uow;

    public ProductUpdateRequestValidator(IUnitOfWork uow)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("A categoria é obrigatória.")
            .MustAsync(CategoryExists).WithMessage("A categoria informada não existe ou não pertence ao seu usuário."); ;
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        var categories = await _uow.Categories.GetAllAsync(u => u.Id == categoryId);

        return categories.Any();
    }
}
