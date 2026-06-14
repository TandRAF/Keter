using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Auth/Login/LoginCommand.cs
using MediatR;

namespace Keter.Api.Features.Auth.Login;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, string Email);

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;