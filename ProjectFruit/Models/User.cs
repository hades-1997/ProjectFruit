using System;
using System.Collections.Generic;

namespace ProjectFruit.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Md5username { get; set; }

    public string Password { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string Email { get; set; } = null!;

    public int? DateTime { get; set; }

    public int? AuthorId { get; set; }

    public byte Status { get; set; }

    public DateTime? AddTime { get; set; }

    public virtual Author? Author { get; set; }
}
