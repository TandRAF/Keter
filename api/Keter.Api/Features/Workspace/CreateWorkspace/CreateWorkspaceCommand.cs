using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Workspaces/CreateWorkspace/CreateWorkspaceCommand.cs
using MediatR;

namespace Keter.Api.Features.Workspaces.CreateWorkspace;

// 1. Asta e ceea ce trimite React-ul (fără UserId)
public record CreateWorkspaceRequest(string Name);

// 2. Asta e ceea ce Endpoint-ul trimite mai departe către Handler
public record CreateWorkspaceCommand(string Name, string UserId) : IRequest<Guid>;