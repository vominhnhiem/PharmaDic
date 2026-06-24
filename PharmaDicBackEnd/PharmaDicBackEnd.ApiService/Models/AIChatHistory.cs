namespace PharmaDicBackEnd.ApiService.Models;

public partial class AIChatHistory
{
    public int ChatId { get; set; }

    public int UserId { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}