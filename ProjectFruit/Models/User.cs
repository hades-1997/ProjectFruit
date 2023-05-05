using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectFruit.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    [Compare("Password")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    public string? Email { get; set; }

    public int? AuthorId { get; set; }

    public byte Status { get; set; }

    public virtual Author? Author { get; set; }
}
