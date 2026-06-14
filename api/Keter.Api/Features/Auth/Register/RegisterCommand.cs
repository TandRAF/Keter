using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Auth/Register/RegisterCommand.cs
using MediatR;

namespace Keter.Api.Features.Auth.Register;

// What React sends
public record RegisterRequest(string Email, string Password, string FullName);

// What MediatR processes
public record RegisterCommand(string Email, string Password, string FullName) : IRequest<Guid>;