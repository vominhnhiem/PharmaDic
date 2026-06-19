using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class SearchHistory
{
    public int SearchId { get; set; }

    public int? UserId { get; set; }

    public string? Keyword { get; set; }

    public string? SearchType { get; set; }

    public DateTime? SearchDate { get; set; }

    public virtual User? User { get; set; }
}
