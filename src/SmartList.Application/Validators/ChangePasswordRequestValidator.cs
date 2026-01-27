using FluentValidation;
using SmartList.Application.DTOs.ChangePassword;

namespace SmartList.Application.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("O nome de usuário é obrigatório.");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("A senha é obrigatória.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("A nova senha é obrigatória.")
            .MinimumLength(8).WithMessage("A nova senha deve ter no mínimo 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("A nova senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("A nova senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("A nova senha deve conter pelo menos um número.")
            .Matches(@"[!""#$%&'()*+,\-./:;<=>?\@\[\]\\^_`{\|}~]").WithMessage("A senha deve conter pelo menos um caractere especial.")
            .NotEqual(x => x.OldPassword).WithMessage("A nova senha não pode ser igual a anterior.");
    }
}
