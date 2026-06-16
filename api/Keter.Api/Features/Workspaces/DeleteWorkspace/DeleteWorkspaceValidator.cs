using FluentValidation;

namespace Keter.Api.Features.Workspaces.DeleteWorkspace;

public class DeleteWorkspaceValidator : AbstractValidator<DeleteWorkspaceCommand>
{
    public DeleteWorkspaceValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty().WithMessage("Workspace ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}