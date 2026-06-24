using System;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class AIChatHistory
{
    public int ChatId { get; set; }

    public int UserId { get; set; }

    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}