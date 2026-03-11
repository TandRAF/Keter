using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.UserDto;

namespace server.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> SignUpAsync(SignUpDto model);
        Task<AuthResponseDto> SignInAsync(SignInDto model);
    }
}