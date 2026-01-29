using Ardalis.GuardClauses.Net9;
using FluentValidation;
using SmartList.Application.DTOs.Category;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Validators;

public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    private readonly IUnitOfWork _uow;

    public CategoryCreateRequestValidator(IUnitOfWork uow)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.")
            .MustAsync(BeUniqueName).WithMessage("Já existe uma categoria com este nome.");

        RuleFor(x => x.Icon)
            .Length(3, 100).WithMessage("O ícone deve ter entre 3 e 100 caracteres.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var categories = await _uow.Categories.GetAllAsync(u => u.Name.ToLower() == name.ToLower());

        return !categories.Any();
    }
}
