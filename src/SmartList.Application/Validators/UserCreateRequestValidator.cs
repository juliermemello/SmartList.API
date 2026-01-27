using Ardalis.GuardClauses.Net9;
using FluentValidation;
using SmartList.Application.DTOs.User;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Validators;

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
{
    private readonly IUnitOfWork _uow;

    public UserCreateRequestValidator(IUnitOfWork uow)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("O nome de usuário é obrigatório.")
            .Length(3, 100).WithMessage("O username deve ter entre 3 e 100 caracteres.")
            .MustAsync(BeUniqueUsername).WithMessage("Este nome de usuário já está sendo utilizado.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .MaximumLength(255).WithMessage("O e-mail deve ter no máximo 255 caracteres.")
            .EmailAddress().WithMessage("O formato do e-mail é inválido.")
            .MustAsync(BeUniqueEmail).WithMessage("Este e-mail já está em uso.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.")
            //.Matches(@"[\!\?\*\.]").WithMessage("A senha deve conter pelo menos um caractere especial (!?*.).")
            .Matches(@"[!""#$%&'()*+,\-./:;<=>?\@\[\]\\^_`{\|}~]").WithMessage("A senha deve conter pelo menos um caractere especial."); ;

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("A confirmação da senha é obrigatória.")
            .Equal(x => x.Password).WithMessage("As senhas não conferem.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var userExists = await _uow.Users.GetAllAsync(u => u.Email == email);

        return !userExists.Any();
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        var userExists = await _uow.Users.GetAllAsync(u => u.Username == username);

        return !userExists.Any();
    }
}
