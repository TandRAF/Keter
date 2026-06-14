using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Infrastructure/Auth/IJwtProvider.cs
using Keter.Domain.Entities;

namespace Keter.Api.Infrastructure.Auth;

public interface IJwtProvider
{
    string Generate(ApplicationUser user);
}