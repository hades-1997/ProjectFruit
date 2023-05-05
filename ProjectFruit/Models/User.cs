using System;
using System.Collections.Generic;

namespace ProjectFruit.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? AuthorId { get; set; }

    public byte Status { get; set; }

    public virtual Author? Author { get; set; }
}
