using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Keter.Domain.Entities;

public class Profile
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }

    public ApplicationUser User { get; set; } = null!;
}