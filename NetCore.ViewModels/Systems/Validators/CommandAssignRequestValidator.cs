using FluentValidation;

namespace NetCore.ViewModels.Systems.Validators;

public class CommandAssignRequestValidator : AbstractValidator<CommandAssignRequest>
{
    public CommandAssignRequestValidator()
    {
        RuleFor(x => x.CommandIds).NotNull()
            .WithMessage(string.Format(Messages.Required, "Mã lệnh"));

        RuleFor(x => x.CommandIds)
            .Must(x => x != null && x.Length > 0)
            .WithMessage("Danh sách mã lệnh phải có ít nhất 1 phần tử");

        RuleForEach(x => x.CommandIds).NotEmpty()
            .WithMessage("Danh sách mã lệnh không được chứa phần tử rỗng");
    }
}