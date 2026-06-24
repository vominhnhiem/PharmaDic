namespace PharmaDicBackEnd.ApiService.DTOs;

public class ChatRequest
{
    public int UserId { get; set; }
    public string Question { get; set; } = string.Empty;
}

public class InteractionRequest
{
    public List<int> MedicineIds { get; set; } = new();
}

public class RegimenRequest
{
    public string Symptoms { get; set; } = string.Empty;
}
