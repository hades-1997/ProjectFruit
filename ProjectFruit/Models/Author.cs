using System;
using System.Collections.Generic;

namespace ProjectFruit.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
