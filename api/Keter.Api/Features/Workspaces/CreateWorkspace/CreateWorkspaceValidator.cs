using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Workspaces/CreateWorkspace/CreateWorkspaceValidator.cs
using FluentValidation;

namespace Keter.Api.Features.Workspaces.CreateWorkspace;

public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workspace name is required.")
            .MinimumLength(3).WithMessage("Workspace name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Workspace name cannot exceed 100 characters.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("A valid User ID is required to create a workspace.");
    }
}